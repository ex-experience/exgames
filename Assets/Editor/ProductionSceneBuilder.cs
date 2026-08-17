#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using EX360.Core;

namespace EX360.Editor
{
    public static class ProductionSceneBuilder
    {
        public const string ScenePath = "Assets/Scenes/Boot.unity";

        [MenuItem("EX 360/Generate Production Scene")]
        public static string EnsureScene()
        {
            Directory.CreateDirectory("Assets/Scenes");
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var bootstrap = new GameObject("EX360_BOOTSTRAP");
            bootstrap.AddComponent<GameBootstrap>();
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[EX360] Production scene generated: " + ScenePath);
            return ScenePath;
        }
    }
}
#endif
