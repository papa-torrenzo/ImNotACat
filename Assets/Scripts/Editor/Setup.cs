using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.AssetDatabase;

namespace com.torrenzo.Foundation
{
    public class Setup {
        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders() {
            Folders.CreateDefaults("_project", "Animation", "Art", "Materials", "Prefabs", "ScriptableObjects",
                "Scripts", "Settings");
            Refresh();
        }

        static class Folders {
            public static void CreateDefaults(string root, params string[] folders) {
                var basepath = Path.Combine(Application.dataPath, root);
                foreach (var folder in folders) {
                    var folderpath = Path.Combine(basepath, folder);
                    if (!Directory.Exists(folderpath)) {
                        Directory.CreateDirectory(folderpath);
                    }
                }
            }
        }
    }
}
