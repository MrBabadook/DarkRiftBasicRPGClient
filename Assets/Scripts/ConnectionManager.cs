using System;
using System.Net;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkRiftRPG
{
    public class ConnectionManager : MonoBehaviour
    {
        //We want a static reference to ConnectionManager so it can be called directly from other scripts
        public static ConnectionManager Instance;
        //A reference to the Client component on this game object. 
        public UnityClient Client { get; private set; }

        public ushort LocalClientID;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this; 
            DontDestroyOnLoad(this); 
        }
        void Start()
        {
            Client = GetComponent<UnityClient>();
            Client.ConnectInBackground(IPAddress.Loopback, Client.Port, false, ConnectCallback);
            Client.MessageReceived += OnMessage;
        }

        public void SendClickPosToServer(Vector3 point)
        {
            using (Message message = Message.Create((ushort)Tags.PlayerMovementRequest, new PlayerMovementRequestData(point)))
            {
                Client.SendMessage(message, SendMode.Reliable);
            }
        }

        private void ConnectCallback(Exception e)
        {
            if (Client.ConnectionState == ConnectionState.Connected) 
            {
                Debug.Log("Connected to server!");
                OnConnectedToServer();

            }
            else
            {
                Debug.LogError($"Unable to connect to server. Reason: {e.Message} "); 
            }
        }

        private void OnConnectedToServer()
        {
            using (Message message = Message.CreateEmpty((ushort)Tags.JoinGameRequest))
            {
                Client.SendMessage(message, SendMode.Reliable);
            }
        }

        private void OnMessage(object sender, MessageReceivedEventArgs e)
        {
            using (Message m = e.GetMessage())
            {
                switch ((Tags)m.Tag)
                {
                    case Tags.JoinGameResponse:
                        OnPlayerJoinGameResponse(m.Deserialize<JoinGameResponseData>());
                        break;
                    case Tags.PlayerMovementUpdate:
                        OnPlayerMovementUpdate(m.Deserialize<ProccessedPlayerMovementData>());
                        break;
                    case Tags.SpawnPlayer:
                        OnSpawnPlayer(m.Deserialize<PlayerSpawnData>());
                        break;
                    case Tags.DespawnPlayer:
                        OnDespawnPlayer(m.Deserialize<PlayerDespawnData>());
                        break;
                }
            }
        }

        private void OnDespawnPlayer(PlayerDespawnData data)
        {
            GameManager.Instance.RemovePlayerFromGame(data);
        }

        private void OnSpawnPlayer(PlayerSpawnData data)
        {
            GameManager.Instance.SpawnPlayer(data);
        }

        private void OnPlayerMovementUpdate(ProccessedPlayerMovementData data)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.HandlePlayerMovementUpdate(data);
        }

        private void OnPlayerJoinGameResponse(JoinGameResponseData data)
        {
            if (!data.JoinGameRequestAccepted)
            {
                Debug.Log("houston we have a problem");
                return;
            }
            LocalClientID = Client.Client.ID;
            SceneManager.LoadScene("Game");
        }

        public void SpawnLocalPlayerRequest()
        {
            using (Message message = Message.CreateEmpty((ushort)Tags.SpawnLocalPlayerRequest))
            {
                Client.SendMessage(message, SendMode.Reliable);
            }
        }

        private void OnDestroy()
        {
            Client.MessageReceived -= OnMessage;
        }
    }

}
