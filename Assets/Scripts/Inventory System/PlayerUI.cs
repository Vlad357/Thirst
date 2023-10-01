using RPGCharacterAnims;
using System;
using UnityEngine;

namespace Thirst
{
    public class PlayerUI : MonoBehaviour
    {
        public Player player;

        public Action melleAttack;
        public Action rangeAttack;
        public Action onBeginRangeAttackAction;

        public bool isButtonMelleAttackPressed;
        public bool isButtonRangeAttackPressed;

        public bool isRangeAttack;

        public void OnBeginRangeAttack()
        {
            onBeginRangeAttackAction.Invoke();
            isRangeAttack = true;
            player.shotCount = 0;
            isButtonRangeAttackPressed = true;
        }
        public void OnEndRangeAttack()
        {
            isButtonRangeAttackPressed = false;
        }
        public void OnBeginAttack()
        {
            isButtonMelleAttackPressed = true;
        }
        public void OnEndAttack()
        {
            isButtonMelleAttackPressed = false;

        }
        public void LateShotSubscribe()
        {
            player.GetComponentInChildren<IKHands>().LastShotInLine = DetectedLastShotInLine;
        }

        private void DetectedLastShotInLine()
        {
            if (player.shotCount >= 3 && !isButtonRangeAttackPressed && isRangeAttack)
            {
                print("last shot");
                isRangeAttack = false;
                player.Unarmed();
            }
        }

        private void Update()
        {
            if (isButtonMelleAttackPressed && !player.isAttack && player.TimeAttackCoolDown <= 0)
            {
                melleAttack.Invoke();
            }

            if (isRangeAttack && player.TimeAttackCoolDown <= 0)
            {
                rangeAttack.Invoke();
            }
        }

    }
}

