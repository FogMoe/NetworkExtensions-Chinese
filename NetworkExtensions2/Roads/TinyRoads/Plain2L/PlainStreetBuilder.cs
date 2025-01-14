﻿using Transit.Addon.RoadExtensions.Menus.Roads;
using Transit.Framework;
using Transit.Framework.Builders;
using Transit.Framework.Network;

namespace Transit.Addon.RoadExtensions.Roads.TinyRoads.Plain2L
{
    public partial class PlainStreetBuilder : Activable, INetInfoBuilderPart
    {
        public int Order { get { return 2; } }
        public int UIOrder { get { return 1; } }

        public string BasedPrefabName { get { return NetInfos.Vanilla.ROAD_2L_GRAVEL; } }
        public string Name { get { return "PlainStreet2L"; } }
        public string DisplayName { get { return "双车道大街"; } }
        public string Description { get { return "一条只能是平的，有路边停车带，没有人行道的双车道街道。可承受低交通流量。"; } }
        public string ShortDescription { get { return "Parking, zoneable, low traffic"; } }
        public string UICategory { get { return RExExtendedMenus.ROADS_TINY; } }

        public string ThumbnailsPath { get { return @"Roads\TinyRoads\Plain2L\thumbnails.png"; } }
        public string InfoTooltipPath { get { return @"Roads\TinyRoads\Plain2L\infotooltip.png"; } }

        public NetInfoVersion SupportedVersions
        {
            get { return NetInfoVersion.Ground; }
        }

        public void BuildUp(NetInfo info, NetInfoVersion version)
        {
            ///////////////////////////
            // Texturing             //
            ///////////////////////////
            SetupTextures(info, version);

            ///////////////////////////
            // Set up                //
            ///////////////////////////
            info.m_createGravel = false;
            info.m_createPavement = true;
            info.m_setVehicleFlags = 0;

            ///////////////////////////
            // AI                    //
            ///////////////////////////
            var playerNetAI = info.GetComponent<PlayerNetAI>();

            if (playerNetAI != null)
            {
                playerNetAI.m_constructionCost = playerNetAI.m_constructionCost * 3 / 2;
                playerNetAI.m_maintenanceCost = playerNetAI.m_maintenanceCost * 3 / 2;
            }
        }
    }
}
