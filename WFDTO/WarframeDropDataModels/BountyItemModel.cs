using System;
using System.Collections.Generic;
using System.Text;

namespace WFDTO.WarframeDropDataModels
{
    public class BountyItemModel : ItemModel
    {
        public string Stage { get; set; }

        public SearchResultModels.BountyItem ToBountyItem()
        {
            return new SearchResultModels.BountyItem
            {
                Id = Id,
                Name = ItemName,
                DropChance = Chance,
                Rarity = Rarity,
                Stage = Stage
            };
        }
    }
}