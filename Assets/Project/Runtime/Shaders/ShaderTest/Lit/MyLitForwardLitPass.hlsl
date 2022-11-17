// Pull in URP library functions and our own common functions
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

// This file contains the vertex and fragment functions for the forward lit pass
// This is the shader pass that computes visible colours for a material
// by reading material, light, shadow, etc. data
TEXTURE2D(_ColourMap); SAMPLER(sampler_ColourMap);
float4 _ColourMap_ST; // Automatically set by Unity. Used in TRANSFORM_TEX to apply UV tiling
float4 _ColourTint;
float _Smoothness;

// Wave parameters
float _WaveAmplitude;
float _WaveFrequency;
float _WaveSpeed;

// This attributes struct receives data about the mesh we're currently rendering
// Data automatically populates fields according to their semantic
struct Attributes
{
    float3 positionOS : POSITION; // Position in object space
    float3 normalOS : NORMAL; // Normal in object space
    float2 uv : TEXCOORD0; // Material texture UVs
};

// This struct is output by the vertex function and input to the fragment function
// Note that fields will be transformed by the intermediary rasterization stage
struct Interpolators
{
    // This value should contain the position in clip space when output from the
    // vertex function. It will be transformed into the pixel position of the
    // current fragment on the screen when read from the fragment function
    float4 positionCS : SV_POSITION;

    // The following variables will retain their values from the vertex stage, except the
    // rasterizer will interpolate them between vertices
    // Two fields should not have the same semantic, the rasterizer can handle many TEXCOORD variables
    float2 uv : TEXCOORD0; // Material texture UVs
    float3 normalWS : TEXCOORD1; // Normal in world space
    float3 positionWS : TEXCOORD2;
};

float3 CalculateWave(float3 positionOS)
{
    float3 positionWS = GetVertexPositionInputs(positionOS).positionWS;
    
    float waveMovement = _Time.y * _WaveSpeed;
    
    float waveoffset = sin(waveMovement + _WaveFrequency * positionWS.x) + cos(waveMovement + _WaveFrequency * positionWS.z);

    float waveScaled = waveoffset * _WaveAmplitude;

    float steppedPosY = step(0.5, positionOS.y);

    float finalWavePositon = positionOS.y + waveScaled * steppedPosY;
    
    return float3(positionOS.x, finalWavePositon, positionOS.z);
}

// The vertex function which runs for each vertex on the mesh.
// It must output the position on the screen, where each vertex should appear,
// as well as any data the fragment function will need
Interpolators Vertex(Attributes input)
{
    Interpolators output;
    
    // These helper functions, found in URP/ShaderLib/ShaderVariablesFunctions.hlsl
    // transform object space values into world and clip space
    // CalulateWaves is a custom function that calculates the wave movement
    float3 waveVertexPosition = CalculateWave(input.positionOS);
    VertexPositionInputs posnInputs = GetVertexPositionInputs(waveVertexPosition);
    VertexNormalInputs normInputs = GetVertexNormalInputs(input.normalOS);
    

    // Pass position, orientation and normal data to the fragment function
    output.positionCS = posnInputs.positionCS;
    output.uv = TRANSFORM_TEX(input.uv, _ColourMap);
    output.normalWS = normInputs.normalWS;
    output.positionWS = posnInputs.positionWS;
    
    return output;
}

// The fragment function runs once per fragment, akin to a pixel on the screen but virtualized
// It must output the final colour of this pixel hence the function is a float4
// The function is tagged with a semantic so that the return value is interpreted in a specific way
float4 Fragment(Interpolators input) : SV_TARGET
{
    float2 uv = input.uv; 
    
    // Sample the colour map
    float4 colourSample = SAMPLE_TEXTURE2D(_ColourMap, sampler_ColourMap, uv);

    // This holds information about the position and orientation of the mesh at the current fragment
    InputData lightingInput = (InputData)0;
    // This holds information about the surface material’s physical properties, like colour
    SurfaceData surfaceInput = (SurfaceData)0;
    // Unlike C#, structure fields must be manually initialized. To set all fields to zero, cast zero to the
    // structure type. This looks strange, but it’s an easy way to initialize a structure without having to
    // know all its fields.

    // Populates the fields in the input structs
    surfaceInput.albedo = colourSample.rgb * _ColourTint.rgb;
    surfaceInput.alpha = colourSample.a * _ColourTint.a;
    surfaceInput.specular = 1; // Set Highlights to white
    surfaceInput.smoothness = _Smoothness;
    lightingInput.positionWS = input.positionWS;
    lightingInput.viewDirectionWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
    lightingInput.shadowCoord = TransformWorldToShadowCoord(input.positionWS);
    // When the normal is rasterized due to the semantic in the Interpolators struct, it's interpolated between
    // values, and for lighting to look its best all normal vectors must have a length of one.
    // Hence the Normalize method, which can be slow, since it has an expensive square root calculation
    // but I think this step is worth the performance cost for smoother lighting. (especially noticeable on specular highlights)
    lightingInput.normalWS = normalize(input.normalWS);
    
    // Computes a standard lighting algorithm called the Blinn-Phong lighting model
    float4 finalColour = UniversalFragmentBlinnPhong(lightingInput, surfaceInput);

    // Apply Ambient Lighting
    float3 ambientColour = float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
    finalColour.rgb += ambientColour * 2 * colourSample;
    
    return finalColour;
}
