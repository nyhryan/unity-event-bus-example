using UnityEngine;

namespace EventBusEx.@event
{
    public readonly struct PlayerDamagedEvent : IEvent
    {
        public int DamageAmount { get; }
        public int CurrentHealth { get; }
        
        public Vector3 PlayerPosition { get; }

        public PlayerDamagedEvent(int damageAmount, int currentHealth, Vector3 playerPosition)
        {
            DamageAmount = damageAmount;
            CurrentHealth = currentHealth;
            PlayerPosition = playerPosition;
        }
    }
}