using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using static AssemblyLoader;

namespace HarmonyInstallChecker
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class HarmonyInstallChecker : MonoBehaviour
    {
        private const string gameDataFolder = "GameData";
        private const string gameDataHarmonyFolder = "000_Harmony";
        private const string checkerAssemblyTitle = "HarmonyInstallChecker";
        private const string harmonyV1AssemblyTitle = "Harmony";
        private const string harmonyV2AssemblyTitle = "0Harmony";

        public void Start()
        {
            // prevent the checker from running more than once in case there is a duplicate installation
            GameObject instance = GameObject.Find(nameof(HarmonyInstallChecker));
            if (instance != gameObject)
            {
                return;
            }
                
            List<LoadedAssembly> harmonyAssemblies = new List<LoadedAssembly>();
            List<LoadedAssembly> harmonyCheckerAssemblies = new List<LoadedAssembly>();
            string message = string.Empty;

            foreach (LoadedAssembly loadedAssembly in loadedAssemblies)
            {
                AssemblyName loadedAssemblyName = loadedAssembly.assembly.GetName();
                if (loadedAssemblyName.Name == checkerAssemblyTitle)
                {
                    harmonyCheckerAssemblies.Add(loadedAssembly);
                }
                else if (loadedAssemblyName.Name == harmonyV1AssemblyTitle || loadedAssemblyName.Name == harmonyV2AssemblyTitle)
                {
                    harmonyAssemblies.Add(loadedAssembly);
                }
            }

            if (harmonyCheckerAssemblies.Count > 1)
            {
                message += "Multiple Harmony installations detected, please read the following message and correct your installation !\n\n";

                string rightPath = Path.Combine(gameDataFolder, gameDataHarmonyFolder);
                bool rightPathExists = false;

                foreach (LoadedAssembly checkerAssembly in harmonyCheckerAssemblies)
                {
                    int relativePathIndex = checkerAssembly.path.IndexOf(gameDataFolder, StringComparison.InvariantCultureIgnoreCase);
                    if (relativePathIndex >= 0)
                    {
                        string path = checkerAssembly.path.Substring(relativePathIndex);
                        path = Path.GetDirectoryName(path);
                        if (rightPath == path)
                        {
                            rightPathExists = true;
                        }
                        else
                        {
                            message += $"Harmony is <b><color=red>wrongly</b></color> installed in <b><color=white>{path}</b></color>\n";
                        }
                    }
                    else
                    {
                        message += $"Harmony is <b><color=red>wrongly</b></color> installed in <b><color=white>{checkerAssembly.path}</b></color>\n";
                    }
                }

                if (rightPathExists)
                {
                    message += $"\nHarmony is <b><color=green>correctly</b></color> installed in <b><color=white>{rightPath}</b></color>, remove the other installation(s).";
                }
                else
                {
                    message += $"\nHarmony should be installed in <b><color=white>{rightPath}</b></color> !";
                }
            }

            if (message.Length == 0 && harmonyAssemblies.Count > 1)
            {
                Version v2 = new Version(2, 0, 0, 0);
                message += "Multiple Harmony installations detected, please close KSP and correct your installation !\n\n";

                string rightPath = Path.Combine(gameDataFolder, gameDataHarmonyFolder);
                bool rightPathExists = false;

                foreach (LoadedAssembly harmonyAssembly in harmonyAssemblies)
                {
                    AssemblyName harmonyAssemblyName = AssemblyName.GetAssemblyName(harmonyAssembly.path);

                    int relativePathIndex = harmonyAssembly.path.IndexOf(gameDataFolder, StringComparison.InvariantCultureIgnoreCase);
                    if (relativePathIndex >= 0)
                    {
                        string path = harmonyAssembly.path.Substring(relativePathIndex);
                        path = Path.GetDirectoryName(path);
                        if (rightPath == path)
                        {
                            rightPathExists = true;
                            message += $"Harmony {harmonyAssemblyName.Version} is <b><color=green>correctly</b></color> installed in <b><color=white>{path}</b></color>\nRemove the other installation(s).\n";
                        }
                        else
                        {
                            message += $"Harmony {harmonyAssemblyName.Version} is <b><color=red>wrongly</b></color> installed in <b><color=white>{path}</b></color>\n";
                        }
                    }
                    else
                    {
                        message += $"Harmony {harmonyAssemblyName.Version} is <b><color=red>wrongly</b></color> installed in <b><color=white>{harmonyAssembly.path}</b></color>\n";
                    }

                    if (harmonyAssemblyName.Version < v2)
                    {
                        message += $"Also, this is Harmony v1, which is depreciated and <b><color=red>conflicts</b></color> with this distribution of Harmony v2.\nRemove that mod, and please ask the author to use the community distributed Harmony instead.\n";
                    }

                    message += "\n";
                }

                if (!rightPathExists)
                {
                    message += $"\nHarmony should be installed in <b><color=white>{rightPath}</b></color> !";
                }
            }

            if (message.Length > 0)
            {
                Debug.LogError(Regex.Replace(message, "<.*?>", string.Empty));

                PopupDialog.SpawnPopupDialog(
                    new Vector2(0.5f, 1.0f),
                    new Vector2(0.5f, 1.0f),
                    new MultiOptionDialog(
                        "Harmony installation error",
                        message,
                        "Harmony installation error",
                        HighLogic.UISkin,
                        new Rect(0.5f, 0.9f, 600f, 60f),
                        new DialogGUIFlexibleSpace(),
                        new DialogGUIHorizontalLayout(
                            new DialogGUIFlexibleSpace(),
                            new DialogGUIButton("Quit KSP", () => Application.Quit(), 150f, 30f, true),
                            new DialogGUIFlexibleSpace()
                        )
                    ),
                    true,
                    HighLogic.UISkin,
                    true);
            }
        }
    }
}
