using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Auriga.Common
{
    public static class Utils
    {
        public static void Deconstruct(this Point p, out double x, out double y)
        {
            x = p.X;
            y = p.Y;
        }
    }
}
