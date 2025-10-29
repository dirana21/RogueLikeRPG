using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class DefaultStatCalculator : IStatCalculator
{
    
    public void RecomputeAll<TKey>(IReadOnlyDictionary<TKey, Stat> stats, IEnumerable<IStatModifier> mods)
    {
        var grouped = mods.GroupBy(m => m.StatId);
        foreach (var kv in stats) {
            var s = kv.Value; 
            float value = s.Base;
            var group = grouped.FirstOrDefault(g => g.Key == s.Id);
            if (group != null) {
                foreach (var m in group.Where(x => x.Op == ModifierOp.Add).OrderBy(x => x.Order)) value += m.Amount;
                float mul = 1f;
                foreach (var m in group.Where(x => x.Op == ModifierOp.Mul).OrderBy(x => x.Order)) mul *= m.Amount;
                value *= mul;
            }
            if (s.Id == "crit_chance" || s.Id == "evasion") value = Mathf.Clamp01(value);
            s.SetComputed(value);
        }
    }
}