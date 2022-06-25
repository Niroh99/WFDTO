using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace WFDTO.Themes
{
    public class Theme : System.ComponentModel.INotifyPropertyChanged
    {
        public Theme(string mainBrush, string hightlightBrush, string backgroundBrush)
        {
            var brushConverter = new BrushConverter();

            MainBrush = (Brush)brushConverter.ConvertFromString(mainBrush);
            MainBrush40Percent = (Brush)brushConverter.ConvertFromString(mainBrush);
            MainBrush40Percent.Opacity = 0.4;

            HighlightBrush = (Brush)brushConverter.ConvertFromString(hightlightBrush);
            HighlightBrush40Percent = (Brush)brushConverter.ConvertFromString(hightlightBrush);
            HighlightBrush40Percent.Opacity = 0.4;

            Background = (Brush)brushConverter.ConvertFromString(backgroundBrush);
            Background.Opacity = 0.8;
        }

        #region PropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            return true;
        }
        #endregion

        public Brush MainBrush { get; set; }

        public Brush MainBrush40Percent { get; set; }

        public Brush HighlightBrush { get; set; }

        public Brush HighlightBrush40Percent { get; set; }

        public Brush Background { get; set; }

        bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value); }
        }

        ICommand _loadThemeCommand;
        public ICommand LoadThemeCommand
        {
            get
            {
                return _loadThemeCommand ?? (_loadThemeCommand = new WFDTOCustomControlLibrary.DefaultCommand(new Action<object>((parameter) =>
                {
                    foreach (var theme in ThemeHelper.Themes) theme.Value.IsActive = false;

                    IsActive = true;

                    ThemeHelper.LoadTheme(this);
                })));
            }
        }
    }
}