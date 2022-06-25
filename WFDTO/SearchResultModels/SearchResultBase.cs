using System;
using System.Collections.Generic;
using System.Text;

namespace WFDTO.SearchResultModels
{
    public class SearchResultBase
    {
        public double SearchIndex { get; set; }

        public string Name { get; set; }

        public List<Item> Rewards { get; set; }

        public List<Item> RotationA { get; set; }

        public List<Item> RotationB { get; set; }

        public List<Item> RotationC { get; set; }

        public List<ItemLocation> Locations { get; set; }
    }
}
