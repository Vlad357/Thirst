using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Aquapunk
{
    public class ItemInfoCard : MonoBehaviour
    {
        #region Fields
        public Image ItemIcon;
        public TextMeshProUGUI ItemName;
        public TextMeshProUGUI ItemInfo;
        public Button applyItemButton;
        public PlayerInfo playerInfo;
        #endregion
        #region Methods
        #region ClassMethods
        //public void GetItemInfo(Item item)
        //{
        //    Apply(item);
        //    if(item.type != Item.TypeItem.Default)
        //    {
        //        applyItemButton.gameObject.SetActive(true);
        //        applyItemButton.onClick.AddListener(() => playerInfo.player.SetItem(item));
        //    }
        //    else
        //    {
        //        applyItemButton.gameObject.SetActive(false);
        //    }
        //}

        protected void Apply(Scriptable scriptable)
        {
            applyItemButton.onClick.RemoveAllListeners();
            ItemIcon.sprite = scriptable.icon;
            ItemName.text = scriptable.title;
            ItemInfo.text = scriptable.info;
        }
        #endregion
        #endregion
    }
}