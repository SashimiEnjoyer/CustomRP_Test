using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    CameraRender cameraRender = new CameraRender();

    // This method is called by Unity to render the scene
    protected override void Render(ScriptableRenderContext renderContext, Camera[] cameras) { }
    protected override void Render (
		ScriptableRenderContext context, List<Camera> cameras
	) {
		for (int i = 0; i < cameras.Count; i++) {
			cameraRender.Render(context, cameras[i]);
		}
	}
}
