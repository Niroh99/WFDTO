using System;
using System.Collections.Generic;
using System.Text;

namespace WFDTO.SearchResultModels
{
    public interface SearchResult
    {
        string Name { get; }

        List<Item> Rewards { get; set; }

        List<Item> RotationA { get; set; }

        List<Item> RotationB { get; set; }

        List<Item> RotationC { get; set; }

        List<ItemLocationGroup> Locations { get; set; }
    }
}
