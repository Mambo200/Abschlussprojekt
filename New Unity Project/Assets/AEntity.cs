using System.Collections;
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
    /// <summary>Max HP property (Everyone can get, only Server can set)</summary>
    public float MaxHP
    {
        get { return maxHP; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set Max HP to " + value);
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
    /// <summary>Current HP property (Everyone can get, only Server can set)</summary>
    public float CurrentHP
    {
        get { return currentHP; }
        [Server]
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set current HP to " + value);
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
    /// <summary>Max SP property (Everyone can get, only Server can set)</summary>
    public float MaxSP
    {
        get { return maxSP; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set current SP to " + value);
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
    /// <summary>Current HP property (Everyone can get, only Server can set)</summary>
    public float CurrentSP
    {
        get { return currentSP; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set current SP to " + value);
                return;
            }

            BeforeCurrentSPChanged();
            currentSP = value;
            AfterCurrentSPChanged();
        }
    }
    /// <summary>Player Armor property</summary>
    protected float PlayerArmor { get { return 0f; } }
    /// <summary>Chaser Armor property</summary>
    protected float ChaserArmor { get { return 50f; } }
    /// <summary>Current Armor variable (DO NOT USE! USE <see cref="CurrentArmor"/> INSTEAD)</summary>
    [SyncVar]
    private float currentArmor;
    /// <summary>Current Armor property (Everyone can get, only Server can set)</summary>
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
                Debug.Log("Client tried to set current armor to " + value);
                return;
            }
            BeforeCurrentArmorChanged();
            currentArmor = value;
            AfterCurrentArmorChanged();
        }
    }
    /// <summary>Chaser variable (DO NOT USE! USE <see cref="IsChaser"/> INSTEAD)</summary>
    [SyncVar]
    private bool isChaser = false;
    /// <summary>How often was player Chaser</summary>
    [SyncVar]
    private int chaserCount = 0;
    public int ChaserCount
    {
        get { return chaserCount; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set Chaser count to " + value);
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
    public bool WasChaserLastRound
    {
        get { return wasChaserLastRound; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set was chaser last round to " + value);
                return;
            }
            BeforeLastRoundChaserChanged();
            wasChaserLastRound = value;
            AfterLastRoundChaserChanged();
        }
    }
    /// <summary>Chaser property (Everyone can get, only Server can set)</summary>
    public bool IsChaser
    {
        get { return isChaser; }
        private set
        {
            if (!isServer)
            {
                Debug.Log("Client tried to set Chaser mode to " + value);
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
    /// Set new current SP using Property (<see cref="CurrentSP"/>)
    /// </summary>
    /// <param name="_newCurrentSP"></param>
    public void SetCurrentSP (float _newCurrentSP)
    {
        CurrentSP = _newCurrentSP;
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
    /// Set Chaser using Property (<see cref="IsChaser"/>) This Function has to be called every round
    /// </summary>
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
        // if both values are equal and is true, something went wrong
        if (IsChaser == _isChaser &&
            _isChaser == true)
        {
            Debug.Log("This message should not appear!");
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
                throw new System.NotImplementedException(
                    "In Chaser Mode, Current Armor has to be "
                    + ChaserArmor
                    + ". Your Armor is "
                    + CurrentArmor
                    + ".");
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

    protected virtual void Initialize()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    [Server]
    protected virtual void StartRound()
    {

    }
    #endregion

}
