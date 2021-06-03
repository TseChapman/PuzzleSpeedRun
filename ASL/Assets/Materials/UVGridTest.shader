Shader "Custom/UVGridTest"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _NormalTex("Normals", 2D) = "white" {}
        _RoughnessTex("Roughness", 2D) = "white" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Scale ("Scale", Vector) = (1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        

        sampler2D _MainTex;
        sampler2D _NormalTex;
        sampler2D _RoughnessTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 worldNormal; INTERNAL_DATA
        };

        half _Metallic;
        fixed4 _Color;
        float3 _Scale;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            //float cx = dot(IN.worldNormal, float3(1, 0, 0));
            //float cy = dot(IN.worldNormal, float3(0, 1, 0));
            //float3x3 axm = float3x3(1, 0, 0, 0, cx, -sin(acos(cx)), 0, sin(acos(cx)), cx);
            //float3x3 aym = float3x3(cy, 0, sin(acos(cy)), 0, 1, 0, -sin(acos(cy)), 0, cy);
            float3 worldNormal = WorldNormalVector(IN, float3(0, 0, 1));
            float nx = abs(worldNormal.x);
            float ny = abs(worldNormal.y);
            float nz = abs(worldNormal.z);
            float maxn = max(max(nx, ny), nz);
            float2 uv;
            float3 pos = IN.worldPos * _Scale;
            if (nx >= maxn) {
                uv = pos.zy;
            }
            else if (ny >= maxn) {
                uv = pos.xz;
            }
            else {
                uv = pos.xy;
            }

            //float3 t = mul(axm, IN.worldPos.xyz);
            //float3 t2 = mul(aym, t);
            //float2 uv = IN.worldPos.xy;// +IN.worldPos.xz + IN.worldPos.zy;
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, uv) * _Color;
            fixed3 normals = tex2D(_NormalTex, uv);
            fixed roughness = tex2D(_RoughnessTex, uv);
            o.Albedo = c.rgb;
            o.Normal = normals;
            o.Smoothness = 1 - roughness;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
