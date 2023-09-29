using UnityEngine;

namespace Aquapunk
{
    public class Flamethrower : Mob
    {
        public float rearDamageMultiplier;


        public override void setDamage(float damage, Entity entity)
        {
            
            Vector3 directionToTarget = entity.transform.position - transform.position;
            Vector3 forward = transform.forward;
            float dotProduct = Vector3.Dot(directionToTarget, forward);
            float multiplier = 1;
            if (dotProduct < 0)
            {
                multiplier = rearDamageMultiplier;
                Debug.Log("DD");
            }
            base.setDamage(damage * multiplier, entity);
        }
    }
}

