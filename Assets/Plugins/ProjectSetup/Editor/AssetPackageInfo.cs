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
        
        [Header("Optional")]
        [TextArea(2, 4)]
        public string Description;
        
        public AssetPackageInfo(string displayName, string fileName, string folderPath, AssetCategory category = AssetCategory.EditorTools)
        {
            this.DisplayName = displayName;
            this.FileName = fileName;
            this.FolderPath = folderPath;
            this.Category = category;
        }
    }
    
    [Serializable]
    public enum AssetCategory
    {
        EditorTools,
        RuntimeLibraries,
        CustomUtils
    }
}
