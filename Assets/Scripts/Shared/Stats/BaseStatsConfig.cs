using UnityEngine;

namespace Game.Shared.Stats
{
    [CreateAssetMenu(menuName = "RPG/Base Stats Config", fileName = "PlayerBaseStats")]
    public sealed class BaseStatsConfig : ScriptableObject
    {
        [Header("Attributes")]
        public float agility = 5f;
        public float strength = 5f;
        public float intellect = 5f;
        public float swordRadius = 1.0f;
        public float knockback = 0.5f;
        [Range(0, 1)] public float critChance = 0.1f;
        public float critDamage = 1.5f;
        public float staminaDamage = 5f;
        public float attackSpeed = 1.0f;
        [Range(0, 1)] public float evasion = 0.05f;

        [Header("Vitals")]
        public float maxHealth = 100f;
        public float maxStamina = 50f;
        public float damageTakenMultiplier = 1.0f;

        [Header("Resistances (0..1)")]
        [Range(0, 1)] public float resFire;
        [Range(0, 1)] public float resCold;
        [Range(0, 1)] public float resDark;
        [Range(0, 1)] public float resPoison;
        [Range(0, 1)] public float resCorrosion;
        [Range(0, 1)] public float resLightning;
    }
}