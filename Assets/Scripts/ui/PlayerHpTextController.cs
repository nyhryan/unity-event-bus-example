using EventBusEx.@event;
using UnityEngine;
using UnityEngine.UI;

namespace EventBusEx.ui
{
    public class PlayerHpTextController : MonoBehaviour
    {
        private Text _hpText;

        private void Awake()
        {
            _hpText = GetComponent<Text>();
            EventBus.Instance.Subscribe<PlayerDamagedEvent>(OnPlayerDamagedEvent);
        }

        private void OnDisable()
        {
            EventBus.Instance.Unsubscribe<PlayerDamagedEvent>(OnPlayerDamagedEvent);
        }

        private void OnPlayerDamagedEvent(PlayerDamagedEvent e)
        {
            _hpText.text = $"HP: {e.CurrentHealth}";
        }
    }
}
