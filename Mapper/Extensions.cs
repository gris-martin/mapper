using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mapper
{
    public static class Extensions
    {
        /// <summary>
        /// Convert this Point to a Vector2.
        /// </summary>
        /// <returns>A Vector2 object with the same x and y values as this Point.</returns>
        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2(Convert.ToSingle(p.X), Convert.ToSingle(p.Y));
        }

        /// <summary>
        /// Convert this Vector2 to a Point.
        /// </summary>
        /// <returns>A Point object with the same x and y values as this Vector2.</returns>
        public static Point ToPoint(this Vector2 v)
        {
            return new Point(v.X, v.Y);
        }
    }
}
