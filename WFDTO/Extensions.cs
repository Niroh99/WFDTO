using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace WFDTO
{
    public static class Extensions
    {
        public static byte? SearchStringCompare(this string searchString, string value)
        {
            if (string.IsNullOrWhiteSpace(searchString) || string.IsNullOrWhiteSpace(value)) return null;

            searchString = searchString?.ToLower();

            value = value?.ToLower();

            if (searchString == value) return 0;

            var searchStringElements = searchString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var valueStringElements = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (searchStringElements.All(i => valueStringElements.Contains(i))) return 1;

            var fittingElementsCount = 0;

            var partiallyFittingElementsCount = 0;
            
            for (int i = 0; i < searchStringElements.Length; i++)
            {
                var searchStringElement = searchStringElements[i];

                if (valueStringElements.Any(j => j == searchStringElement)) fittingElementsCount++;
                else if (valueStringElements.Any(j => j.Contains(searchStringElement))) partiallyFittingElementsCount++;
            }

            if (fittingElementsCount == 0 && partiallyFittingElementsCount == 0) return null;

            var searchIndex = searchStringElements.Length * 2 + 2;

            searchIndex -= fittingElementsCount * 2;
            searchIndex -= partiallyFittingElementsCount;

            return (byte)searchIndex;
        }

        public static T ChildOfType<T>(this DependencyObject dependencyObject)
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject);

            var targetType = typeof(T);

            object child = null;

            for (int i = 0; i < childrenCount; i++)
            {
                child = VisualTreeHelper.GetChild(dependencyObject, i);

                if (child != null && child.GetType() == targetType) return (T)child;

                child = ((DependencyObject)child).ChildOfType<T>();

                if (child != null) return (T)child;
            }

            return (T)child;
        }
    }
}