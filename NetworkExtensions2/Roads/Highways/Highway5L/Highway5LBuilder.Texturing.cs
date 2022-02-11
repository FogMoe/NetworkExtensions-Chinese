﻿using Transit.Addon.RoadExtensions.Compatibility;
using Transit.Framework;
using Transit.Framework.Texturing;

namespace Transit.Addon.RoadExtensions.Roads.Highways.Highway5L
{
    public partial class Highway5LBuilder
    {
        private static void SetupTextures(NetInfo info, NetInfoVersion version)
        {
            var aprPathPrefix = (RoadColorChanger.IsPluginActive ? RoadColorChanger.GetTexturePrefix() : string.Empty);

            switch (version)
            {
                case NetInfoVersion.Ground:
                    info.SetAllSegmentsTexture(
                        new TextureSet(
                            @"Roads\Highways\Highway5L\Textures\Ground_Segment__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway5L\Textures\Ground_Segment__APRMap.png"),
                        new LODTextureSet
                           (@"Roads\Highways\Highway5L\Textures\Ground_SegmentLOD__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway5L\Textures\Ground_SegmentLOD__APRMap.png",
                            @"Roads\Highways\Highway5L\Textures\Ground_SegmentLOD__XYSMap.png"));
                    info.SetAllNodesTexture(
                        new TextureSet
                           (@"Roads\Highways\Highway5L\Textures\Ground_Node__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway5L\Textures\Ground_Node__APRMap.png"),
                        new LODTextureSet
                           (@"Roads\Highways\Highway5L\Textures\Ground_NodeLOD__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway5L\Textures\Ground_NodeLOD__APRMap.png",
                            @"Roads\Highways\Highway5L\Textures\Ground_NodeLOD__XYSMap.png"));
                    break;

                case NetInfoVersion.Elevated:
                case NetInfoVersion.Bridge:
                    info.SetAllSegmentsTexture(
                        new TextureSet(
                            @"Roads\Highways\Highway5L\Textures\Elevated_Segment__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway5L\Textures\Elevated_Segment__APRMap.png"),
                        new LODTextureSet
                           (@"Roads\Highways\Highway5L\Textures\Elevated_SegmentLOD__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway5L\Textures\Elevated_SegmentLOD__APRMap.png",
                            @"Roads\Highways\Highway5L\Textures\Elevated_LOD__XYSMap.png"));
                    info.SetAllNodesTexture(
                        new TextureSet
                           (@"Roads\Highways\Highway5L\Textures\Elevated_Node__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway5L\Textures\Elevated_Node__APRMap.png"),
                        new LODTextureSet
                           (@"Roads\Highways\Highway5L\Textures\Elevated_NodeLOD__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway5L\Textures\Elevated_NodeLOD__APRMap.png",
                            @"Roads\Highways\Highway5L\Textures\Elevated_LOD__XYSMap.png"));
                    break;

                case NetInfoVersion.Slope:
                    info.SetAllSegmentsTexture(
                        new TextureSet
                           (@"Roads\Highways\Highway5L\Textures\Slope_Segment__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway5L\Textures\Slope_Segment__APRMap.png"),
                        new LODTextureSet
                            (@"Roads\Highways\Highway5L\Textures\Slope_SegmentLOD__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway5L\Textures\Slope_SegmentLOD__APRMap.png",
                            @"Roads\Highways\Highway5L\Textures\Slope_SegmentLOD__XYSMap.png"));
                    info.SetAllNodesTexture(
                        new TextureSet
                           (@"Roads\Highways\Highway6L\Textures\Tunnel_Node__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway6L\Textures\Ground_Node__APRMap.png"),
                        new LODTextureSet
                           (@"Roads\Highways\Highway6L\Textures\Ground_NodeLOD__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway6L\Textures\Ground_NodeLOD__APRMap.png",
                            @"Roads\Highways\Highway6L\Textures\Ground_NodeLOD__XYSMap.png"));
                    break;
                case NetInfoVersion.Tunnel:
                    info.SetAllSegmentsTexture(
                        new TextureSet
                           (@"Roads\Highways\Highway5L\Textures\Tunnel_Segment__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway5L\Textures\Tunnel_Segment__APRMap.png"));
                    //new LODTextureSet
                    //   (@"Roads\Highways\Highway5L\Textures\Tunnel_SegmentLOD__MainTex.png",
                    //    @"Roads\Highways\Highway5L\Textures\Tunnel_SegmentLOD__APRMap.png",
                    //    @"Roads\Highways\Highway5L\Textures\Slope_NodeLOD__XYSMap.png"));
                    info.SetAllNodesTexture(
                        new TextureSet
                           (@"Roads\Highways\Highway5L\Textures\Tunnel_Node__MainTex.png",
                            aprPathPrefix + @"Roads\Highways\Highway5L\Textures\Tunnel_" + (RoadColorChanger.IsPluginActive ? "Node" : "Segment") + "__APRMap.png"));
                    //new LODTextureSet
                    //   (@"Roads\Highways\Highway5L\Textures\Tunnel_NodeLOD__MainTex.png",
                    //    @"Roads\Highways\Highway5L\Textures\Tunnel_SegmentLOD__APRMap.png",
                    //    @"Roads\Highways\Highway5L\Textures\Slope_NodeLOD__XYSMap.png"));
                    break;
            }
        }
    }
}