// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#ifndef WCECF_FOR_SHADER_GRAPH_INCLUDED
#define WCECF_FOR_SHADER_GRAPH_INCLUDED

#include "../../Effect/Shaders/WaterCausticsEffectCommon.hlsl"

// Custom Function (for ShaderGraph)
void WCECF_Emission_float(float3 WorldPos, half3 NormalWS, float2 ScreenUV, UnityTexture2D CausticsTex, float2 TexRotSinCos, int3 TexChannels, bool UseTiling, int TilingSeed, float TilingRot, float TilingHard, float Density, float SurfaceY, float SurfFadeStart, float SurfFadeCoef, float DepthFadeStart, float DepthFadeCoef, half MainLitIntensity, half AddLitIntensity, half ShadowIntensity, float2 ColorShift, half LitSaturation, half NormalAtten, half NormalAttenRate, half TransparentBack, half BacksideShadow, out half3 EmissionColor) {

    SurfFadeStart = max(SurfFadeStart, 0);
    TilingSeed = UseTiling ? max(TilingSeed, 0) : - 1;
    TilingHard = clamp(TilingHard, 0.75, 0.999);

    half3 e = WCE_EffectCore(WorldPos, NormalWS, ScreenUV, CausticsTex.tex, CausticsTex.samplerstate, TexRotSinCos, TexChannels, TilingSeed, TilingRot, TilingHard, Density, SurfaceY, SurfFadeStart, SurfFadeCoef, DepthFadeStart, DepthFadeCoef, MainLitIntensity, AddLitIntensity, ShadowIntensity, ColorShift, LitSaturation, NormalAtten, NormalAttenRate, TransparentBack, BacksideShadow);

    EmissionColor = e;
}

// Custom Function Through
void WCECF_EmissionThrough_float(float3 WorldPos, half3 NormalWS, float2 ScreenUV, UnityTexture2D CausticsTex, float2 TexRotSinCos, int3 TexChannels, int TilingSeed, float TilingRot, float TilingHard, float Density, float SurfaceY, float SurfFadeStart, float SurfFadeCoef, float DepthFadeStart, float DepthFadeCoef, half MainLitIntensity, half AddLitIntensity, half ShadowIntensity, float2 ColorShift, half LitSaturation, half NormalAtten, half NormalAttenRate, half TransparentBack, half BacksideShadow, out half3 EmissionColor) {

    half3 e = WCE_EffectCore(WorldPos, NormalWS, ScreenUV, CausticsTex.tex, CausticsTex.samplerstate, TexRotSinCos, TexChannels, TilingSeed, TilingRot, TilingHard, Density, SurfaceY, SurfFadeStart, SurfFadeCoef, DepthFadeStart, DepthFadeCoef, MainLitIntensity, AddLitIntensity, ShadowIntensity, ColorShift, LitSaturation, NormalAtten, NormalAttenRate, TransparentBack, BacksideShadow);

    EmissionColor = e;
}

#endif

