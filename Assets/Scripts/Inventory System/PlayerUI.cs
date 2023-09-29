using System;
using UnityEngine;

namespace Aquapunk
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
        private void Update()
        {
            if (isButtonMelleAttackPressed && !player.isAttack && player.TimeAttackCoolDown <= 0)
            {
                melleAttack.Invoke();
            }

            if (player.shotCount >= 3 && !isButtonRangeAttackPressed && isRangeAttack && player.endShot && player.TimeAttackCoolDown <= 0)
            {
                isRangeAttack = false;
                player.Unarmed();
            }
            if (isRangeAttack && player.TimeAttackCoolDown <= 0)
            {
                rangeAttack.Invoke();
            }
        }

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

    }
}

