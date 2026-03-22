using System;

namespace Shared.Primitives
{
    public class Vector3
    {
        private const float _scale = 100f;

        public float Xf => X / _scale;
        public float Yf => Y / _scale;
        public float Zf => Z / _scale;

        public readonly short X;
        public readonly short Y;
        public readonly short Z;

        public Vector3(float x, float y, float z)
        {
            X = (short)MathF.Round(x * _scale);
            Y = (short)MathF.Round(y * _scale);
            Z = (short)MathF.Round(z * _scale);
        }

        public Vector3(short x, short y, short z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3((short)(v1.X - v2.X), (short)(v1.Y - v2.Y), (short)(v1.Z - v2.Z));
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3((short)(v1.X + v2.X), (short)(v1.Y + v2.Y), (short)(v1.Z + v2.Z));
        }

        public static Vector3 operator *(float s, Vector3 v)
        {
            return new Vector3((short)(v.X * s), (short)(v.Y * s), (short)(v.Z * s)
            );
        }

        public static Vector3 operator /(Vector3 v, float s)
        {
            return new Vector3((short)(v.X / s), (short)(v.Y / s), (short)(v.Z / s));
        }

        public float Length()
        {
            return MathF.Sqrt(LengthSquared());
        }

        public float LengthSquared()
        {
            var x = Xf;
            var y = Yf;
            var z = Zf;

            return x * x + y * y + z * z;
        }

        public Vector3 Normalize()
        {
            var length = Length();
            if (length == 0)
            {
                return new Vector3(0, 0, 0);
            }

            return new Vector3(
                Xf / length,
                Yf / length,
                Zf / length
            );
        }


        public override string ToString()
        {
            return $"({Xf}, {Yf}, {Zf})";
        }
    }
}
