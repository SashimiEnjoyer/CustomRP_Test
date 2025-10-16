using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
	CameraRender cameraRender = new CameraRender();
	[SerializeField] bool useSRPBatch = true;

	//Enable SRP Batching in Constructor
	public CustomRenderPipeline()
	{
		GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatch;
        GraphicsSettings.lightsUseLinearIntensity = true;
    }
	

    // This method is called by Unity to render the scene
    protected override void Render(ScriptableRenderContext renderContext, Camera[] cameras)
    {
        for (int i = 0; i < cameras.Length; i++) {
			cameraRender.Render(renderContext, cameras[i]);
		}
	}
    
}
