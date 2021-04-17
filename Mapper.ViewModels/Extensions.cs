using System.Numerics;

namespace Mapper.ViewModels
{
    public static class Extensions
    {
        /// <summary>
        /// Returns a vector with the same direction as the original vector but with length 1.
        /// </summary>
        /// <returns>A new vector with the same direction and length 1.</returns>
        public static Vector2 UnitVector(this Vector2 v) => v / v.Length();
    }
}
