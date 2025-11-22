using UnityEngine;
using TMPro;

public class TalentTooltipUI : MonoBehaviour
{
    [SerializeField] private RectTransform root;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descText;

    void Awake()
    {
        if (!root) root = GetComponent<RectTransform>();
        Hide();
    }

    public void Show(TalentNodeSO node, Vector2 screenPos)
    {
        titleText.text = node.title;
        descText.text  = node.description;

        root.gameObject.SetActive(true);
        root.position = screenPos;
    }

    public void Hide()
    {
        root.gameObject.SetActive(false);
    }
}