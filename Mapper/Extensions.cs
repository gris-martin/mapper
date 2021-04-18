using System;
using System.Windows;

using Mapper.Models;

namespace Mapper
{
    public static class Extensions
    {
        /// <summary>
        /// Convert this Point to a Vec2.
        /// </summary>
        /// <returns>A Vec2 object with the same x and y values as this Point.</returns>
        public static Vec2 ToVec2(this Point p)
        {
            return new Vec2(p.X, p.Y);
        }

        /// <summary>
        /// Convert this Vec2 to a Point.
        /// </summary>
        /// <returns>A Point object with the same x and y values as this Vec2.</returns>
        public static Point ToPoint(this Vec2 v)
        {
            return new Point(v.X, v.Y);
        }
    }
}
