using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class NewShipControl : ShipControl
{
    public AndroidInput androidInput;
    public KeyboardInput keyboardInput;

    public Canvas m_MyCanvas;

    IInputInterface currentInput;

    //public override void UpdateClient()
    //{
    //    HandleFrictionGraphics();
    //    HandleIfBuffed();
    //
    //    if (!IsLocalPlayer)
    //    {
    //        return;
    //    }
    //
    //    // movement
    //    int spin = 0;
    //    if (Input.GetKey(KeyCode.LeftArrow))
    //    {
    //        spin += 1;
    //    }
    //
    //    if (Input.GetKey(KeyCode.RightArrow))
    //    {
    //        spin -= 1;
    //    }
    //
    //    int moveForce = 0;
    //    if (Input.GetKey(KeyCode.UpArrow))
    //    {
    //        moveForce += 1;
    //    }
    //
    //    if (Input.GetKey(KeyCode.DownArrow))
    //    {
    //        //moveForce -= 1;
    //    }
    //
    //    if (m_OldMoveForce != moveForce || m_OldSpin != spin)
    //    {
    //        ThrustServerRpc(moveForce, spin);
    //        m_OldMoveForce = moveForce;
    //        m_OldSpin = spin;
    //    }
    //
    //    // control thrust particles
    //    if (moveForce == 0.0f)
    //    {
    //        m_ThrustMain.startLifetime = 0.1f;
    //        m_ThrustMain.startSize = 1f;
    //        m_ThrustMain1.startLifetime = 0.1f;
    //        m_ThrustMain1.startSize = 1f;
    //
    //        GetComponent<AudioSource>().Pause();
    //    }
    //    else
    //    {
    //        m_ThrustMain.startLifetime = 0.4f;
    //        m_ThrustMain.startSize = 1.2f;
    //        m_ThrustMain1.startLifetime = 0.4f;
    //        m_ThrustMain1.startSize = 1.2f;
    //
    //        GetComponent<AudioSource>().Play();
    //    }
    //
    //    // fire
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        NewFireServerRpc();
    //    }
    //}

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (Application.platform == RuntimePlatform.Android)
        {
            if (!IsLocalPlayer)
            {
                m_MyCanvas.gameObject.SetActive(false);
            }
        }

    }

    public override void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_ObjectPool = GameObject.FindWithTag(s_ObjectPoolTag).GetComponent<NetworkObjectPool>();
        Assert.IsNotNull(m_ObjectPool, $"{nameof(NetworkObjectPool)} not found in scene. Did you apply the {s_ObjectPoolTag} to the GameObject?");

        m_ThrustMain = m_Thrust.main;
        m_ThrustMain1 = m_Thrust1.main;

        m_ShipGlow.color = m_ShipGlowDefaultColor;
        m_IsBuffed = false;

        m_RootVisualElement = m_UIDocument.rootVisualElement;
        m_PlayerUIWrapper = m_RootVisualElement.Q<VisualElement>("PlayerUIWrapper");
        m_HealthBar = m_RootVisualElement.Q<ProgressBar>(name: "HealthBar");
        m_EnergyBar = m_RootVisualElement.Q<ProgressBar>(name: "EnergyBar");
        m_PlayerName = m_RootVisualElement.Q<TextElement>("PlayerName");
        m_MainCamera = Camera.main;

        //Set input interface according to platform
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            currentInput = keyboardInput;
            //androidInput.transform.parent.gameObject.SetActive(false);

            //Only disable canvas showing
            m_MyCanvas.gameObject.SetActive(false);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            currentInput = androidInput;
        }


    }

    public override void UpdateClient()
    {
        HandleFrictionGraphics();
        HandleIfBuffed();

        if (!IsLocalPlayer && !IsOwner)
        {
            return;
        }

        // movement
        int spin = 0;
        if (currentInput.GetKey("Left"))
        {
            spin += 1;
        }

        if (currentInput.GetKey("Right"))
        {
            spin -= 1;
        }

        int moveForce = 0;
        if (currentInput.GetKey("Up"))
        {
            moveForce += 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            //moveForce -= 1;
        }

        if (m_OldMoveForce != moveForce || m_OldSpin != spin)
        {
            ThrustServerRpc(moveForce, spin);
            m_OldMoveForce = moveForce;
            m_OldSpin = spin;
        }

        // control thrust particles
        if (moveForce == 0.0f)
        {
            m_ThrustMain.startLifetime = 0.1f;
            m_ThrustMain.startSize = 1f;
            m_ThrustMain1.startLifetime = 0.1f;
            m_ThrustMain1.startSize = 1f;

            GetComponent<AudioSource>().Pause();
        }
        else
        {
            m_ThrustMain.startLifetime = 0.4f;
            m_ThrustMain.startSize = 1.2f;
            m_ThrustMain1.startLifetime = 0.4f;
            m_ThrustMain1.startSize = 1.2f;

            GetComponent<AudioSource>().Play();
        }

        // fire
        if (currentInput.GetKeyDown("Fire"))
        {
            NewFireServerRpc();
        }
    }

}
