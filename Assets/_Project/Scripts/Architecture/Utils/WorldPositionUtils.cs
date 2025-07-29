using System;
using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Scripts.Architecture.Utils
{
    /// <summary>
    /// Utility class for world position calculations and coordinate transformations
    /// </summary>
    public static class WorldPositionUtils
    {
        private static Camera _cachedMainCamera;

        /// <summary>
        /// Converts screen position to world position using specified camera, forcing Z to zValue
        /// </summary>
        /// <param name="camera">Camera to use for conversion</param>
        /// <param name="screenPosition">Screen position to convert</param>
        /// <param name="zValue">Z coordinate value to force (default: 0f for 2D)</param>
        /// <returns>World position with specified Z value</returns>
        /// <exception cref="ArgumentNullException">Thrown when camera is null</exception>
        public static Vector3 ScreenToWorldPosition(Camera camera, Vector3 screenPosition, float zValue = 0f)
        {
            if (camera == null)
                throw new ArgumentNullException(nameof(camera), "Camera cannot be null for screen to world conversion");

            var worldPosition = camera.ScreenToWorldPoint(screenPosition);
            worldPosition.z = zValue;
            return worldPosition;
        }

        /// <summary>
        /// Converts screen position to 2D world position using main camera
        /// </summary>
        /// <param name="screenPosition">Screen position to convert</param>
        /// <returns>World position with Z=0, or Vector3.zero if main camera not available</returns>
        public static Vector3 ScreenToWorldPosition(Vector3 screenPosition)
        {
            var camera = GetMainCameraSafe();
            return camera != null ? ScreenToWorldPosition(camera, screenPosition) : Vector3.zero;
        }

        /// <summary>
        /// Gets the main camera with caching and safe error handling
        /// </summary>
        /// <returns>Main camera or null if not found</returns>
        [CanBeNull]
        public static Camera GetMainCameraSafe()
        {
            if (_cachedMainCamera == null)
            {
                _cachedMainCamera = Camera.main;

                if (_cachedMainCamera == null)
                {
                    Debug.LogError(
                        "Main camera not found. Ensure there is a camera with the 'MainCamera' tag in the scene."
                    );
                }
            }

            return _cachedMainCamera;
        }

        /// <summary>
        /// Clears the cached main camera. Useful when cameras change during runtime.
        /// </summary>
        public static void ClearCameraCache()
        {
            _cachedMainCamera = null;
        }
    }
}