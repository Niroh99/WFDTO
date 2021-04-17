using System;
using System.Collections.Generic;
using System.Text;

namespace WFDTO.WarframeDropDataModels
{
    public class ItemModel
    {
        public string Id { get; set; }

        public string ItemName { get; set; }

        public double Chance { get; set; }

        public string Rarity { get; set; }

        public SearchResultModels.Item ToItem()
        {
            return new SearchResultModels.Item
            {
                Id = Id,
                Name = ItemName,
                DropChance = Chance,
                Rarity = Rarity
            };
        }
    }
}