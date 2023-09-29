using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Thirst
{
    public class ItemInfoCard : MonoBehaviour
    {
        public TextMeshProUGUI ItemName;
        public TextMeshProUGUI ItemInfo;
        public PlayerInfo playerInfo;
        public Button applyItemButton;
        public Image ItemIcon;


        protected void Apply(Scriptable scriptable)
        {
            applyItemButton.onClick.RemoveAllListeners();
            ItemIcon.sprite = scriptable.icon;
            ItemName.text = scriptable.title;
            ItemInfo.text = scriptable.info;
        }
    }
}