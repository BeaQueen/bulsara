struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
#if defined(PSX_VERTEX_LIT)||defined(PSX_CUBEMAP)
	float3 normal : NORMAL;
#endif
};

struct v2f
{
	float4 vertex : POSITION;
	float4 affineUV1 : TEXCOORD0;

	UNITY_FOG_COORDS(1)

#ifdef PSX_VERTEX_LIT
		float4 color : COLOR0;
		float4 affineUV2 : TEXCOORD2;
#endif

#ifdef PSX_CUBEMAP
	float3 reflectionDir : TEXCOORD3;
#endif
};

struct fragOut
{
	half4 color : COLOR;
	float depth : DEPTH;
};

fixed4 _Color;
sampler2D _MainTex;
float4 _MainTex_ST;

#ifdef PSX_VERTEX_LIT
fixed4 _EmissionColor;
sampler2D _EmissiveTex;
float4 _EmissiveTex_ST;
fixed _PSX_LightFalloffPercent;
#endif

float invLerp(float from, float to, float value)
{
	return (value - from) / (to - from);
}

#if defined(PSX_VERTEX_LIT)

float3 ShadeUnityVertexLightsFull(float4 viewpos, float3 viewN, int lightCount, bool spotLight)
{
	float3 lightColor = UNITY_LIGHTMODEL_AMBIENT.xyz;
	for (int i = 0; i < lightCount; i++)
	{
		float3 toLight = unity_LightPosition[i].xyz - viewpos.xyz * unity_LightPosition[i].w;
		float lengthSq = dot(toLight, toLight);

		// don't produce NaNs if some vertex position overlaps with the light
		lengthSq = max(lengthSq, 0.000001);

		toLight *= rsqrt(lengthSq);

		float atten = 1.0 / (1.0 + lengthSq * unity_LightAtten[i].z);
		if (spotLight)
		{
			float rho = max(0, dot(toLight, unity_SpotDirection[i].xyz));
			float spotAtt = (rho - unity_LightAtten[i].x) * unity_LightAtten[i].y;
			atten *= saturate(spotAtt);
		}

		float diff = max(0, dot(viewN, toLight));
		lightColor += unity_LightColor[i].rgb * (diff * atten);
	}
	return lightColor;
}

float3 ShadePSXVertexLightsFull (float4 viewpos, float3 viewN, int lightCount, bool spotLight)
{
	fixed3 lightColor = UNITY_LIGHTMODEL_AMBIENT.xyz;

    for (int i = 0; i < lightCount; i++) {
		fixed3 toLight = unity_LightPosition[i].xyz - viewpos.xyz * unity_LightPosition[i].w;
        fixed lightDist = length(toLight);
		toLight = normalize(toLight);

        // don't produce NaNs if some vertex position overlaps with the light
        lightDist = max(lightDist, 0.000001);

		float lightRange = sqrt(unity_LightAtten[i].w);
		fixed atten = invLerp(lightRange, lightRange * _PSX_LightFalloffPercent, lightDist);

		float diff = max (0, dot (viewN, toLight));
        lightColor += unity_LightColor[i].rgb * saturate(atten) * 0.25 * lerp(1, diff, _PSX_LightingNormalFactor);
    }
    return lightColor;
}

float3 ShadePSXVertexLights (float4 vertex, float3 normal)
{
#ifdef PSX_ENABLE_CUSTOM_VERTEX_LIGHTING
	return ShadePSXVertexLightsFull(vertex, normal, 4, false);
#else
	return ShadeUnityVertexLightsFull(vertex, normal, 4, false);
#endif
    
}

#endif

v2f vert(appdata v)
{
	v2f o;

	float4x4 matrix_mv = UNITY_MATRIX_MV;
	float4x4 matrix_p = UNITY_MATRIX_P;

	o.vertex = mul(matrix_mv, v.vertex);

#ifdef PSX_VERTEX_LIT
	fixed3 viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
	o.color.rgb = ShadePSXVertexLights(o.vertex, viewNormal);
	o.color.a = 1;
#endif

	o.affineUV1 = CalculateAffineUV(o.vertex, TRANSFORM_TEX(v.uv, _MainTex));
#ifdef PSX_VERTEX_LIT
	o.affineUV2 = CalculateAffineUV(o.vertex, TRANSFORM_TEX(v.uv, _EmissiveTex));
#endif
	
#ifdef PSX_CUBEMAP
	float3 viewDir = mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos;
	float3 normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
	o.reflectionDir = reflect(viewDir, normalDir);
#endif

	o.vertex.xyz = SnapVertexToGrid(o.vertex.xyz);
	o.vertex = mul(matrix_p, o.vertex);

	UNITY_TRANSFER_FOG(o, o.vertex);

	return o;
}

fixed4 frag(v2f i) : COLOR
{
	float4 color = tex2D(_MainTex, i.affineUV1.xy / i.affineUV1.z) * _Color;
	
#ifdef PSX_VERTEX_LIT
	color *= i.color;
#endif

#ifdef PSX_CUTOUT_VAL
	clip(color.a - PSX_CUTOUT_VAL);
#endif

#ifdef PSX_VERTEX_LIT
	color.rgb += tex2D(_EmissiveTex, i.affineUV2.xy / i.affineUV2.z) * _EmissionColor.rgb * _EmissionColor.a;
#endif

#ifdef PSX_CUBEMAP
	color.rgb += texCUBE(PSX_CUBEMAP, i.reflectionDir) * PSX_CUBEMAP_COLOR.rgb * PSX_CUBEMAP_COLOR.a * tex2D(_ReflectionMap, i.affineUV1.xy / i.affineUV1.z);
#endif

	UNITY_APPLY_FOG(i.fogCoord, color);

	return color;
}
