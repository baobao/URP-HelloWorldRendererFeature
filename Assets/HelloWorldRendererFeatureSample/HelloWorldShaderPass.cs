using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HelloWorldShaderPass : ScriptableRenderPass
{
    private const string CommandBufferName = nameof(HelloWorldShaderPass);
    private readonly int RenderTargetTexId = Shader.PropertyToID("_RenderTargetTex");
    
    private RenderTargetIdentifier _currentRenderTarget;
    private readonly Material _material;

    public HelloWorldShaderPass(Material material)
    {
        _material = material;
        renderPassEvent = RenderPassEvent.BeforeRenderingSkybox;
    }

    public void SetParam(RenderTargetIdentifier target, Color color)
    {
        _material.color = color;
        _currentRenderTarget = target;
    }

    public override void Execute(ScriptableRenderContext context, 
        ref RenderingData renderingData)
    {
        var commandBuffer = CommandBufferPool.Get(CommandBufferName);
        var cameraData = renderingData.cameraData;
        var w = cameraData.camera.scaledPixelWidth;
        var h = cameraData.camera.scaledPixelHeight;
        
        commandBuffer.GetTemporaryRT(RenderTargetTexId, w, h, 0, FilterMode.Bilinear);
        
        // ApplyShader
        commandBuffer.Blit(_currentRenderTarget, RenderTargetTexId, _material);
        
        // Back RenderTarget
        commandBuffer.Blit(RenderTargetTexId, _currentRenderTarget);
        commandBuffer.ReleaseTemporaryRT(RenderTargetTexId);
        
        context.ExecuteCommandBuffer(commandBuffer);
        context.Submit();
        
        
        CommandBufferPool.Release(commandBuffer);
    }
}
