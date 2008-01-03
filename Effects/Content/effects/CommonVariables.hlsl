#define MAX_NUM_LIGHTS 4
#define MAX_NUM_BONES 59

/*******************************
 * Light properties
 *******************************/
float3 AmbientLightColor;
float3 LightColors[MAX_NUM_LIGHTS];
float3 LightPositions[MAX_NUM_LIGHTS];
float3 LightDirections[MAX_NUM_LIGHTS];

/*******************************
 * Material properties
 *******************************/
float3 AmbientColor;
float3 DiffuseColor;
float3 SpecularColor;
float Shininess;
float SpecularPower;
float ReflectiveFactor;

/*******************************
 * Transformation matrices
 *******************************/
float4x4 World[MAX_NUM_BONES];
float4x4 View;
float4x4 Projection;

/*******************************
 * Textures
 *******************************/
texture2D Texture;
texture2D NormalMap;
