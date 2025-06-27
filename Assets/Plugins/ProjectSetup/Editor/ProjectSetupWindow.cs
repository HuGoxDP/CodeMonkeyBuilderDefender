using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Plugins.ProjectSetup.Editor
{
    public class ProjectSetupWindow : EditorWindow
    {
        private Vector2 scrollPosition;
        private AssetPackageConfig config;
        
        // Динамические словари для хранения состояний чекбоксов
        private Dictionary<AssetPackageInfo, bool> packageSelections = new Dictionary<AssetPackageInfo, bool>();
        
        // Настройки структуры проекта
        private bool createFolders = true;
        private bool moveScenes = true;
        private bool moveSettings = true;
        private bool deleteTutorialInfo = true;
        
        [MenuItem("Tools/Setup/Project Setup Window", priority = 0)]
        public static void ShowWindow()
        {
            var window = GetWindow<ProjectSetupWindow>("Project Setup");
            window.minSize = new Vector2(400, 300);
        }
        
        private void OnEnable()
        {
            LoadConfig();
            InitializeSelections();
        }
        
        private void LoadConfig()
        {
            // Ищем конфиг в проекте
            string[] guids = AssetDatabase.FindAssets("t:AssetPackageConfig");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                config = AssetDatabase.LoadAssetAtPath<AssetPackageConfig>(path);
            }
            
            // Если конфига нет, создаем временный для демонстрации
            if (config == null)
            {
                config = CreateInstance<AssetPackageConfig>();
                config.InitializeDefaults();
            }
        }
        
        private void InitializeSelections()
        {
            packageSelections.Clear();
            foreach (var package in config.Packages)
            {
                packageSelections[package] = package.EnabledByDefault;
            }
            
            // Загружаем настройки структуры из конфига
            moveScenes = config.MoveScenesToProject;
            moveSettings = config.MoveSettingsToProject;
            deleteTutorialInfo = config.DeleteTutorialInfo;
        }
        
        private void OnGUI()
        {
            if (config == null)
            {
                EditorGUILayout.HelpBox("Asset Package Config not found. Create one in Project window: Right-click → Create → Project Setup → Asset Package Config", MessageType.Warning);
                
                if (GUILayout.Button("Create Config"))
                {
                    CreateNewConfig();
                }
                return;
            }
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            EditorGUILayout.LabelField("Project Setup Configuration", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            DrawAssetPackagesSection();
            EditorGUILayout.Space();
            
            DrawProjectStructureSection();
            EditorGUILayout.Space();
            
            DrawActionButtons();
            
            EditorGUILayout.EndScrollView();
        }
        
        private void DrawAssetPackagesSection()
        {
            EditorGUILayout.LabelField("Asset Packages", EditorStyles.boldLabel);
            
            // Группируем пакеты по категориям
            var categories = System.Enum.GetValues(typeof(AssetCategory)).Cast<AssetCategory>();
            
            foreach (var category in categories)
            {
                var categoryPackages = config.GetPackagesByCategory(category);
                if (categoryPackages.Count == 0) continue;
                
                EditorGUILayout.BeginVertical("box");
                
                // Заголовок категории с кнопкой "Select All"
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(GetCategoryDisplayName(category), EditorStyles.boldLabel);
                
                if (GUILayout.Button("All", GUILayout.Width(40)))
                {
                    SetCategorySelection(category, true);
                }
                if (GUILayout.Button("None", GUILayout.Width(40)))
                {
                    SetCategorySelection(category, false);
                }
                EditorGUILayout.EndHorizontal();
                
                // Список пакетов в категории
                EditorGUI.indentLevel++;
                foreach (var package in categoryPackages)
                {
                    bool currentValue = packageSelections.ContainsKey(package) ? packageSelections[package] : package.EnabledByDefault;
                    bool newValue = EditorGUILayout.ToggleLeft(package.DisplayName, currentValue);
                    packageSelections[package] = newValue;
                    
                    // Показываем описание если есть
                    if (!string.IsNullOrEmpty(package.Description))
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.LabelField(package.Description, EditorStyles.miniLabel);
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
                
                EditorGUILayout.EndVertical();
            }
        }
        
        private void DrawProjectStructureSection()
        {
            EditorGUILayout.LabelField("Project Structure", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            
            createFolders = EditorGUILayout.ToggleLeft("Create Project Folders", createFolders);
            
            EditorGUI.BeginDisabledGroup(!createFolders);
            EditorGUI.indentLevel++;
            moveScenes = EditorGUILayout.ToggleLeft("Move Scenes to _Project", moveScenes);
            moveSettings = EditorGUILayout.ToggleLeft("Move Settings to _Project", moveSettings);
            deleteTutorialInfo = EditorGUILayout.ToggleLeft("Delete TutorialInfo folder", deleteTutorialInfo);
            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.EndVertical();
        }
        
        private void DrawActionButtons()
        {
            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
            
            var selectedPackages = GetSelectedPackages();
            EditorGUILayout.LabelField($"Selected: {selectedPackages.Count} packages", EditorStyles.miniLabel);
            
            EditorGUILayout.BeginHorizontal();
            
            EditorGUI.BeginDisabledGroup(selectedPackages.Count == 0);
            if (GUILayout.Button("Import Selected Assets", GUILayout.Height(30)))
            {
                ImportSelectedAssets();
            }
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.BeginDisabledGroup(!createFolders);
            if (GUILayout.Button("Setup Project Structure", GUILayout.Height(30)))
            {
                SetupProjectStructure();
            }
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.BeginDisabledGroup(selectedPackages.Count == 0 && !createFolders);
            if (GUILayout.Button("Do Everything", GUILayout.Height(35)))
            {
                DoCompleteSetup();
            }
            EditorGUI.EndDisabledGroup();
        }
        
        private string GetCategoryDisplayName(AssetCategory category)
        {
            return category switch
            {
                AssetCategory.EditorTools => "Editor Tools",
                AssetCategory.RuntimeLibraries => "Runtime Libraries", 
                AssetCategory.CustomUtils => "Custom Utils",
                _ => category.ToString()
            };
        }
        
        private void SetCategorySelection(AssetCategory category, bool selected)
        {
            var categoryPackages = config.GetPackagesByCategory(category);
            foreach (var package in categoryPackages)
            {
                packageSelections[package] = selected;
            }
        }
        
        private List<AssetPackageInfo> GetSelectedPackages()
        {
            return packageSelections.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
        }
        
        private void ImportSelectedAssets()
        {
            var selectedPackages = GetSelectedPackages();
            if (selectedPackages.Count > 0)
            {
                ProjectSetup.ImportSelectedAssets(selectedPackages);
                Debug.Log($"Started importing {selectedPackages.Count} selected assets.");
            }
            else
            {
                Debug.LogWarning("No assets selected for import.");
            }
        }
        
        private void SetupProjectStructure()
        {
            if (createFolders)
            {
                ProjectSetup.CreateSelectedFolders(moveScenes, moveSettings, deleteTutorialInfo);
                Debug.Log("Project structure setup completed.");
            }
        }
        
        private void DoCompleteSetup()
        {
            ImportSelectedAssets();
            SetupProjectStructure();
        }
        
        private void CreateNewConfig()
        {
            var newConfig = CreateInstance<AssetPackageConfig>();
            newConfig.InitializeDefaults();
            
            string path = "Assets/Plugins/ProjectSetup/Editor/DefaultAssetPackageConfig.asset";
            AssetDatabase.CreateAsset(newConfig, path);
            AssetDatabase.SaveAssets();
            
            config = newConfig;
            InitializeSelections();
            
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newConfig;
        }
    }
}
