using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;

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

        public TabItem TabItem;

        byte _searchType;
        public byte SearchType
        {
            get { return _searchType; }
            set
            {
                if (SetProperty(ref _searchType, value))
                {
                    UpdateTabItemHeader();

                    Filter();
                }
            }
        }

        string _searchString;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                if (SetProperty(ref _searchString, value))
                {
                    UpdateTabItemHeader();

                    Filter();
                }
            }
        }

        private int _page = 0;

        private List<SearchResultModels.SearchResultBase> _fittingResults = new List<SearchResultModels.SearchResultBase>();

        AdvancedObservableCollection<SearchResultModels.SearchResultBase> _searchItems;
        public AdvancedObservableCollection<SearchResultModels.SearchResultBase> SearchItems
        {
            get
            {
                if (_searchItems == null) _searchItems = new AdvancedObservableCollection<SearchResultModels.SearchResultBase>();

                return _searchItems;
            }
        }

        private void UpdateTabItemHeader()
        {
            string header = "Search";

            switch (SearchType)
            {
                case 0: header = "Relic"; break;
                case 1: header = "Missions"; break;
                case 2: header = "Bounties"; break;
            }

            header += string.IsNullOrWhiteSpace(SearchString) ? string.Empty : " - " + SearchString;

            TabItem.Header = header;
        }

        private void Filter()
        {
            _page = 0;
            SearchItems.Clear();
            _fittingResults.Clear();

            if (string.IsNullOrWhiteSpace(SearchString)) return;

            BusyRectangle.Visibility = Visibility.Visible;

            Task.Factory.StartNew(() =>
            {
                var lowerSearchString = SearchString?.ToLower();

                switch (SearchType)
                {
                    case 0:
                        {
                            double noLicationsOffset = 0.0;

                            for (int i = 0; i < WarframeDropDataHelper.Relics.Count; i++)
                            {
                                var relic = WarframeDropDataHelper.Relics[i];

                                var searchIndex = GetAverage(lowerSearchString.SearchStringCompare(relic.Name), relic.Rewards.Min(i => lowerSearchString.SearchStringCompare(i.Name)));

                                if (searchIndex == null) continue;

                                relic.SearchIndex = searchIndex.Value;

                                if (relic.Locations.Count == 0 && relic.SearchIndex > noLicationsOffset) noLicationsOffset = relic.SearchIndex;

                                _fittingResults.Add(relic);
                            }

                            for (int i = 0; i < _fittingResults.Count; i++)
                            {
                                var fittingResult = _fittingResults[i];

                                if (fittingResult.Locations.Count == 0) fittingResult.SearchIndex += noLicationsOffset;
                            }

                            break;
                        }
                    case 1:
                        {
                            for (int i = 0; i < WarframeDropDataHelper.Missions.Count; i++)
                            {
                                var mission = WarframeDropDataHelper.Missions[i];

                                mission.SearchIndex = 0;

                                var searchIndex = GetAverage(lowerSearchString.SearchStringCompare(mission.Name),
                                    lowerSearchString.SearchStringCompare(mission.Planet),
                                    lowerSearchString.SearchStringCompare(mission.Type),
                                    mission.Rewards?.Min(i => lowerSearchString.SearchStringCompare(i.Name)),
                                    mission.RotationA?.Min(i => lowerSearchString.SearchStringCompare(i.Name)),
                                    mission.RotationB?.Min(i => lowerSearchString.SearchStringCompare(i.Name)),
                                    mission.RotationC?.Min(i => lowerSearchString.SearchStringCompare(i.Name)));

                                if (searchIndex == null) continue;

                                mission.SearchIndex = searchIndex.Value;

                                _fittingResults.Add(mission);
                            }

                            break;
                        }
                    case 2:
                        {
                            for (int i = 0; i < WarframeDropDataHelper.Bounties.Count; i++)
                            {
                                var bounty = WarframeDropDataHelper.Bounties[i];

                                bounty.SearchIndex = 0;

                                var searchIndex = GetAverage(lowerSearchString.SearchStringCompare(bounty.Name),
                                    lowerSearchString.SearchStringCompare(bounty.Planet),
                                    bounty.RotationA?.Min(i => lowerSearchString.SearchStringCompare(i.Name)),
                                    bounty.RotationB?.Min(i => lowerSearchString.SearchStringCompare(i.Name)),
                                    bounty.RotationC?.Min(i => lowerSearchString.SearchStringCompare(i.Name)));

                                if (searchIndex == null) continue;

                                bounty.SearchIndex = searchIndex.Value;

                                _fittingResults.Add(bounty);
                            }

                            break;
                        }
                }
            }).ContinueWith(r =>
            {
                BusyRectangle.Visibility = Visibility.Collapsed;

                SearchItems.AddRange(_fittingResults.OrderBy(i => i.SearchIndex).Take(20));

                SearchResultListBox.ChildOfType<ScrollViewer>().ScrollToVerticalOffset(0);

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private double? GetAverage(params byte?[] values)
        {
            if (values.All(i => i == null)) return null;

            var total = 0.0;
            var valueCount = 0;

            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];

                if (value != null)
                {
                    total += value.Value;
                    valueCount++;
                }
            }

            return total / valueCount;
        }

        private void SearchResultListBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset + e.ViewportHeight == e.ExtentHeight && SearchItems.Count < _fittingResults.Count)
            {
                _page++;

                SearchItems.AddRange(_fittingResults.OrderBy(i => i.SearchIndex).Skip(_page * 20).Take(20));
            }
        }
    }
}