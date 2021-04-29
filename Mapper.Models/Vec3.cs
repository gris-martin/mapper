using System;

namespace Mapper.Models
{
    /// <summary>
    /// Represents a 2-dimensional vector.
    /// </summary>
    public struct Vec3 : IFormattable
    {
        /// <summary>
        /// Construct a Vec3 object from 3 values.
        /// </summary>
        /// <param name="x">The value which <see cref="X"/> will receive.</param>
        /// <param name="y">The value which <see cref="Y"/> will receive.</param>
        /// <param name="z">The value which <see cref="Z"/> will receive.</param>
        public Vec3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Construct a Vec3 object from a Vec2 and a double.
        /// </summary>
        /// <param name="xy">A Vec2 with the values which <see cref="X"/> and <see cref="Y"/> will receive.</param>
        /// <param name="z">The value which <see cref="Z"/> will receive.</param>
        public Vec3(Vec2 xy, double z)
        {
            this.X = xy.X;
            this.Y = xy.Y;
            this.Z = z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        /// <summary>
        /// Adds 2 vectors.
        /// </summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The summed vector.</returns>
        public static Vec3 operator +(Vec3 left, Vec3 right)
            => new Vec3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

        /// <summary>
        /// Adds a scalar to a vector.
        /// </summary>
        /// <param name="left">The vector to which to add the scalar.</param>
        /// <param name="right">The scalar to add to the vector.</param>
        /// <returns>The summed vector.</returns>
        public static Vec3 operator +(Vec3 left, double right)
            => new Vec3(left.X + right, left.Y + right, left.Z + right);

        /// <summary>
        /// Subtract a vector from another.
        /// </summary>
        /// <param name="left">The vector which will be subtracted from.</param>
        /// <param name="right">The vector which will be subtracted.</param>
        /// <returns>The subtracted vector.</returns>
        public static Vec3 operator -(Vec3 left, Vec3 right)
            => new Vec3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

        /// <summary>
        /// Subtract a scalar from a vector.
        /// </summary>
        /// <param name="left">The vector which will be subtracted from.</param>
        /// <param name="right">The scalar to be subtracted.</param>
        /// <returns>The subtracted vector.</returns>
        public static Vec3 operator -(Vec3 left, double right)
            => new Vec3(left.X - right, left.Y - right, left.Z - right);

        /// <summary>
        /// Multiply 2 vectors (element-wise multiplication/dot product).
        /// </summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The multiplied vector.</returns>
        public static Vec3 operator *(Vec3 left, Vec3 right)
            => new Vec3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

        /// <summary>
        /// Multiply a vector with a scalar (element-wise).
        /// </summary>
        /// <param name="left">The vector to be multiplied.</param>
        /// <param name="right">The scalar to multiply with.</param>
        /// <returns>The multiplied vector.</returns>
        public static Vec3 operator *(Vec3 left, double right)
            => new Vec3(left.X * right, left.Y * right, left.Z * right);

        /// <summary>
        /// Divide a vector with another (element-wise).
        /// </summary>
        /// <param name="left">The vector which will be divided.</param>
        /// <param name="right">The vector to divide with.</param>
        /// <returns>The divided vector.</returns>
        public static Vec3 operator /(Vec3 left, Vec3 right)
            => new Vec3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);

        /// <summary>
        /// Divide a vector with a scalar (element-wise).
        /// </summary>
        /// <param name="left">The vector which will be divided.</param>
        /// <param name="right">The scalar to divide with.</param>
        /// <returns>The divided vector.</returns>
        public static Vec3 operator /(Vec3 left, double right)
            => new Vec3(left.X / right, left.Y / right, left.Z / right);

        /// <summary>
        /// The inverse of Z. Useful when using the Vec3 to represent an underwater position.
        /// </summary>
        public double Depth => -this.Z;

        /// <summary>
        /// Get the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        public double Length()
            => Math.Sqrt(X * X + Y * Y + Z * Z);

        /// <summary>
        /// Get a unit vector (where the length is 1) with the same direction as this vector.
        /// If the vector is a 0 vector, the 0 vector will be returned).
        /// </summary>
        /// <returns>The unit vector.</returns>
        public Vec3 Unit()
        {
            if (this.X == 0 && this.Y == 0 && this.Z == 0)
                return new Vec3(0, 0, 0);
            return this / Length();
        }

        /// <summary>
        /// Convert the vector to a string on the form "(x, y, z)"
        /// </summary>
        public string ToString(string format, IFormatProvider provider)
            => $"({X}, {Y}, {Z})".ToString(provider);

        /// <summary>
        /// Convert a Vec3 to a Vec2, using the specified axes.
        /// </summary>
        /// <param name="axes">The axes that the Vec2 should use. X, Y by default.</param>
        /// <returns>A new Vec2 object corresponding to the axes.</returns>
        public Vec2 ToVec2(Vec2Axes axes = Vec2Axes.XY)
        {
            if (axes == Vec2Axes.XY)
                return new Vec2(this.X, this.Y);
            else if (axes == Vec2Axes.XZ)
                return new Vec2(this.X, this.Z);
            else  // YZ
                return new Vec2(this.Y, this.Z);
        }

        public enum Vec2Axes
        {
            XY,
            XZ,
            YZ
        }
    }
}
