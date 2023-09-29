using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using RPGCharacterAnims;
using RPGCharacterAnims.Lookups;
using RPGCharacterAnims.Actions;

namespace Aquapunk
{
    public class Mob : Entity
    {
        public ParticleSystem bloodParticle;
        public List<GameObject> dropItems;

        public Vector3 offset;
        public Vector3 startPosition;
        public float timeDeath;
        public float radiusPatrol;
        public float stoppingDistance = 1f;
        public bool isPatrolling = true;
        public bool agreed = true;


        private Player _killer;

        protected Coroutine _patroling;
        protected float _minStartPosDistance = 5;
        protected float _timeWaitPatrol = 5;

        public bool Agreed
        {
            get { return agreed; }
            set { agreed = value; }
            
        }


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
            }
        }
        public void StartPatrol()
        {
            agreed = true;
            isPatrolling = true;
            _patroling = StartCoroutine(TerritoryPatrol());
        }
        public void StopPatrol()
        {
            agreed = false;
            isPatrolling = false;
            StopCoroutine(_patroling);
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
                    _killer = entity.GetComponent<Player>();
                }
                Instantiate(bloodParticle, transform.position + offset, transform.rotation);
            }
            
        }

        protected IEnumerator DeathCorrutine()
        {
            yield return new WaitForSeconds(timeDeath);

            GetComponent<RPGCharacterController>().enabled = false;
            GetComponent<Mob>().enabled = false;
        }
        protected override void DeathObject()
        {
            StopPatrol();

            foreach (GameObject item in dropItems)
            {
                Vector3 spawnPoint = transform.position;
                spawnPoint.y = item.transform.position.y;
                GameObject itemObject = Instantiate(item, spawnPoint, item.transform.rotation);
            }

            _state = StateEntity.Death;

            if (_killer) { _killer.GetComponent<Player>().Kill(this); }

            GetComponent<RPGCharacterNavigationController>().enabled = false;
            GetComponentInChildren<Animator>().Play("Unarmed-Knockdown1");
            rpgCharacterController.StartAction(HandlerTypes.Knockback, new HitContext((int)KnockbackType.Knockback1, Vector3.back));
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
                    rpgCharacterController.StartAction(HandlerTypes.Navigation, trigger.transform.position);
                }
            }
        }

        private Vector3 RandomOffset(Vector3 position)
        { return new Vector3(position.x - Random.Range(1, 2), position.y, position.z - Random.Range(1, 2)); }

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
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("entity"))
            {
                Vector3 toEnemy = collision.transform.position - transform.position;

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
            _rigidbody = GetComponent<Rigidbody>();

            startPosition = transform.position;

            StartPatrol();
        }
    }
}