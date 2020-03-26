// Available under Attribution-ShareAlike 3.0 Unported (CC BY-SA 3.0) - https://creativecommons.org/licenses/by-sa/3.0/
// Adapted from https://wiki.unity3d.com/index.php/AutoSave
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

class AutoSaveWindow : EditorWindow 
{
    private bool autoSaveScene = true;
	private bool showMessage = true;
	private bool isStarted = false;
	private int intervalScene;	
	private DateTime lastSaveTimeScene = DateTime.Now;

    [MenuItem ("Window/Auto Save")]

    public static void  ShowWindow () 
	{
        EditorWindow.GetWindow(typeof(AutoSaveWindow));
    }
    
    void OnGUI () 
    {
		GUILayout.Label ("Auto Save Options:", EditorStyles.boldLabel);
		autoSaveScene = EditorGUILayout.BeginToggleGroup ("Auto save", autoSaveScene);
		intervalScene = EditorGUILayout.IntSlider ("Interval (minutes)", intervalScene, 1, 10);
		
        if(isStarted) 
        {
			EditorGUILayout.LabelField ("Last save:", "" + lastSaveTimeScene);
		}

		EditorGUILayout.EndToggleGroup();
		showMessage = EditorGUILayout.BeginToggleGroup ("Show Message", showMessage);
		EditorGUILayout.EndToggleGroup ();
    }

    void Update()
    {
		if(autoSaveScene) 
        {
			if(DateTime.Now.Minute >= (lastSaveTimeScene.Minute+intervalScene) || DateTime.Now.Minute == 59 && DateTime.Now.Second == 59)
            {
				saveScene();
			}
		} 
        else 
        {
			isStarted = false;
		}
 
	}

    void saveScene() 
    {
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		lastSaveTimeScene = DateTime.Now;
		isStarted = true;
		
        AutoSaveWindow repaintSaveWindow = (AutoSaveWindow)EditorWindow.GetWindow (typeof (AutoSaveWindow));
		repaintSaveWindow.Repaint();
        
        if(showMessage)
        {
            Debug.Log("Saved scene " + EditorSceneManager.GetActiveScene().name);
        }
	}
}