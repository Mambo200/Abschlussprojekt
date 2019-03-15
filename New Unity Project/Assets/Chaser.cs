using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Chaser : PlayerEntity {
    /// <summary>Chaser Singleton</summary>
    private static Chaser _single;
    private static object _lock;

    /// <summary>Chaser Singleton Getter</summary>
    public static Chaser Get
    {
        [Server]
        get
        {
            if (_single == null)
            {
                lock (_lock)
                {
                    if (_single == null)
                    {
                        _single = new Chaser();
                    }
                }
            }

            return _single;
        }
    }

    /// <summary>Chaser Singleton</summary>
    public PlayerEntity currentChaser;

    /// <summary>List with all players</summary>
    private List<PlayerEntity> allPlayers = new List<PlayerEntity>();

    /// <summary>
    /// Add Player to list
    /// </summary>
    /// <param name="_player">Player to add</param>
    public void AddPlayer(PlayerEntity _player)
    {
        allPlayers.Add(_player);
    }

    /// <summary>
    /// Remove Player from list
    /// </summary>
    /// <param name="_player">Player to remove</param>
    public void RemovePlayer(PlayerEntity _player)
    {
        allPlayers.Remove(_player);
    }
}
