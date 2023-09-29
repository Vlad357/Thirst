using UnityEngine;

namespace Thirst {
    public class MusicPlayer : MonoBehaviour
    {
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.Play();
        }

    }
}

