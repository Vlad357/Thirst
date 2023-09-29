using UnityEngine;

namespace Aquapunk
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
    public class Item : Scriptable
    {
        public int value;
        public float MaxHealth;
        public TypeItem type = default;

        public virtual void PickUp(Player player)
        {
            owner = player;
        }
        public void SetParameters()
        {
            owner.healthMax += MaxHealth;
        }
        public void ResetParameters()
        {
            owner.healthMax -= MaxHealth;
            
        }

        public enum TypeItem
        {
            Default,
            Armor,
            Tool,
            Weapon,
            Artefact
        }
    }
}