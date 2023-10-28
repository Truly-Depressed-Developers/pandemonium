using UnityEngine;

public static class Utils {
    public static void ApplicationQuit(string reason = "")
    {
        string message = "Quit";

        if (!string.IsNullOrEmpty(reason))
            message += $" ({reason})";
        
        Debug.Log(message);
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public static float RandomizeValue(float baseValue, float modifier) {
        return Random.Range(baseValue * (1 - modifier), baseValue * (1 + modifier));
    }
}
