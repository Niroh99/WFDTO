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
        }

        private void MainWindow_TemplateApplyed(object sender, EventArgs e)
        {
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
            var itemSearchUserControl = new ItemSearchUserControl();

            var grid = new Grid();

            grid.Children.Add(itemSearchUserControl);

            var tabItem = new ClosableTabItem
            {
                Header = "Relics",
                Content = grid
            };

            itemSearchUserControl.TabItem = tabItem;

            return tabItem;
        }

        private TabItem SearchTabControl_RequestTabItem()
        {
            return GetTabItem();
        }
    }
}