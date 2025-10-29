using UnityEngine;

public class StatsDebugUI : MonoBehaviour
{
    public TalentController controller;
    public TalentNodeSO node;             // перетащи сюда твой Damage Reduction +5%
    public CharacterStatsMono stats;

    void Start()
    {
        if (!stats) stats = controller.GetComponent<CharacterStatsMono>();
        Debug.Log($"damageTakenMult BEFORE: {stats.Model.Get(CharacterStats.DamageTakenMult)}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            bool ok = controller.Unlock(node);
            Debug.Log($"Unlock pressed: {ok}");
            Debug.Log($"damageTakenMult AFTER: {stats.Model.Get(CharacterStats.DamageTakenMult)}");

            // мини-проверка урона
            float hit = 100f;
            float final = hit * stats.Model.Get(CharacterStats.DamageTakenMult);
            Debug.Log($"Hit {hit} → final with DR: {final}");
        }
    }
}