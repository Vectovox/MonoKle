sampler inputTexture;

Texture2D inverterTexture;
sampler inverterSampler = sampler_state {
    Texture = <inverterTexture >;
};

// Shader
float4 MainPS(float2 textureCoordinates: TEXCOORD0): COLOR0
{
	float4 color = tex2D(inputTexture, textureCoordinates);
	float4 inverterColor = tex2D(inverterSampler, textureCoordinates);
	if (inverterColor.x > 0 && inverterColor.y > 0 && inverterColor.z > 0) {
	    return 1 - color;
	}
	return color;
}

technique Techninque1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 MainPS();
	}
};