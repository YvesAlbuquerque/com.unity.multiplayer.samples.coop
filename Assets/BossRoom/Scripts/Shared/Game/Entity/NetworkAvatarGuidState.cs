using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Multiplayer.Samples.BossRoom
{
    /// <summary>
    /// NetworkBehaviour component to send/receive GUIDs from server to clients.
    /// </summary>
    public class NetworkAvatarGuidState : NetworkBehaviour
    {
        [FormerlySerializedAs("AvatarGuidArray")]
        [HideInInspector]
        public NetworkVariable<NetworkGuid> AvatarGuid = new NetworkVariable<NetworkGuid>();

        CharacterClassContainer m_CharacterClassContainer;

        [SerializeField]
        AvatarRegistry m_AvatarRegistry;

        Avatar m_Avatar;

        public Avatar RegisteredAvatar
        {
            get
            {
                if (m_Avatar == null)
                {
                    RegisterAvatar(AvatarGuid.Value.ToGuid());
                }

                return m_Avatar;
            }
        }

        private void Awake()
        {
            m_CharacterClassContainer = GetComponent<CharacterClassContainer>();
            DontDestroyOnLoad(gameObject);

            Debug.Log(gameObject);
            Debug.Log(m_AvatarRegistry);
        }

        public void RegisterAvatar(Guid guid)
        {
            Debug.Log(guid);
            if (guid.Equals(Guid.Empty))
            {
                // not a valid Guid
                return;
            }

            // based on the Guid received, Avatar is fetched from AvatarRegistry
            if (!m_AvatarRegistry.TryGetAvatar(guid, out Avatar avatar))
            {
                Debug.LogError("Avatar not found!");
                return;
            }
            Debug.Log(avatar);
            if (m_Avatar != null)
            {
                // already set, this is an idempotent call, we don't want to Instantiate twice
                return;
            }

            m_Avatar = avatar;
            Debug.Log(avatar.CharacterClass);

            m_CharacterClassContainer.SetCharacterClass(avatar.CharacterClass);
        }
    }
}
