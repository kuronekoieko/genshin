using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayModeStateChangeObserver
{
    [InitializeOnLoadMethod]
    private static void Init()
    {
        EditorApplication.playModeStateChanged += PlayModeStateChanged;
        EditorApplication.pauseStateChanged += PauseStateChanged;
    }

    private static void PauseStateChanged(PauseState state)
    {
        switch (state)
        {
            case PauseState.Paused:
                // Debug.Log("Pause");
                break;
            case PauseState.Unpaused:
                // Debug.Log("UnPause");
                break;
        }
    }

    private static void PlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.ExitingEditMode:
                // Debug.Log("Press Play Button");
                break;
            case PlayModeStateChange.EnteredPlayMode:
                //  Debug.Log("Play");
                break;
            case PlayModeStateChange.ExitingPlayMode:
                //  Debug.Log("Press Stop Button");
                break;
            case PlayModeStateChange.EnteredEditMode:
                AssetDatabase.Refresh();
                // Debug.Log("Stop");
                break;
        }
    }
}
