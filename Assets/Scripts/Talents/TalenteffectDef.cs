using UnityEngine;
using Game.Shared.Stats;

[System.Serializable]
public struct TalentEffectDef
{
    public string statId;           // напр. CharacterStats.Strength
    public ModifierOp op;           // Add / Mul
    public float amount;            // 5  или 1.10f (+10%)
    public int order;               // порядок применения (как у StatModifier)
    public float duration;          // сек; 0 или <0 = бессрочно
}