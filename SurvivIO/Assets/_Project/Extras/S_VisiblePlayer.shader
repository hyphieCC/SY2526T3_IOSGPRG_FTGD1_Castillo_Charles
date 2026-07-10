Shader "Unlit/S_VisiblePlayer"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _RevealRadius ("Reveal Radius", Float) = 1.5
        _RevealSoftness ("Reveal Softness", Float) = 0.75
        _MinimumAlpha ("Minimum Alpha", Range(0,1)) = 0.2

        [MaterialToggle] PixelSnap ("Pixel Snap", Float) = 0
    }
    SubShader
    {
        Tags 
        { 
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True" 
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma vertex SpriteVert
            #pragma fragment Fragment
            #pragma target 2.0
            #pragma multi_compile _ PIXELSNAP_ON

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;

            float4 _PlayerPosition;
            float _RevealRadius;
            float _RevealSoftness;
            float _MinimumAlpha;

            struct VertexInput
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct VertexOutput
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float3 worldPosition : TEXCOORD1;
            };

            VertexOutput SpriteVert(VertexInput input)
            {
                VertexOutput output;

                output.vertex = UnityObjectToClipPos(input.vertex);
                output.worldPosition =
                    mul(unity_ObjectToWorld, input.vertex).xyz;

                output.texcoord = input.texcoord;
                output.color = input.color * _Color;

                #ifdef PIXELSNAP_ON
                output.vertex = UnityPixelSnap(output.vertex);
                #endif

                return output;
            }

            fixed4 Fragment(VertexOutput input) : SV_Target
            {
                fixed4 spriteColor =
                    tex2D(_MainTex, input.texcoord) * input.color;

                float distanceFromPlayer = distance(
                    input.worldPosition.xy,
                    _PlayerPosition.xy
                );

                float revealFactor = smoothstep(
                    _RevealRadius,
                    _RevealRadius + _RevealSoftness,
                    distanceFromPlayer
                );

                float fadeAlpha = lerp(
                    _MinimumAlpha,
                    1.0,
                    revealFactor
                );

                spriteColor.a *= fadeAlpha;

                spriteColor.rgb *= spriteColor.a;

                return spriteColor;
            }

            ENDCG
        }
    }
}
