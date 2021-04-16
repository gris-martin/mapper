using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mapper
{
    public static class Extensions
    {
        public static Point Add(this Point current, Point other)
        {
            return new Point(current.X + other.X, current.Y + other.Y);
        }

        public static Point Subtract(this Point current, Point other)
        {
            return new Point(current.X - other.X, current.Y - other.Y);
        }

        public static Point Multiply(this Point current, double constant)
        {
            return new Point(current.X * constant, current.Y * constant);
        }

        public static Point Divide(this Point current, double constant)
        {
            return new Point(current.X / constant, current.Y / constant);
        }

        public static double Length(this Point current)
        {
            return Math.Sqrt(current.X * current.X + current.Y * current.Y);
        }
    }
}
