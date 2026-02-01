using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BreakableBlock : MonoBehaviour, IInteractable
{

    [SerializeField] public InventoryItemData requiredMask;
    [SerializeField] public List<BreakableBlock> linkedBlocks;

    [SerializeField] public GameObject interactionPromptObject;
    [SerializeField] public TextMeshPro tooltipText;

    public void Start()
    {
        tooltipText.gameObject.SetActive(false);
        tooltipText.text = String.Format("Requires {0} to break", requiredMask.displayName);
    }

    public String GetTooltipText()
    {
        if (InventoryManager.Instance.IsEquippedByItemData(requiredMask))
        {
            return "Press E to break";
        }
        else
        {
            return String.Format("Requires {0} to break", requiredMask.displayName);
        }
    }

    public void BreakBlock()
    {
        Destroy(gameObject);

        // one layer of linked blocks
        // we dont go recursive.
        foreach (var linkedBlock in linkedBlocks)
        {
            if (linkedBlock != null)
            {
                Destroy(linkedBlock.gameObject);
            }
        }
    }

    public void OnInteract()
    {
        GameManager.Instance.HandleBreakBlock(this.gameObject);
    }

    public void ShowTooltip(bool show)
    {
        tooltipText.text = GetTooltipText();
        tooltipText.gameObject.SetActive(show);
    }
}
