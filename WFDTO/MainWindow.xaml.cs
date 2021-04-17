using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WFDTOCustomControlLibrary;

namespace WFDTO
{
    public partial class MainWindow : OverlayWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            TemplateApplyed += MainWindow_TemplateApplyed;
        }

        private void MainWindow_TemplateApplyed(object sender, EventArgs e)
        {
            SearchTabControl.RequestTabItem += SearchTabControl_RequestTabItem;

            Task.Factory.StartNew(() => WarframeDropDataHelper.Initialize())
            .ContinueWith((r) =>
            {
                if (r.Exception != null) MessageBox.Show("Error Fetching Drop Tables:\n\n" + r.Exception.Message);
                else
                {
                    LoadingTextBlock.Visibility = Visibility.Collapsed;

                    var tabItem = GetTabItem();

                    SearchTabControl.Items.Add(tabItem);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private ClosableTabItem GetTabItem()
        {
            var grid = new Grid();

            grid.Children.Add(new ItemSearchUserControl());

            return new ClosableTabItem
            {
                Header = "Search",
                Content = grid
            };
        }

        private TabItem SearchTabControl_RequestTabItem()
        {
            return GetTabItem();
        }
    }
}