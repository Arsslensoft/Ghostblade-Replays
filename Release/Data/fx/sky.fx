uniform extern float4x4 g_WorldViewProjection;
uniform extern texture  g_SkyboxTex;
uniform extern float    g_Radius;

sampler g_SkyBoxSampler = sampler_state
{
    Texture   = <g_SkyboxTex>;
    MinFilter = LINEAR; 
    MagFilter = LINEAR;
    MipFilter = LINEAR;
    AddressU  = WRAP;
    AddressV  = WRAP;
};


void SkyVS(float3 posL : POSITION0, 
           out float4 outPosHeight : POSITION0, 
           out float3 outSkyBoxTex : TEXCOORD0)
{
    outPosHeight = mul(float4(posL, 1.0f), g_WorldViewProjection).xyww; 
    outSkyBoxTex= posL;
}

float4 SkyPS(float3 skyBoxTex : TEXCOORD0) : COLOR
{
    return texCUBE(g_SkyBoxSampler, skyBoxTex);
}

technique SkyBoxTech
{
    pass P0
    {
        vertexShader = compile vs_2_0 SkyVS();
        pixelShader  = compile ps_2_0 SkyPS();
		CullMode = None;
		ZFunc = Always; // Always write sky to depth buffer
		StencilEnable = true;
		StencilFunc   = Always;
		StencilPass   = Replace;
		StencilRef    = 0; // clear to zero
    }
}

