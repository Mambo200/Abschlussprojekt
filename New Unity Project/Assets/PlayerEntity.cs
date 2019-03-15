﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerEntity : AEntity {

    ///<summary>Movement Speed of Player</summary>
    public float m_MovementSpeed;
    ///<summary>Speed with which the Player can rotate</summary>
    public float m_RotationSpeed;
    ///<summary>The force with which the Player can Jump</summary>
    public float m_JumpForce;
    ///<summary>Player Camera</summary>
    [SerializeField]
    protected Camera m_playerCamera;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region Override Functions
    protected override void Initialize()
    {
        base.Initialize();
        // set players current values
        SetCurrentHP(MaxHP);
        SetCurrentSP(MaxSP);
        SetCurrentArmor(PlayerArmor);

        // add player to list
        if (isServer)
            Chaser.Get.AddPlayer(GetComponent<PlayerEntity>());
    }
    private void Jump()
    {
        m_rigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.VelocityChange);
    }

    private void Move(Vector3 _direction)
    {
        Vector3 velocity = _direction.normalized * m_MovementSpeed;
        velocity.y = m_rigidbody.velocity.y;
        m_rigidbody.velocity = velocity;
    }

    private void Rotate(Vector3 _rotation)
    {
        Vector3 rotation = transform.localEulerAngles
        + (_rotation.normalized * Time.deltaTime * m_RotationSpeed);
        transform.localEulerAngles = rotation;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        // activate Player Camera
        m_playerCamera.gameObject.SetActive(true);
    }
    #endregion

    /// <summary>
    /// Tell Client that he is Chaser
    /// </summary>
    private void RpcBecomeChaser()
    {

    }

    private void OnDestroy()
    {
        if (isServer)
            Chaser.Get.RemovePlayer(GetComponent<PlayerEntity>());
        OnNetworkDestroy();
    }
}