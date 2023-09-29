using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Aquapunk
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Consumable")]
    public class Consumable : Item
    {
        #region Fields
        public ConsumableType consumableType;
        #endregion
        #region Methods
        #region ClassMethods
        public override void PickUp(Player player)
        {
            switch (consumableType)
            {
                case ConsumableType.water:
                    player.WaterCounter += value;
                    player.UpdateWaterCount();
                    break;
            }
        }
        #endregion
        #endregion
        #region Enums
        public enum ConsumableType
        {
            water,
            gold
        }
        #endregion
    }
}