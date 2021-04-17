using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WFDTO
{
    public partial class ItemSearchUserControl : UserControl
    {
        public ItemSearchUserControl()
        {
            InitializeComponent();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            return true;
        }

        byte _searchType;
        public byte SearchType
        {
            get { return _searchType; }
            set { if (SetProperty(ref _searchType, value)) Filter(); }
        }

        string _searchString;
        public string SearchString
        {
            get { return _searchString; }
            set { if (SetProperty(ref _searchString, value)) Filter(); }
        }

        ObservableCollection<SearchResultModels.SearchResult> _searchItems;
        public ObservableCollection<SearchResultModels.SearchResult> SearchItems
        {
            get
            {
                if (_searchItems == null) _searchItems = new ObservableCollection<SearchResultModels.SearchResult>();

                return _searchItems;
            }
        }

        private void SearchTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        private void Filter()
        {
            SearchItems.Clear();

            var lowerSearchStringElements = SearchString.ToLower();

            switch (SearchType)
            {
                case 0:
                {
                    var fittingResults = WarframeDropDataHelper.Relics.Where(i => i.Name?.ToLower().Contains(lowerSearchStringElements) == true || i.Rewards.Any(j => j.Name?.ToLower().Contains(lowerSearchStringElements) == true)).ToList();

                    for (int i = 0; i < fittingResults.Count; i++)
                    {
                        var fittingResult = fittingResults[i];

                        SearchItems.Add(fittingResult);
                    }

                    break;
                }
            }


        }
    }
}