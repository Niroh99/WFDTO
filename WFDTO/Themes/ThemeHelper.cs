using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace WFDTO.Themes
{
    public class ThemeHelper : INotifyPropertyChanged
    {
        static ThemeHelper()
        {
            Instance = new ThemeHelper();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        public static ThemeHelper Instance { get; private set; }

        Brush _mainBrush;
        public Brush MainBrush
        {
            get { return _mainBrush; }
            set { SetProperty(ref _mainBrush, value); }
        }

        Brush _mainBrush40Percent;
        public Brush MainBrush40Percent
        {
            get { return _mainBrush40Percent; }
            set { SetProperty(ref _mainBrush40Percent, value); }
        }

        Brush _highlightBrush;
        public Brush HighlightBrush
        {
            get { return _highlightBrush; }
            set { SetProperty(ref _highlightBrush, value); }
        }

        Brush _hightlightBrush40Percent;
        public Brush HighlightBrush40Percent
        {
            get { return _hightlightBrush40Percent; }
            set { SetProperty(ref _hightlightBrush40Percent, value); }
        }

        Brush _background;
        public Brush Background
        {
            get { return _background; }
            set { SetProperty(ref _background, value); }
        }

        public static void LoadTheme(Theme theme)
        {
            Instance.MainBrush = theme.MainBrush;
            Instance.MainBrush40Percent = theme.MainBrush40Percent;

            Instance.HighlightBrush = theme.HighlightBrush;
            Instance.HighlightBrush40Percent = theme.HighlightBrush40Percent;

            Instance.Background = theme.Background;
        }

        private static Dictionary<string, Theme> _themes;
        public static Dictionary<string, Theme> Themes
        {
            get
            {
                if (_themes == null) _themes = new Dictionary<string, Theme>
                {
                    { "Corpus", new Theme("#23C9F5", "#6FE5FD", "#050F1C") },
                    { "Dark Lotus", new Theme("#8C7793", "#C8A9ED", "#210554") },
                    { "Equinox", new Theme("#9E9FA7", "#E8E3E3", "#0A0209") },
                    { "Fortuna", new Theme("#3969C0", "#FF73E6", "#010209") },
                    { "High Contrast", new Theme("#027FD9", "#FFFF00", "#08122A") },
                    { "Legacy", new Theme("#FFFFFF", "#E8D55D", "#284351") },
                    { "Lotus", new Theme("#24B8F2", "#FFF1BF", "#28335B") },
                    { "Nidus", new Theme("#8C265C", "#F5495D", "#E9E8D6") },
                    { "Orokin", new Theme("#14291D", "#B27D05", "#ECF0EF") },
                    { "Stalker", new Theme("#991F23", "#FF3D33", "#030101") },
                    { "Tenno", new Theme("#094E6A", "#066D4A", "#C7DADB") },
                    { "Vitruvian", new Theme("#BEA966", "#F5E3AD", "#14131F") },
                    { "Zephyr Harrier", new Theme("#FD8402", "#FF3500", "#0B1420") }
                };

                return _themes;
            }
        }
    }
}