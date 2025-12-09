using UnityEngine;

public class TalentPanelToggle : MonoBehaviour
{
    [SerializeField] private GameObject talentPanel;

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TogglePanel();
        }
    }

    private void TogglePanel()
    {
        isOpen = !isOpen;
        if (talentPanel != null)
            talentPanel.SetActive(isOpen);
    }
}