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
                    // Handle different package sources
                    switch (package.Source)
                    {
                        case PackageSource.LocalPackage:
                            Assets.ImportAsset(package.FileName, package.FolderPath);
                            Debug.Log($"Successfully imported local package: {package.DisplayName}");
                            break;
                        
                        case PackageSource.GitRepository:
                            InstallGitPackage(package);
                            Debug.Log($"Successfully added Git package to manifest: {package.DisplayName}");
                            break;
                        
                        default:
                            Debug.LogWarning($"Unknown package source for {package.DisplayName}");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to import {package.DisplayName}: {e.Message}");
                }
            }
        }
        
        private static void InstallGitPackage(AssetPackageInfo package)
        {
            if (string.IsNullOrEmpty(package.GitURL))
            {
                Debug.LogError($"Git URL is empty for package: {package.DisplayName}");
                return;
            }
            
            // Try to get package name from package.json first
            string packageName = GetPackageNameFromPackageJson(package.GitURL);
            
            if (string.IsNullOrEmpty(packageName))
            {
                // Fallback to URL parsing if package.json fetch fails
                packageName = ExtractPackageNameFromGitUrl(package.GitURL);
                Debug.LogWarning($"Could not fetch package.json for {package.DisplayName}, using generated name: {packageName}");
            }
            else
            {
                Debug.Log($"Successfully extracted package name from package.json: {packageName}");
            }
            
            if (string.IsNullOrEmpty(packageName))
            {
                Debug.LogError($"Could not determine package name for {package.DisplayName}");
                return;
            }
            
            // Add the package to the manifest
            AddPackageToManifest(packageName, package.GitURL);
        }
        
        private static string ExtractPackageNameFromGitUrl(string gitUrl)
        {
            try
            {
                // Parse Git URL to extract repository information
                // Format: https://github.com/user/repo.git?path=path/to/package#version
                
                // Remove Git protocol and extract repo path
                var uri = new System.Uri(gitUrl.Split('?')[0]); // Remove query parameters
                string repoPath = uri.AbsolutePath.TrimStart('/').Replace(".git", "");
                
                // Extract path from query parameters if exists
                string packagePath = "";
                if (gitUrl.Contains("?path="))
                {
                    var queryStart = gitUrl.IndexOf("?path=") + 6;
                    var queryEnd = gitUrl.IndexOf("#", queryStart);
                    if (queryEnd == -1) queryEnd = gitUrl.Length;
                    
                    packagePath = gitUrl.Substring(queryStart, queryEnd - queryStart);
                }
                
                // Try to construct package name based on common patterns
                
                // Pattern 1: If it's a Unity-style package (com.company.package)
                if (repoPath.Contains("/com."))
                {
                    var comIndex = repoPath.LastIndexOf("/com.");
                    return repoPath.Substring(comIndex + 1);
                }
                
                // Pattern 2: Check if package path contains standard prefixes
                if (!string.IsNullOrEmpty(packagePath))
                {
                    var parts = packagePath.Split('/');
                    foreach (var part in parts)
                    {
                        if (part.StartsWith("com.") || part.StartsWith("jp.") || 
                            part.StartsWith("org.") || part.StartsWith("io.") ||
                            part.StartsWith("unity."))
                        {
                            return part;
                        }
                    }
                }
                
                // Pattern 3: Generate package name from repository
                var ownerRepo = repoPath.Split('/');
                if (ownerRepo.Length >= 2)
                {
                    var owner = ownerRepo[0].ToLower().Replace("-", "").Replace("_", "");
                    var repo = ownerRepo[1].ToLower().Replace("-", "").Replace("_", "");
                    
                    // Use appropriate TLD based on common patterns
                    string tld = "com";
                    if (owner.Contains("unity")) tld = "unity";
                    
                    return $"{tld}.{owner}.{repo}";
                }
                
                // Fallback: use repo name directly
                var repoName = System.IO.Path.GetFileName(repoPath);
                return repoName.ToLower().Replace("-", "").Replace("_", "");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error parsing Git URL {gitUrl}: {ex.Message}");
                return null;
            }
        }
        
        private static void AddPackageToManifest(string packageName, string gitUrl)
        {
            // Read the manifest file
            string manifestPath = "Packages/manifest.json";
            string manifestContent = File.ReadAllText(manifestPath);
            
            // Check if the package is already in the manifest
            if (manifestContent.Contains($"\"{packageName}\""))
            {
                Debug.Log($"Package {packageName} is already in the manifest");
                return;
            }
            
            // Simple JSON manipulation to add the package
            int dependenciesIndex = manifestContent.IndexOf("\"dependencies\": {", StringComparison.Ordinal) + "\"dependencies\": {".Length;
            string packageEntry = $"\n    \"{packageName}\": \"{gitUrl}\",";
            
            var manifestJson = new System.Text.StringBuilder(manifestContent);
            manifestJson.Insert(dependenciesIndex, packageEntry);
            
            // Write back to the manifest
            File.WriteAllText(manifestPath, manifestJson.ToString());
            
            Debug.Log($"Added package '{packageName}' to manifest.json");
            
            // Refresh the package database
            UnityEditor.PackageManager.Client.Resolve();
        }
        
        // Get package name from package.json file in the repository
        private static string GetPackageNameFromPackageJson(string gitUrl)
        {
            try
            {
                // Parse Git URL to construct raw file URL
                string packageJsonUrl = ConstructPackageJsonUrl(gitUrl);
                
                if (string.IsNullOrEmpty(packageJsonUrl))
                {
                    return null;
                }
                
                Debug.Log($"Attempting to fetch package.json from: {packageJsonUrl}");
                
                // Download package.json content
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Headers.Add("User-Agent", "Unity Package Manager");
                    string jsonContent = webClient.DownloadString(packageJsonUrl);
                    
                    // Parse JSON to extract package name
                    return ExtractPackageNameFromJson(jsonContent);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Failed to fetch package.json: {ex.Message}");
                return null;
            }
        }
        
        private static string ConstructPackageJsonUrl(string gitUrl)
        {
            try
            {
                // Convert Git URL to raw file URL
                // https://github.com/user/repo.git?path=path/to/package#version
                // -> https://raw.githubusercontent.com/user/repo/branch/path/to/package/package.json
                
                var uri = new System.Uri(gitUrl.Split('?')[0]);
                string repoPath = uri.AbsolutePath.TrimStart('/').Replace(".git", "");
                
                // Extract path from query parameters
                string packagePath = "";
                if (gitUrl.Contains("?path="))
                {
                    var queryStart = gitUrl.IndexOf("?path=") + 6;
                    var queryEnd = gitUrl.IndexOf("#", queryStart);
                    if (queryEnd == -1) queryEnd = gitUrl.Length;
                    
                    packagePath = gitUrl.Substring(queryStart, queryEnd - queryStart);
                }
                
                // Extract branch/tag from fragment
                string branch = "main"; // Default branch
                if (gitUrl.Contains("#"))
                {
                    var fragmentStart = gitUrl.LastIndexOf("#") + 1;
                    branch = gitUrl.Substring(fragmentStart);
                }
                
                // Construct raw GitHub URL
                string baseUrl = $"https://raw.githubusercontent.com/{repoPath}/{branch}";
                
                if (!string.IsNullOrEmpty(packagePath))
                {
                    return $"{baseUrl}/{packagePath}/package.json";
                }
                else
                {
                    return $"{baseUrl}/package.json";
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error constructing package.json URL: {ex.Message}");
                return null;
            }
        }
        
        private static string ExtractPackageNameFromJson(string jsonContent)
        {
            try
            {
                // Simple JSON parsing to extract "name" field
                // Look for "name": "package-name"
                var lines = jsonContent.Split('\n');
                
                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    if (trimmedLine.StartsWith("\"name\""))
                    {
                        // Extract the value between quotes after the colon
                        var colonIndex = trimmedLine.IndexOf(':');
                        if (colonIndex != -1)
                        {
                            var valueStart = trimmedLine.IndexOf('"', colonIndex) + 1;
                            var valueEnd = trimmedLine.IndexOf('"', valueStart);
                            
                            if (valueStart > 0 && valueEnd > valueStart)
                            {
                                return trimmedLine.Substring(valueStart, valueEnd - valueStart);
                            }
                        }
                    }
                }
                
                return null;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error parsing package.json: {ex.Message}");
                return null;
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
            
            // Create the custom packages directory if it doesn't exist
            var config = LoadConfig();
            if (config != null && config.LoadCustomPackages)
            {
                string path = config.CustomPackagesPath;
                if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
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
        
            public static bool IsValidFolder(string folderPath)
            {
                return Directory.Exists(folderPath) || AssetDatabase.IsValidFolder(folderPath);
            }
        }
    }
}