using System;
using System.Collections.Generic;
using UnityEngine;

using RPGCharacterAnims;
using RPGCharacterAnims.Actions;
using RPGCharacterAnims.Lookups;
using DG.Tweening;

namespace Thirst
{
    public class Entity : MonoBehaviour
    {
        public RPGCharacterController rpgCharacterController;

        [Header("HP system")]
        public float healthMax = 100f;
        public float healthCurrent;

        [Header("Attack")]
        public float shotCount;
        public float projectileMaxDeflection = 1;
        public float projectileMinDeflection;
        public float projectileDeflection;
        public float recoil;
        

        public bool notBurn;
        public bool endShot = true;

        public GameObject projectileTrale;
        public GameObject trigger = null;
        public GameObject projectile;
        public GameObject flameEffect;

        public LayerMask layer;
        public Transform atackPoint;
        public List<GameObject> enemys;

        protected float _attackRange = 2, _attackDamage = 75;
        protected float _melleHitAnimTime = 0.5f;
        protected float _forceRangeMultiplyRange = 10,
            _forceRangeMultiplyMelee = 100, _forceRangeMultiplyTick = 50; 
        protected float _timeAttackCoolDown, _attackCollDown = 0.5f,
            _timeStanCoolDown, _stanCollDown = 0.3f,
            _timeForceCoolDown, _forceCoolDown = 0.3f,
            _timeProjectileCoolDown, _projectileCoolDown = 0.1f;

        protected Vector3 _projetileSpawnOffset = new Vector3(0, 1, 0);
        protected Vector3 _attackOffset;
        protected TypeAttack _typeAttack;
        protected StateEntity _state = StateEntity.Idle;

        protected GameObject _flame;
        protected Rigidbody _rigidbody;

        public float TimeAttackCoolDown
        {
            get
            {
                return _timeAttackCoolDown;
            }
            set
            {
                if (value >= 0) { _timeAttackCoolDown = value; }
            }
        }

