using System;

namespace Shared.Primitives
{
    public class Vector3
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator *(Vector3 v, float s)
        {
            return new Vector3(
                v.X * s,
                v.Y * s,
                v.Z * s
            );
        }

        public static Vector3 operator *(float s, Vector3 v)
        {
            return new Vector3(
                v.X * s,
                v.Y * s,
                v.Z * s
            );
        }

        public static Vector3 operator /(Vector3 v, float s)
        {
            return new Vector3(
                v.X / s,
                v.Y / s,
                v.Z / s
            );
        }

        public float Length()
        {
            return MathF.Sqrt(LengthSquared());
        }

        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        public Vector3 Normalize()
        {
            var length = Length();

            return new Vector3(
                X / length,
                Y / length,
                Z / length
            );
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}
