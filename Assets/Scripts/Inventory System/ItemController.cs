using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    public class ItemController : MonoBehaviour
    {
        #region Fields
        public Item item;
        public float coolDown = 0.3f;
        #endregion
        #region Methods
        #region ClassMethods
        private void PickUpItem(Player player)
        {
            if (player != null)
            {
                item.PickUp(player);
                Destroy(gameObject);
            }
        }
        #endregion
        #region UnityMethods
        private void OnTriggerEnter(Collider other)
        {
            if(coolDown <= 0)
            {
                PickUpItem(other.GetComponent<Player>());
            }
        }
        private void Update()
        {
            if(coolDown >= 0)
            {
                coolDown -= Time.deltaTime;
            }
        }
        #endregion
        #endregion

    }
}

