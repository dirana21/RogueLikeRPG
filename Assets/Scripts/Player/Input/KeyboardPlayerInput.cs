using UnityEngine;

public class KeyboardPlayerInput : MonoBehaviour, IPlayerInput, ICombatInput
{
    [SerializeField] private bool topDown = false;

    public float Horizontal { get; private set; }
    public float Vertical   { get; private set; }
    public bool AttackPressed { get; private set; }

    void Update()
    {
        Horizontal     = Input.GetAxisRaw("Horizontal");
        Vertical       = topDown ? Input.GetAxisRaw("Vertical") : 0f;
        AttackPressed  = Input.GetKeyDown(KeyCode.Space);
    }
}