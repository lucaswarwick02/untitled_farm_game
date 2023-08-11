using Data;
using Databases;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        private static readonly Color SelectedColor = new(1f, 1f, 1f, 1f);
        private static readonly Color DisabledColor = new(1f, 1f, 1f, 0.5f);
    
        [SerializeField] private GameObject[] slots;
        [SerializeField] private TextMeshProUGUI heldItemText;

        private void OnEnable()
        {
            PlayerInteract.OnSlotChange += UpdateSlotVisuals;
            SaveManager.CurrentSave.inventory.OnInventoryChanged += UpdateSlotVisuals;
        }

        private void OnDisable()
        {
            PlayerInteract.OnSlotChange -= UpdateSlotVisuals;
            SaveManager.CurrentSave.inventory.OnInventoryChanged -= UpdateSlotVisuals;
        }

        private void Start()
        {
            UpdateSlotVisuals();
        }

        private void UpdateSlotVisuals()
        {
            foreach (var slot in slots)
            {
                slot.GetComponent<Image>().color = DisabledColor;
            }

            slots[PlayerHandler.Instance.interact.CurrentIndex].GetComponent<Image>().color = SelectedColor;
            if (string.IsNullOrEmpty(SaveManager.CurrentSave.inventory.equipped[PlayerHandler.Instance.interact.CurrentIndex].itemID))
            {
                heldItemText.text = "";
            }
            else
            {
                var currentItem = ItemDatabase.Query(PlayerHandler.Instance.interact.CurrentItem.itemID);
                heldItemText.text = Colors.FromRarity(currentItem.Rarity).RichTag() + currentItem.Name;
            }
        
            for(var i = 0; i < slots.Length; i++)
            {
                var itemInstance = SaveManager.CurrentSave.inventory.equipped[i];
                if (string.IsNullOrEmpty(itemInstance.itemID))
                {
                    slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
                else
                {
                    slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    slots[i].transform.GetChild(0).GetComponent<Image>().sprite =
                        ItemDatabase.Query(itemInstance.itemID).Image;
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = itemInstance.amount.ToString();
                }
            }
        }
    }
}
