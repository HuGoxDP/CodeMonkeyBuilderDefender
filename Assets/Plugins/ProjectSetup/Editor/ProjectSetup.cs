using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static System.Environment;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;

namespace Plugins.ProjectSetup.Editor
{
    public static class ProjectSetup {
        
        [MenuItem("Tools/Setup/Import Essential Assets", priority = 1)]
        public static void ImportEssentials() {
            // Загружаем конфиг и импортируем все включенные по умолчанию пакеты
            var config = LoadConfig();
            if (config != null)
            {
                var enabledPackages = config.GetEnabledPackages();
                ImportSelectedAssets(enabledPackages);
            }
            else
            {
                Debug.LogWarning("AssetPackageConfig not found. Please create one using Tools/Setup/Project Setup Window");
            }
        }

        [MenuItem("Tools/Setup/Create Folders", priority = 2)]
        public static void CreateFolders() {
            var config = LoadConfig();
            bool moveScenes = config != null ? config.MoveScenesToProject : true;
            bool moveSettings = config != null ? config.MoveSettingsToProject : true;
            bool deleteTutorial = config != null ? config.DeleteTutorialInfo : true;
            
            CreateSelectedFolders(moveScenes, moveSettings, deleteTutorial);
        }

        // Новые методы для работы с конфигурационной системой
        public static void ImportSelectedAssets(List<AssetPackageInfo> packagesToImport)
        {
            if (packagesToImport == null || packagesToImport.Count == 0)
            {
                Debug.LogWarning("No packages to import.");
                return;
            }

            foreach (var package in packagesToImport)
            {
                try
                {
                    Assets.ImportAsset(package.FileName, package.FolderPath);
                    Debug.Log($"Successfully imported: {package.DisplayName}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to import {package.DisplayName}: {e.Message}");
                }
            }
        }
        
        public static void CreateSelectedFolders(bool moveScenes, bool moveSettings, bool deleteTutorial)
        {
            // Создаем основную структуру папок
            Folders.Create("_Project", "Animation", "Art", "Materials", "Prefabs", "Scripts/Tests", "Scripts/Architecture");
            Refresh();
            
            // Выборочно выполняем дополнительные действия
            if (moveScenes)
            {
                Folders.Move("_Project", "Scenes");
            }
            
            if (moveSettings)
            {
                Folders.Move("_Project", "Settings");
            }
            
            if (deleteTutorial)
            {
                Folders.Delete("TutorialInfo");
            }
            
            Refresh();
        }
        
        private static AssetPackageConfig LoadConfig()
        {
            string[] guids = AssetDatabase.FindAssets("t:AssetPackageConfig");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return AssetDatabase.LoadAssetAtPath<AssetPackageConfig>(path);
            }
            return null;
        }

        static class Assets {
            public static void ImportAsset(string asset, string folder) {
                string basePath;
                if (OSVersion.Platform is PlatformID.MacOSX or PlatformID.Unix) {
                    string homeDirectory = GetFolderPath(SpecialFolder.Personal);
                    basePath = Combine(homeDirectory, "Library/Unity/Asset Store-5.x");
                } else {
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