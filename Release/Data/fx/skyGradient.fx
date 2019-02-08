uniform extern float4x4 g_WorldViewProjection;
//Used with regular skybox, just so we can swap between which shader to initialize with and not have to change up the params #lazy
uniform extern texture  g_SkyboxTex;
uniform extern float    g_Radius;
//RGBA
float4 apexColour = float4(0.0f, 0.15f, 0.66f, 1.0f);
float4 centerColour = float4(0.81f, 0.38f, 0.66f, 1.0f);


void SkyVS(float3 posL : POSITION0, 
           out float4 outPosHeight : POSITION0, 
           out float3 outSkyBoxTex : TEXCOORD0)
{
    outPosHeight = mul(float4(posL, 1.0f), g_WorldViewProjection).xyww; 
    outSkyBoxTex= posL;
}


float4 SkyPS(float3 skyBoxTex : TEXCOORD0) : COLOR
{
    float height;
    height = skyBoxTex.y / g_Radius;

    if(height < 0.0)
    {
      height = 0.0f;
    }
    return lerp(centerColour, apexColour, height);
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

