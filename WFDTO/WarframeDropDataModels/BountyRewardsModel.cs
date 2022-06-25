using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace WFDTO.WarframeDropDataModels
{
    [JsonObject]
    public class BountyRewardsModel
    {
        public List<BountyItemModel> A { get; set; }

        public List<BountyItemModel> B { get; set; }

        public List<BountyItemModel> C { get; set; }
    }
}