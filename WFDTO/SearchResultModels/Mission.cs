using System;
using System.Collections.Generic;
using System.Text;

namespace WFDTO.SearchResultModels
{
    public class Mission : SearchResult
    {
        public string Planet { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public List<Item> Rewards { get; set; }

        public List<Item> RotationA { get; set; }

        public List<Item> RotationB { get; set; }

        public List<Item> RotationC { get; set; }

        public List<ItemLocationGroup> Locations { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}