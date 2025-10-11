using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;

public partial class CameraRender
{
    partial void DrawGizmosPost();
    partial void PrepareBuffer ();
    partial void DrawUnsupportedShaders();
    partial void PrepareForSceneWindow ();

#if UNITY_EDITOR

    static ShaderTagId[] legacyShaderTagIds = {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };
    static Material errorMaterial;

    string SampleName { get; set; }
    
    partial void PrepareBuffer()
    {
        Profiler.BeginSample("Editor Only");
        buffer.name = SampleName = camera.name;
        Profiler.EndSample();
    }
    
    partial void PrepareForSceneWindow () 
    {
		if (camera.cameraType == CameraType.SceneView) 
        {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
		}
	}

    partial void DrawGizmosPost () 
    {
        if (Handles.ShouldRenderGizmos())
        {
            context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        }
    }


    partial void DrawUnsupportedShaders()
    {
        if (errorMaterial == null)
        {
            errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }

        var rendererListDesc = new RendererListDesc(
            legacyShaderTagIds,
            cullingResults,
            camera)
        {

            sortingCriteria = SortingCriteria.CommonTransparent,
            renderQueueRange = RenderQueueRange.opaque,
            layerMask = camera.cullingMask,
            overrideMaterial = errorMaterial,
        };

        var rendererList = context.CreateRendererList(rendererListDesc);
        buffer.DrawRendererList(rendererList);
    }
    #else

    const string SampleName = bufferName;
    
#endif
}
