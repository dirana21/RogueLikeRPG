using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFacing : MonoBehaviour, IFacing
{
    private SpriteRenderer _sr;
    public int FacingSign { get; private set; } = 1; // 1 вправо, -1 влево

    void Awake() => _sr = GetComponent<SpriteRenderer>();

    public void UpdateFacing(float xInput)
    {
        if (Mathf.Abs(xInput) < 0.01f) return;
        FacingSign = xInput > 0 ? 1 : -1;
        _sr.flipX = FacingSign < 0;
    }
}