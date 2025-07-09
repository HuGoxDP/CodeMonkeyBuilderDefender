using System;
using UnityEngine;

namespace Plugins.ProjectSetup.Editor
{
    [Serializable]
    public class AssetPackageInfo
    {
        [Header("Package Info")]
        public string DisplayName;
        public string FileName;
        public string FolderPath;
        
        [Header("Settings")]
        public AssetCategory Category;
        public bool EnabledByDefault = true;
        public PackageSource Source = PackageSource.LocalPackage;
        
        [Header("Git Settings")]
        [Tooltip("For Git packages: URL format: https://github.com/user/repo.git?path=path/to/package#version")]
        public string GitURL;
        
        [Header("Optional")]
        [TextArea(2, 4)]
        public string Description;
        
        public AssetPackageInfo(string displayName, string fileName, string folderPath, AssetCategory category = AssetCategory.EditorTools)
        {
            this.DisplayName = displayName;
            this.FileName = fileName;
            this.FolderPath = folderPath;
            this.Category = category;
            this.Source = PackageSource.LocalPackage;
        }
        
        // Constructor for Git packages
        public AssetPackageInfo(string displayName, string gitUrl, AssetCategory category = AssetCategory.RuntimeLibraries)
        {
            this.DisplayName = displayName;
            this.GitURL = gitUrl;
            this.Category = category;
            this.Source = PackageSource.GitRepository;
        }
    }
    
    [Serializable]
    public enum AssetCategory
    {
        EditorTools,
        RuntimeLibraries,
        CustomUtils
    }
    
    [Serializable]
    public enum PackageSource
    {
        LocalPackage,
        GitRepository
    }
}
