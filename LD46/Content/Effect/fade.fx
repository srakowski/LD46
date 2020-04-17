#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

extern float Percent;

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 pixel = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy) * input.Color;
	
	//pixel += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(1, 1));
	//pixel += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(-1, -1));
	//pixel = pixel / 3;

	// pixel.r = sin(input.TextureCoordinates.x);

	return pixel * Percent;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};