        public TypeAttack typeAttack
        {
            get
            {
                return _typeAttack;
            }
            private set { }
        }
        public void MelleHit()
        {
            Collider[] enemysAtack = Physics.OverlapSphere(atackPoint.position + _attackOffset, _attackRange, layer);

            foreach (Collider enemy in enemysAtack)
            {
                if (enemy.gameObject != gameObject && !enemy.isTrigger
                    && _attackRange > (enemy.transform.position - transform.position).magnitude)
                {
                    enemy.GetComponent<Entity>().setDamage(_attackDamage, this);
                }
            }
        }
        public void RotateTo(Vector3 direction, Action action = null)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.DORotateQuaternion(Quaternion.Lerp(transform.rotation, lookRotation, 1), 0.1f).OnComplete(() => action.Invoke());
        }
        public void RangeShot()
        {
            Vector3 spwnProjectilePoint = transform.position;
            spwnProjectilePoint.y = 0 + transform.position.y;

            RangeProjectile projectileObj = Instantiate(projectile, spwnProjectilePoint + _projetileSpawnOffset, new Quaternion(0, 0, 0, 0)).GetComponent<RangeProjectile>();
            ProjectileTrack projectileTraleObj = Instantiate(projectileTrale, transform.position + _projetileSpawnOffset, transform.rotation).GetComponent<ProjectileTrack>();
            projectileTraleObj.target = projectileObj.transform;

            projectileObj.direction = transform.forward +
                new Vector3(
                    UnityEngine.Random.Range(0, projectileDeflection),
                    UnityEngine.Random.Range(0, projectileDeflection),
                    UnityEngine.Random.Range(0, projectileDeflection)).normalized;
            projectileObj.owner = this;
        }
        public virtual void setDamage(float damage, Entity entity)
        {
            healthCurrent -= damage;

            if (healthCurrent <= 0)
            {
                DeathObject();
            }
        }
        public virtual void Unarmed()
        {
            if (!rpgCharacterController.HandlerExists(HandlerTypes.SwitchWeapon)) { return; }

            var doSwitch = false;

            // Create a new SwitchWeaponContext with the switch settings.
            var switchWeaponContext = new SwitchWeaponContext();

            // Unarmed.
            if (rpgCharacterController.rightWeapon != Weapon.Unarmed
                || rpgCharacterController.leftWeapon != Weapon.Unarmed)
            {
                doSwitch = true;
                switchWeaponContext.type = "Switch";
                switchWeaponContext.side = "Both";
                switchWeaponContext.leftWeapon = Weapon.Unarmed;
                switchWeaponContext.rightWeapon = Weapon.Unarmed;
            }

            switchWeaponContext.type = "Instant";

            // Perform the weapon switch using the SwitchWeaponContext created earlier.
            if (doSwitch) { rpgCharacterController.TryStartAction(HandlerTypes.SwitchWeapon, switchWeaponContext); }

        }
        public virtual void Attack()
        {
            if (_timeAttackCoolDown <= 0 && _timeStanCoolDown <= 0)
            {
                switch (typeAttack)
                {
                    case TypeAttack.Melee:
                        _attackCollDown = 0.04f;
                        MelleAttack();
                        break;
                    case TypeAttack.RangeTick:
                        RangeTickAttack();
                        break;
                    case TypeAttack.Range:
                        _attackCollDown = 0.4f;
                        RangeAttack();
                        break;
                }
            }
        }
        public virtual void SortTrigger(GameObject gameObjectObj = null)
        {
            if (trigger == null && gameObjectObj != null && gameObjectObj.GetComponent<Entity>()._state != StateEntity.Death)
            {
                trigger = gameObjectObj;
            }
            if (trigger != null)
            {
                if (enemys.Count > 1)
                {
                    foreach (GameObject entity in enemys)
                    {
                        Entity type = entity.GetComponent<Entity>();
                        switch (type)
                        {
                            case Player _:
                                trigger = entity;
                                break;
                            case Mob _:
                                if (entity && trigger != null &&
                                    (entity.transform.position - transform.position).magnitude <
                                    (trigger.transform.position - transform.position).magnitude)
                                {
                                    trigger = entity;
                                }
                                break;
                        }
                    }
                }
                if (enemys != null
                    || gameObjectObj.GetComponent<Entity>()!._state == StateEntity.Death
                    && trigger.gameObject == gameObjectObj)
                {
                    trigger = null;
                }
            }

        }

