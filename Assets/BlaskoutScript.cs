using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace Thirst
{
    public class BlaskoutScript : MonoBehaviour
    {
        public GameObject audio;
        public SwitchSceneScript switchScene;
        public void EndScene()
        {
            audio.GetComponent<Animator>().Play("endSound");
        }
        public void StartScene()
        {
            audio.GetComponent<Animator>().Play("startSound");
        }
        public void endBlackout()
        {
            SceneManager.LoadScene(switchScene.sceneName);
        }
    }

}
