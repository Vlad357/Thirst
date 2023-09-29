using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using System.Drawing;

namespace Aquapunk
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MobMovement : EntityMovement
    {
        #region Fields
        public NavMeshAgent agent;
        #endregion

        #region methods
        #region class methods

        public void MoveToPoint(Vector3 point)
        {
            if(agent != null)
            {
                if (agent.path != null)
                {
                    agent.ResetPath();
                }
                MoveAgent(new Vector3(point.x, 0.5f, point.z));
            }
            
        }

        public void MoveAgent(Vector3 point)
        {
            Vector3 direction = (point - transform.position).normalized;

            RotateTo(direction, () =>
            {
                agent.SetDestination(new Vector3(point.x, 0.5f, point.z));
            });
        }

        public override void Movement(Vector3 moveToDirection)
        {
            agent.isStopped = !agent.isStopped;
            agent.ResetPath();
            base.Movement(moveToDirection);
            agent.isStopped = !agent.isStopped;
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>(); 
            _rigidbody = GetComponent<Rigidbody>();
        }
        #endregion
        #endregion
    }
}