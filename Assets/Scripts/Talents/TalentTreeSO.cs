using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="RPG/Talents/Talent Tree")]
public class TalentTreeSO : ScriptableObject
{
    public List<TalentNodeSO> nodes; // все ноды всех веток (фильтруем по branch)
    public int startPoints = 0;      // стартовые очки талантов
}
