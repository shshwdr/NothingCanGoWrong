Shader "Hidden/ShutdownBlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Progress ("Shutdown Progress", Range(0,1)) = 0.0
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
            float _Progress;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // 关机渐暗
                col.rgb *= 1.0 - _Progress;

                // 垂直缩小
                float center = 0.5;
                float scanline = smoothstep(center - _Progress * 0.5, center + _Progress * 0.5, i.uv.y);
                col.rgb *= scanline;

                return col;
            }
            ENDCG
        }
    }
}
