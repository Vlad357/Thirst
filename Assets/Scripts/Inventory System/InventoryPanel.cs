using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Aquapunk
{
    public class InventoryPanel : MonoBehaviour
    {
        #region Fields
        public PlayerInfo playerInfo;
        public GameObject itemCell;
        public GameObject context;
        public ItemInfoCard info;

        #endregion
        #region Methods
        #region Class Methods
        //public void LoadItem()
        //{
        //    foreach(Transform child in context.transform)
        //    {
        //        Destroy(child.gameObject);
        //    }
        //    foreach(Item item in playerInfo.player.items)
        //    {
        //        GameObject cell = Instantiate(itemCell, context.transform);
        //
        //        cell.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = item.name;
        //        cell.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
        //        cell.GetComponent<Button>().onClick.AddListener(() => info.GetItemInfo(item));
        //    }
        //}
        #endregion
        #endregion
    }
}