using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace WFDTO.Themes
{
    public class ColorBinding : Binding
    {
        public ColorBinding(string path)
        {
            Source = ThemeHelper.Instance;

            Path = new System.Windows.PropertyPath(path);
        }
    }
}