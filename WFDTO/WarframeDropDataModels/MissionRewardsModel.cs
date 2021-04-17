using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace WFDTO.WarframeDropDataModels
{
    [JsonObject]
    public class MissionRewardsModel : List<ItemModel>
    {
        public List<ItemModel> A { get; set; }

        public List<ItemModel> B { get; set; }

        public List<ItemModel> C { get; set; }
    }
}