sampler SingleFrame;
sampler FrameStack;
float frameCount;


float4 PixelShaderFunction(float2 uv: TEXCOORD0, float4 c: COLOR0) : COLOR0
{
    //return float4(1.0f,1.0f,1.0f,1.0f);
    if(frameCount < 0.9f){
        return tex2D(SingleFrame, uv);
    }
    return tex2D(FrameStack, uv) * (1.0f-1.0f/(frameCount+1)) + tex2D(SingleFrame, uv) * (1.0f/(frameCount+1.0f));
}

technique Test
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}