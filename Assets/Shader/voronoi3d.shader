Shader "Custom/voronoi3d"
{
    Properties
    {
        _MainTex ("Texture", 3D) = "white" {}
        Resolution ("Texture", 3D) = "white" {}
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

            sampler3D _MainTex;
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

            fixed4 frag(v2f i) : SV_Target
            {
                float minDist = 5;
                float3 minPos = float3(0,0,0);
                float4 col = float4(0,0,0,0);
                float timeScaler = 1.0 / 10.0;

                float3 uvPos = float3(i.uv, _Time.y * timeScaler);
                uvPos = round(uvPos * 1000) / 1000;
                float3 currentPositionInSection = frac(uvPos * _MainTex_TexelSize.z);
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            float3 offset = float3(k, j, l);

                            float3 offsetTextureCoords = uvPos + (offset / _MainTex_TexelSize.zzz);

                            float3 sectionVector = float3(tex3D(_MainTex, offsetTextureCoords).xyz);

                            float dist = distance(offset + sectionVector, currentPositionInSection);

                            if (dist < minDist)
                            {
                                minPos = offsetTextureCoords;
                                minDist = dist;
                            }
                        }
                    }
                }
                col = tex3D(_MainTex, minPos);
                //return float4(pow(minDist.xxxx - 0.3,2)) * float4(1.5,0.1,0.1,0);
                return (pow(minDist,4)-0.05) * float4(0.8,0.1,0.1,0.1);
                //return col * minDist;
                //return col;
                
            }
            ENDCG
        }
    }
}
