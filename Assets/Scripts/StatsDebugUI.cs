using UnityEngine;
using Game.Shared.Stats; // если у тебя неймспейсы

public class StatsDebugUI : MonoBehaviour
{
    [SerializeField] private CharacterStatsMono statsMono;
    [SerializeField] private Animator anim; // твой Animator игрока

    void Awake()
    {
        if (!statsMono) statsMono = GetComponent<CharacterStatsMono>();

        statsMono.Model.Health.OnChanged  += OnHpChanged;
        statsMono.Model.Health.OnDepleted += OnDeath;
    }

    private void OnHpChanged(IVital hp)
    {
        Debug.Log($"HP: {hp.Current}/{hp.Max}");
    }

    private void OnDeath()
    {
        Debug.Log("DEAD");
        if (anim) anim.SetBool("IsDead", true);
        // тут можно отключить управление/движение
    }
}