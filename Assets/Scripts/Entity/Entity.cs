using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using RPGCharacterAnims;
using RPGCharacterAnims.Actions;
using RPGCharacterAnims.Lookups;
using UnityEngine.InputSystem.OnScreen;
using DG.Tweening;

namespace Aquapunk
{
    public class Entity : MonoBehaviour
    {
        public float shotCount;

        public bool endShot = true;
        public GameObject projectileTrale;
        #region Fields
        //public Canvas canvasWorld;
        public Vector3 offsetHPBar;
        protected Rigidbody _rigidbody;
        [SerializeField] protected StateEntity _state = StateEntity.Idle;

        [Header("Attack")]
        public RPGCharacterController rpgCharacterController;
        public GameObject trigger = null;

        public LayerMask layer;
        public Transform atackPoint;

        public List<GameObject> enemys;

        public bool notBurn;

        public float projectileMaxDeflection;
        public float projectileMinDeflection;
        public float projectileDeflection;
        public float recoil;
        public GameObject projectile;
        [SerializeField]
        protected Vector3 _projetileSpawnOffser;

        public GameObject flameEffect;
        protected GameObject flame;


        [SerializeField]
        protected TypeAttack _typeAttack;
        [SerializeField]protected float _melleHitAnimTime = 1.1f;
        [SerializeField] protected float _forceRangeMultiplyRange = 10, 
            _forceRangeMultiplyMelee = 100, _forceRangeMultiplyTick = 50;
        [SerializeField] protected float _attackRange = 1.5f, _attackDamage = 20f;
        public float _timeAttackCoolDown, _attackCollDown = 0.5f, 
            _timeStanCoolDown, _stanCollDown = 0.3f,
            _timeForceCoolDown, _forceCoolDown = 0.3f,
            _timeProjectileCoolDown, _projectileCoolDown = 0.1f;
        [SerializeField] protected Vector3 _attackOffset;

        [Header("Level system")]

        //public float level;
        //public float experienceLevel;
        //
        //protected float maxExpLevel = 100;
        //protected float experienceDeath;
        //protected float procentExp = 10;
        //protected float expRange;
        //protected LayerMask layerXP;

        [Header("HP system")]

        public float healthMax = 100f;
        public float healthCurrent;

        public HPBar hpBar;

        public float TimeAttackCoolDown { 
            get 
            {
                return _timeAttackCoolDown;
            } 
            private set {} 
        }
        #endregion
        #region Methods
        #region Class Methods
        public TypeAttack typeAttack
        {
            get
            {
                return _typeAttack;
            }
            private set { }
        }

        //public void DeleteHPBar()
        //{
        //    if (hpBar != null)
        //    {
        //        Destroy(hpBar.gameObject);
        //    }
        //}

        protected void CoolDown(out float coolDown, float postCoolDown)
        {
            coolDown = postCoolDown;
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
                    //if (GUI.Button(new Rect(1115, offset, 100, 30), label))
                    //{
                    doSwitch = true;
                    switchWeaponContext.type = "Switch";
                    switchWeaponContext.side = "None";
                    switchWeaponContext.leftWeapon = Weapon.Unarmed;
                    switchWeaponContext.rightWeapon = weapon;
                    //}
                }
                //offset += 30;
            }
            // Instant weapon toggle.
            //useInstant = true;// GUI.Toggle(new Rect(1000, 310, 100, 30), useInstant, "Instant");
            //if (useInstant) {
            switchWeaponContext.type = "Instant"; //}

