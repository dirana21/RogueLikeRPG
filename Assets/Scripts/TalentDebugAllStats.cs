using System.Text;
using UnityEngine;

public class TalentDebugAllStats : MonoBehaviour
{
    [SerializeField] private TalentController controller;
    [SerializeField] private TalentNodeSO nodeToUnlock;
    [SerializeField] private CharacterStatsMono statsMono;

    void Awake()
    {
        if (!statsMono) statsMono = GetComponent<CharacterStatsMono>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            var before = DumpAll("BEFORE");

            bool ok = controller.Unlock(nodeToUnlock);
            Debug.Log($"Unlock {nodeToUnlock.id} => {ok}");

            var after = DumpAll("AFTER");

            Debug.Log(before);
            Debug.Log(after);
        }
    }

    private string DumpAll(string tag)
    {
        var m = statsMono.Model;
        var sb = new StringBuilder();
        sb.AppendLine($"===== {tag} STATS =====");

        // HP / Stamina
        sb.AppendLine($"HP: {m.Health.Current} / {m.Health.Max}");
        sb.AppendLine($"Stamina: {m.Stamina.Current} / {m.Stamina.Max}");

        // Обычные статы
        sb.AppendLine("-- Base Stats --");
        foreach (var kv in m.Stats)
            sb.AppendLine($"{kv.Key} = {kv.Value.Value}");

        // Резисты
        sb.AppendLine("-- Resistances --");
        foreach (DamageType dt in System.Enum.GetValues(typeof(DamageType)))
            sb.AppendLine($"{dt} = {m.GetResistance(dt)}");

        return sb.ToString();
    }
}