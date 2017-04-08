// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEditor;
// using UnityEditor.SceneManagement;

// public class PrelaunchScene
// {

//     [MenuItem("Edit/Play-Stop, But From Prelaunch Scene %0")]
//     public static void PlayFromPrelaunchScene()
//     {
//         if (EditorApplication.isPlaying == true)
//         {
//             EditorApplication.isPlaying = false;
//             return;
//         }

//         EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
//         EditorSceneManager.OpenScene("Assets/Scenes/main.unity");
//         EditorApplication.isPlaying = true;
//     }
// }
