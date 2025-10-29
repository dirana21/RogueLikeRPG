using System.Collections.Generic;
using System.Linq;

public interface IStatCalculator
{
    void RecomputeAll<TKey>(IReadOnlyDictionary<TKey, Stat> stats, IEnumerable<IStatModifier> mods);
}