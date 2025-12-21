using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="RPG/Talents/Talent Tree")]
public class TalentTreeSO : ScriptableObject
{
    public List<TalentNodeSO> nodes;
    public int startPoints = 0;
}
