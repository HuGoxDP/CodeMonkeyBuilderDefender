using System;
using UnityEngine;

namespace _Project.Scripts.Architecture.Refactoring
{
    public abstract class OverlayConfigBase : ScriptableObject
    {
        public abstract Type OverlayType { get; }
    }
}