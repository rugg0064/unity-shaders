Shader "Custom/voronoi"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float minDist = 999999999;
                float2 minPos;
                float4 col;

                float2 uvPos = float2(i.uv);
                uvPos = round(i.uv * 1000) / 1000;
                float2 currentPositionInSection = frac(uvPos * (_MainTex_TexelSize.z));
                
                for (int j = -2; j <= 2; j++)
                {
                    for (int k = -2; k <= 2; k++)
                    {
                        float2 offset = float2(k, j);
                        
                        float2 offsetTextureCoords = uvPos + (offset * _MainTex_TexelSize.xy);

                        float2 sectionVector = float2(tex2D(_MainTex, offsetTextureCoords).xy);

                        float dist = distance(offset + sectionVector, currentPositionInSection);
                        
                        if (dist < minDist)
                        {
                            minPos = offsetTextureCoords;
                            minDist = dist;
                        }
                    }
                }
                col = tex2D(_MainTex, minPos);
                return col;
            }
            ENDCG
        }
    }
}
