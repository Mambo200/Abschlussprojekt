using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyLobbyManager : NetworkLobbyManager {

    /// <summary>Get every Player in Lobby</summary>
    public static NetworkLobbyPlayer[] GetSingletonPlayer { get { return ((MyLobbyManager)singleton).lobbySlots; } }
    /// <summary>Get Networkmanager Singleton</summary>
    public static NetworkManager GetSingleton { get { return singleton; } }

    public override void OnLobbyClientEnter()
    {
        base.OnLobbyClientEnter();
    }
}
