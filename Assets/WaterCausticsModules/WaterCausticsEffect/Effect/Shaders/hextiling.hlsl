//
// Description : HexTilingFunction based on mmikk's hextile-demo
//    Original : mmikk
//     License : Copyright (c) 2022 mmikk
//               Distributed under the MIT License. See LICENSE file.
//               https://github.com/mmikk/hextile-demo/blob/main/LICENSE
//   Edited by : M.Hakozaki
//

#ifndef HEX_TILING_INCLUDED
#define HEX_TILING_INCLUDED

uint lowBias32(uint x) {
    // Best Integer Hash by FabriceNeyret2
    // https://www.shadertoy.com/view/WttXWX#
    x ^= x >> 16;
    x *= 0x7feb352dU;
    x ^= x >> 15;
    x *= 0x846ca68bU;
    x ^= x >> 16;
    return x;
}

float2 HEX_TILING_hash3_2(int2 v, uint hashSeed) {
    uint2 u = uint2(asuint(v));
    uint x0 = u.x + (u.y << 16) + hashSeed;
    uint x1 = (u.x << 16) + u.y + (hashSeed << 16);
    return float2(lowBias32(x0), lowBias32(x1)) / float(0xffffffffU);
}

float2 HEX_TILING_hash1_2(uint hashSeed) {
    return float2(hashSeed, lowBias32(hashSeed)) / float(0xffffffffU);
}

float2x2 HEX_TILING_getRot(float2 hash, float rot) {
    const float M_PI = 3.14159;
    float rad = (frac((hash.x + hash.y) * 65536) - 0.5) * M_PI * rot;
    float c = cos(rad);
    float s = sin(rad);
    return float2x2(c, -s, s, c);
}

void HEX_TILING_GetUV(float2 uv, float rot, uint seed, out float2 uv1, out float2 uv2, out float2 uv3, out float3 w) {
    const float TILE_SIZE = 1.3;
    const float HEX_RATE = sqrt(3) * 0.5; // 0.86602... 六角形の縦横比
    const float2x2 gridToSkewedGrid = float2x2(1 / HEX_RATE, 0, -0.5 / HEX_RATE, 1);
    uint hashSeed = lowBias32(seed);
    uv += HEX_TILING_hash1_2(hashSeed);
    float2 skewedCoord = mul(gridToSkewedGrid, uv * TILE_SIZE);
    int2 baseIdx = floor(skewedCoord);
    float3 temp = float3(frac(skewedCoord), 0);
    temp.z = 1 - temp.x - temp.y;
    float s = step(0, -temp.z);
    float s2 = 2 * s - 1;
    int2 idx1 = baseIdx + int2(s, s);
    int2 idx2 = baseIdx + int2(s, 1 - s);
    int2 idx3 = baseIdx + int2(1 - s, s);
    float2 hash1 = HEX_TILING_hash3_2(idx1, hashSeed);
    float2 hash2 = HEX_TILING_hash3_2(idx2, hashSeed);
    float2 hash3 = HEX_TILING_hash3_2(idx3, hashSeed);
    float2x2 rot1 = HEX_TILING_getRot(hash1, rot);
    float2x2 rot2 = HEX_TILING_getRot(hash2, rot);
    float2x2 rot3 = HEX_TILING_getRot(hash3, rot);
    uv1 = mul(uv, rot1) + hash1;
    uv2 = mul(uv, rot2) + hash2;
    uv3 = mul(uv, rot3) + hash3;
    w.x = -temp.z * s2;
    w.y = s - temp.y * s2;
    w.z = s - temp.x * s2;
    return;
}

float3 HEX_TILING_gain3(float3 w, float hardness) {
    float k = log(1 - hardness) / log(0.5);
    float3 s = 2 * step(0.5, w);
    float3 m = 2 * (1 - s);
    w = 0.5 * s + 0.25 * m * pow(max(0.0, s + w * m), k);
    return w.xyz / (w.x + w.y + w.z);
}

half3 HEX_TILING_MixColor(float hardness, half3 c1, half3 c2, half3 c3, float3 w) {
    half3 Lw = half3(0.299, 0.587, 0.114);
    half3 Dw = half3(dot(c1, Lw), dot(c2, Lw), dot(c3, Lw));
    Dw = lerp(1, Dw, 0.1);
    w = Dw * pow(w, 7); // 7
    w /= (w.x + w.y + w.z);
    w = HEX_TILING_gain3(w, hardness);
    return w.x * c1 + w.y * c2 + w.z * c3;
}

#endif




