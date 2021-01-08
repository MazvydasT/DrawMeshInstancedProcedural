using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    [SerializeField, Range(10, 1000)]
    int resolution = 10;

    [SerializeField, Range(0.1f, 1f)]
    float prefabScale = 0.5f;

    [SerializeField]
    Mesh mesh;

    [SerializeField]
    Material material;

    [SerializeField]
    ComputeShader computeShader;

    Bounds bounds = new Bounds(Vector3.one * 0.5f, Vector3.one);
    ComputeBuffer computeBuffer;
    int groups;

    static readonly int positionsId = Shader.PropertyToID("_Positions");
    static readonly int resolutionId = Shader.PropertyToID("_Resolution");
    static readonly int scaleId = Shader.PropertyToID("_Scale");
    static readonly int timeId = Shader.PropertyToID("_Time");

    void OnDisable()
    {
        computeBuffer?.Release();
        computeBuffer = null;
    }

    // Update is called once per frame
    void Update()
    {
        var resolutionSquared = resolution * resolution;

        var computeBufferCount = computeBuffer?.count ?? 0;

        if(computeBufferCount != resolutionSquared)
        {
            computeBuffer?.Release();
            computeBuffer = new ComputeBuffer(resolutionSquared, 3 * 4);

            computeShader.SetBuffer(0, positionsId, computeBuffer);
            material.SetBuffer(positionsId, computeBuffer);

            groups = Mathf.CeilToInt(resolution / 8f);
        }

        var scale = 1f / resolution;

        var time = Time.time;

        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(scaleId, scale);
        computeShader.SetFloat(timeId, time);

        computeShader.Dispatch(0, groups, 1, groups);

        material.SetFloat(scaleId, scale * prefabScale);

        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, resolutionSquared);
    }
}
