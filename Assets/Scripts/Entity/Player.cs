using Cinemachine;
using TMPro;
using UnityEngine;
using RPGCharacterAnims;
using RPGCharacterAnims.Lookups;

namespace Thirst
{
    public class Player : Entity
    {

        public CinemachineVirtualCamera camera;
        public TextMeshProUGUI textWaterCounter;

        public float runSpeed = 1;
        public float walkSpeed = 0.5f;
        public bool isAttack;

        private PlayerUI _playerUI;
        public float WaterCounter { get; set; }


        public void UpdateWaterCount()
        {
            textWaterCounter.text = WaterCounter.ToString();
            FindObjectOfType<LoadManager>().SaveGame();
        }
        public override void Unarmed()
        {
            GetComponent<RPGCharacterMovementController>().runSpeed = 1;
            base.Unarmed();
            _timeAttackCoolDown = _attackCollDown;
            isAttack = false;
        }
        public void Kill(Entity entity)
        {
            if (trigger.GetComponent<Entity>() == entity)
            {
                trigger = null;
            }
            enemys.Remove(entity.gameObject);
            SortTrigger(entity.gameObject);
        }


        protected override void Armed()
        {
            if (!rpgCharacterController.HandlerExists(HandlerTypes.SwitchWeapon)) { return; }
            isAttack = true;
            Stopped();

            base.Armed();

            if (rpgCharacterController.TryEndAction(HandlerTypes.SwitchWeapon))
            {
                Attack();
            }
        }
        protected override void RangeArmed()
        {
            if (!rpgCharacterController.HandlerExists(HandlerTypes.SwitchWeapon)) { return; }
            Stopped();
            base.RangeArmed();

        }
        protected override void DeathObject()
        {
            print("death " + name);
            Destroy(gameObject);
        }

        private void Stopped()
        {
            GetComponent<RPGCharacterMovementController>().runSpeed = 0;
            _rigidbody.velocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if(_timeStanCoolDown <= 0 && _timeAttackCoolDown <= 0)
            {
                if (_rigidbody.velocity.magnitude > 0.01)
                {
                    MoveState();
                }
                else if (_rigidbody.velocity.magnitude < 0.01 && _state != StateEntity.Idle)
                {
                    IdleState();
                }
            }
        }
        private void Update()
        {
            ProcessCooldown();
            ProcessStates();
            if(trigger == null && enemys.Count > 0)
            {
                SortTrigger(null);
            }

            if ((_playerUI.isButtonMelleAttackPressed || _playerUI.isRangeAttack) && trigger!=null)
            {
                RotateTo(trigger.transform.position - transform.position);
            }
        }
        private void Start()
        {
            FindObjectOfType<PlayerInfo>().player = this;
            FindObjectOfType<CameraModifier>().player = this;

            camera = FindObjectOfType<CinemachineVirtualCamera>();
            _playerUI = FindAnyObjectByType<PlayerUI>();
            _rigidbody = GetComponent<Rigidbody>();

            camera.Follow = gameObject.transform;
            camera.LookAt = gameObject.transform;

            _playerUI.melleAttack = () => Armed();
            _playerUI.onBeginRangeAttackAction = () => RangeArmed();
            _playerUI.rangeAttack = () => Attack();
            _playerUI.player = this;
        }

        public delegate void SetPostItem(Item item);
    }
}