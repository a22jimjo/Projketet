Shader "Custom/FadeShader"
{
  Properties
  {
    _Color ("Color", Color) = (1,1,1,1)
    _Fade ("Fade", Range(0, 1)) = 0
  }
  
  SubShader
  {
    Pass
    {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      
      struct appdata
      {
        float4 vertex : POSITION;
      };
      
      struct v2f
      {
        float4 vertex : SV_POSITION;
      };
      
      float4 _Color;
      float _Fade;
      
      v2f vert(appdata v)
      {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        return o;
      }
      
      fixed4 frag(v2f i) : SV_Target
      {
        // multiply the color by the fade value to control transparency
        fixed4 color = _Color * _Fade;
        return color;
      }
      ENDCG
    }
  }
}
