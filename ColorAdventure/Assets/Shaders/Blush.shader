// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Blush"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Blush ("Blush", 2D) = "white" {}
        _BlushScale ("BlashScale", float) = 0.0
        _BlushColor ("BlushColor", Color) = (1,1,1,1)
        _PaintUV("Hit Uv Pos", Vector) = (0,0,0,0)
    }
    SubShader
    {
        CGINCLUDE

            struct app_data {
                float4 vertex:POSITION;
                float4 uv:TEXCOORD0;
            };

            struct v2f {
                float4 screen:SV_POSITION;
                float4 uv:TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _Blush;
            float4 _PaintUV;
            float _BlushScale;
            float4 _BlushColor;
        ENDCG
        
        Pass{
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            v2f vert(app_data i){
                v2f o;
                o.screen = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
                return o;
            }
            
            float4 frag(v2f i):SV_TARGET{
                float h = _BlushScale;
                if(_PaintUV.x - h < i.uv.x && i.uv.x < _PaintUV.x + h && _PaintUV.y - h < i.uv.y && i.uv.y < _PaintUV.y + h){
                    float4 col = tex2D(_Blush, (_PaintUV.xy-i.uv)/h*0.5+0.5);
                    if(col.a-1>=0){
                        return _BlushColor;
                    }
                }
                return tex2D(_MainTex, i.uv);
            }
        ENDCG
        }

    }
    FallBack "Diffuse"
}