            // Perform the weapon switch using the SwitchWeaponContext created earlier.
            if (doSwitch) { rpgCharacterController.TryStartAction(HandlerTypes.SwitchWeapon, switchWeaponContext); }
        }

        protected virtual void Armed()
        {
            //if (!rpgCharacterController.HandlerExists(HandlerTypes.SwitchWeapon)) { return; }

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
                    //if (GUI.Button(new Rect(1115, offset, 100, 30), label))
                    //{
                    doSwitch = true;
                    switchWeaponContext.type = "Switch";
                    switchWeaponContext.side = "None";
                    switchWeaponContext.leftWeapon = Weapon.Unarmed;
                    switchWeaponContext.rightWeapon = weapon;
                    //}
                }
                //offset += 30;
            }
            // Instant weapon toggle.
            //useInstant = true;// GUI.Toggle(new Rect(1000, 310, 100, 30), useInstant, "Instant");
            //if (useInstant) {
            switchWeaponContext.type = "Instant"; //}

            // Perform the weapon switch using the SwitchWeaponContext created earlier.
            if (doSwitch) { rpgCharacterController.TryStartAction(HandlerTypes.SwitchWeapon, switchWeaponContext); }
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
                //if (GUI.Button(new Rect(1115, 280, 100, 30), "Unarmed"))
                //{
                doSwitch = true;
                switchWeaponContext.type = "Switch";
                switchWeaponContext.side = "Both";
                switchWeaponContext.leftWeapon = Weapon.Unarmed;
                switchWeaponContext.rightWeapon = Weapon.Unarmed;
                //}
            }

            switchWeaponContext.type = "Instant"; //}

            // Perform the weapon switch using the SwitchWeaponContext created earlier.
            if (doSwitch) { rpgCharacterController.TryStartAction(HandlerTypes.SwitchWeapon, switchWeaponContext); }
            
        }
        public virtual void setDamage(float damage, Entity entity)
        {
            healthCurrent -= damage;

            if (healthCurrent <= 0)
            {
                DeathObject();
            }
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

        protected virtual void MelleAttack()
        {
            if(_timeAttackCoolDown <= 0)
            {
                _timeAttackCoolDown = _attackCollDown;
                rpgCharacterController.StartAction(HandlerTypes.Attack, new AttackContext("Attack", Side.None));
            }
        }


        public void MelleHit()
        {
            Collider[] enemysAtack = Physics.OverlapSphere(atackPoint.position + _attackOffset, _attackRange, layer);
            // animate
            // damage
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

                    //rpgCharacterController.StartAction(HandlerTypes.Navigation, direction.normalized);
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

        public void RangeShot()
        {
            Vector3 spwnProjectilePoint = transform.position;
            spwnProjectilePoint.y = 0 + transform.position.y;

            RangeProjectile projectileObj = Instantiate(projectile, spwnProjectilePoint + _projetileSpawnOffser, new Quaternion(0, 0, 0, 0)).GetComponent<RangeProjectile>();
            ProjectileTrack projectileTraleObj = Instantiate(projectileTrale, transform.position + _projetileSpawnOffser, transform.rotation).GetComponent<ProjectileTrack>();
            projectileTraleObj.target = projectileObj.transform;

            projectileObj.direction = transform.forward +
                new Vector3(
                    UnityEngine.Random.Range(0, projectileDeflection),
                    UnityEngine.Random.Range(0, projectileDeflection),
                    UnityEngine.Random.Range(0, projectileDeflection)).normalized;
            projectileObj.owner = this;
            //_rigidbody.AddForce((transform.position - trigger.transform.position).normalized * recoil);
        }

        protected virtual void RangeTickAttack()
        {
            flame = Instantiate(flameEffect, transform.position, transform.rotation);
            flame.GetComponent<FlameScript>().rider = transform;
        }

        public virtual void SortTrigger(GameObject gameObjectObj = null)
        {
            if(trigger == null && gameObjectObj != null && gameObjectObj.GetComponent<Entity>()._state != StateEntity.Death)
            {
                trigger = gameObjectObj;
            }
            if(trigger != null)
            {
                if (enemys.Count > 1)
                {
                    foreach (GameObject entity in enemys)
                    {
                        Entity type = entity.GetComponent<Entity>();
                        switch (type)
                        {
                            case Player _:
                                //if (trigger.GetComponent<Entity>().GetType().ToString() == "Aquapunk.Mob")
                                //{
                                trigger = entity;
                                //}
                                break;
                            case Mob _:
                                if (entity && trigger != null &&
                                    (entity.transform.position - transform.position).magnitude <
                                    (trigger.transform.position - transform.position).magnitude)
                                //&& trigger.GetComponent<Entity>().GetType().ToString() != "Aquapunk.Player")
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

        protected virtual void DeathObject()
        {
            //if(hpBar != null)
            //{
            //    Destroy(hpBar.gameObject);
            //}

            if (flame)
            {
                Destroy(flame);
            }
            //GiveExp();
            Destroy(gameObject);
        }
        //protected virtual void GiveExp()
        //{
        //    List<Collider> expColliders = Physics.OverlapSphere(transform.position, expRange, layerXP).ToList();
        //    for (int c = 0; c != expColliders.Count; c++)
        //    {
        //        if (expColliders[c].gameObject != gameObject && !expColliders[c].isTrigger)
        //        {
        //            expColliders[c].GetComponent<Player>().SetExp(experienceDeath / expColliders.Count);
        //        }
        //    }
        //}

        protected virtual void ProcessCooldown()
        {
            CoolDown(out _timeAttackCoolDown, _timeAttackCoolDown);
            
            CoolDown(out _timeStanCoolDown, _timeStanCoolDown);

            CoolDown(out _timeForceCoolDown, _timeForceCoolDown);
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
        #endregion
        #region Unity Methods

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
        #endregion
        #endregion
        #region delegates and enums
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
        #endregion
    }
}