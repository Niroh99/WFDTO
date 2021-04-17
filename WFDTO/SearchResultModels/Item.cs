using System;
using System.Collections.Generic;
using System.Text;

namespace WFDTO.SearchResultModels
{
    public class Item
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double DropChance { get; set; }

        public string Rarity { get; set; }
    }
}