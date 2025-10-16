using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting 
{
    const int maxDirLightCount = 2;
    const string bufferName = "Lighting";

    static int dirLightCountId = Shader.PropertyToID("_DirectionalLightCount");
    static int dirLightDirectionId = Shader.PropertyToID("_DirectionalLightDirections");
    static int dirLightColorId = Shader.PropertyToID("_DirectionalLightColors");

    CommandBuffer buffer = new()
    {
        name = bufferName,
    };

    static Vector4[]
		dirLightColors = new Vector4[maxDirLightCount],
		dirLightDirections = new Vector4[maxDirLightCount];

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
        NativeArray<VisibleLight> visibleLights = result.visibleLights;
        
        int dirLightCount = 0;

        for (int i = 0; i < visibleLights.Length; i++) 
        {
            VisibleLight visibleLight = visibleLights[i];
            if (visibleLight.lightType == LightType.Directional) 
            {
                SetupDirectionalLight(dirLightCount++, ref visibleLight);    
                if (dirLightCount >= maxDirLightCount) 
                    break;
            }
        }

		buffer.SetGlobalInt(dirLightCountId, dirLightCount);
		buffer.SetGlobalVectorArray(dirLightColorId, dirLightColors);
		buffer.SetGlobalVectorArray(dirLightDirectionId, dirLightDirections);
    }

    void SetupDirectionalLight(int index, ref VisibleLight visibleLight)
    {
        dirLightColors[index] = visibleLight.finalColor;
		dirLightDirections[index] = -visibleLight.localToWorldMatrix.GetColumn(2);
    }
}
