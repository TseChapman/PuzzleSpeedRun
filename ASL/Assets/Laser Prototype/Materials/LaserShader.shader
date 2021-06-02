Shader "Unlit/LaserShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Rotation("Rotation", Vector) = (1,0,0)
        _Anistropy("Anistropy", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Blend srcColor OneMinusSrcColor
        //Blend DstColor Zero
        //Cull front
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert alpha
            #pragma fragment frag alpha
            // make fog work
            // #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;

                UNITY_VERTEX_INPUT_INSTANCE_ID //Insert
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float3 viewDir : TEXCOORD2;
                //UNITY_FOG_COORDS(1)

                UNITY_VERTEX_OUTPUT_STEREO //Insert
            };

            float4 _Color;
            float3 _Rotation;
            float _Anistropy;

            v2f vert (appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.normal = mul(unity_ObjectToWorld, v.normal);
                o.tangent = mul(unity_ObjectToWorld, v.tangent);
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float random(float3 v) {
                return frac(sin(dot(v, float3(12.9898, 78.233, 47.9817))) * 43758.5453123);
            }

            float3 random3(float3 v) {
                float x = random(v);
                float y = random(v + x);
                float z = random(v + y);
                return float3(x,y,z);
            }

            float perlinNoise(float3 v, float scale) {
                v *= scale;
                float3 a = floor(v);
                float3 b = ceil(v);
                // corner positions;
                float3 v0 = float3(a.x, a.y, a.z);
                float3 v1 = float3(a.x, a.y, b.z);
                float3 v2 = float3(a.x, b.y, a.z);
                float3 v3 = float3(a.x, b.y, b.z);
                float3 v4 = float3(b.x, a.y, a.z);
                float3 v5 = float3(b.x, a.y, b.z);
                float3 v6 = float3(b.x, b.y, a.z);
                float3 v7 = float3(b.x, b.y, b.z);
                float3 d0 = v - v0;
                float3 d1 = v - v1;
                float3 d2 = v - v2;
                float3 d3 = v - v3;
                float3 d4 = v - v4;
                float3 d5 = v - v5;
                float3 d6 = v - v6;
                float3 d7 = v - v7;
                float r0 = dot(random3(v0), d0);
                float r1 = dot(random3(v1), d1);
                float r2 = dot(random3(v2), d2);
                float r3 = dot(random3(v3), d3);
                float r4 = dot(random3(v4), d4);
                float r5 = dot(random3(v5), d5);
                float r6 = dot(random3(v6), d6);
                float r7 = dot(random3(v7), d7);

                float x0 = lerp(r0, r4, d0.x);
                float x1 = lerp(r1, r5, d0.x);
                float x2 = lerp(r2, r6, d0.x);
                float x3 = lerp(r3, r7, d0.x);

                float y0 = lerp(x0, x2, d0.y);
                float y1 = lerp(x1, x3, d0.y);

                return lerp(y0, y1, d0.z);
                //return d0.z;
            }

            UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex); //Insert

            fixed4 frag(v2f i) : SV_Target
            {

                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert

                float t = _Time.x * 2;
                float t2 = 5 * sin(t);
                float sx = 15 + 5 * sin(t);
                float sy = 15 + 5 * sin(t + 3.141/3);
                float sz = 15 + 5 * sin(t + 2*3.141/3);
                float3 tv = float3(sin(t), cos(t), t);
                float nvx = perlinNoise(i.worldPos - tv, 5);
                float nvy = perlinNoise(i.worldPos - tv, 10);
                float nvz = perlinNoise(i.worldPos - tv, 15);
                float3 nv = float3(nvx, nvy, nvz);
                float n0 = perlinNoise(nv, 100);
                float n1 = perlinNoise(nv, 40);
                float n2 = perlinNoise(nv, 8);
                float n3 = perlinNoise(nv, 5);
                float n = 1 - abs(n0 + n1 + n2 + n3);

                float3 viewDir = mul(unity_CameraToWorld, float3(0, 0, 1));

                float r = n * 2 * abs(0.5 - i.uv.y);
                r *= 2;
                float centerWidth = 0.1;
                float centerWidth2 = 0.15;
                float r0 = max(0, (1 - r));
                float r1 = r0 * (1 + centerWidth);
                float r2 = r1 > 1 ? 1 : r1;
                float r3 = r1 > 1 ? (r1 - 1) / centerWidth : 0;
                float r4 = r3 * r3;
                // sample the texture
                float4 col = _Color * r0;
                col = fixed4(r2 * r2, r3 * r3, r4 * r4, 1);
                col *= lerp(1, n, 0.5);

                float a = acos(dot(_Rotation, i.viewDir)) / 3.141592653;
                a = a * _Anistropy;//abs(2 * a - 1)*_Anistropy;
                a = min(max(pow(a, 5), 0), 4);
                col *= 1 + a;
                col = min(max(col, 0), 1);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
