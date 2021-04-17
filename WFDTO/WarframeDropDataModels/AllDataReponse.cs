using System;
using System.Collections.Generic;
using System.Text;

namespace WFDTO.WarframeDropDataModels
{
    public class AllDataReponse
    {
        public List<RelicModel> Relics { get; set; }

        public Dictionary<string, Dictionary<string, MissionModel>> MissionRewards { get; set; }
    }
}
