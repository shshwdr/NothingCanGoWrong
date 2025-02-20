Shader "Hidden/GlitchEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlitchStrength ("Glitch Strength", Range(0,1)) = 0.3
        _TimeSpeed ("Time Speed", Range(0, 10)) = 1.0
        _RandomOffset ("Random Offset", Vector) = (0,0,0,0)
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _GlitchStrength;
            float _TimeSpeed;
            float4 _RandomOffset;  // 随机偏移

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // UV 扭曲 (Glitch 失真效果)
                float noise = sin(_Time.y * _TimeSpeed + v.uv.y * 100.0) * _GlitchStrength;
                o.uv.x += noise * _RandomOffset.x;  // 使用随机 X 偏移

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // 颜色偏移 (RGB 随机偏移)
                float shiftR = _RandomOffset.y * 0.1;
                float shiftB = _RandomOffset.z * 0.1;
                col.r = tex2D(_MainTex, i.uv + float2(shiftR, 0)).r;
                col.b = tex2D(_MainTex, i.uv - float2(shiftB, 0)).b;

                return col;
            }
            ENDCG
        }
    }
}
