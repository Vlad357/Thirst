using UnityEngine;
using UnityEngine.UI;

namespace Aquapunk
{
    public class KitInventory : MonoBehaviour
    {
        public PlayerInfo playerInfo;

        public GameObject playerModel;
        public Image weaponIcon;
        public Image toolIcon;
        public Image armorIcon;
        public Image artefactIcon;

        public void UpdateInventoryKit(Item item)
        {
            switch (item.type)
            {
                case Item.TypeItem.Weapon:
                    weaponIcon.sprite = item.icon;
                    break;
                case Item.TypeItem.Tool:
                    toolIcon.sprite = item.icon;
                    break;
                case Item.TypeItem.Armor:
                    armorIcon.sprite = item.icon;
                    break;
                case Item.TypeItem.Artefact:
                    artefactIcon.sprite = item.icon;
                    break;
            }
        }
    }
}