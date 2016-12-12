using UnityEngine;
using System.Collections;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VCode {
    public class MakeCommand : MonoBehaviour {
#if UNITY_EDITOR
        [MenuItem("VCode/Commands/new command")]
        public static void CreateCommandAsset() {
            CommandData asset = ScriptableObject.CreateInstance<CommandData>();
            CreateAsset(asset, "Assets/VCode/Commands", "NewCommand");

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }


        private static void CreateAsset(Object asset, string path, string name) {   // helper class
            if (Directory.Exists(path) == false)  // make path valid
                Directory.CreateDirectory(path);

            AssetDatabase.CreateAsset(asset, path+"/"+name+".asset");
            AssetDatabase.SaveAssets();
        }
#endif
    }
}