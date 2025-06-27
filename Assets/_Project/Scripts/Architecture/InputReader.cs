using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Architecture
{
    public class InputReader : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private InputAction _pointPosition;
        public int Aware;
        private const int Awares = 5;
        [SerializeField] private string _testUp;
        [SerializeField] public string Test;
        protected int TestProt;
        private static int s_dfs;


        public Vector2 PointPosition => _pointPosition.ReadValue<Vector2>();

        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();

        }
    }
}
