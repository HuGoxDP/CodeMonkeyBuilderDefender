using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Architecture
{
    public class AssetManager : MonoBehaviour
    {
        private readonly Dictionary<string, object> _assetCache = new();
        private readonly Dictionary<string, AsyncOperationHandle> _operationHandles = new();
        private UniTaskCompletionSource<bool> _initTaskSource;


        private bool _isInitialized;
        public static AssetManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize().Forget();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            ReleaseAll();
        }

        private async UniTaskVoid Initialize()
        {
            if (_isInitialized) return;

            await Addressables.InitializeAsync().ToUniTask();
            _isInitialized = true;
            _initTaskSource?.TrySetResult(true);

            Debug.Log("AssetManager initialized");
        }

        private async UniTask WaitForInitialization()
        {
            if (_isInitialized) return;

            _initTaskSource = new UniTaskCompletionSource<bool>();
            await _initTaskSource.Task;
        }

        public async UniTask<T> LoadAsset<T>(string key)
        {
            if (_assetCache.TryGetValue(key, out var cachedAsset))
            {
                return (T)cachedAsset;
            }

            var handle = Addressables.LoadAssetAsync<T>(key);
            _operationHandles[key] = handle;

            T asset = await handle.ToUniTask();

            if (asset == null)
            {
                Debug.LogError($"Failed to load asset: {key}");
                return default;
            }

            _assetCache[key] = asset;
            return asset;
        }

        public async UniTask<GameObject> InstantiatePrefab(string key, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            await WaitForInitialization();
            var prefab = await LoadAsset<GameObject>(key);
            var instance = Instantiate(prefab, position, rotation, parent);
            return instance;
        }

        public void ReleaseAsset(string key)
        {
            if (_operationHandles.TryGetValue(key, out var handle))
            {
                Addressables.Release(handle);
                _operationHandles.Remove(key);
                _assetCache.Remove(key);
            }
        }

        private void ReleaseAll()
        {
            foreach (var handle in _operationHandles.Values)
            {
                Addressables.Release(handle);
            }

            _operationHandles.Clear();
            _assetCache.Clear();
        }
    }
}