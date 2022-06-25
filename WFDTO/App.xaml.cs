using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WFDTO
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.KeyUpEvent, new KeyEventHandler(TextBoxKeyUp));

            var theme = Themes.ThemeHelper.Themes["Vitruvian"];

            theme.LoadThemeCommand.Execute(null);
        }

        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((TextBox)sender).GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            }
        }
    }

    public class AdvancedObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>
    {
        private bool _suppressNotifications = false;

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotifications) base.OnCollectionChanged(e);
        }

        public void AddRange(IEnumerable<T> range)
        {
            _suppressNotifications = true;

            PrivateAddRange(range);

            _suppressNotifications = false;

            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }

        private void PrivateAddRange(IEnumerable<T> range)
        {
            for (int i = 0; i < range.Count(); i++)
            {
                Add(range.ElementAt(i));
            }
        }

        public void Order<TKey>(Func<T, TKey> keySelector)
        {
            var orderedCollection = this.OrderBy(keySelector).ToList();

            _suppressNotifications = true;

            Clear();

            PrivateAddRange(orderedCollection);

            _suppressNotifications = false;

            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }

        public void Order<TKey1, TKey2>(Func<T, TKey1> firstKeySelector, Func<T, TKey2> secondKeySelector)
        {
            var orderedCollection = this.OrderBy(firstKeySelector).ThenBy(secondKeySelector);

            _suppressNotifications = true;

            Clear();

            PrivateAddRange(orderedCollection);

            _suppressNotifications = false;

            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }
    }
}
