﻿using System.Linq;
using Transit.Addon.RoadExtensions.Menus;
using Transit.Addon.RoadExtensions.Menus.Roads;
using Transit.Addon.RoadExtensions.Roads.Common;
using Transit.Framework;
using Transit.Framework.Builders;
using Transit.Framework.Network;

namespace Transit.Addon.RoadExtensions.Roads.SmallHeavyRoads.BasicRoadTL
{
    public partial class BasicRoadTLBuilder : Activable, INetInfoBuilderPart
    {
        public int Order { get { return 7; } }
        public int UIOrder { get { return 9; } }

        public string BasedPrefabName { get { return NetInfos.Vanilla.ROAD_2L; } }
        public string Name { get { return "BasicRoadTL"; } }
        public string DisplayName { get { return "Basic Road with Turning Lane"; } }
        public string Description { get { return "A basic two lane road with an additional center turning lane and no parkings spaces. Supports medium and local traffic. Note: The turning lane goes in both direction so collisions may occur!"; } }
        public string ShortDescription { get { return "No parking, zoneable, low traffic"; } }
        public string UICategory { get { return RExExtendedMenus.ROADS_SMALL_HV; } }

        public string ThumbnailsPath { get { return @"Roads\SmallHeavyRoads\BasicRoadTL\thumbnails.png"; } }
        public string InfoTooltipPath { get { return @"Roads\SmallHeavyRoads\BasicRoadTL\infotooltip.png"; } }

        public NetInfoVersion SupportedVersions
        {
            get { return NetInfoVersion.All; }
        }

        public void BuildUp(NetInfo info, NetInfoVersion version)
        {
            ///////////////////////////
            // Template              //
            ///////////////////////////
            var roadInfo = Prefabs.Find<NetInfo>(NetInfos.Vanilla.ROAD_2L);
            var owRoadTunnelInfo = Prefabs.Find<NetInfo>(NetInfos.Vanilla.ONEWAY_2L_TUNNEL);

            ///////////////////////////
            // 3DModeling            //
            ///////////////////////////
            info.Setup16m3mSWMesh(version);

            ///////////////////////////
            // Texturing             //
            ///////////////////////////
            SetupTextures(info, version);
            
            ///////////////////////////
            // Set up                //
            ///////////////////////////
            info.m_hasParkingSpaces = false;
            info.m_class = roadInfo.m_class.Clone(NetInfoClasses.NEXT_SMALL3L_ROAD);
            info.m_pavementWidth = (version != NetInfoVersion.Slope && version != NetInfoVersion.Tunnel ? 3 : 6);
            info.m_halfWidth = (version != NetInfoVersion.Slope && version != NetInfoVersion.Tunnel ? 8 : 11);

            if (version == NetInfoVersion.Tunnel)
            {
                info.m_setVehicleFlags = Vehicle.Flags.Transition | Vehicle.Flags.Underground;
                info.m_setCitizenFlags = CitizenInstance.Flags.Transition | CitizenInstance.Flags.Underground;
                info.m_class = owRoadTunnelInfo.m_class.Clone(NetInfoClasses.NEXT_SMALL3L_ROAD_TUNNEL);
            }
            else
            {
                info.m_class = roadInfo.m_class.Clone(NetInfoClasses.NEXT_SMALL3L_ROAD);
            }

            // Setting up lanes
            info.SetRoadLanes(version, new LanesConfiguration
            {
                IsTwoWay = true,
                LanesToAdd = 2,
                LaneWidth = 3.3f,
                SpeedLimit = 1.2f,
                CenterLane = CenterLaneType.TurningLane
            });
            var leftPedLane = info.GetLeftRoadShoulder();
            var rightPedLane = info.GetRightRoadShoulder();

            //Setting Up Props
            var leftRoadProps = leftPedLane.m_laneProps.m_props.ToList();
            var rightRoadProps = rightPedLane.m_laneProps.m_props.ToList();

            if (version == NetInfoVersion.Slope)
            {
                leftRoadProps.AddLeftWallLights(info.m_pavementWidth);
                rightRoadProps.AddRightWallLights(info.m_pavementWidth);
            }
            
            leftPedLane.m_laneProps.m_props = leftRoadProps.ToArray();
            rightPedLane.m_laneProps.m_props = rightRoadProps.ToArray();

            info.TrimAboveGroundProps(version);
            info.SetupNewSpeedLimitProps(50, 40);

            
            // AI
            var owPlayerNetAI = roadInfo.GetComponent<PlayerNetAI>();
            var playerNetAI = info.GetComponent<PlayerNetAI>();

            if (owPlayerNetAI != null && playerNetAI != null)
            {
                playerNetAI.m_constructionCost = owPlayerNetAI.m_constructionCost * 3 / 2; // Charge by the lane?
                playerNetAI.m_maintenanceCost = owPlayerNetAI.m_maintenanceCost * 3 / 2; // Charge by the lane?
            }

            var roadBaseAI = info.GetComponent<RoadBaseAI>();
            if (roadBaseAI != null)
            {
                roadBaseAI.m_trafficLights = false;
            }
        }
    }
}
