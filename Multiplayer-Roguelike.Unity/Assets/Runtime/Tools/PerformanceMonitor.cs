using UnityEngine;

namespace Runtime.Tools
{
    public class PerformanceMonitor : MonoBehaviour
    {
        private float _lastTime;
        private float _deltaTime;

        private float _delayMs;
        private float _fps;

        private void Start()
        {
            _lastTime = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            var currentTime = Time.realtimeSinceStartup;

            _deltaTime = currentTime - _lastTime;
            _delayMs = _deltaTime * 1000f;

            if (_deltaTime > 0f)
            {
                _fps = 1f / _deltaTime;
            }

            _lastTime = currentTime;
        }

        private void OnGUI()
        {
            var style = new GUIStyle(GUI.skin.label);
            style.fontSize = 20;
            style.normal.textColor = Color.green;

            GUI.Label(new Rect(10, 10, 300, 60),
                $"FPS: {_fps:0}\nUpdate Delay: {_delayMs:0.00} ms",
                style);
        }
    }
}
