using EventBusEx.@event;
using UnityEngine;

namespace EventBusEx
{
    public class PlayerController : MonoBehaviour
    {
        private CharacterController _characterController;

        [SerializeField] private float _speed = 0.1f;
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private float _damageCooldown = 1f;
        
        private int _currentHealth;
        private float _lastDamageTime = -999f;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _currentHealth = _maxHealth;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                _characterController.Move(_speed * Vector3.left);
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                _characterController.Move(_speed * Vector3.right);
            }
            
            if (Input.GetKey(KeyCode.W))
            {
                _characterController.Move(_speed * Vector3.forward);
            }
            
            if (Input.GetKey(KeyCode.S))
            {
                _characterController.Move(_speed * Vector3.back);
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag("Enemy"))
            {
                if (Time.time >= _lastDamageTime + _damageCooldown)
                {
                    TakeDamage(10);
                    _lastDamageTime = Time.time;
                }
            }
        }

        private void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            _currentHealth = Mathf.Max(_currentHealth, 0);
            
            EventBus.Instance.NotifyAll(new PlayerDamagedEvent(damage, _currentHealth, transform.position));
            
            Debug.Log($"Player took {damage} damage! Current health: {_currentHealth}");
        }
    }
}
