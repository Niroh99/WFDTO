using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WFDTOCustomControlLibrary
{
    public class AdvancedTabControl : TabControl
    {
        static AdvancedTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedTabControl), new FrameworkPropertyMetadata(typeof(AdvancedTabControl)));
        }

        private Button AddTabButton;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AddTabButton = (Button)GetTemplateChild("AddTabButton");

            AddTabButton.Click += AddTabButton_Click;
        }

        public RequestTabItem RequestTabItem;

        private void AddTabButton_Click(object sender, RoutedEventArgs e)
        {
            var tabItem = RequestTabItem?.Invoke();

            if (tabItem == null) tabItem = new TabItem();

            Items.Add(tabItem);

            SelectedItem = tabItem;
        }
    }

    public delegate TabItem RequestTabItem();
}