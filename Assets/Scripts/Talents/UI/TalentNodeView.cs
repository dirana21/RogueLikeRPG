using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalentNodeView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button btn;
    [SerializeField] private GameObject comingSoonBadge;

    private TalentNodeSO _data;
    private TalentController _controller;

    public TalentNodeSO Data => _data;

    public void Bind(TalentNodeSO data, TalentController controller)
    {
        _data = data; 
        _controller = controller;

        if (icon) icon.sprite = data.icon;
        if (comingSoonBadge) comingSoonBadge.SetActive(data.comingSoon);

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnClick);
        RefreshState();
    }

    void RefreshState()
    {
        bool unlocked   = _controller.IsUnlocked(_data);
        bool canUnlock  = _controller.CanUnlock(_data);

        btn.interactable = canUnlock;

        var c = Color.white;
        if (unlocked) c = new Color(0.6f, 1f, 0.6f);
        else if (!canUnlock) c = new Color(0.6f, 0.6f, 0.6f);
        GetComponent<Image>().color = c;
    }

    void OnClick()
    {
        if (_controller.Unlock(_data))
            RefreshState();
    }
}