Shader "Unlit/ShaderHeat"
{
       Properties
  {
    _MainTex("Texture", 2D) = "white" {}
      _ColorMain("Color Main",Color) = (0,0,0,1)


      _Diameter("Diameter",Range(0,1)) = 1.0
  }
    SubShader
    {
      Tags { "RenderType" = "Opaque" }
      LOD 100

      Pass
      {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
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
        float4 _MainTex_ST;



        float4 _ColorMain;
        float4 _ColorSelect;


        float _Diameter;

        v2f vert(appdata v)
        {
          v2f o;
          o.vertex = UnityObjectToClipPos(v.vertex);
          o.uv = TRANSFORM_TEX(v.uv, _MainTex);
          UNITY_TRANSFER_FOG(o,o.vertex);
          return o;
        }
        //----

        
        float _Hits[2 * 32]; //passed in array of pointranges 2floats/point, x,y, size
        int _HitCount = 0;


        float3 getHeatForPixel(float weight)
        {
           

          if (weight <= 0)
          {
            return _ColorMain;
          }
          else// (weight >= 1)
          {
            return _ColorSelect;
          }
          return _ColorSelect;

        }

        //Note: if distance is > 1.0, zero contribution, 1.0 is 1/2 of the 2x2 uv size
        float distsq(float2 a, float2 b, float index)
        {
          float area_of_effect_size = _Hits[index * 3 + 2];

          return  pow(max(0.0, 1.0 - distance(a, b) / area_of_effect_size), 2.0);
        }


        fixed4 frag(v2f i) : SV_Target
        {
          fixed4 col = tex2D(_MainTex, i.uv);


          float2 uv = i.uv;

          float totalWeight = 0.0;
          for (float i = 0.0; i < _HitCount; i++)
          {
            float2 work_pt = float2(_Hits[i * 3], _Hits[i * 3 + 1]);

            totalWeight += 0.5 * distsq(uv, work_pt, i);
          }
          return col + float4(getHeatForPixel(totalWeight), 0.5);
        }


        ENDCG
      }
    }
}

