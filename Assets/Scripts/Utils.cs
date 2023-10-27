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
}
