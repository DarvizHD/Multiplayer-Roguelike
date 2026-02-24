using UnityEngine;

namespace Runtime.CustomAsync
{
    public class ScheduleUpdater : MonoBehaviour
    {
        private void Update()
        {
            Scheduler.Instance.Update(Time.deltaTime);
        }
    }
}