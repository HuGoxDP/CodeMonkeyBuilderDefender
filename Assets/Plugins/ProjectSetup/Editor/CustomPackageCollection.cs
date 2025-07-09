using System.Collections.Generic;
using UnityEngine;

namespace Plugins.ProjectSetup.Editor
{
    [CreateAssetMenu(fileName = "NewCustomPackages", menuName = "Project Setup/Custom Package Collection")]
    public class CustomPackageCollection : ScriptableObject
    {
        [Header("Custom Packages")]
        [SerializeField] private List<AssetPackageInfo> _packages = new List<AssetPackageInfo>();
        
        public List<AssetPackageInfo> Packages => _packages;
        
        /// <summary>
        /// Adds a new local package to the collection
        /// </summary>
        public void AddLocalPackage(string displayName, string fileName, string folderPath, AssetCategory category = AssetCategory.EditorTools)
        {
            _packages.Add(new AssetPackageInfo(displayName, fileName, folderPath, category));
        }
        
        /// <summary>
        /// Adds a new Git package to the collection
        /// </summary>
        public void AddGitPackage(string displayName, string gitUrl, AssetCategory category = AssetCategory.RuntimeLibraries)
        {
            _packages.Add(new AssetPackageInfo(displayName, gitUrl, category));
        }
    }
}
