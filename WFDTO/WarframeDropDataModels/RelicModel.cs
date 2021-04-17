using System;
using System.Collections.Generic;
using System.Text;

namespace WFDTO.WarframeDropDataModels
{
    public class RelicModel
    {
        public string Id { get; set; }

        public string Tier { get; set; }

        public string RelicName { get; set; }

        public string State { get; set; }

        public List<ItemModel> Rewards { get; set; }
    }
}