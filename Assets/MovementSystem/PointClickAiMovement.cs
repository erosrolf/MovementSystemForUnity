using MovementSystem.Interfaces;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace MovementSystem
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PointClickAiMovement : MonoBehaviour, IPlayerMovement
    {
        private NavMeshAgent _navMeshAgent;
        private MouseInput _mouseInput;

        [Header("Movement")]
        [SerializeField] private ParticleSystem clickEffect;
        [SerializeField] private LayerMask clickableLayers;
        [SerializeField] private float lookRotationSpeed = 8f;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _mouseInput = new MouseInput();
            _mouseInput.Mouse.MouseClick.performed += _ => HandleControl();
        }

        public void HandleControl()
        {
            if (Camera.main == null)
            {
                Debug.LogError("Main camera not found");
                return;
            }

            if (Physics.Raycast(Camera.main.ScreenPointToRay(_mouseInput.Mouse.MousePosition.ReadValue<Vector2>()),
                    out var hit,
                    float.PositiveInfinity,
                    clickableLayers) == false)
            {
                return;
            }
            
            if (hit.collider.gameObject.GetComponent<NavMeshSurface>() == null)
            {
                Debug.LogError("There is no NavMeshSurface on the clicked surface!");
                return;
            }

            _navMeshAgent.destination = hit.point;
            if (clickEffect != null)
            {
                Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
            }
        }

        private void OnEnable()
        {
            _mouseInput.Enable();
        }

        private void OnDisable()
        {
            _mouseInput.Disable();
        }
    }
}