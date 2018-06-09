// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/PerlinNoise"
{
    Properties
    {
        // SpriteRenderer requires _MainTex property
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _HeightMap("HeightMap", 2D) = "white" {}
    }

    SubShader
    {
        // Passing the shader to transparent queue
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        // Change the LOD to 100
        LOD 100

        // Disable ZWrite and cross blend source alpha
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {

        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Defines the main color
            fixed4 _Color;

            // Defines the outline color
            fixed4 _OutlineColor;

            // Defines the outline color
            int _OutlinePixel;

            // Defines MainTexture texture
            sampler2D _MainTex;

            // Defines heightmap texture
            sampler2D _HeightMap;

            // Defines heigtmap cap
            int _HeightCap;

            struct a2v
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_HeightMap, i.uv.xy);

                // Any pixel lower than the height, use main color
                if (i.pos.y < col.r * _HeightCap)
                {
                    return _Color;
                }

                // Draw outline
                else if (i.pos.y < col.r * _HeightCap + _OutlinePixel)
                {
                    return _OutlineColor;
                }

                // Any other pixels are transparent
                else
                {
                    return fixed4(0,0,0,0);
                }
            }
        ENDCG
        }
    }

    FallBack "Diffuse"
}
