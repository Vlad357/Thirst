using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

namespace Aquapunk
{
    public class EntityMovement : MonoBehaviour
    {
        #region Fields
        public float speedRotate;

        protected Animator _animator;

        [SerializeField]
        protected float _rollSpeed = 5f;
        [SerializeField]
        protected float _timerRoll, _rollTime = 1f;
        [SerializeField]
        protected float _timeRollCoolDown, _rollCoolDown = 0.5f;
        [SerializeField]
        protected bool _isRolling;

        protected Rigidbody _rigidbody;
        [SerializeField] protected float _speed = 5.5f;
        #endregion
        #region Methods
        #region Class Methods

        public void RotateTo(Vector3 direction)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.DORotateQuaternion(Quaternion.Lerp(transform.rotation, lookRotation, 1), speedRotate);
        }


        public void RotateTo(Vector3 direction, TweenCallback tweenCallback)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.DORotateQuaternion(Quaternion.Lerp(transform.rotation, lookRotation, 1), speedRotate).OnComplete(tweenCallback);
        }

        public virtual void Movement(Vector3 moveToDirection)
        {
            //move to directional on joistick
            moveToDirection = new Vector3(moveToDirection.x, 0, moveToDirection.z);
            Vector3 dir = moveToDirection.normalized;
            _rigidbody.velocity = (dir * _speed * Time.fixedDeltaTime);
            //rotate to directional movement
            RotateTo(dir);
        }

        public virtual void Roll()
        {
            if (!_isRolling && _timeRollCoolDown <= 0)
            {
                _isRolling = true;
                _timerRoll = 0;
                //_animator.SetTrigger("Roll");
                
            }
        }

        private void Rolling()
        {
            _timerRoll += Time.deltaTime;
            float t = Mathf.Clamp01(_timerRoll / _rollTime);
            float rollDistance = _rollSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * rollDistance);

            if (t >= 1f)
            {
                _isRolling = false;
                _timeRollCoolDown = _rollCoolDown;
            }
        }
        #endregion
        #region Unity Methods
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_isRolling)
            {
                Rolling();
            }
        }

        private void FixedUpdate()
        {
            if(_timeRollCoolDown > 0)
            {
                _timeRollCoolDown -= Time.fixedDeltaTime;
            }
        }
        #endregion
        #endregion
    }
}