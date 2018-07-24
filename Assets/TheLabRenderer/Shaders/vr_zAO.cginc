//AO terms

#ifndef VR_AO_INCLUDED
#define VR_AO_INCLUDED

CBUFFER_START( ZVrAO )

	int		g_nNumAOSpheres;
	float4	g_zAOSphere[ 128 ]; // xyz = position , w = raius

	int		g_nNumAOPoints;
	float4	g_zAOPoint[ 128 ]; // xyz = position , w = raius

CBUFFER_END





	float CalculateSphericalAO(float3 posWs, float3 vNormalWs)
	{

	float OcclusionOuts = 1;

		 for ( int i = 0; i < g_nNumAOSpheres; i++ )
		{	

		if (distance(posWs, g_zAOSphere[i].xyz) >= g_zAOSphere[i].w * 5)
		{
		continue;
		}

		else{
		float3	LocalPosW = posWs - g_zAOSphere[i].xyz;
		float	ignoreBackfacing = saturate( 1 - dot( normalize(LocalPosW) , vNormalWs) ) ;
		float3	NormalizeRadius = LocalPosW / g_zAOSphere[i].w ;
		float	SphericalOcclusion = 1 / sqrt( pow (dot(NormalizeRadius,NormalizeRadius), 3) );
		float	OcclusionOutput =  saturate(1 - ((SphericalOcclusion) * ignoreBackfacing));
		OcclusionOuts *=  OcclusionOutput ;
		}
		} 
	 return saturate(OcclusionOuts);
	}



	float CalculatePointAO(float3 posWs, float3 vNormalWs)
	{

	float OcclusionOuts = 1;

		 for ( int i = 0; i < g_nNumAOPoints; i++ )
		{	
		float PointDistance = distance(posWs, g_zAOPoint[i].xyz);

		if (PointDistance >= g_zAOPoint[i].w)
		{
		continue;
		}

		float3	LocalPosW = posWs - g_zAOPoint[i].xyz;
		float	ignoreBackfacing =  1 - ((dot( normalize(LocalPosW) , vNormalWs) * 0.5) + 0.5 ) ;
		float	PointOcclusion = saturate(1 - distance(posWs, g_zAOPoint[i].xyz) / g_zAOPoint[i].w);
		float	OcclusionOutput = saturate( 1 - PointOcclusion * ignoreBackfacing);
		OcclusionOuts *= OcclusionOutput;
		}
	return saturate(OcclusionOuts);
	}

	float CalculateShapeAO(float3 posWs, float3 vNormalWs)
	{

		float AO = 1 * CalculateSphericalAO( posWs,  vNormalWs);
		AO *= CalculatePointAO( posWs,  vNormalWs);

		return AO;

	}



	#endif