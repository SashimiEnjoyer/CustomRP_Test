using UnityEngine;
using UnityEngine.Rendering;

public class CameraRender
{
    const string RenderCameraBufferName = "Render Camera";

    ScriptableRenderContext context;
    Camera camera;

    CommandBuffer buffer = new CommandBuffer { name = RenderCameraBufferName };

    // Draw all geometry that its camera can see
    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        Setup();
        DrawVisibleGeometry();
        Submit();
    }

    private void DrawVisibleGeometry()
    {
        //Create skybox renderer list and draw it
        var skyboxRendererList = context.CreateSkyboxRendererList(camera);

        buffer.DrawRendererList(skyboxRendererList);
    }

    //Setup camera properties. So that we can render from the camera's point of view
    private void Setup()
    {
        context.SetupCameraProperties(camera);

        //Clear the render target
        buffer.ClearRenderTarget(true, true, Color.clear);

        //Sample for frame debugger and profiling
        buffer.BeginSample(RenderCameraBufferName);

        
    }

    //Submit the context to execute the commands in GPU
    private void Submit()
    {
        buffer.EndSample(RenderCameraBufferName);
        ExecuteBuffer();

        context.Submit();
    }

    //Execute the command buffer and then clear it
    void ExecuteBuffer () {
		context.ExecuteCommandBuffer(buffer);
		buffer.Clear();
	}

}
