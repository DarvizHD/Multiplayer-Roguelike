using UnityEngine;

namespace Runtime
{
    public class TestCounter
    {
        public static int Counter;

        public static void Increase()
        {
            Counter++;
            Debug.Log("Counter: " + Counter);
        }
    }
}
