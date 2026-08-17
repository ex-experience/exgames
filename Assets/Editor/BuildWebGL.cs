#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace EX360.Editor
{
    public static class BuildWebGL
    {
        public static void PerformBuild()
        {
            string output = ReadArg("-buildPath", "build/WebGL");
            string scene = ProductionSceneBuilder.EnsureScene();
            Directory.CreateDirectory(output);

            PlayerSettings.productName = "ARABIA STRIKE 360";
            PlayerSettings.companyName = "EX Experience";
            PlayerSettings.bundleVersion = "0.1.0";
            PlayerSettings.runInBackground = true;
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            PlayerSettings.WebGL.decompressionFallback = false;
            PlayerSettings.WebGL.dataCaching = true;

            var options = new BuildPlayerOptions
            {
                scenes = new[] { scene },
                locationPathName = output,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary summary = report.summary;
            Debug.Log($"[EX360] WebGL build result={summary.result} size={summary.totalSize} bytes time={summary.totalTime}");
            if (summary.result != BuildResult.Succeeded)
                throw new Exception("EX360 WebGL build failed: " + summary.result);
        }

        static string ReadArg(string key, string fallback)
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length - 1; i++)
                if (string.Equals(args[i], key, StringComparison.OrdinalIgnoreCase)) return args[i + 1];
            return fallback;
        }
    }
}
#endif
