using System;

namespace Mapper.Models
{
    /// <summary>
    /// Represents a 2-dimensional vector.
    /// </summary>
    public struct Vec2 : IFormattable
    {
        /// <summary>
        /// Construct a Vec2 object where both <see cref="X"/> and <see cref="Y"/> will receive the same value.
        /// </summary>
        /// <param name="v">The value to be assigned to <see cref="X"/> and <see cref="Y"/>.</param>
        public Vec2(double v)
        {
            X = v;
            Y = v;
        }

        /// <summary>
        /// Construct a Vec2 object from 2 values.
        /// </summary>
        /// <param name="x">The value which <see cref="X"/> will receive.</param>
        /// <param name="y">The value which <see cref="Y"/> will receive.</param>
        public Vec2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        /// <summary>
        /// Adds 2 vectors.
        /// </summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The summed vector.</returns>
        public static Vec2 operator +(Vec2 left, Vec2 right)
            => new Vec2(left.X + right.X, left.Y + right.Y);

        /// <summary>
        /// Adds a scalar to a vector.
        /// </summary>
        /// <param name="left">The vector to which to add the scalar.</param>
        /// <param name="right">The scalar to add to the vector.</param>
        /// <returns>The summed vector.</returns>
        public static Vec2 operator +(Vec2 left, double right)
            => new Vec2(left.X + right, left.Y + right);

        /// <summary>
        /// Subtract a vector from another.
        /// </summary>
        /// <param name="left">The vector which will be subtracted from.</param>
        /// <param name="right">The vector which will be subtracted.</param>
        /// <returns>The subtracted vector.</returns>
        public static Vec2 operator -(Vec2 left, Vec2 right)
            => new Vec2(left.X - right.X, left.Y - right.Y);

        /// <summary>
        /// Subtract a scalar from a vector.
        /// </summary>
        /// <param name="left">The vector which will be subtracted from.</param>
        /// <param name="right">The scalar to be subtracted.</param>
        /// <returns>The subtracted vector.</returns>
        public static Vec2 operator -(Vec2 left, double right)
            => new Vec2(left.X - right, left.Y - right);

        /// <summary>
        /// Multiply 2 vectors (element-wise multiplication/dot product).
        /// </summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The multiplied vector.</returns>
        public static Vec2 operator *(Vec2 left, Vec2 right)
            => new Vec2(left.X * right.X, left.Y * right.Y);

        /// <summary>
        /// Multiply a vector with a scalar (element-wise).
        /// </summary>
        /// <param name="left">The vector to be multiplied.</param>
        /// <param name="right">The scalar to multiply with.</param>
        /// <returns>The multiplied vector.</returns>
        public static Vec2 operator *(Vec2 left, double right)
            => new Vec2(left.X * right, left.Y * right);

        /// <summary>
        /// Divide a vector with another (element-wise).
        /// </summary>
        /// <param name="left">The vector which will be divided.</param>
        /// <param name="right">The vector to divide with.</param>
        /// <returns>The divided vector.</returns>
        public static Vec2 operator /(Vec2 left, Vec2 right)
            => new Vec2(left.X / right.X, left.Y / right.Y);

        /// <summary>
        /// Divide a vector with a scalar (element-wise).
        /// </summary>
        /// <param name="left">The vector which will be divided.</param>
        /// <param name="right">The scalar to divide with.</param>
        /// <returns>The divided vector.</returns>
        public static Vec2 operator /(Vec2 left, double right)
            => new Vec2(left.X / right, left.Y / right);

        /// <summary>
        /// Get the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        public double Length()
            => Math.Sqrt(X * X + Y * Y);

        /// <summary>
        /// Get a unit vector (where the length is 1) with the same direction as this vector.
        /// If the vector is a 0-vector, an arbitrary unit vector will be returned (currently (0, 1)).
        /// </summary>
        /// <returns>The unit vector.</returns>
        public Vec2 Unit()
        {
            if (this.X == 0 && this.Y == 0)
                return new Vec2(0, 1);
            return this / Length();
        }

        /// <summary>
        /// Convert the vector to a string on the form "(x, y)"
        /// </summary>
        public string ToString(string format, IFormatProvider provider)
            => $"({X}, {Y})".ToString(provider);
    }
}
