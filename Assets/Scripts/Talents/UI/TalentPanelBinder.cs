using UnityEngine;

public class TalentPanelBinder : MonoBehaviour
{
    [SerializeField] private TalentTreeSO tree;
    [SerializeField] private TalentController controller;

    [Header("Prefabs")]
    [SerializeField] private TalentNodeView nodePrefab;

    [Header("Branch Roots (5 columns)")]
    [SerializeField] private Transform vitalityRoot;
    [SerializeField] private Transform reflexRoot;
    [SerializeField] private Transform impactRoot;
    [SerializeField] private Transform intelligenceRoot;
    [SerializeField] private Transform vitalCoreRoot;

    [Header("Tooltip")]
    [SerializeField] private TalentTooltipUI tooltipUI;

    void Start() => Build();

    public void Build()
    {
        Clear(vitalityRoot);
        Clear(reflexRoot);
        Clear(impactRoot);
        Clear(intelligenceRoot);
        Clear(vitalCoreRoot);

        foreach (var node in tree.nodes)
        {
            if (!node) continue;

            Transform root = GetRoot(node.branch);
            if (!root) continue;

            var view = Instantiate(nodePrefab, root);
            view.Bind(node, controller);

            var trigger = view.GetComponent<TalentTooltipTrigger>();
            if (trigger) trigger.SetTooltip(tooltipUI);
        }
    }

    private void Clear(Transform root)
    {
        if (!root) return;
        for (int i = root.childCount - 1; i >= 0; i--)
            Destroy(root.GetChild(i).gameObject);
    }

    private Transform GetRoot(TalentBranch branch)
    {
        return branch switch
        {
            TalentBranch.Vitality => vitalityRoot,
            TalentBranch.Reflex   => reflexRoot,
            TalentBranch.Impact   => impactRoot,
            TalentBranch.Intelligence   => intelligenceRoot,
            TalentBranch.VitalCore    => vitalCoreRoot,
            _ => null
        };
    }
}