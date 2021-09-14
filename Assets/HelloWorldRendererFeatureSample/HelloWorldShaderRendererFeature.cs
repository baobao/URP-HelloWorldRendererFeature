using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HelloWorldShaderRendererFeature : ScriptableRendererFeature
{
    private HelloWorldShaderPass _pass;
    [SerializeField] private Shader _shader;
    public Color color;

    public override void Create()
    {
        var material = CoreUtils.CreateEngineMaterial(_shader);
        _pass = new HelloWorldShaderPass(material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer,
        ref RenderingData renderingData)
    {
        _pass.SetParam(renderer.cameraColorTarget, color);
        renderer.EnqueuePass(_pass);
    }
}