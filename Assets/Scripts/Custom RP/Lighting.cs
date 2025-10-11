using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting 
{
    static int dirLightDirectionId = Shader.PropertyToID("_MainLightDirection");
    static int dirLightColorId = Shader.PropertyToID("_MainLightColor");
    const string bufferName = "Lighting";

    CommandBuffer buffer = new()
    {
        name = bufferName,
    };

    CullingResults result;

    public void Setup(ScriptableRenderContext ctx, CullingResults res)
    {
        result = res;
        buffer.BeginSample(bufferName);
        SetupLights();
        buffer.EndSample(bufferName);
        ctx.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    void SetupLights()
    {
        SetupDirectionalLight(result.visibleLights);
    }

    void SetupDirectionalLight(NativeArray<VisibleLight> visibleLights)
    {
        VisibleLight mainLight = visibleLights[0];
        Vector3 lightDir = -mainLight.localToWorldMatrix.GetColumn(2);
        Color lightColor = mainLight.finalColor;

        buffer.SetGlobalVector(dirLightColorId, lightColor.linear);
        buffer.SetGlobalVector(dirLightDirectionId, new Vector4(lightDir.x, lightDir.y, lightDir.z, 0));
    }
}
