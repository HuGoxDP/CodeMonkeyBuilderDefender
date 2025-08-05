using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IResourceScanner
    {
        int CountResourceMatches(ResourceTypeSo resourceType, Vector3 position, float range);
        Collider2D[] FindResourceMatches(ResourceTypeSo resourceType, Vector3 position, float range);
    }
}