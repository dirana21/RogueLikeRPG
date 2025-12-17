using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TalentTooltipUI : MonoBehaviour
{
    [SerializeField] private RectTransform root;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private Canvas canvas;

    private RectTransform canvasRect;

    void Awake()
    {
        if (!root) root = GetComponent<RectTransform>();
        canvasRect = canvas.GetComponent<RectTransform>();
        Hide();
    }

    public void Show(TalentNodeSO node, Vector2 mouseScreenPos)
    {
        titleText.text = node.title;
        descText.text  = node.description;

        root.gameObject.SetActive(true);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(root);

        RectTransform canvasRect = canvas.transform as RectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mouseScreenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localMousePos
        );

        Vector2 tooltipSize = root.rect.size;
        Vector2 canvasSize  = canvasRect.rect.size;

        float padding = 12f;

        float topLimit    =  canvasSize.y / 2f;
        float bottomLimit = -canvasSize.y / 2f;

        float desiredY = localMousePos.y - tooltipSize.y / 2f - padding;

        // ⬇️ Если вылезаем снизу — показываем СВЕРХУ
        if (desiredY - tooltipSize.y / 2f < bottomLimit)
        {
            desiredY = localMousePos.y + tooltipSize.y / 2f + padding;
        }

        // ⬆️ Если вылезаем сверху — возвращаем вниз
        if (desiredY + tooltipSize.y / 2f > topLimit)
        {
            desiredY = localMousePos.y - tooltipSize.y / 2f - padding;
        }

        root.anchoredPosition = new Vector2(localMousePos.x, desiredY);
    }

    public void Hide()
    {
        root.gameObject.SetActive(false);
    }
}
