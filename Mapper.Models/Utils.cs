using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper.Models
{

    public static class Utils
    {
        /// <summary>
        /// Get the "compass angle" between 2 vectors, in degrees.
        /// The compass angle is 0 when x = 0, y = 1 and increases clockwise so that
        /// north => 0, east => 90, south => 180 and west => 270.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetCompassAngle(Vec3 p1, Vec3 p2)
        {
            var dir = (p2 - p1).Unit();
            var rotatedDir = new Vec2(dir.Y, -dir.X);  // Rotate vector so that north == (1, 0) (instead of (0, 1)), for Atan2
            var angle = Math.Atan2(rotatedDir.Y, rotatedDir.X);

            if (rotatedDir.Y > 0)
                angle = Math.PI * 2.0 - angle;
            else
                angle = -angle;
            return angle * 180.0 / Math.PI;
        }

        /// <summary>
        /// Set the "compass" angle (<see cref="GetCompassAngle(Vec3, Vec3)"/>) between p1 and p2.
        /// Depending on the value of <paramref name="moveFirst"/>, p1 or p2 will have its position
        /// changed so that the angle between the points is correct. By default p2 will be moved.
        /// </summary>
        /// <param name="p1">The "static" point, which will not move.</param>
        /// <param name="p2">The point that will be moved.</param>
        /// <param name="angle">The desired compass angle (in degrees).</param>
        /// <param name="moveFirst">Set to true if p1 should be moved, and false if p2 should be moved. Default is false.</param>
        /// <returns>The translated point (p1 if moveFirst is true, p2 otherwise).</returns>
        public static Vec3 SetCompassAngle(Vec3 p1, Vec3 p2, double angle, bool moveFirst = false)
        {
            var p1Plane = new Vec2(p1.X, p1.Y);
            var p2Plane = new Vec2(p2.X, p2.Y);
            var length = (p2Plane - p1Plane).Length();

            // The compass angle has 0 when x = 0, y = 1, and increases clockwise
            // When doing rotations the angle will be 0 at x = 1, y = 0 and increase counter-clockwise
            // So the rotation angle will be 90 when the compass angle is 0, and decrease with increasing compass angle
            var rotAngle = ToRadians(90 - angle);

            // Create a unit vector with the direction specified by the angle
            var rotatedUnitVector = new Vec2(Math.Cos(rotAngle), Math.Sin(rotAngle));

            // Set the length and translate it
            if (moveFirst)
            {
                var p1PlaneNew = p2Plane - rotatedUnitVector * length;
                return new Vec3(p1PlaneNew, p1.Z);
            }

            var p2PlaneNew = rotatedUnitVector * length + p1Plane;
            return new Vec3(p2PlaneNew, p2.Z);

            //// Translate p2 to origin to be able to do the rotation
            //var p2PlaneOrigin = p2Plane - p1Plane;

            //// Do the rotation
            //var c = Math.Cos(rotAngle);
            //var s = Math.Sin(rotAngle);
            //var xRotated = p2PlaneOrigin.X * c - p2PlaneOrigin.Y * s;
            //var yRotated = p2PlaneOrigin.X * s + p2PlaneOrigin.Y * c;
            //var p2PlaneOriginRotated = new Vec2(xRotated, yRotated);

            //// Translate it back
            //var p2PlaneNew = p2PlaneOriginRotated + p1Plane;

            //return new Vec3(p2PlaneNew.X, p2PlaneNew.Y, p2.Z);

            //var p2Origin = new Vec2(p2.Y, -p2.X) - new Vec2(p1.Y, -p1.X);
            //double s = Math.Sin(angle);
            //double c = Math.Cos(angle);
            //var p2OriginRotated = new Vec2(p2Origin.X * c - p2Origin.Y * s,
            //                               p2Origin.X * s + p2Origin.Y * c);
            //var newP2 = new Vec2(p2OriginRotated.X, p2OriginRotated.Y) + new Vec2(p1.Y, -p1.X);
            //return new Vec3(newP2.Y, -newP2.X, p2.Z);
        }

        public static string GetCompassString(double angle)
        {
            var i = 22.5;
            if (angle < 11.25)
                return "N";
            else if (angle < (12.25 + i))
                return "NNE";
            else if (angle < (12.25 + i * 2))
                return "NE";
            else if (angle < (12.25 + i * 3))
                return "ENE";
            else if (angle < (12.25 + i * 4))
                return "E";
            else if (angle < (12.25 + i * 5))
                return "ESE";
            else if (angle < (12.25 + i * 6))
                return "SE";
            else if (angle < (12.25 + i * 7))
                return "SSE";
            else if (angle < (12.25 + i * 8))
                return "S";
            else if (angle < (12.25 + i * 9))
                return "SSW";
            else if (angle < (12.25 + i * 10))
                return "SW";
            else if (angle < (12.25 + i * 11))
                return "WSW";
            else if (angle < (12.25 + i * 12))
                return "W";
            else if (angle < (12.25 + i * 13))
                return "WNW";
            else if (angle < (12.25 + i * 14))
                return "NW";
            else if (angle < (12.25 + i * 15))
                return "NNW";
            return "N";
        }

        /// <summary>
        /// Convert an angle in degrees to radians.
        /// </summary>
        /// <param name="degrees">Angle in degrees.</param>
        /// <returns>Angle in radians.</returns>
        public static double ToRadians(double degrees) =>
            degrees / 180 * Math.PI;

        /// <summary>
        /// Convert an angle in radians to degrees.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>Angle in degrees.</returns>
        public static double ToDegrees(double radians) =>
            radians / Math.PI * 180 ;
    }
}
