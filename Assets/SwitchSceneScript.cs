using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thirst
{

    public class SwitchSceneScript : MonoBehaviour
    {
        public string sceneName;
        public GameObject switchScenePanel;
        public GameObject playerUI;
        public GameObject blackout;

        public void Yes()
        {
            blackout.SetActive(true);
            blackout.GetComponent<Animator>().SetTrigger("end game");

            switchScenePanel.SetActive(false);
            playerUI.SetActive(true);
        }


        public void No()
        {
            switchScenePanel.SetActive(false);
            playerUI.SetActive(true);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>())
            {
                playerUI.SetActive(false);
                switchScenePanel.SetActive(true);
            }
        }
    }
}
