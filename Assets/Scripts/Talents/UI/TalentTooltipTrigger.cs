using UnityEngine;
using UnityEngine.EventSystems;

public class TalentTooltipTrigger : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TalentTooltipUI tooltipUI;

    private TalentNodeView _view;
    private bool shown;

    void Awake()
    {
        _view = GetComponent<TalentNodeView>();
    }

    public void SetTooltip(TalentTooltipUI ui) => tooltipUI = ui;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_view || _view.Data == null || !tooltipUI) return;

        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (shown)
        {
            tooltipUI.Hide();
            shown = false;
        }
        else
        {
            tooltipUI.Show(_view.Data, eventData.position);
            shown = true;
        }
    }
}