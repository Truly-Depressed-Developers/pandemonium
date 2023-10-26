using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Editor {
    /// <summary>
    /// Replaces keywords in a Script file with useful data defined by you.
    /// Needs to be in an "Editor" folder to work.
    /// </summary>
    public class KeywordReplace : AssetModificationProcessor {
        public static void OnWillCreateAsset(string path) {
            if (!path.EndsWith(".cs.meta")) return;

            // Wait for a moment to let Unity finish creating the asset
            EditorApplication.delayCall += () => ModifyScriptFile(path);
        }

        private static void ModifyScriptFile(string metaFilePath) {
            string originalFilePath = AssetDatabase.GetAssetPathFromTextMetaFilePath(metaFilePath);
            List<string> lines = File.ReadAllLines(originalFilePath).ToList();

            SetNamespace(metaFilePath, lines);

            File.WriteAllLines(originalFilePath, lines);
            AssetDatabase.Refresh();
        }

        private static void SetNamespace(string metaFilePath, List<string> lines) {
            string properNamespace = DetermineProperNamespace(metaFilePath);

            int namespaceStart = lines.FindIndex(line => line.Contains("#NAMESPACE_START#"));
            int namespaceEnd = lines.FindIndex(line => line.Contains("#NAMESPACE_END#"));

            HashSet<string> noNamespaceDirectories = new() { "Scripts", "Assets" };
            if (noNamespaceDirectories.Contains(properNamespace)) {
                for (int i = namespaceStart + 1; i < namespaceEnd; i++) {
                    lines[i] = lines[i].Remove(0, 4);
                }
                lines.RemoveAt(namespaceEnd);
                lines.RemoveAt(namespaceStart);
            } else {
                lines[namespaceStart] = lines[namespaceStart].Replace("#NAMESPACE_START#", $"namespace {properNamespace} {{");
                lines[namespaceEnd] = lines[namespaceEnd].Replace("#NAMESPACE_END#", "}");
            }
        }

        private static string DetermineProperNamespace(string metaFilePath) {
            string namespacePath = metaFilePath[..metaFilePath.LastIndexOf('/')];
            namespacePath = namespacePath
                .Replace("Assets/", "")
                .Replace("Scripts/", "")
                .Replace('/', '.');
            return namespacePath;
        }
    }
}
