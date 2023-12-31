﻿using RPGCharacterAnims.Extensions;
using System.Collections;
using RPGCharacterAnims.Lookups;
using UnityEngine;
using Thirst;
using System;

namespace RPGCharacterAnims
{
    public class IKHands : MonoBehaviour
    {
		public Action LastShotInLine;
		public GameObject moveTrale;
		public Entity entity;
        public Transform leftHandObj;
        public Transform attachLeft;
        public bool canBeUsed;
		public bool isUsed;
        [Range(0, 1)] public float leftHandPositionWeight;
        [Range(0, 1)] public float leftHandRotationWeight;

        private Transform blendToTransform;
		private Coroutine co;

        private Animator animator;
        private RPGCharacterWeaponController rpgCharacterWeaponController;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rpgCharacterWeaponController = GetComponentInParent<RPGCharacterWeaponController>();
        }

		/// <summary>
		/// If there is movement and/or rotation data in the animation for the Left Hand, use IK to
		/// set the position of the Left Hand of the character.
		/// </summary>
		private void OnAnimatorIK(int layerIndex)
		{
			if (!leftHandObj) { return; }
			animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandPositionWeight);
			animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandRotationWeight);
			if (!attachLeft) { return; }
			animator.SetIKPosition(AvatarIKGoal.LeftHand, attachLeft.position);
			animator.SetIKRotation(AvatarIKGoal.LeftHand, attachLeft.rotation);
		}

		/// <summary>
		/// Smoothly blend IK on and off so there's no snapping into position.
		/// </summary>
		/// 

		public void MovePatr()
        {
			Instantiate(moveTrale, transform.position, transform.rotation);
        }

		public void BlendIK(bool blendOn, float delay, float timeToBlend, Weapon weapon)
		{
            // If using 2 handed weapon.
			if (weapon.Is2HandedWeapon()) {
				if (blendOn) { isUsed = true; }
			}
			if (canBeUsed & isUsed) {
				StopAllCoroutines();
				co = StartCoroutine(_BlendIK(blendOn, delay, timeToBlend, weapon));
			}
			if (!blendOn) { isUsed = false; }
		}

		private IEnumerator _BlendIK(bool blendOn, float delay, float timeToBlend, Weapon weapon)
        {
            GetCurrentWeaponAttachPoint(weapon);
			yield return new WaitForSeconds(delay);
			var t = 0f;
			var blendTo = 0;
			var blendFrom = 0;

			if (blendOn) { blendTo = 1; }
			else { blendFrom = 1; }
			while (t < 1) {
				t += Time.deltaTime / timeToBlend;
				attachLeft = blendToTransform;
				leftHandPositionWeight = Mathf.Lerp(blendFrom, blendTo, t);
				leftHandRotationWeight = Mathf.Lerp(blendFrom, blendTo, t);
				yield return null;
			}
        }

		public void Shot()
        {
			entity.RangeShot();
		}

		public void LastShot()
		{
			LastShotInLine?.Invoke();
		}

		public void Hit()
        {
			entity.MelleHit();
        }

		public void EndHit()
        {
			Player player = transform.parent.GetComponent<Player>();
            if (player)
            {
				player.Unarmed();
            }
        }

		public void Death()
        {

			transform.parent.GetComponent<RPGCharacterController>()!.EndAction(HandlerTypes.Navigation);
			transform.parent.GetComponent<RPGCharacterController>()!.EndAction(HandlerTypes.Move);
			transform.parent.GetComponent<CapsuleCollider>()!.enabled = false;

			transform.parent.GetComponent<Rigidbody>()!.constraints = RigidbodyConstraints.FreezePosition;
			//transform.parent.GetComponent<RPGCharacterMovementController>()!.LockMovement();
			transform.parent.GetComponent<RPGCharacterController>()!.Lock(true, true, true, 0.5f, 9999999999);

			transform.parent.GetComponent<Mob>().enabled = false;
		}

		/// <summary>
		/// Pauses IK while character uses Left Hand during an animation.
		/// </summary>

		public void SetIKPause(float pauseTime)
		{
			if (!canBeUsed || !isUsed) { return; }
			StopAllCoroutines();
			co = StartCoroutine(_SetIKPause(pauseTime));
		}

		private IEnumerator _SetIKPause(float pauseTime)
		{
			var t = 0f;
			while (t < 1) {
				t += Time.deltaTime / 0.1f;
				leftHandPositionWeight = Mathf.Lerp(1, 0, t);
				leftHandRotationWeight = Mathf.Lerp(1, 0, t);
				yield return null;
			}
			yield return new WaitForSeconds(pauseTime - 0.2f);
			t = 0f;
			while (t < 1) {
				t += Time.deltaTime / 0.1f;
				leftHandPositionWeight = Mathf.Lerp(0, 1, t);
				leftHandRotationWeight = Mathf.Lerp(0, 1, t);
				yield return null;
			}
		}

		private void GetCurrentWeaponAttachPoint(Weapon weapon)
		{
			var weaponType = (Weapon)weapon;
			switch (weaponType) {
				case Weapon.TwoHandSword: blendToTransform = rpgCharacterWeaponController.twoHandSword.transform; break;
				case Weapon.Range: blendToTransform = rpgCharacterWeaponController.range.transform.GetChild(0).transform; break;
			}
		}
    }
}