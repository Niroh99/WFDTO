using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WFDTO
{
    public class SearchType
    {
        public static ObservableCollection<SearchType> SearchTypes
        {
            get
            {
                return new ObservableCollection<SearchType>
                {
                    new SearchType
                    {
                        Id = 0,
                        Name = "Relics"
                    },
                    new SearchType
                    {
                        Id = 1,
                        Name = "Missions"
                    },
                    new SearchType
                    {
                        Id = 2,
                        Name = "Bounties"
                    }
                };
            }
        }

        public byte Id { get; set; }

        public string Name { get; set; }
    }
}
