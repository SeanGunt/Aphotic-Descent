// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#ifndef WCECF_FOR_HLSL_INCLUDED
#define WCECF_FOR_HLSL_INCLUDED

#define REQUIRE_OPAQUE_TEXTURE
#include "../Common/WaterCausticsEmissionSync_Common.hlsl"

// --------------------------------------------
// Custom Function (for HLSL Scripting)
half3 WCE_WaterCausticsEmission(float3 WorldPos, half3 NormalWS, Texture2D CausticsTex, SamplerState CausticsTexSS, float2 TexRotSinCos = float2(0, 1), int3 TexChannels = int3(0, 1, 2), bool UseTiling = false, int TilingSeed = 0, float TilingRot = 0, float TilingHard = 0.85, float Density = 0.2, float SurfaceY = 2, float SurfFadeStart = 0, float SurfFadeEnd = 0.5, float DepthFadeStart = 0, float DepthFadeEnd = 10000, half MainLitIntensity = 1, half AddLitIntensity = 1, half ShadowIntensity = 1, float ColorShiftX = 0.4, float ColorShiftZ = -0.1, half LitSaturation = 0.2, half NormalAtten = 1, half NormalAttenRate = 2, half TransparentBack = 0, half BacksideShadow = 0) {

    float4 posSS = ComputeScreenPos(TransformWorldToHClip(WorldPos));
    float2 ScreenUV = posSS.xy / posSS.w;
    float SurfFadeCoef = 1 / max(SurfFadeEnd - SurfFadeStart, 0.000001);
    float DepthFadeCoef = 1 / max(DepthFadeEnd - DepthFadeStart, 0.000001);
    float2 ColorShift = float2(ColorShiftX, ColorShiftZ) * 0.01;
    SurfFadeStart = max(SurfFadeStart, 0);
    TilingSeed = UseTiling ? max(TilingSeed, 0) : - 1;
    TilingHard = clamp(TilingHard, 0.75, 0.999);

    half3 e = WCE_EffectCore(WorldPos, NormalWS, ScreenUV, CausticsTex, CausticsTexSS, TexRotSinCos, TexChannels, TilingSeed, TilingRot, TilingHard, Density, SurfaceY, SurfFadeStart, SurfFadeCoef, DepthFadeStart, DepthFadeCoef, MainLitIntensity, AddLitIntensity, ShadowIntensity, ColorShift, LitSaturation, NormalAtten, NormalAttenRate, TransparentBack, BacksideShadow);

    return e;
}

// Old 1.2.0
half3 WCE_WaterCausticsEmission(float3 WorldPos, half3 NormalWS, Texture2D CausticsTex, SamplerState CausticsTexSS, float Scale, float SurfaceY, float SurfFadeWidth, float SurfFadeOffset, float DepthFadeCoef, half MainLitIntensity, half AddLitIntensity, float ColorShiftX, float ColorShiftZ, half LitSaturation, half NormalAtten, half NormalAttenRate, half TransparentBack) {
    float Density = 1 / max(Scale, 0.0001);
    float DepthFadeEnd = 1 / max(DepthFadeCoef, 0.000001);
    // to Newest
    return WCE_WaterCausticsEmission(WorldPos, NormalWS, CausticsTex, CausticsTexSS, float2(0, 1), int3(0, 1, 2), false, 0, 0, 0.85, Density, SurfaceY, SurfFadeOffset, SurfFadeWidth, 0, DepthFadeEnd, MainLitIntensity, AddLitIntensity, 1, ColorShiftX, ColorShiftZ, LitSaturation, NormalAtten, NormalAttenRate, TransparentBack, 0);
}

// Old 1.1.4
half3 WCE_WaterCausticsEmission(float3 WorldPos, half3 NormalWS, Texture2D CausticsTex, SamplerState CausticsTexSS, float Scale, float SurfaceY, float SurfFadeWidth, float SurfFadeOffset, half MainLitIntensity, half AddLitIntensity, float ColorShiftX, float ColorShiftZ, half LitSaturation, half NormalAtten, half NormalAttenRate, half TransparentBack) {

    // to 1.2.0
    return WCE_WaterCausticsEmission(WorldPos, NormalWS, CausticsTex, CausticsTexSS, Scale, SurfaceY, SurfFadeWidth, SurfFadeOffset, 0, MainLitIntensity, AddLitIntensity, ColorShiftX, ColorShiftZ, LitSaturation, NormalAtten, NormalAttenRate, TransparentBack);
}

// --------------------------------------------
// Custom Function  Sync with effect script on the scene.  (for HLSL Scripting)
half3 WCE_WaterCausticsEmissionSync(float3 WorldPos, half3 NormalWS, half3 BaseColor = half3(1, 1, 1)) {
    float4 posSS = ComputeScreenPos(TransformWorldToHClip(WorldPos));
    float2 ScreenUV = posSS.xy / posSS.w;
    return WCECF_SyncCommon(WorldPos, NormalWS, ScreenUV, BaseColor);
}

// --------------------------------------------
#endif

