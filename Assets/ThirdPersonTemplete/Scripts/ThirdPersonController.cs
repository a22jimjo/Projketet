using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        public AudioClip[] FastAttackAudioClips;
        public AudioClip[] HeavyAttackAudioClips;
        public AudioClip[] DashAudioClips;
        public AudioClip TakeDamageAudioClip;
        
        [Header("Volumes")]
        [Range(0, 1)] public float FootstepAudioVolume = 0.08f;
        [Range(0, 1)] public float FastAttackAudioVolume = 0.6f;
        [Range(0, 1)] public float HeavyAttackAudioVolume = 0.6f;
        [Range(0, 1)] public float DashAudioVolume = 0.6f;
        [Range(0, 1)] public float TakeDamageAudioVolume = 0.8f;
        

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        [Header("FastAttackValues")]
        [Tooltip("Cooldown until you can make your next attack")]
        public float AttackCooldown = 0.34f;
        public float DamageCooldown = 0.05f;
        public float SlowDownMultiplier = 0.5f;
        public float SlowDownTime;
        [Header("HeavyAttackValues")]
        public float HeavyAttackCooldown = 0.34f;
        public float HeavyAttackDamageCooldown = 0.3f;
        [Header("DashValues")]
        public float DashSpeed = 1.1f;
        public float DashDuration = 1;
        public float DashCooldown = 1;
        [Header("Tid mellan fotsteg")] public float FootstepCooldown = 0.01f;

        public bool canMove = true;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private float _attackTime;
        private float _damageTime;
        private bool _slowDown = false;
        private float _dashTime = 0;
        private float _dashDuration = 0;
        private float _footStepTimer = 0;
        private float _slowDownTimer;

        public bool _fixedPosition;
        public bool invincible = false;

        // timeout deltatime
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIdWalkAni;
        private int _animIdAttackForwardAni;
        private int _animIdRightSideAttackAni;
        private int _animIdSlashSlashAttackAni;
        private int _animIdtakeDamageEx1Ani;
        private int _deadForNowAni;
        private int _animIdDash;

        private Vector3 _edgeToPlayerCamera;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private BasicRigidBodyPush _basicRigidBodyPush;
        private CharacterController _controller;
        public StarterAssetsInputs input;
        private GameObject _mainCamera;
        private GameObject _sword;
        private AudioSource _audio;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;
        
        private Vector3 _relativePosition;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _audio = GetComponent<AudioSource>();
            _audio.spatialBlend = 0;
            _sword = GameObject.FindGameObjectWithTag("Sword");
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
            _edgeToPlayerCamera = Camera.main.WorldToScreenPoint(transform.position);
            _edgeToPlayerCamera = new Vector3(_edgeToPlayerCamera.x, 0, _edgeToPlayerCamera.y);
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            AttackTest();

            if (canMove)
            {
                Move();

            }
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIdWalkAni = Animator.StringToHash("PlayerWalk");
            _animIdAttackForwardAni = Animator.StringToHash("Heavy");
            _animIdSlashSlashAttackAni = Animator.StringToHash("Fast");
            _animIdRightSideAttackAni = Animator.StringToHash("SecondAttack");
            _animIdtakeDamageEx1Ani = Animator.StringToHash("TakeDamage");
            _deadForNowAni = Animator.StringToHash("Death");
            _animIdDash = Animator.StringToHash("Dash");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            if (!_fixedPosition && !_slowDown)
            {
                // set target speed based on move speed, sprint speed and if sprint is pressed
                float targetSpeed = input.sprint ? SprintSpeed : MoveSpeed;

                if (_slowDown)
                {
                    targetSpeed *= SlowDownMultiplier;
                }

                float dash = 0;

                if ((input.dash && _dashTime < 0))
                {
                    _dashDuration = DashDuration;
                    _animator.SetTrigger(_animIdDash);
                    _slowDown = false;
                    _audio.PlayOneShot(DashAudioClips[Random.Range(0, DashAudioClips.Length)], DashAudioVolume);
                }

                if (_dashDuration > 0)
                {
                    invincible = true;
                    dash = DashSpeed * Time.deltaTime;
                    _dashTime = DashCooldown;
                    input.dash = false;
                }
                else if(_dashDuration < -0.1) invincible = false;
                _dashTime -= Time.deltaTime;
                _dashDuration -= Time.deltaTime;
                
                // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

                // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
                // if there is no input, set the target speed to 0
                if (input.move == Vector2.zero) targetSpeed = 0.0f;

                // a reference to the players current horizontal velocity
                float currentHorizontalSpeed =
                    new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

                float speedOffset = 0.1f;
                float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

                // accelerate or decelerate to target speed
                if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                    currentHorizontalSpeed > targetSpeed + speedOffset)
                {
                    // creates curved result rather than a linear one giving a more organic speed change
                    // note T in Lerp is clamped, so we don't need to clamp our speed
                    _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                        Time.deltaTime * SpeedChangeRate);

                    // round speed to 3 decimal places
                    _speed = Mathf.Round(_speed * 1000f) / 1000f;
                }
                else
                {
                    _speed = targetSpeed;
                }

                _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
                if (_animationBlend < 0.01f) _animationBlend = 0f;

                // normalise input direction
                Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

                // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
                // if there is a move input rotate player when the player is moving
                if (input.move != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      _mainCamera.transform.eulerAngles.y;

                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
                        ref _rotationVelocity,
                        RotationSmoothTime);
                    if (_attackTime <= 0)
                    {
                        // rotate to face input direction relative to camera position
                        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                    }

                    if (_footStepTimer < 0)
                    {
                        _audio.PlayOneShot(FootstepAudioClips[Random.Range(0, FootstepAudioClips.Length)], FootstepAudioVolume);
                        _footStepTimer = FootstepCooldown;
                    }
                }

                _footStepTimer -= Time.deltaTime;
                
                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

                // move the player
                _controller.Move(targetDirection.normalized * ((_speed + dash) * Time.deltaTime) +
                                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
                _dashDuration -= Time.deltaTime;
                // update animator if using character
                if (_hasAnimator)
                {
                    if (_speed > 0)
                    {
                        _animator.SetBool(_animIdWalkAni, true);
                    }
                    else _animator.SetBool(_animIdWalkAni, false);
                }
            }
        }

        private void AttackRotation()
        {
            // rotate to face input direction relative to camera position
            Vector3 relativePos = new Vector3(input.see.x, 0f, input.see.y -60)- _edgeToPlayerCamera;
         
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = rotation;
        }


        //Checks attack input and does whatever we want attack to do
        private void AttackTest()
        {
            if (!invincible)
            {
                //Step 1: Check attack input and cooldowns
                if (input.attack && (_attackTime <= 0))
                {
                    AttackRotation();
                    _animator.SetBool(_animIdSlashSlashAttackAni, true);
                    _attackTime = AttackCooldown;
                    _damageTime = DamageCooldown;
                    _slowDown = true;
                    _slowDownTimer = SlowDownTime;
                    _audio.PlayOneShot(FastAttackAudioClips[Random.Range(0, FastAttackAudioClips.Length)],
                        FastAttackAudioVolume);
                    input.attack = false;
                }
                else if (input.heavyAttack && (_attackTime <= 0))
                {
                    AttackRotation();
                    _animator.SetBool(_animIdAttackForwardAni, true);
                    _attackTime = HeavyAttackCooldown;
                    _damageTime = HeavyAttackDamageCooldown;
                    _fixedPosition = true;
                    if (_sword.TryGetComponent<Sword>(out Sword sword)) sword.heavyAttack = true;
                    _audio.PlayOneShot(HeavyAttackAudioClips[Random.Range(0, HeavyAttackAudioClips.Length)],
                        HeavyAttackAudioVolume);
                    input.heavyAttack = false;
                }
                else
                {
                    _damageTime -= Time.deltaTime;
                    _attackTime -= Time.deltaTime;
                    _slowDownTimer -= Time.deltaTime;
                    _animator.SetBool(_animIdSlashSlashAttackAni, false);
                    _animator.SetBool(_animIdAttackForwardAni, false);
                }

                if (_slowDownTimer < 0)
                {
                    _slowDown = false;
                }

                //Step 2: Remove input

                //Step 3: Check if the attack is done
                if (_attackTime <= 0)
                {
                    if (_sword.TryGetComponent<Sword>(out Sword sword)) sword.attacking = false;
                    sword.heavyAttack = false;
                }
                else if (_damageTime < 0)
                {
                    if (_sword.TryGetComponent<Sword>(out Sword sword) && _damageTime > -10)
                    {
                        Debug.Log("AHAHHAHA");
                        sword.attacking = true;
                        sword.Vfx();
                        _fixedPosition = false;
                        _damageTime = -10;
                    }
                }
                //else AttackRotation();
            }
        }

        public bool TakeDamage()
        {
            if (invincible) return false;
            _animator.SetTrigger(_animIdtakeDamageEx1Ani);
            _audio.PlayOneShot(TakeDamageAudioClip, TakeDamageAudioVolume);
            return true;
        }

        public void ClearInputs()
        {
            input.attack = false;
            input.dash = false;
            input.heavyAttack = false;
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
            }
            else
            {
                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}