        protected void CoolDown(ref float coolDown)
        {
            if (coolDown >= 0f)
            {
                coolDown -= Time.fixedDeltaTime;
            }
        }
        protected virtual void RangeArmed()
        {
            if (!rpgCharacterController.HandlerExists(HandlerTypes.SwitchWeapon)) { return; }

            var doSwitch = false;
            projectileDeflection = projectileMinDeflection;
            _typeAttack = TypeAttack.Range;
            // Create a new SwitchWeaponContext with the switch settings.
            var switchWeaponContext = new SwitchWeaponContext();

            foreach (var weapon in WeaponGroupings.Range)
            {
                if (rpgCharacterController.rightWeapon != weapon)
                {
                    var label = weapon.ToString();
                    if (label.StartsWith("TwoHand")) { label = label.Replace("TwoHand", "2H "); }
                    doSwitch = true;
                    switchWeaponContext.type = "Switch";
                    switchWeaponContext.side = "None";
                    switchWeaponContext.leftWeapon = Weapon.Unarmed;
                    switchWeaponContext.rightWeapon = weapon;
                }
            }
            // Instant weapon toggle.
            switchWeaponContext.type = "Instant";

            // Perform the weapon switch using the SwitchWeaponContext created earlier.
            if (doSwitch) { rpgCharacterController.TryStartAction(HandlerTypes.SwitchWeapon, switchWeaponContext); }
        }
        protected virtual void Armed()
        {
            var doSwitch = false;

            _typeAttack = TypeAttack.Melee;
            // Create a new SwitchWeaponContext with the switch settings.
            var switchWeaponContext = new SwitchWeaponContext();

            foreach (var weapon in WeaponGroupings.TwoHandedWeapons)
            {
                if (rpgCharacterController.rightWeapon != weapon)
                {
                    var label = weapon.ToString();
                    if (label.StartsWith("TwoHand")) { label = label.Replace("TwoHand", "2H "); }
                    doSwitch = true;
                    switchWeaponContext.type = "Switch";
                    switchWeaponContext.side = "None";
                    switchWeaponContext.leftWeapon = Weapon.Unarmed;
                    switchWeaponContext.rightWeapon = weapon;
                }
            }
            switchWeaponContext.type = "Instant";

            // Perform the weapon switch using the SwitchWeaponContext created earlier.
            if (doSwitch) { rpgCharacterController.TryStartAction(HandlerTypes.SwitchWeapon, switchWeaponContext); }
        }
        protected virtual void MelleAttack()
        {
            if(_timeAttackCoolDown <= 0)
            {
                _timeAttackCoolDown = _attackCollDown;
                rpgCharacterController.StartAction(HandlerTypes.Attack, new AttackContext("Attack", Side.None));
            }
        }
        protected virtual void RangeAttack()
        {
            if(_timeAttackCoolDown <= 0)
            {
                
                if (projectileDeflection < projectileMaxDeflection)
                {
                    projectileDeflection += 0.05f;
                }
                if(projectileDeflection >= projectileMaxDeflection)
                {
                    projectileDeflection = projectileMinDeflection;
                }

                if (trigger != null)
                {
                    Vector3 direction = trigger.transform.position - transform.position;
                    RotateTo(direction,
                        () => {
                            rpgCharacterController.StartAction(HandlerTypes.Attack, new AttackContext("Attack", Side.None));
                            _timeAttackCoolDown = _attackCollDown;
                            endShot = false;
                        });
                }
                else
                {
                    rpgCharacterController.StartAction(HandlerTypes.Attack, new AttackContext("Attack", Side.None));
                    _timeAttackCoolDown = _attackCollDown;
                    endShot = false;

                    if (rpgCharacterController.CanEndAction(HandlerTypes.Attack)) { print("все!"); }
                }
            }
        }
        protected virtual void RangeTickAttack()
        {
            _flame = Instantiate(flameEffect, transform.position, transform.rotation);
            _flame.GetComponent<FlameScript>().rider = transform;
        }
        protected virtual void DeathObject()
        {
            if (_flame)
            {
                Destroy(_flame);
            }
            Destroy(gameObject);
        }
        protected virtual void ProcessCooldown()
        {
            CoolDown(ref _timeAttackCoolDown);
            
            CoolDown(ref _timeStanCoolDown);

            CoolDown(ref _timeForceCoolDown);
        }
        protected virtual void ProcessStates()
        {
            if (_state != StateEntity.Idle && _rigidbody.velocity == Vector3.zero
                && _timeAttackCoolDown <= 0 && _timeStanCoolDown <= 0f && _state != StateEntity.Death)
            {
                IdleState();
            }
        }
        protected virtual void IdleState()
        {
            _state = StateEntity.Idle;
            _rigidbody.velocity = Vector3.zero;
        }
        protected virtual void MoveState()
        {
            _state = StateEntity.Move;
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("flame") && !notBurn)
            {
                setDamage(5f * Time.deltaTime, other.transform.parent.GetComponent<FlameScript>().rider.GetComponent<Entity>()); // уменьшаем здоровье игрока со временем
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Entity>() && !other.isTrigger)
            {
                enemys.Add(other.gameObject);
                SortTrigger(other.gameObject);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (enemys.Contains(other.gameObject))
            {
                enemys.Remove(other.gameObject);
                SortTrigger(other.gameObject);
            }
        }
        private void Awake()
        {
            rpgCharacterController = GetComponent<RPGCharacterController>();
            healthCurrent = healthMax;
        }

        public enum StateEntity
        {
            Idle,
            Move,
            Sprint,
            Death
        }
        public enum TypeAttack
        {
            Range,
            Melee,
            RangeTick
        }
        public delegate void MoveFunk(Vector3 dir);
    }
}