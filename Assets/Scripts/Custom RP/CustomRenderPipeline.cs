using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
	CameraRender cameraRender = new CameraRender();

    // This method is called by Unity to render the scene
    protected override void Render(ScriptableRenderContext renderContext, Camera[] cameras)
    {
        for (int i = 0; i < cameras.Length; i++) {
			cameraRender.Render(renderContext, cameras[i]);
		}
	}
    
    // protected override void Render (ScriptableRenderContext context, List<Camera> cameras)
	// {
	// 	for (int i = 0; i < cameras.Count; i++) {
	// 		cameraRender.Render(context, cameras[i]);
	// 	}
	// }
}
