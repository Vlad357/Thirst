using Cinemachine;
using UnityEngine;

namespace Thirst
{
    public class CameraModifier : MonoBehaviour
    {
        public Player player;
        public float buildOrthoSize, trevelOrthoSize;

        [SerializeField] private CinemachineVirtualCamera VirtualCamera;

        public void OnBuildMod()
        {
            CameraMod(buildOrthoSize, null, true);
        }

        public void OnTrevelMod()
        {
            CameraMod(trevelOrthoSize, player.transform, false);
        }

        private void CameraMod(float orthoSize, Transform trigger, bool buildMod)
        {
            VirtualCamera.m_Lens.OrthographicSize = orthoSize;
            VirtualCamera.LookAt = trigger;
            VirtualCamera.Follow = trigger;
            //player.buildMod = buildMod;
        }

        private void Start()
        {
            VirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
    }
}