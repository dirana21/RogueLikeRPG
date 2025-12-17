using System.Collections.Generic;
using UnityEngine;
using Game.Shared.Stats;
public class TalentController : MonoBehaviour
    {
        [SerializeField] private TalentTreeSO tree;
        [SerializeField] private CharacterStatsMono statsMono;

        public int AvailablePoints { get; private set; }
        private readonly HashSet<string> _unlocked = new();
        private readonly Dictionary<string, List<IStatModifier>> _appliedMods = new();

        void Awake()
        {
            if (!statsMono) statsMono = GetComponent<CharacterStatsMono>();
            AvailablePoints = tree ? tree.startPoints : 0;
        }

        public bool IsUnlocked(TalentNodeSO node) => _unlocked.Contains(node.id);

        public bool CanUnlock(TalentNodeSO node)
        {
            if (node == null || node.comingSoon) return false;
            if (IsUnlocked(node)) return false;
            if (AvailablePoints < node.pointCost) return false;
            if (node.requirements != null)
                foreach (var req in node.requirements)
                    if (req && !_unlocked.Contains(req.id)) return false;
            return true;
        }

        public bool Unlock(TalentNodeSO node)
        {
            if (!CanUnlock(node)) return false;

            
            var created = new List<IStatModifier>();
            float now = Time.time;
            foreach (var e in node.effects)
            {
                var mod = new StatModifier(e.statId, e.op, e.amount, e.order, e.duration, now);
                statsMono.Model.AddModifier(mod);
                created.Add(mod);
            }
            _appliedMods[node.id] = created;

            _unlocked.Add(node.id);
            AvailablePoints -= node.pointCost;
            return true;
        }

        
        public void ResetAll()
        {
           foreach (var kv in _appliedMods)
               statsMono.Model.RemoveModifiers(kv.Value);
           
           _appliedMods.Clear();
           _unlocked.Clear();
           
           AvailablePoints = tree ? tree.startPoints : 0;
           
           statsMono.Model.Recompute();
        }

        public IEnumerable<TalentNodeSO> NodesByBranch(TalentBranch branch)
        {
            foreach (var n in tree.nodes) if (n && n.branch == branch) yield return n;
        }
    }
