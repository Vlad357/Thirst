using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Aquapunk
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Consumable")]
    public class Consumable : Item
    {
        public ConsumableType consumableType;

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

        public enum ConsumableType
        {
            water,
            gold
        }
    }
}