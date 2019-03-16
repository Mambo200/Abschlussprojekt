using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Chaser : PlayerEntity {
    
    /// <summary>static Random</summary>
    private static readonly System.Random random = new System.Random();
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
    private List<PlayerEntity> AllPlayers
    {
        [Server]
        get
        {
            // create new list
            List<PlayerEntity>playerList = new List<PlayerEntity>();
            // get all players
            NetworkLobbyPlayer[] player = ((MyLobbyManager)MyLobbyManager.GetSingleton).lobbySlots;

            // get Playerentity from players and add to list
            foreach (NetworkLobbyPlayer p in player)
            {
                playerList.Add(p.gameObject.GetComponent<PlayerEntity>());
            }

            // return list
            return playerList;
        }
    }

    /// <summary>
    /// Chooses the chaser randomly. Eliminate players who do not have the lowest amount of Chaser players, afterwards choose randomly from pool
    /// </summary>
    [Server]
    private void ChooseChaser()
    {
        // set stats for last round chaser
        currentChaser.SetChaser(false);

        // get chaser pool and exclude current chaser
        List<PlayerEntity> chaserPool = AllPlayers;
        chaserPool.Remove(currentChaser);
        currentChaser = null;

        // get lowest Chaser number and remove player who was chaser last round
        int lowest = int.MaxValue;
        foreach (PlayerEntity item in AllPlayers)
        {
            // check if player was chaser last round
            if (item.WasChaserLastRound)
            {
                // set chaser to false
                item.SetChaser(false);
                chaserPool.Remove(item);
                continue;
            }

            // check if chaser amount is lower than lowest value
            if (item.ChaserCount <= lowest)
            {
                // if true set lowest value
                lowest = ChaserCount;
            }
            else
            {
                // set chaser to false
                item.SetChaser(false);
                // if false remove this player from chaser pool
                chaserPool.Remove(item);
            }
        }

        // a Copy of the current chaser pool for foreach
        List<PlayerEntity> copyChaserPool = chaserPool;

        // check if players chasercount in List are equal lowest. if not remove from pool
        foreach (PlayerEntity item in copyChaserPool)
        {
            if (item.ChaserCount != lowest)
            {
                // set chaser mode to false
                item.SetChaser(false);
                // remove player from pool
                chaserPool.Remove(item);
            }
        }

        // delete poolcopy
        copyChaserPool = null;

        // chose player randomly
        int chaserIndex = Random.Range(0, (chaserPool.Count - 1));

        // save player
        currentChaser = chaserPool[chaserIndex];

        // set chaser
        currentChaser.SetChaser(true);

        // set all other players chaser status to false
        foreach (PlayerEntity item in chaserPool)
        {
            // if current item is the chaser continue
            if (item == currentChaser)
                continue;

            item.SetChaser(false);
        }
    }
}
