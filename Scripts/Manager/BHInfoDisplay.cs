using UnityEngine;
using UnityEditor;


namespace BH_Engine
{
    [ExecuteInEditMode]
    public class BHInfoDisplay : MonoBehaviour
    {
        // 显示帧率
        public bool showFPS = true;
        // 显示屏幕子弹数量
        public bool showBulletCount = true;
        // 字体颜色
        public Color fontColor = Color.white;

        private int frameCount = 0;
        private float dt = 0.0f;
        private float fps = 0.0f;
        private float updateRate = 4.0f;  // 4 updates per second

        private void Update()
        {
            // 确保脚本在编辑模式和播放模式下运行
            frameCount++;
            dt += Time.deltaTime;
            if (dt > 1.0f / updateRate)
            {
                fps = frameCount / dt;
                frameCount = 0;
                dt -= 1.0f / updateRate;
            }
        }

        private void OnGUI()
        {
            if (showFPS || showBulletCount)
            {
                // 使用GUI显示帧数
                GUIStyle style = new GUIStyle();
                style.fontSize = 24;
                style.normal.textColor = fontColor;

                float width = 200;
                float height = 40;
                float x = Screen.width - width - 10;
                float y = Screen.height - height - 50;

                var fpsStr = showFPS ? fps.ToString("F2") + " FPS" : "";
                var bulletCountStr = showBulletCount ? "Bullet Count: " + (BulletBehaviourManager.instance?.activeBullets?.Count ?? 0) : "";
                var guiStr = fpsStr + "\n" + bulletCountStr;
                GUI.Label(new Rect(x, y, width, height), guiStr, style);
            }
        }
    }

}