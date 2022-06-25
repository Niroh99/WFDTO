using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WFDTOCustomControlLibrary
{
    public class ClosableTabItem : TabItem
    {
        static ClosableTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClosableTabItem), new FrameworkPropertyMetadata(typeof(ClosableTabItem)));
        }

        private Button CloseButton;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CloseButton = (Button)GetTemplateChild("CloseButton");

            CloseButton.Click += CloseButton_Click;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Parent is TabControl parentTabControl)
            {
                if (parentTabControl.Items.Count == 1)
                {

                }
                else parentTabControl.Items.Remove(this);
            }
        }
    }
}