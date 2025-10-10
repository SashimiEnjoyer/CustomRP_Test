using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;

public partial class CameraRender
{
    const string RenderCameraBufferName = "Render Camera";
    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
   
    ScriptableRenderContext context;
    Camera camera;
    CullingResults cullingResults;
    CommandBuffer buffer = new CommandBuffer { name = RenderCameraBufferName };

    // Draw all geometry that its camera can see
    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;
        
        PrepareBuffer();
        PrepareForSceneWindow();

        if(!Cull())
            return;
           
        Setup();
        DrawOpaqueGeometry();
        DrawSkybox();
        DrawTransparentGeometry();
        DrawUnsupportedShaders();

        DrawGizmosPost();
        
        Submit();
    }

    private void DrawOpaqueGeometry()
    {
        //// ---------- Draw Opaque Setup ----------
        var rendererListDesc = new RendererListDesc(
            unlitShaderTagId,
            cullingResults,
            camera)
        {

            sortingCriteria = SortingCriteria.CommonOpaque,
            renderQueueRange = RenderQueueRange.opaque,
            layerMask = camera.cullingMask
        };

        //// ---------- Create Renderer List ----------
        var rendererList = context.CreateRendererList(rendererListDesc);

        //// ---------- Ask Buffer to Draw  ----------
        buffer.DrawRendererList(rendererList);

    }

    private void DrawTransparentGeometry()
    {
        //// ---------- Draw Transparent Setup ----------
        var rendererListDesc = new RendererListDesc(
            unlitShaderTagId,
            cullingResults,
            camera)
        {

            sortingCriteria = SortingCriteria.CommonTransparent,
            renderQueueRange = RenderQueueRange.transparent,
            layerMask = camera.cullingMask
        };

        //// ---------- Create Renderer List ----------
        var rendererList = context.CreateRendererList(rendererListDesc);

        //// ---------- Ask Buffer to Draw  ----------
        buffer.DrawRendererList(rendererList);

    }
    
    private void DrawSkybox()
    {
        var skyboxRendererList = context.CreateSkyboxRendererList(camera);
        buffer.DrawRendererList(skyboxRendererList);
    }

    //Setup camera properties. So that we can render from the camera's point of view
    private void Setup()
    {
        context.SetupCameraProperties(camera);

        CameraClearFlags flags = camera.clearFlags;

        //Clear the render target
        buffer.ClearRenderTarget(
            flags <= CameraClearFlags.Depth,
            flags == CameraClearFlags.Color,
            flags == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear);

        //Sample for frame debugger
        buffer.BeginSample(SampleName);

    }

    //Submit the context to execute the commands in GPU
    private void Submit()
    {
        buffer.EndSample(SampleName);
        ExecuteBuffer();

        //Submit commands to GPU
        context.Submit();
    }

    //Execute the command buffer and then clear it
    private void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    private bool Cull()
    {
        if (camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            cullingResults = context.Cull(ref p);
            return true;
        }
        return false;
    }

}
