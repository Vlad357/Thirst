using UnityEngine;

namespace Thirst
{
    public class RangeProjectile : MonoBehaviour
    {
        public Vector3 direction;
        public TypeProjectile typeProjectile;
        public Entity owner;

        [SerializeField] protected float _liveTime;
        [SerializeField] protected float _speed;
        [SerializeField] protected float _damage;

        protected Rigidbody _rigidbody;

        private void DeathPrijectile()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            print(collision.gameObject);
            if (collision.gameObject.GetComponent<Entity>())
            {
                collision.gameObject.GetComponent<Entity>().setDamage(_damage, owner);
            }
            DeathPrijectile();
        }

        private void FixedUpdate()
        {
            if(_liveTime > 0)
            {
                print(direction);
                transform.Translate(direction.normalized * Time.fixedDeltaTime * _speed);
                _liveTime -= Time.fixedDeltaTime;
            }
            else
            {
                DeathPrijectile();
            }
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public enum TypeProjectile
        {
            Default,
            Homing
        }
    }
}

