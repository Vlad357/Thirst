using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using RPGCharacterAnims;
using RPGCharacterAnims.Lookups;
using RPGCharacterAnims.Actions;

namespace Aquapunk
{
    public class Mob : Entity
    {
        #region fields
        private Player killer;
        public Vector3 offset;
        private Vector3 deathPos;
        public float timeDeath;
        public ParticleSystem bloodParticle;
        public Vector3 startPosition;

        private RPGCharacterController rpgCharacterController;
        private RPGCharacterNavigationController rpgNavigationController;

        public List<GameObject> dropItems;

        public bool isPatrolling = true;
        public bool agreed = true;
        public float radiusPatrol;

        public float stoppingDistance = 1f;

        protected Coroutine patroling;
        //protected MobMovement _mobMovement;
        [SerializeField] protected float _minStartPosDistance;
        [SerializeField] protected float _timeWaitPatrol;
        #endregion
        #region Properties
        public bool Agreed
        {
            get { return agreed; }
            set { agreed = value; }
            
        }
        #endregion
        #region Methods
        #region Class Methods

        public IEnumerator TerritoryPatrol()
        {
            // Move to a random point if not attacking or stunned
            while (isPatrolling && _state != StateEntity.Death)
            {
                if (_timeStanCoolDown <= 0 && _timeAttackCoolDown <= 0)
                {
                    if (trigger == null)
                    {
                        Vector3 point = startPosition + (Random.insideUnitSphere * radiusPatrol);
                        try
                        {
                            MoveState();
                            rpgCharacterController.StartAction(HandlerTypes.Navigation, point);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"Error moving to point {point}: {e.Message}");
                        }
                    }
                }
                yield return new WaitForSeconds(_timeWaitPatrol);
                // Reset the agent's path
                //if (_mobMovement.agent.path != null)
                //{
                //    _mobMovement.agent.ResetPath();
                //}
            }
        }

        public void StartPatrol()
        {
            agreed = true;
            isPatrolling = true;
            patroling = StartCoroutine(TerritoryPatrol());
        }

        public void StopPatrol()
        {
            agreed = false;
            isPatrolling = false;
            StopCoroutine(patroling);
        }

        public override void setDamage(float damage, Entity entity)
        {
            if(_state != StateEntity.Death)
            {
                base.setDamage(damage, entity);
                if(healthCurrent > 0)
                {

                    GetComponentInChildren<Animator>().Play("Unarmed-GetHit-F1");
                }
                if (entity != null)
                {
                    trigger = entity.gameObject;
                }
                if (healthCurrent <= 0 && entity.GetComponent<Player>())
                {
                    killer = entity.GetComponent<Player>();
                }
                Instantiate(bloodParticle, transform.position + offset, transform.rotation);
            }
            
        }

        //public override void SortTrigger()
        //{
        //    base.SortTrigger();
        //    if(enemys.Count > 0)
        //    {
        //        foreach (GameObject entity in enemys)
        //        {
        //            if(entity != null)
        //            {
        //                switch (entity.GetComponent<Entity>().GetType().ToString())
        //                {
        //                    case "Aquapunk.Player":
        //                        trigger = entity;
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //}

        protected override void DeathObject()
        {

            //StopAllCoroutines();
            StopPatrol();
            foreach (GameObject item in dropItems)
            {
                Vector3 spawnPoint = transform.position;
                spawnPoint.y = item.transform.position.y;
                GameObject itemObject = Instantiate(item, spawnPoint, item.transform.rotation);
            }
            //base.DeathObject();
            _state = StateEntity.Death;
            if (killer) { killer.GetComponent<Player>().Kill(this); }
            GetComponent<RPGCharacterNavigationController>().enabled = false;
            GetComponentInChildren<Animator>().Play("Unarmed-Knockdown1");
            rpgCharacterController.StartAction(HandlerTypes.Knockback, new HitContext((int)KnockbackType.Knockback1, Vector3.back));

            //StartCoroutine(DeathCorrutine());
        }

        protected IEnumerator DeathCorrutine()
        {
            yield return new WaitForSeconds(timeDeath);

            GetComponent<RPGCharacterController>().enabled =false;
            GetComponent<Mob>().enabled = false;

            //Destroy(gameObject);
        }

        protected virtual void BehaveAtTrigger()
        {
            if (trigger != null && _timeStanCoolDown <= 0 && _timeAttackCoolDown <= 0 && _state != StateEntity.Death)
            {
                
                float distance = (trigger.transform.position - transform.position).magnitude;
                if (distance <= _attackRange)
                {
                    Armed();

                    Vector3 targetDirection = trigger.transform.position - transform.position;
                    targetDirection.y = 0;

                    RotateTo(targetDirection);
                    IdleState();
                    Attack();
                }
                else
                {
                    Unarmed();
                    MoveState();
                    //_mobMovement.Movement(trigger.transform.position - transform.position);
                    rpgCharacterController.StartAction(HandlerTypes.Navigation, trigger.transform.position);
                }
            }
        }

        private Vector3 RandomOffset(Vector3 position)
        { return new Vector3(position.x - Random.Range(1, 2), position.y, position.z - Random.Range(1, 2)); }

        #endregion
        #region Unity Methods
        private void OnTriggerExit(Collider other)
        {
            if (enemys.Contains(other.gameObject))
            {
                enemys.Remove(other.gameObject);
                SortTrigger();
                if (trigger == null)
                {
                    StartPatrol();
                }
            }
        }

        //проверить
        private void OnTriggerStay(Collider other)
        {

            if(trigger == null && other.GetComponent<Entity>() && other.GetComponent<Entity>().GetType() != typeof(Mob) 
                && agreed && !other.isTrigger)
            {
                if (isPatrolling)
                {
                    StopPatrol();
                }
                if (!enemys.Contains(other.gameObject))
                {
                    enemys.Add(other.gameObject);
                }
                SortTrigger();
            }
        }

        //проверить
        private void OnCollisionStay(Collision collision)
        {
            // ѕровер€ем, столкнулись ли мы с объектом на слое "Enemy"
            if (collision.gameObject.layer == LayerMask.NameToLayer("entity"))
            {
                // ≈сли столкнулись, вычисл€ем вектор до врага
                Vector3 toEnemy = collision.transform.position - transform.position;

                // ≈сли игрок находитс€ ближе к врагу, чем определенное рассто€ние,
                // то отмен€ем движение в этом направлении
                if (toEnemy.magnitude < stoppingDistance)
                {
                    Vector3 cancelMove = Vector3.Project(_rigidbody.velocity, -toEnemy.normalized);
                    _rigidbody.velocity -= cancelMove;
                }
            }
        }
        private void FixedUpdate()
        {
            BehaveAtTrigger();
            ProcessCooldown();
            ProcessStates();
        }

        private void Start()
        {
            rpgCharacterController = GetComponent<RPGCharacterController>();
            rpgNavigationController = GetComponent<RPGCharacterNavigationController>();
            _rigidbody = GetComponent<Rigidbody>();
            //_mobMovement = GetComponent<MobMovement>();
            startPosition = transform.position;
            StartPatrol();
        }
        #endregion
        #endregion
    }
}