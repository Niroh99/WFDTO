using System;
using System.Collections.Generic;
using System.Text;

namespace WFDTO.SearchResultModels
{
    public class Relic : SearchResultBase
    {
        public string Era { get; set; }

        public string Description { get; set; }

        public string Name
        {
            get { return $"{Era} {Description}"; }
            set { }
        }
    }
}