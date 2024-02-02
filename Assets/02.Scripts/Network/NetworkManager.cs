using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using Fusion.Sockets;
using Fusion.XR.Host.Rig;
using Cysharp.Threading.Tasks;
using Photon.Realtime;

public struct ENetworkInfo
{
    public string playerName;
}

// 네트워크 매니저
// 최초 작성자 : 김기홍
// 수정자 : 
// 최종 수정일 : 2024-01-10
public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    // Singleton
    public static NetworkManager _instance = null;

    public NetworkRunner _runner { get; private set; }
    [SerializeField] NetworkObject _audiencePrefab;
    [SerializeField] NetworkObject _idolPrefab;

    [Header("for Debug")]
    [SerializeField] PlayerManager _playerManager;
    [SerializeField] AudienceHardwareRig _audienceRig;
    [SerializeField] IdolHardwareRig _idolRig;

    // Dictionary of spawned user prefabs, to store them on the server for host topology, and destroy them on disconnection (for shared topology, use Network Objects's "Destroy When State Authority Leaves" option)
    public Dictionary<PlayerRef, NetworkObject> SpawnedUsers { get; set; }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _runner = GetComponent<NetworkRunner>();
    }


    [Header("Session List")]
    private List<SessionInfo> sessions = new List<SessionInfo>();
    public List<SessionInfo> Sessions { get { return sessions; } }

    // Lobby 입장
    public async UniTask ConnectToLobby(string _playername)
    {
        if (_runner == null)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
        }

        Debug.Log("Join Lobby : Loading");
        await _runner.JoinSessionLobby(SessionLobby.ClientServer);
        Debug.Log("Join Lobby : Success To Connect");
    }

    // Session 생성 (Host)
    public async UniTask CreateSession(string _roomName)
    {
        if (_runner == null)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
        }
        if(_idolRig == null)
        {
            _idolRig = FindObjectOfType<IdolManager>().GetComponent<IdolHardwareRig>();
        }
        if (_audienceRig == null)
        {
            _audienceRig = FindObjectOfType<AudienceManager>().GetComponent<AudienceHardwareRig>();
        }

        _idolRig.gameObject.SetActive(true);
        _audienceRig.gameObject.SetActive(false);
        _playerManager = _idolRig.GetComponent<PlayerManager>();
        _playerManager.SetRole(PlayerManager.Role.Idol);

        Debug.Log("Create Session(Room) : Loading");
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = _roomName,
            PlayerCount = 20,
        });
        Debug.Log("Create Session(Room) : Success To Create");

    }

    // Session 입장 (Client)
    public async UniTask ConnectToSession(string _roomName)
    {
        if(_runner == null)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
        }
        if (_idolRig == null)
        {
            _idolRig = FindObjectOfType<IdolManager>().GetComponent<IdolHardwareRig>();
        }
        if (_audienceRig == null)
        {
            _audienceRig = FindObjectOfType<AudienceManager>().GetComponent<AudienceHardwareRig>();
        }

        _idolRig.gameObject.SetActive(false);
        _audienceRig.gameObject.SetActive(true);
        _playerManager = _audienceRig.GetComponent <PlayerManager>();
        _playerManager.SetRole(PlayerManager.Role.Audience);

        Debug.Log("Connect Session(Room) : Loading");
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName = _roomName,
        });
        Debug.Log("Connect Session(Room) : Success To Connect");
    }
    public async UniTask DisconnectToSession()
    {
        Destroy(_runner);

        _runner = gameObject.AddComponent<NetworkRunner>();
        await _runner.JoinSessionLobby(SessionLobby.ClientServer);
    }

    #region INetworkRunnerCallbacks - Player
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"OnPlayerJoined. PlayerId: {player.PlayerId}");
        if (SpawnedUsers == null) SpawnedUsers = new Dictionary<PlayerRef, NetworkObject>();
        NetworkObject networkPlayerObject = null;
        if (runner.IsServer)
        {
            if (SpawnedUsers.Count == 0)
            {
                networkPlayerObject =
                    runner.Spawn
                    (
                        _idolPrefab,
                        position: transform.position,
                        rotation: transform.rotation,
                        inputAuthority: player,
                        (runner, obj) => { }
                    );
            }
            else
            {
                networkPlayerObject =
                    runner.Spawn
                    (
                        _audiencePrefab,
                        position: transform.position,
                        rotation: transform.rotation,
                        inputAuthority: player,
                        (runner, obj) => { }
                    );
            }
            SpawnedUsers.Add(player, networkPlayerObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"OnPlayerLeft. PlayerId: {player.PlayerId}");
        // Find and remove the players avatar (only the host would have stored the spawned game object)
        if (SpawnedUsers.TryGetValue(player, out NetworkObject networkObject))
        {
            if(networkObject != null)
                runner.Despawn(networkObject);
            SpawnedUsers.Remove(player);
        }
    }
    #endregion

    #region INetworkRunnerCallbacks - Connect
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        sessions.Clear();
        sessions = sessionList;
    }
    #endregion

    #region INetworkRunnerCallbacks (debug log only)
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("OnConnectRequest");
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectedToServer");

    }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("Shutdown: " + shutdownReason);
    }
    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("OnDisconnectedFromServer");
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("OnConnectFailed: " + reason);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // Debug.Log("OnInput");
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("OnInputMissing");
    }
    #endregion

    #region Not Use - INetworkRunnerCallbacks
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }


    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }
    #endregion
}
