using UnityEngine;

namespace Runtime.Tools
{
    public static class Vector3Extensions
    {
        public static Vector3 ToUnityVector3(this Shared.Primitives.Vector3 vector3)
        {
            return new Vector3(vector3.X, vector3.Y, vector3.Z);
        }

        public static Shared.Primitives.Vector3 ToSharedVector2(this Vector2 vector2)
        {
            return new Shared.Primitives.Vector3(vector2.x, 0f, vector2.y);
        }

        public static Shared.Primitives.Vector3 ToSharedVector3(this Vector3 vector3)
        {
            return new Shared.Primitives.Vector3(vector3.x, vector3.y, vector3.z);
        }
    }
}
