using UnityEngine;

namespace Thirst
{
    public class ProjectileTrack : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;

        private void Update()
        {
            if (target != null)
            {
                transform.position = target.position + offset;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

