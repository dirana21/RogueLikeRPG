using UnityEngine;
using Game.Shared.Stats;

[System.Serializable]
public struct TalentEffectDef
{
    public string statId;
    public ModifierOp op;
    public float amount;
    public int order;
    public float duration;
}