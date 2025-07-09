using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Plugins.ProjectSetup.Editor
{
    [CreateAssetMenu(fileName = "AssetPackageConfig", menuName = "Project Setup/Asset Package Config")]
    public class AssetPackageConfig : ScriptableObject
    {
        [Header("Asset Packages Configuration")]
        [SerializeField] private List<AssetPackageInfo> _packages = new List<AssetPackageInfo>();
        
        [Header("Custom Packages Settings")]
        [SerializeField] private bool _loadCustomPackages = true;
        [SerializeField] private string _customPackagesPath = "Assets/Plugins/ProjectSetup/ScriptableObject/Assets";
        
        [Header("Folder Structure Settings")]
        [SerializeField] private bool _moveScenesToProject = true;
        [SerializeField] private bool _moveSettingsToProject = true;
        [SerializeField] private bool _deleteTutorialInfo = true;
        
        public List<AssetPackageInfo> Packages => _packages;
        public bool MoveScenesToProject => _moveScenesToProject;
        public bool MoveSettingsToProject => _moveSettingsToProject;
        public bool DeleteTutorialInfo => _deleteTutorialInfo;
        public bool LoadCustomPackages => _loadCustomPackages;
        public string CustomPackagesPath => _customPackagesPath;
        
        // Методы для получения пакетов по категориям
        public List<AssetPackageInfo> GetPackagesByCategory(AssetCategory category)
        {
            return _packages.FindAll(p => p.Category == category);
        }
        
        public List<AssetPackageInfo> GetEnabledPackages()
        {
            return _packages.FindAll(p => p.EnabledByDefault);
        }
        
        // Метод для добавления нового пакета (для будущего drag&drop функционала)
        public void AddPackage(AssetPackageInfo package)
        {
            if (!_packages.Contains(package))
            {
                _packages.Add(package);
            }
        }
        
        // Load custom packages from ScriptableObjects in the custom packages directory
        public void LoadCustomPackagesFromFiles()
        {
            if (!_loadCustomPackages) return;
            
            // Create the directory if it doesn't exist
            if (!Directory.Exists(_customPackagesPath))
            {
                Directory.CreateDirectory(_customPackagesPath);
                AssetDatabase.Refresh();
            }
            
            // Find all CustomPackageCollection scriptable objects
            string[] guids = AssetDatabase.FindAssets("t:CustomPackageCollection", new[] { _customPackagesPath });
            
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                CustomPackageCollection collection = AssetDatabase.LoadAssetAtPath<CustomPackageCollection>(path);
                
                if (collection != null && collection.Packages != null)
                {
                    foreach (var package in collection.Packages)
                    {
                        AddPackage(package);
                    }
                }
            }
        }
        
        // Инициализация с базовыми пакетами
        [ContextMenu("Initialize Default Packages")]
        public void InitializeDefaults()
        {
            _packages.Clear();
            
            // Load custom packages from ScriptableObjects
            LoadCustomPackagesFromFiles();
        }
    }
}
