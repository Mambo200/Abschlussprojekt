﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public abstract class AEntity : NetworkBehaviour
{
    protected Rigidbody m_rigidbody;

    #region Game Variables
    /// <summary>Max HP variable (DO NOT USE! USE <see cref="MaxHP"/> INSTEAD)</summary>
    [SyncVar]
    private float maxHP = 150;
    /// <summary>Max HP property (Everyone can get, only Server can set in <see cref="SetMaxHP(float)"/>)</summary>
    public float MaxHP
    {
        get { return maxHP; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set Max HP to " + value, this.gameObject);
                return;
            }

            BeforeMaxHPChanged();
            maxHP = value;
            AfterMaxHPChanged();
        }
    }

    /// <summary>Current HP variable (DO NOT USE! USE <see cref="CurrentHP"/> INSTEAD)</summary>
    [SyncVar]
    private float currentHP;
    /// <summary>Current HP property (Everyone can get, only Server can set is <see cref="SetCurrentHP(float)"/></summary>
    public float CurrentHP
    {
        get { return currentHP; }
        [Server]
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set current HP to " + value, this.gameObject);
                return;
            }

            BeforeCurrentHPChanged();
            currentHP = value;
            AfterCurrentHPChanged();
        }
    }

    /// <summary>Max SP variable (DO NOT USE! USE <see cref="MaxSP"/> INSTEAD)</summary>
    [SyncVar]
    private float maxSP = 150;
    /// <summary>Max SP property (Everyone can get, only Server can set in <see cref="SetMaxSP(float)"/>)</summary>
    public float MaxSP
    {
        get { return maxSP; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set current SP to " + value, this.gameObject);
                return;
            }

            BeforeMaxSPChanged();
            maxSP = value;
            AfterMaxSPChanged();

        }
        /// <summary>Max HP property (Everyone can get, only Server can set)</summary>
    /// <summary>Max HP property (Everyone can get, only Server can set)</summary>
    }

    /// <summary>Current SP variable (DO NOT USE! USE <see cref="CurrentSP"/> INSTEAD)</summary>
    [SyncVar]
    private float currentSP;
    /// <summary>Current HP property (Everyone can get, only Server can set in <see cref="SetCurrentSP(float)"/>)</summary>
    public float CurrentSP
    {
        get { return currentSP; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set current SP to " + value, this.gameObject);
                return;
            }

            BeforeCurrentSPChanged();
            currentSP = value;
            AfterCurrentSPChanged();
        }
    }

    /// <summary>Default variable of Player armor. 
    /// If you want to change the current armor of player use <see cref="CurrentArmor"/> instead. (DO NOT USE! USE <see cref="PlayerArmor"/> INSTEAD)</summary>
    [SyncVar]
    private float playerArmor = 0f;
    /// <summary>Player Armor property. Please change current armor in <see cref="CurrentArmor"/>. (Everyone can get, only Server can set in <see cref="SetDefaultPlayerArmor(float)"/>)</summary>
    public float PlayerArmor
    {
        get { return playerArmor; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set default value of Player Armor to " + value, this.gameObject);
                return;
            }

            BeforeDefaultPlayerArmorChanged();
            playerArmor = value;
            AfterDefaultPlayerArmorChanged();
        }
    }

    /// <summary>Default variable of Chaser armor. 
    /// If you want to change the current armor of chaser use <see cref="CurrentArmor"/> instead. (DO NOT USE! USE <see cref="ChaserArmor"/> INSTEAD)</summary>
    [SyncVar]
    private float chaserArmor = 50f;
    /// <summary>Chaser Armor property. Please change current armor in <see cref="CurrentArmor"/>. (Everyone can get, only Server can set in <see cref="SetDefaultChaserArmor(float)"/>)</summary>
    public float ChaserArmor
    {
        get { return chaserArmor; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set default value of chaser Armor to " + value, this.gameObject);
                return;
            }

            BeforeDefaultChaserArmorChanged();
            playerArmor = value;
            AfterDefaultChaserArmorChanged();
        }

    }

    /// <summary>Current Armor variable (DO NOT USE! USE <see cref="CurrentArmor"/> INSTEAD)</summary>
    [SyncVar]
    private float currentArmor;
    /// <summary>Current Armor property (Everyone can get, only Server can set in <see cref="SetCurrentArmor(float)"/>)</summary>
    public float CurrentArmor
    {
        get
        {
            return currentArmor;
        }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set current armor to " + value, this.gameObject);
                return;
            }
            BeforeCurrentArmorChanged();
            currentArmor = value;
            AfterCurrentArmorChanged();
        }
    }

    /// <summary>How often was player Chaser</summary>
    [SyncVar]
    private int chaserCount = 0;
    /// <summary>Current Chaser count property (Everyone can get, only Server can set)</summary>
    public int ChaserCount
    {
        get { return chaserCount; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set Chaser count to " + value, this.gameObject);
                return;
            }
            BeforeChaserCountChanged();
            chaserCount = value;
            AfterChaserCountChanged();
        }
    }

    /// <summary>True if player was chaser last round</summary>
    [SyncVar]
    private bool wasChaserLastRound = false;
    /// <summary>Was Chaser last round property (Everyone can get, only Server can set)</summary>
    public bool WasChaserLastRound
    {
        get { return wasChaserLastRound; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set was chaser last round to " + value, this.gameObject);
                return;
            }
            BeforeLastRoundChaserChanged();
            wasChaserLastRound = value;
            AfterLastRoundChaserChanged();
        }
    }

    /// <summary>Chaser variable (DO NOT USE! USE <see cref="IsChaser"/> INSTEAD)</summary>
    [SyncVar]
    private bool isChaser = false;
    /// <summary>Chaser property (Everyone can get, only Server can set in <see cref="SetChaser(bool)"/>)</summary>
    public bool IsChaser
    {
        get { return isChaser; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set Chaser mode to " + value, this.gameObject);
                return;
            }
            BeforeChaserChanged();
            isChaser = value;
            AfterChaserChanged();
        }
    }
    #endregion

    #region Override Functions
    public override void OnStartServer()
    {
        base.OnStartServer();
        Initialize();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Initialize();
    }
    #endregion

    #region Abstract Functions
    #endregion

    #region Public Functions
    /// <summary>
    /// Set new max HP using Property (<see cref="MaxHP"/>)
    /// </summary>
    /// <param name="_newMaxHP">new max HP of player</param>
    public void SetMaxHP (float _newMaxHP)
    {
        MaxHP = _newMaxHP;
    }
    /// <summary>
    /// Set new current HP using Property (<see cref="CurrentHP"/>)
    /// </summary>
    /// <param name="_newCurrentHP">new current HP of player</param>
    public void SetCurrentHP (float _newCurrentHP)
    {
        CurrentHP = _newCurrentHP;
    }
    /// <summary>
    /// Set new max SP using Property (<see cref="MaxSP"/>).
    /// </summary>
    /// <param name="_newMaxSP">new max SP of player</param>
    public void SetMaxSP (float _newMaxSP)
    {
        MaxSP = _newMaxSP;
    }
    /// <summary>
    /// Set new current SP using Property (<see cref="CurrentSP"/>)
    /// </summary>
    /// <param name="_newCurrentSP">new current SP of player</param>
    public void SetCurrentSP (float _newCurrentSP)
    {
        CurrentSP = _newCurrentSP;
    }
    /// <summary>
    /// Set new player default player armor using Property (<see cref="PlayerArmor"/>).
    /// Change Current armor in <see cref="SetCurrentArmor(float)"/>
    /// </summary>
    /// <param name="_newDefaultPlayerArmor">new player default player armor value</param>
    public void SetDefaultPlayerArmor (float _newDefaultPlayerArmor)
    {
        PlayerArmor = _newDefaultPlayerArmor;
    }
    /// <summary>
    /// Set new player default chaser armor using Property (<see cref="ChaserArmor"/>)
    /// Change Current armor in <see cref="SetCurrentArmor(float)"/>
    /// </summary>
    /// <param name="_newDefaultPlayerArmor">new player default chaser armor value</param>
    public void SetDefaultChaserArmor(float _newDefaultChaserArmor)
    {
        ChaserArmor = _newDefaultChaserArmor;
    }
    /// <summary>
    /// Set new current Armor using Property (<see cref="CurrentArmor"/>)
    /// </summary>
    /// <param name="_newCurrentArmor">new current Armor of player</param>
    public void SetCurrentArmor (float _newCurrentArmor)
    {
        CurrentArmor = _newCurrentArmor;
    }
    /// <summary>
    /// Set Chaser using Property (<see cref="IsChaser"/>) This Function has to be called every round for each player!!!
    /// </summary>
    /// <exception cref="System.NotImplementedException"/>
    /// <param name="_isChaser">chaser value</param>
    public void SetChaser(bool _isChaser)
    {
        // Check if value is the same and if new variable is false
        if (IsChaser == _isChaser &&
            _isChaser == false)
        {
            // set chaser last round variable to false
            WasChaserLastRound = false;
            return;
        }
        // if both values are true, something went wrong
        if (IsChaser == _isChaser &&
            _isChaser == true)
        {
            Debug.LogException(new System.NotImplementedException("This message should not appear! This Player is chaser two times in a row"), this.gameObject);
        }

        // set new chaser status
        IsChaser = _isChaser;

        // if last round player was no Chaser and now is increase chaser counter
        if (_isChaser)
            ChaserCount++;
        // if last round player was Chaser and now is not set last round chaser to true;
        else
            WasChaserLastRound = true;
    }
    #endregion

    #region Private Functions
    #endregion

    #region Public static Functions
    #endregion

    #region Private static Functions
    #endregion

    #region Virtual Functions
    #region Max HP Changed
    /// <summary>
    /// Function is called before players Max HP is set
    /// </summary>
    public virtual void BeforeMaxHPChanged() { }
    /// <summary>
    /// Function is called after players Max HP was set
    /// </summary>
    public virtual void AfterMaxHPChanged() { }
    #endregion
    #region Current HP Changed
    /// <summary>
    /// Function is called before players Current HP is set
    /// </summary>
    public virtual void BeforeCurrentHPChanged() { }
    /// <summary>
    /// Function is called after players Current HP was set
    /// </summary>
    public virtual void AfterCurrentHPChanged() { }
    #endregion
    #region Max Sp Changed
    /// <summary>
    /// Function is called before player Max SP is set
    /// </summary>
    public virtual void BeforeMaxSPChanged() { }
    /// <summary>
    /// Function is called after players Max SP was set
    /// </summary>
    public virtual void AfterMaxSPChanged() { }
    #endregion
    #region Current SP Changed
    /// <summary>
    /// Function is called before players Current SP is set
    /// </summary>
    public virtual void BeforeCurrentSPChanged() { }
    /// <summary>
    /// Function is called after players Current SP was set
    /// </summary>
    public virtual void AfterCurrentSPChanged() { }
    #endregion
    #region Default Armor Changed    
    /// <summary>
    /// Function is called before players default value for Player Armor is set
    /// </summary>
    public virtual void BeforeDefaultPlayerArmorChanged() { }
    /// <summary>
    /// Function is called after players default value for Player Armor was set
    /// </summary>
    public virtual void AfterDefaultPlayerArmorChanged() { }
    /// <summary>
    /// Function is called before players default value for Chaser Armor is set
    /// </summary>
    public virtual void BeforeDefaultChaserArmorChanged() { }
    /// <summary>
    /// Function is called after players default value for Chaser Armor was set
    /// </summary>
    public virtual void AfterDefaultChaserArmorChanged() { }
    #endregion
    #region Current Armor changed
    /// <summary>
    /// Function is called before players Current armor is set
    /// </summary>
    public virtual void BeforeCurrentArmorChanged() { }
    /// <summary>
    /// Function is called after playes Current armor was set
    /// </summary>
    public virtual void AfterCurrentArmorChanged()
    {
        // check if armor was given right

        // check if player is chaser
        if (IsChaser)
        {
            // check if current armor equals Chaser armor
            if (CurrentArmor != ChaserArmor)
            {
                // if not throw new exception
                Debug.LogException(
                    new System.NotImplementedException(
                        "In Chaser Mode, Current Armor has to be "
                        + ChaserArmor
                        + ". Your Armor is "
                        + CurrentArmor
                        + ".")
                        );

            }
        }

        // check is player is not cheaser
        else if (!IsChaser)
        {
            // check if current armor equals Player armor
            if (CurrentArmor != PlayerArmor)
            {
                // if not throw new exception
                throw new System.NotImplementedException(
                    "In Player Mode, Current Armor has to be "
                    + PlayerArmor
                    + ". Your Armor is "
                    + CurrentArmor
                    + ".");
            }

        }
    }
    #endregion
    #region Chaser Count Changed
    /// <summary>
    /// Function is called before players Chaser count is set
    /// </summary>
    public virtual void BeforeChaserCountChanged() { }
    /// <summary>
    /// Function is called after players Chaser count was set
    /// </summary>
    public virtual void AfterChaserCountChanged() { }
    #endregion
    #region Last Round Chaser changed
    /// <summary>
    /// Function is called before players last round Chaser value is set
    /// </summary>
    public virtual void BeforeLastRoundChaserChanged() { }
    /// <summary>
    /// Function is called after players last round Chaser value was set
    /// </summary>
    public virtual void AfterLastRoundChaserChanged() { }
    #endregion
    #region Chaser changed
    /// <summary>
    /// Function is called before players Chaser status is set
    /// </summary>
    public virtual void BeforeChaserChanged() { }
    /// <summary>
    /// Function is called after players Chaser status was set.
    /// Set Armor
    /// </summary>
    public virtual void AfterChaserChanged()
    {
        if (IsChaser)
            CurrentArmor = ChaserArmor;
        else
            CurrentArmor = PlayerArmor;
    }
    #endregion
    #endregion

    /// <summary>
    /// Initializes this instance
    /// </summary>
    protected virtual void Initialize()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    [Server]
    protected virtual void StartRound()
    {

    }

}
