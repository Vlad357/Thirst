using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Aquapunk
{
    public class KitInventory : MonoBehaviour
    {
        #region Fields
        [Header("Main settings")]
        public PlayerInfo playerInfo;

        [Header("Inventory main kit and player model")]
        public GameObject playerModel;
        public Image weaponIcon;
        public Image toolIcon;
        public Image armorIcon;
        public Image artefactIcon;
        #endregion

        #region Methods
        #region Class Methods
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
        #endregion
        #region Unity Methods

        //private void Awake()
        //{
        //    playerInfo.player.setNewItem += UpdateInventoryKit;
        //}
        #endregion
        #endregion
    }
}