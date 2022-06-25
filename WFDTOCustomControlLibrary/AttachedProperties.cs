using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WFDTOCustomControlLibrary
{
    public class AttachedProperties
    {
        static AttachedProperties()
        {
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.TextChangedEvent, new TextChangedEventHandler(OnTextBoxTextChanged), true);
        }

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(AttachedProperties), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetWatermark(DependencyObject element, string value)
        {
            element.SetValue(WatermarkProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static string GetWatermark(DependencyObject element)
        {
            return (string)element.GetValue(WatermarkProperty);
        }

        public static readonly DependencyPropertyKey IsEmptyPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsEmpty", typeof(bool), typeof(AttachedProperties), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IsEmptyProperty = IsEmptyPropertyKey.DependencyProperty;

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetIsEmpty(DependencyObject element)
        {
            return (bool)element.GetValue(IsEmptyProperty);
        }

        private static void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SetValue(IsEmptyPropertyKey, string.IsNullOrEmpty(textBox.Text));
            }
            else return;
        }
    }
}