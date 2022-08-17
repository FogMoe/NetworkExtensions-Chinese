﻿using System.Linq;
using Transit.Addon.RoadExtensions.Props;
using Transit.Addon.RoadExtensions.Roads.Common;
using Transit.Framework;
using Transit.Framework.Builders;
using Transit.Framework.Network;

namespace Transit.Addon.RoadExtensions.Roads.Avenues.MediumAvenue4L
{
    public partial class MediumAvenue4LBuilder : Activable, INetInfoBuilderPart, INetInfoModifier
    {
        public int Order { get { return 20; } }
        public int UIOrder { get { return 4; } }

        public string BasedPrefabName { get { return NetInfos.Vanilla.ROAD_6L; } }
        public string Name { get { return "Medium Avenue"; } }
        public string DisplayName { get { return "四车道公路"; } }     
        public string Description { get { return "A four-lane road with parking spaces. Supports medium traffic."; } }
        public string ShortDescription { get { return "Parkings, zoneable, medium traffic"; } }
        public string UICategory { get { return "RoadsMedium"; } }

        public string ThumbnailsPath { get { return @"Roads\Avenues\MediumAvenue4L\thumbnails.png"; } }
        public string InfoTooltipPath { get { return @"Roads\Avenues\MediumAvenue4L\infotooltip.png"; } }

        public NetInfoVersion SupportedVersions
        {
            get { return NetInfoVersion.All; }
        }

        public void BuildUp(NetInfo info, NetInfoVersion version)
        {
            ///////////////////////////
            // Template              //
            ///////////////////////////
            var roadInfo = Prefabs.Find<NetInfo>(NetInfos.Vanilla.ROAD_6L);
            var roadTunnelInfo = Prefabs.Find<NetInfo>(NetInfos.Vanilla.ROAD_6L_TUNNEL);
            var bridgeInfo = Prefabs.Find<NetInfo>(NetInfos.Vanilla.ROAD_4L_BRIDGE).Clone("temp");

            ///////////////////////////
            // 3DModeling            //
            ///////////////////////////
            info.Setup32m5mSWMesh(version);

            ///////////////////////////
            // Texturing             //
            ///////////////////////////
            SetupTextures(info, version);

            ///////////////////////////
            // Set up                //
            ///////////////////////////
            info.m_hasParkingSpaces = true;
            info.m_pavementWidth = (version != NetInfoVersion.Slope && version != NetInfoVersion.Tunnel ? 5 : 7);
            info.m_halfWidth = (version == NetInfoVersion.Bridge || version == NetInfoVersion.Elevated ? 14 : 16);

            if (version == NetInfoVersion.Tunnel)
            {
                info.m_setVehicleFlags = Vehicle.Flags.Transition | Vehicle.Flags.Underground;
                info.m_setCitizenFlags = CitizenInstance.Flags.Transition | CitizenInstance.Flags.Underground;
                info.m_class = roadTunnelInfo.m_class.Clone(NetInfoClasses.NEXT_MEDIUM_ROAD_TUNNEL);
            }
            else if (version == NetInfoVersion.Bridge)
            {
                info.m_class = bridgeInfo.m_class.Clone(NetInfoClasses.NEXT_MEDIUM_ROAD);
            }
            else
            {
                info.m_class = roadInfo.m_class.Clone(NetInfoClasses.NEXT_MEDIUM_ROAD);
            }

            // Setting up lanes
            info.SetRoadLanes(version, new LanesConfiguration
            {
                IsTwoWay = true,
                LanesToAdd = -2,
                LaneWidth = 3.4f,
                BusStopOffset = 2.9f,
                CenterLane = CenterLaneType.Median,
                CenterLaneWidth = 4.4f
            });
            var leftPedLane = info.GetLeftRoadShoulder();
            var rightPedLane = info.GetRightRoadShoulder();

            // Fix for T++ legacy support
            if (version == NetInfoVersion.Ground)
            {
                var lanes = info.m_lanes.OrderBy(l => l.m_position).ToArray();
                var lanesLegacyOrder = new[]
                {
                    lanes[2],
                    lanes[3],
                    lanes[4],
                    lanes[5],
                    lanes[0],
                    lanes[7],
                    lanes[1],
                    lanes[6]
                };

                info.m_lanes = lanesLegacyOrder;
            }

            //Setting Up Props
            var leftRoadProps = leftPedLane.m_laneProps.m_props.ToList();
            var rightRoadProps = rightPedLane.m_laneProps.m_props.ToList();

            if (version != NetInfoVersion.Tunnel)
            {
                var leftStreetLight = leftRoadProps.FirstOrDefault(p => p.m_prop.name.ToLower().Contains("new street light"));
                if (leftStreetLight != null)
                {
                    leftStreetLight.m_finalProp =
                    leftStreetLight.m_prop = Prefabs.Find<PropInfo>(MediumAvenueSideLightBuilder.NAME);
                }

                var rightStreetLight = rightRoadProps.FirstOrDefault(p => p.m_prop.name.ToLower().Contains("new street light"));
                if (rightStreetLight != null)
                {
                    rightStreetLight.m_finalProp =
                    rightStreetLight.m_prop = Prefabs.Find<PropInfo>(MediumAvenueSideLightBuilder.NAME);
                }
            }

            if (version == NetInfoVersion.Slope)
            {
                leftRoadProps.AddLeftWallLights(info.m_pavementWidth);
                rightRoadProps.AddRightWallLights(info.m_pavementWidth);
            }

            leftPedLane.m_laneProps.m_props = leftRoadProps.ToArray();
            rightPedLane.m_laneProps.m_props = rightRoadProps.ToArray();

            info.TrimAboveGroundProps(version);
            info.SetupNewSpeedLimitProps(50, 60);


            // AI
            var owPlayerNetAI = roadInfo.GetComponent<PlayerNetAI>();
            var playerNetAI = info.GetComponent<PlayerNetAI>();

            if (owPlayerNetAI != null && playerNetAI != null)
            {
                playerNetAI.m_constructionCost = owPlayerNetAI.m_constructionCost * 2 / 3; // Charge by the lane?
                playerNetAI.m_maintenanceCost = owPlayerNetAI.m_maintenanceCost * 2 / 3; // Charge by the lane?
            }

            var roadBaseAI = info.GetComponent<RoadBaseAI>();

            if (roadBaseAI != null)
            {
                roadBaseAI.m_trafficLights = true;
            }
        }

        public void ModifyExistingNetInfo()
        {
            var avenue4L = Prefabs.Find<NetInfo>(NetInfos.Vanilla.ROAD_4L, false);
            if (avenue4L != null)
            {
                avenue4L.ModifyTitle("四车道公路+中央分隔带");
            }
        }
    }
}
