using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyLobbyManager : NetworkLobbyManager {

    public static NetworkLobbyPlayer[] GetSingleton { get { return ((MyLobbyManager)singleton).lobbySlots; } }
    private void Start()
    {
        GetSingleton[0].GetComponent<PlayerEntity>().ch
        //((MyLobbyManager)singleton).lobbySlots;
    }

    public override void OnLobbyClientEnter()
    {
        base.OnLobbyClientEnter();
    }
}
