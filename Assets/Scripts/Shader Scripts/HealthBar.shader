Shader "Unlit/HealthBar"
{
    Properties
    {
        [NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
        _Pixels ("Resolution", int) = 512
        _PixelWidth ("Pixel Width", float) = 16
        _PixelHeight ("Pixel Height", float) = 16
        _Health ("Health", Range (0, 1)) = 1
        _BorderSize ("Border Size", Range (0, 0.5)) = 0.1
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue" = "Transparent"
        }

        Pass
        {
            ZWrite Off //We do not want this to write to the depth buffer.
            Blend SrcAlpha OneMinusSrcAlpha //Alpha Blending = src * srcAlpha + dist * (1 - srcAlpha) | (original formula = src * X +/- dist * Y)
                                            //src is the color we want to be transparent, and dist is the background.
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Health;
            float _BorderSize;
            float _Pixels;
            float _PixelWidth;
            float _PixelHeight;
            float _dX;
            float _dY;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 InverseLerp (float a, float b, float v)//at 0.2 = 0, at 0.8 = 1
            {
                return(v-a)/(b-a);
            }
            float4 frag (Interpolators i) : SV_Target 
            {
                //Make the shader Render in Pixels.
                _dX = _PixelWidth * (1 / _Pixels);
                _dY = _PixelHeight * (1 / _Pixels);
                float2 coord = float2(_dX * float(i.uv.x / _dX), _dY * float(i.uv.y / _dY));

                //Make circular edges and add border.
                coord.x *= 8;
                float2 pointOnLineSeg = float2(clamp(coord.x, 0.5, 7.5), 0.5);
                float sdf = distance(coord, pointOnLineSeg) * 2 - 1;
                clip(-sdf);
                float borderSdf = sdf + _BorderSize;
                float pd = fwidth(borderSdf);
                float borderMask = 1 - saturate(borderSdf / pd);

                //Make the healthbar mask adjust based on the comparison of uv.x and Health value.
                float healthBarMask = _Health > i.uv.x;
                float3 healthBarColor = tex2D(_MainTex, float2 (_Health, coord.y)); //Blend this color based on how much health you have.

                //Make the healthbar flash when the health is in 20%
                if(_Health < 0.2)
                {
                    float flash = cos (_Time.y * 4) * 0.6 + 1;
                    healthBarColor *= flash;
                }

                return float4 (healthBarColor * healthBarMask * borderMask, 1);
            }
            ENDCG
        }
    }
}
