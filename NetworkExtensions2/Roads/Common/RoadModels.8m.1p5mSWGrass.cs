﻿using Transit.Framework;

namespace Transit.Addon.RoadExtensions.Roads.Common
{
    public static partial class RoadModels
    {
        public static NetInfo Setup8m1p5mSWGrassMesh(this NetInfo info, NetInfoVersion version, LanesLayoutStyle laneStyle = LanesLayoutStyle.Symmetrical)
        {
            switch (version)
            {
                case NetInfoVersion.Ground:
                    {
                        var segments0 = info.m_segments[0].ShallowClone();
                        var nodes0 = info.m_nodes[0].ShallowClone();
                        var nodes1 = info.m_nodes[0].ShallowClone();

                        segments0
                            .SetFlagsDefault()
                            .SetMeshes
                            (@"Roads\Common\Meshes\8m\1p5mSWGrass\Ground.obj",
                             @"Roads\Common\Meshes\8m\1p5mSWGrass\Ground_LOD.obj");

                        nodes0
                            .SetFlags(NetNode.Flags.None, NetNode.Flags.Transition)
                            .SetMeshes
                            (@"Roads\Common\Meshes\8m\1p5mSWGrass\Ground_Node.obj",
                             @"Roads\Common\Meshes\8m\1p5mSWGrass\Ground_Node_LOD.obj");

                        nodes1
                            .SetFlags(NetNode.Flags.Transition, NetNode.Flags.None)
                            .SetMeshes
                            (@"Roads\Common\Meshes\8m\1p5mSWGrass\Ground_Trans.obj",
                             @"Roads\Common\Meshes\8m\1p5mSWGrass\Ground_Node_LOD.obj");
                        
                        RoadHelper.HandleAsymSegmentFlags(segments0);

                        info.m_segments = new[] { segments0 };
                        info.m_nodes = new[] { nodes0, nodes1 };
                        break;
                    }
            }
            return info;
        }
    }
}