using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class EditorFPSDisplay
{
    private static int frameCount = 0;
    private static float dt = 0.0f;
    private static float fps = 0.0f;
    private static float updateRate = 4.0f;  // 4 updates per sec.

    static EditorFPSDisplay()
    {
        // Subscribe to the editor update and GUI events
        EditorApplication.update += Update;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0f / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;
        GUILayout.Label(fps.ToString("F2") + " FPS", style);
        Handles.EndGUI();
    }
}
