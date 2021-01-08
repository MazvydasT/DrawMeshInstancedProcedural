#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
    StructuredBuffer<float3> _Positions;
#endif

float _Scale;

void SetObjectTransformationMatrix()
{
#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
    float3 position = _Positions[unity_InstanceID];
    
    unity_ObjectToWorld = 0.0;
    unity_ObjectToWorld._m03_m13_m23_m33 = float4(position, 1.0);
    unity_ObjectToWorld._m00_m11_m22 = _Scale;
#endif
}

void Dummy_float(float3 In, out float3 Out)
{
    Out = In;
}

void Dummy_half(float3 In, out float3 Out)
{
    Out = In;
}