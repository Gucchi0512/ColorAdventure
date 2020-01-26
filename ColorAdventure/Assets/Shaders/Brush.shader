// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Brush"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Brush ("Brush", 2D) = "white" {}
        _BrushScale ("BrushScale", float) = 0.0
        _BrushColor ("BrushColor", Color) = (1,1,1,1)
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
            sampler2D _Brush;
            float4 _PaintUV;
            float _BrushScale;
            float4 _BrushColor;
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
                float h = _BrushScale;
                if(_PaintUV.x - h < i.uv.x && i.uv.x < _PaintUV.x + h && _PaintUV.y - h < i.uv.y && i.uv.y < _PaintUV.y + h){
                    float4 col = tex2D(_Brush, (_PaintUV.xy-i.uv)/h*0.5+0.5);
                    if(col.a-1>=0){
                        return _BrushColor;
                    }
                }
                return tex2D(_MainTex, i.uv);
            }
        ENDCG
        }

    }
    FallBack "Diffuse"
}
