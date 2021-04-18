using Mapper.Models;

namespace Mapper.ViewModels
{
    public static class Extensions
    {
        /// <summary>
        /// Returns a vector with the same direction as the original vector but with length 1.
        /// </summary>
        /// <returns>A new vector with the same direction and length 1.</returns>
        public static Vec2 UnitVector(this Vec2 v) => v / v.Length();
    }
}
