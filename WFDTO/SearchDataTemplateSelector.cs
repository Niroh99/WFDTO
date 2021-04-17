using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WFDTO
{
    public class SearchDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RelicDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SearchResultModels.Relic) return RelicDataTemplate;
            else return null;
        }
    }
}
