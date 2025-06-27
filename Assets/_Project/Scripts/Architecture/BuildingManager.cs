using System;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class BuildingManager : MonoBehaviour
    {
        private void Update()
        {
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
