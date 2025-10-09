using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline Asset")]
public class CustomRenderPipelineAsset : RenderPipelineAsset<CustomRenderPipeline>
{
    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline();
    }
}
