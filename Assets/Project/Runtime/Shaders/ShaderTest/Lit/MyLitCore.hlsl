// Contains URP library common functions
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

// This method calculates the offset vertex positions to create a wave effect
float3 CalculateWave(float3 positionOS, float waveSpeed, float waveFrequency, float waveAmplitude)
{
    float3 positionWS = GetVertexPositionInputs(positionOS).positionWS;
    
    float waveMovement = _Time.y * waveSpeed;
    
    float waveoffset = sin(waveMovement + waveFrequency * positionWS.x) + cos(waveMovement + waveFrequency * positionWS.z);

    float waveScaled = waveoffset * waveAmplitude;

    float steppedPosY = step(0.5, positionOS.y);

    float finalWavePositon = positionOS.y + waveScaled * steppedPosY;
    
    return float3(positionOS.x, finalWavePositon, positionOS.z);
}
