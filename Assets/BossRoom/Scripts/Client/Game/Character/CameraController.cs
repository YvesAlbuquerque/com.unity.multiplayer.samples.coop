using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Multiplayer.Samples.BossRoom.Visual
{
    public class CameraController : MonoBehaviour
    {
        private Animator animator;
        private CinemachineFreeLook m_MainCamera;

        void Start()
        {
            AttachCamera();
        }

        private void AttachCamera()
        {
            m_MainCamera = GameObject.FindObjectOfType<CinemachineFreeLook>();
            animator = GetComponent<Animator>();
            Assert.IsNotNull(m_MainCamera, "CameraController.AttachCamera: Couldn't find gameplay freelook camera");

            if (m_MainCamera)
            {
                // camera body / aim
                m_MainCamera.Follow = transform.parent;
                m_MainCamera.LookAt = transform.parent;
                // default rotation / zoom
                //m_MainCamera.m_Heading.m_Bias = 0f;
                m_MainCamera.m_YAxis.Value = 0.5f;
            }
        }
    }
}
