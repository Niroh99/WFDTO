using System;
using System.Collections.Generic;
using System.Text;

namespace WFDTO.SearchResultModels
{
    public class Relic : SearchResult
    {
        public string Era { get; set; }

        public string Description { get; set; }

        public string Name
        {
            get
            {
                return $"{Era} {Description}";
            }
        }

        public List<Item> Rewards { get; set; }

        public List<Item> RotationA { get; set; }

        public List<Item> RotationB { get; set; }

        public List<Item> RotationC { get; set; }

        public List<ItemLocationGroup> Locations { get; set; }
    }
}