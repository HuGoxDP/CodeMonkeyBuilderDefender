using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static System.Environment;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;

namespace Plugins.ProjectSetup.Editor
{
    public static class ProjectSetup {
        [MenuItem("Tools/Setup/Import Essential Assets", priority = 1)]
        public static void ImportEssentials() {
            #region Editor ExtensionsSystem
            Assets.ImportAsset("Advanced PlayerPrefs Window.unitypackage", "Editor ExtensionsSystem");
            Assets.ImportAsset("Animation Preview v1.2.0 (17 Jul 2024).unitypackage", "Editor ExtensionsSystem");
            Assets.ImportAsset("Asset Cleaner PRO - Clean Find References v1.32.unitypackage", "Editor ExtensionsSystem");
            Assets.ImportAsset("Better Hierarchy.unitypackage", "Editor ExtensionsSystem");
            Assets.ImportAsset("Component Names v1.2.1.unitypackage", "Editor ExtensionsSystem");
            Assets.ImportAsset("Editor Auto Save.unitypackage", "Editor ExtensionsSystem");
            Assets.ImportAsset("Editor Console Pro v3.977.unitypackage", "Editor ExtensionsSystem");
            Assets.ImportAsset("Selection History.unitypackage", "Editor ExtensionsSystem");
            Assets.ImportAsset("Unity Assets Fullscreen Editor v2.2.8.unitypackage", "Editor ExtensionsSystem");
            Assets.ImportAsset("vFavorites 2 v2.0.7.unitypackage", "Editor ExtensionsSystem");
            
            #endregion
            
            #region RunTime ExtensionsSystem
            Assets.ImportAsset("DOTween Pro v1.0.381.unitypackage", "RunTime ExtensionsSystem");
            Assets.ImportAsset("DOTween HOTween v2.unitypackage", "RunTime ExtensionsSystem");
            Assets.ImportAsset("NuGetForUnity.4.1.1.unitypackage", "RunTime ExtensionsSystem");
            Assets.ImportAsset("Hot Reload Edit Code Without Compiling v1.12.9.unitypackage", "RunTime ExtensionsSystem");
            Assets.ImportAsset("UniTask.2.5.10.unitypackage", "RunTime ExtensionsSystem");
            Assets.ImportAsset("Odin Inspector 3.3.1.11.unitypackage", "RunTime ExtensionsSystem");

            #endregion

            #region ExtensionsSystem
            Assets.ImportAsset("Utils.unitypackage", "ExtensionsSystem");
            
            #endregion


        }

        [MenuItem("Tools/Setup/Install Essential Packages", priority = 2)]
        public static void InstallPackages() {
             Packages.InstallPackages(new[] {
            "git+https://github.com/Cysharp/R3.git?path=src/R3.Unity/Assets/R3.Unity",
            "com.unity.project-auditor"
             });
        }

        [MenuItem("Tools/Setup/Create Folders", priority = 3)]
        public static void CreateFolders() {
            Folders.Create("_Project", "Animation", "Art", "Materials", "Prefabs", "Scripts/Tests", "Scripts/Architecture");
            Refresh();
            Folders.Move("_Project", "Scenes");
            Folders.Move("_Project", "Settings");

            Folders.Delete("TutorialInfo");
            Refresh();

            MoveAsset("Assets/InputSystem_Actions.inputactions", "Assets/_Project/Settings/InputSystem_Actions.inputactions");
            DeleteAsset("Assets/Readme.asset");
            Refresh();
        
            // Optional: Disable Domain Reload
            // EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload | EnterPlayModeOptions.DisableSceneReload;
        }

        static class Assets {
            public static void ImportAsset(string asset, string folder) {
                string basePath;
                if (OSVersion.Platform is PlatformID.MacOSX or PlatformID.Unix) {
                    string homeDirectory = GetFolderPath(SpecialFolder.Personal);
                    basePath = Combine(homeDirectory, "Library/Unity/Asset Store-5.x");
                } else {
                    string defaultPath = Combine(GetFolderPath(SpecialFolder.ApplicationData), "Unity");
                    basePath = Combine("D:\\Programs\\UnityHub\\Unity-package\\BestPreset");
                }

                asset = asset.EndsWith(".unitypackage") ? asset : asset + ".unitypackage";

                string fullPath = Combine(basePath, folder, asset);

                if (!File.Exists(fullPath)) {
                    throw new FileNotFoundException($"The asset package was not found at the path: {fullPath}");
                }

                ImportPackage(fullPath, false);
            }
        }

        static class Packages {
            static AddRequest _request;
            static Queue<string> _packagesToInstall = new Queue<string>();

            public static void InstallPackages(string[] packages) {
                foreach (var package in packages) {
                    _packagesToInstall.Enqueue(package);
                }

                if (_packagesToInstall.Count > 0) {
                    StartNextPackageInstallation();
                }
            }

            static async void StartNextPackageInstallation() {
                _request = Client.Add(_packagesToInstall.Dequeue());
            
                while (!_request.IsCompleted) await Task.Delay(10);
            
                if (_request.Status == StatusCode.Success) Debug.Log("Installed: " + _request.Result.packageId);
                else if (_request.Status >= StatusCode.Failure) Debug.LogError(_request.Error.message);

                if (_packagesToInstall.Count > 0) {
                    await Task.Delay(1000);
                    StartNextPackageInstallation();
                }
            }
        }

        static class Folders {
            public static void Create(string root, params string[] folders) {
                var fullpath = Combine(Application.dataPath, root);
                if (!Directory.Exists(fullpath)) {
                    Directory.CreateDirectory(fullpath);
                }

                foreach (var folder in folders) {
                    CreateSubFolders(fullpath, folder);
                }
            }
        
            static void CreateSubFolders(string rootPath, string folderHierarchy) {
                var folders = folderHierarchy.Split('/');
                var currentPath = rootPath;

                foreach (var folder in folders) {
                    currentPath = Combine(currentPath, folder);
                    if (!Directory.Exists(currentPath)) {
                        Directory.CreateDirectory(currentPath);
                    }
                }
            }
        
            public static void Move(string newParent, string folderName) {
                var sourcePath = $"Assets/{folderName}";
                if (IsValidFolder(sourcePath)) {
                    var destinationPath = $"Assets/{newParent}/{folderName}";
                    var error = MoveAsset(sourcePath, destinationPath);

                    if (!string.IsNullOrEmpty(error)) {
                        Debug.LogError($"Failed to move {folderName}: {error}");
                    }
                }
            }
        
            public static void Delete(string folderName) {
                var pathToDelete = $"Assets/{folderName}";

                if (IsValidFolder(pathToDelete)) {
                    DeleteAsset(pathToDelete);
                }
            }
        }
    }
}