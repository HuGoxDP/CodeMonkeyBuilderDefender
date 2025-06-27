using System.Collections.Generic;
using UnityEngine;

namespace Plugins.ProjectSetup.Editor
{
    [CreateAssetMenu(fileName = "AssetPackageConfig", menuName = "Project Setup/Asset Package Config")]
    public class AssetPackageConfig : ScriptableObject
    {
        [Header("Asset Packages Configuration")]
        [SerializeField] private List<AssetPackageInfo> packages = new List<AssetPackageInfo>();
        
        [Header("Folder Structure Settings")]
        [SerializeField] private bool moveScenesToProject = true;
        [SerializeField] private bool moveSettingsToProject = true;
        [SerializeField] private bool deleteTutorialInfo = true;
        
        public List<AssetPackageInfo> Packages => packages;
        public bool MoveScenesToProject => moveScenesToProject;
        public bool MoveSettingsToProject => moveSettingsToProject;
        public bool DeleteTutorialInfo => deleteTutorialInfo;
        
        // Методы для получения пакетов по категориям
        public List<AssetPackageInfo> GetPackagesByCategory(AssetCategory category)
        {
            return packages.FindAll(p => p.Category == category);
        }
        
        public List<AssetPackageInfo> GetEnabledPackages()
        {
            return packages.FindAll(p => p.EnabledByDefault);
        }
        
        // Метод для добавления нового пакета (для будущего drag&drop функционала)
        public void AddPackage(AssetPackageInfo package)
        {
            if (!packages.Contains(package))
            {
                packages.Add(package);
            }
        }
        
        // Инициализация с базовыми пакетами
        [ContextMenu("Initialize Default Packages")]
        public void InitializeDefaults()
        {
            packages.Clear();
            
            // Editor Tools
            packages.Add(new AssetPackageInfo("Advanced PlayerPrefs Window", "Advanced PlayerPrefs Window.unitypackage", "Editor ExtensionsSystem"));
            packages.Add(new AssetPackageInfo("Animation Preview", "Animation Preview v1.2.0 (17 Jul 2024).unitypackage", "Editor ExtensionsSystem"));
            packages.Add(new AssetPackageInfo("Asset Cleaner PRO", "Asset Cleaner PRO - Clean Find References v1.32.unitypackage", "Editor ExtensionsSystem"));
            packages.Add(new AssetPackageInfo("Better Hierarchy", "Better Hierarchy.unitypackage", "Editor ExtensionsSystem"));
            packages.Add(new AssetPackageInfo("Component Names", "Component Names v1.2.1.unitypackage", "Editor ExtensionsSystem"));
            packages.Add(new AssetPackageInfo("Editor Auto Save", "Editor Auto Save.unitypackage", "Editor ExtensionsSystem"));
            packages.Add(new AssetPackageInfo("Editor Console Pro", "Editor Console Pro v3.977.unitypackage", "Editor ExtensionsSystem"));
            packages.Add(new AssetPackageInfo("Selection History", "Selection History.unitypackage", "Editor ExtensionsSystem"));
            packages.Add(new AssetPackageInfo("Fullscreen Editor", "Unity Assets Fullscreen Editor v2.2.8.unitypackage", "Editor ExtensionsSystem"));
            packages.Add(new AssetPackageInfo("vFavorites 2", "vFavorites 2 v2.0.7.unitypackage", "Editor ExtensionsSystem"));
            
            // Runtime Libraries
            packages.Add(new AssetPackageInfo("DOTween Pro", "DOTween Pro v1.0.381.unitypackage", "RunTime ExtensionsSystem", AssetCategory.RuntimeLibraries));
            packages.Add(new AssetPackageInfo("DOTween HOTween", "DOTween HOTween v2.unitypackage", "RunTime ExtensionsSystem", AssetCategory.RuntimeLibraries));
            packages.Add(new AssetPackageInfo("NuGetForUnity", "NuGetForUnity.4.1.1.unitypackage", "RunTime ExtensionsSystem", AssetCategory.RuntimeLibraries));
            packages.Add(new AssetPackageInfo("Hot Reload", "Hot Reload Edit Code Without Compiling v1.12.9.unitypackage", "RunTime ExtensionsSystem", AssetCategory.RuntimeLibraries));
            packages.Add(new AssetPackageInfo("UniTask", "UniTask.2.5.10.unitypackage", "RunTime ExtensionsSystem", AssetCategory.RuntimeLibraries));
            packages.Add(new AssetPackageInfo("Odin Inspector", "Odin Inspector 3.3.1.11.unitypackage", "RunTime ExtensionsSystem", AssetCategory.RuntimeLibraries));
            
            // Custom Utils
            packages.Add(new AssetPackageInfo("Utils", "Utils.unitypackage", "ExtensionsSystem", AssetCategory.CustomUtils));
        }
    }
}
