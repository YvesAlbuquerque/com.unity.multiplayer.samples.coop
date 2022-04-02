using Unity.Multiplayer.Samples.BossRoom;
using Netcode.Transports.PhotonRealtime;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;
using Unity.Netcode;

public class RoomNameBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_RoomNameText;

    void OnEnable()
    {
        Assert.IsNotNull(m_RoomNameText, $"{nameof(m_RoomNameText)} not assigned!");

        var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        bool isUsingRelay = true;
        switch (transport)
        {
            case PhotonRealtimeTransport realtimeTransport:
                m_RoomNameText.text = $"Loading room key...";
                break;
            case UnityTransport utp:
                if (utp.Protocol == UnityTransport.ProtocolType.RelayUnityTransport)
                {
                    m_RoomNameText.text = $"Loading join code...";
                }
                else
                {
                    isUsingRelay = false;
                }
                break;
            default:
                isUsingRelay = false;
                break;
        }

        if (!isUsingRelay)
        {
            // RoomName should only be displayed when using relay.
            Destroy(gameObject);
        }
    }

    // This update loop exists because there is currently a bug in Netcode for GameObjects which runs client connected callbacks before the transport has
    // fully finished the asynchronous connection. That's why are loading the character select screen too early and need this update loop to
    // update the room key once we are fully connected to the Photon cloud.
    public void ConnectionFinish()
    {
        var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        if (transport == null)
            return;

        if (transport is PhotonRealtimeTransport realtimeTransport && realtimeTransport.Client != null && string.IsNullOrEmpty(realtimeTransport.Client.CloudRegion) == false)
        {
            string roomName = $"{realtimeTransport.Client.CloudRegion.ToUpper()}_{realtimeTransport.RoomName}";
            m_RoomNameText.text = $"Room Name: {roomName}";
        }
        else if (transport is UnityTransport utp && !string.IsNullOrEmpty(RelayJoinCode.Code))
        {
            m_RoomNameText.text = RelayJoinCode.Code;
        }
    }
}
