﻿using System.Collections.Generic;
using System.Linq;
using Transit.Addon.RoadExtensions.Roads.Common;
using Transit.Framework;
using Transit.Framework.Builders;
using Transit.Framework.Network;
using UnityEngine;

namespace Transit.Addon.RoadExtensions.Roads.SmallRoads.BasicRoadMdn
{
    public partial class BasicRoadMdnBuilder : Activable, IMultiNetInfoBuilderPart
    {
        public int Order { get { return 9; } }
        public string BasedPrefabName { get { return NetInfos.Vanilla.ROAD_2L; } }
        public string Name { get { return "BasicRoadMdn"; } }
        public string DisplayName { get { return "双车道+中央分隔带公路"; } }
        public string ShortDescription { get { return "No parking, zoneable, low traffic"; } }
        public NetInfoVersion SupportedVersions { get { return NetInfoVersion.AllWithDecoration; } }

        public IEnumerable<IMenuItemBuilder> MenuItemBuilders
        {
            get
            {
                yield return new MenuItemBuilder
                {
                    UICategory = "RoadsSmall",
                    UIOrder = 10,
                    Name = "BasicRoadMdn",
                    DisplayName = "双车道+中央分隔带公路",
                    Description = "一条基本的双车道道路，中间没有停车位。 可以承受低交通流量。",
                    ThumbnailsPath = @"Roads\SmallRoads\BasicRoadMdn\thumbnails.png",
                    InfoTooltipPath = @"Roads\SmallRoads\BasicRoadMdn\infotooltip.png"
                };
                yield return new MenuItemBuilder
                {
                    UICategory = "RoadsSmall",
                    UIOrder = 11,
                    Name = "BasicRoadMdn Decoration Grass",
                    DisplayName = "双车道+中央绿化分隔带公路",
                    Description = "一条基本的双车道道路，设有中央绿化分隔带，没有停车位。可以承受低交通流量。",
                    ThumbnailsPath = @"Roads\SmallRoads\BasicRoadMdn\thumbnails_grass.png",
                    InfoTooltipPath = @"Roads\SmallRoads\BasicRoadMdn\infotooltip_grass.png"
                };
                yield return new MenuItemBuilder
                {
                    UICategory = "RoadsSmall",
                    UIOrder = 12,
                    Name = "BasicRoadMdn Decoration Trees",
                    DisplayName = "双车道+中央行道树分隔带公路",
                    Description = "一条基本的双车道道路，设有中央行道树分隔带和无停车场。可以承受低交通流量。",
                    ThumbnailsPath = @"Roads\SmallRoads\BasicRoadMdn\thumbnails_trees.png",
                    InfoTooltipPath = @"Roads\SmallRoads\BasicRoadMdn\infotooltip_trees.png"
                };
            }
        }
        public void BuildUp(NetInfo info, NetInfoVersion version)
        {
            ///////////////////////////
            // Template              //
            ///////////////////////////
            if (version == NetInfoVersion.GroundGrass || version == NetInfoVersion.GroundTrees)
            {
                var roadInfo = Prefabs.Find<NetInfo>(NetInfos.Vanilla.ROAD_2L);
                info.m_segments = roadInfo.m_segments.Select(x => x.ShallowClone()).ToArray();
                info.m_nodes = roadInfo.m_nodes.Select(x => x.ShallowClone()).ToArray();
                info.m_lanes = roadInfo.m_lanes.Select(x => x.ShallowClone()).ToArray();
            }

            ///////////////////////////
            // 3DModeling            //
            ///////////////////////////
            info.Setup16m3mSW3mMdnMesh(version);

            ///////////////////////////
            // Texturing             //
            ///////////////////////////
            SetupTextures(info, version);

            ///////////////////////////
            // Set up                //
            ///////////////////////////
            info.m_hasParkingSpaces = false;

            info.m_pavementWidth = (version != NetInfoVersion.Slope && version != NetInfoVersion.Tunnel ? 3 : 6);
            info.m_halfWidth = (version != NetInfoVersion.Slope && version != NetInfoVersion.Tunnel ? 8 : 11);
            info.m_canCrossLanes = false;
            if (version == NetInfoVersion.Tunnel)
            {
                info.m_setVehicleFlags = Vehicle.Flags.Transition | Vehicle.Flags.Underground;
                info.m_setCitizenFlags = CitizenInstance.Flags.Transition | CitizenInstance.Flags.Underground;
                info.m_class = info.m_class.Clone("NEXTbasicroadmedian" + version.ToString());
            }
            else
            {
                info.m_class = info.m_class.Clone("NEXTbasicroadmedian" + version.ToString());
            }

            // Setting up lanes
            info.SetRoadLanes(version, new LanesConfiguration
            {
                IsTwoWay = true,
                LaneWidth = 3.3f,
                SpeedLimit = 0.8f,
                CenterLane = CenterLaneType.Median,
                CenterLaneWidth = 3.3f
            });
            var leftPedLane = info.GetLeftRoadShoulder();
            var rightPedLane = info.GetRightRoadShoulder();

            //Setting Up Props
            var leftRoadProps = leftPedLane.m_laneProps.m_props.ToList();
            var rightRoadProps = rightPedLane.m_laneProps.m_props.ToList();
            var centerLane = new NetInfo.Lane();
            centerLane.m_position = 0;
            centerLane.m_laneProps = ScriptableObject.CreateInstance<NetLaneProps>();
            centerLane.m_laneProps.name = "Center Lane Props";
            centerLane.m_vehicleType = VehicleInfo.VehicleType.None;
            var centerLaneProps = new List<NetLaneProps.Prop>();
            if (version == NetInfoVersion.GroundTrees)
            {
                var treeProp = new NetLaneProps.Prop()
                {
                    m_tree = Prefabs.Find<TreeInfo>("Tree2variant"),
                    m_repeatDistance = 20,
                    m_probability = 100,
                };
                treeProp.m_position.x = 0;
                centerLaneProps.Add(treeProp);
            }
            else if (version == NetInfoVersion.Slope)
            {
                leftRoadProps.AddLeftWallLights(info.m_pavementWidth);
                rightRoadProps.AddRightWallLights(info.m_pavementWidth);
            }
            centerLane.m_laneProps.m_props = centerLaneProps.ToArray();
            leftPedLane.m_laneProps.m_props = leftRoadProps.ToArray();
            rightPedLane.m_laneProps.m_props = rightRoadProps.ToArray();
            var lanes = info.m_lanes.ToList();
            lanes.Add(centerLane);
            info.m_lanes = lanes.ToArray();
            //info.TrimAboveGroundProps(version);
            //info.SetupNewSpeedLimitProps(50, 40);


            // AI
            //var owPlayerNetAI = roadInfo.GetComponent<PlayerNetAI>();
            //var playerNetAI = info.GetComponent<PlayerNetAI>();

            //if (owPlayerNetAI != null && playerNetAI != null)
            //{
            //    playerNetAI.m_constructionCost = owPlayerNetAI.m_constructionCost ; // Charge by the lane?
            //    playerNetAI.m_maintenanceCost = owPlayerNetAI.m_maintenanceCost; // Charge by the lane?
            //}

            var roadBaseAI = info.GetComponent<RoadBaseAI>();
            if (roadBaseAI != null)
            {
                roadBaseAI.m_trafficLights = false;
            }
        }
    }
}
