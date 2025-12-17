using System.Collections.Generic;
using UnityEngine;

public enum TalentBranch 
{ 
    Vitality, 
    Reflex, 
    Impact, 
    Intelligence, 
    VitalCore 
} 

[CreateAssetMenu(menuName="RPG/Talents/Talent Node")]
public class TalentNodeSO : ScriptableObject
{
    [Header("ID & UI")]
    public string id;                 // Унікальний ID(РК), 
    public string title;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Tree")]
    public TalentBranch branch;
    public List<TalentNodeSO> requirements; 
    public bool comingSoon;

    [Header("Costs")]
    public int pointCost = 1;

    [Header("Effects")]
    public List<TalentEffectDef> effects;
}
