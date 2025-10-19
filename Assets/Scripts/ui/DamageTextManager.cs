using EventBusEx.@event;
using UnityEngine;

namespace EventBusEx.ui
{
    public class DamageTextManager : MonoBehaviour
    {
        [SerializeField] private FloatingDamageText _floatingDamageTextPrefab;
        [SerializeField] private Transform _canvas;

        private void Start()
        {
            EventBus.Instance.Subscribe<PlayerDamagedEvent>(OnPlayerDamaged);
        }

        private void OnDestroy()
        {
            EventBus.Instance.Unsubscribe<PlayerDamagedEvent>(OnPlayerDamaged);
        }

        private void OnPlayerDamaged(PlayerDamagedEvent e)
        {
            var floatingText = Instantiate(_floatingDamageTextPrefab, _canvas);
            floatingText.Initialize(e.DamageAmount, e.PlayerPosition);
        }
    }
}
