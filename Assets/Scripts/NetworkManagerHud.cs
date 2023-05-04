using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.UIElements;

[RequireComponent(typeof(NetworkManager))]
[DisallowMultipleComponent]
public class NetworkManagerHud : MonoBehaviour
{
    public enum ConnectionType { NONE, HOST, SERVER, CLIENT };

    public ConnectionType connectionRequestType;

    NetworkManager m_NetworkManager;

    UnityTransport m_Transport;

    // This is needed to make the port field more convenient. GUILayout.TextField is very limited and we want to be able to clear the field entirely so we can't cache this as ushort.
    string m_PortString = "7777";
    string m_ConnectAddress = "127.0.0.1";

    //[SerializeField]
    //UIDocument m_MainMenuUIDocument;

    //[SerializeField]
    //UIDocument m_InGameUIDocument;

    //VisualElement m_MainMenuRootVisualElement;
    [SerializeField]
    GameObject m_MainMenuRoot;
    //VisualElement m_InGameRootVisualElement;
    [SerializeField]
    GameObject m_InGameRoot;
    [SerializeField]
    Button m_HostButton;
    [SerializeField]
    Button m_ServerButton;
    [SerializeField]
    Button m_ClientButton;
    [SerializeField]
    Button m_ShutdownButton;

    //Will put ip address and port field for every menu ;-)

    //Host Input Fields
    [SerializeField]
    TMP_InputField m_HostIPAddressField;
    [SerializeField]
    TMP_InputField m_HostPortField;
    //Server Input Fields
    [SerializeField]
    TMP_InputField m_ServerIPAddressField;
    [SerializeField]
    TMP_InputField m_ServerPortField;
    //Client Input Fields
    [SerializeField]
    TMP_InputField m_ClientIPAddressField;
    [SerializeField]
    TMP_InputField m_ClientPortField;

    //Host status displays
    [SerializeField]
    TMP_Text m_HostMenuStatusText;
    [SerializeField]
    TMP_Text m_ServerMenuStatusText;
    [SerializeField]
    TMP_Text m_ClientMenuStatusText;

    //[SerializeField]
    //TMP_Text m_MenuStatusText;
    [SerializeField]
    TMP_Text m_InGameStatusText;

    [SerializeField]
    TMP_Text m_InGameIpText;
    [SerializeField]
    TMP_Text m_InGamePortText;

    [SerializeField]
    GameObject m_AndroidUI;

    [Header("Menu Panels")]

    public GameObject m_HostMenuPanel;
    public GameObject m_ServerMenuPanel;
    public GameObject m_ClientMenuPanel;
    public GameObject m_AppShutdownPopup;

    void Awake()
    {
        // Only cache networking manager but not transport here because transport could change anytime.
        m_NetworkManager = GetComponent<NetworkManager>();

        //m_MainMenuRootVisualElement = m_MainMenuUIDocument.rootVisualElement;
        //
        //m_IPAddressField = m_MainMenuRootVisualElement.Q<TextField>("IPAddressField");
        //m_PortField = m_MainMenuRootVisualElement.Q<TextField>("PortField");
        //m_HostButton = m_MainMenuRootVisualElement.Q<Button>("HostButton");
        //m_ClientButton = m_MainMenuRootVisualElement.Q<Button>("ClientButton");
        //m_ServerButton = m_MainMenuRootVisualElement.Q<Button>("ServerButton");
        //m_MenuStatusText = m_MainMenuRootVisualElement.Q<TextElement>("ConnectionStatusText");
        //
        //m_InGameRootVisualElement = m_InGameUIDocument.rootVisualElement;
        //m_ShutdownButton = m_InGameRootVisualElement.Q<Button>("ShutdownButton");
        //m_InGameStatusText = m_InGameRootVisualElement.Q<TextElement>("InGameStatusText");
        //
        //m_IPAddressField.value = m_ConnectAddress;
        //m_PortField.value = m_PortString;
        //
        //m_HostButton.clickable.clickedWithEventInfo += HostButtonClicked;
        //m_ServerButton.clickable.clickedWithEventInfo += ServerButtonClicked;
        //m_ClientButton.clickable.clickedWithEventInfo += ClientButtonClicked;
        //m_ShutdownButton.clickable.clickedWithEventInfo += ShutdownButtonClicked;


        m_HostButton.onClick.AddListener(StartHostButtonClicked);
        m_ServerButton.onClick.AddListener(StartServerButtonClicked);
        m_ClientButton.onClick.AddListener(StartClientButtonClicked);
        m_ShutdownButton.onClick.AddListener(ShutdownButtonClicked);

    }

    void Start()
    {
        m_Transport = (UnityTransport)m_NetworkManager.NetworkConfig.NetworkTransport;

        ShowMainMenuUI(true);
        ShowInGameUI(false);
        ShowStatusText(false, ConnectionType.NONE);

        NetworkManager.Singleton.OnClientConnectedCallback += OnOnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnOnClientDisconnectCallback;
    }

    void OnOnClientConnectedCallback(ulong obj)
    {
        ShowMainMenuUI(false);
        ResetMasterMenu();
        ShowInGameUI(true);
    }

    void OnOnClientDisconnectCallback(ulong clientId)
    {
        if ((NetworkManager.Singleton.IsServer && clientId != NetworkManager.ServerClientId))
        {
            return;
        }
        ResetMasterMenu();
        ShowMainMenuUI(true);
        ShowInGameUI(false);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool IsRunning(NetworkManager networkManager) => networkManager.IsServer || networkManager.IsClient;

    bool SetConnectionData()
    {
        //m_ConnectAddress = SanitizeInput(m_IPAddressField.text);
        //m_PortString = SanitizeInput(m_PortField.text);
        //
        //if (m_ConnectAddress == "")
        //{
        //    m_MenuStatusText.text = "IP Address Invalid";
        //    StopAllCoroutines();
        //    StartCoroutine(ShowInvalidInputStatus());
        //    return false;
        //}
        //
        //if (m_PortString == "")
        //{
        //    m_MenuStatusText.text = "Port Invalid";
        //    StopAllCoroutines();
        //    StartCoroutine(ShowInvalidInputStatus());
        //    return false;
        //}
        //
        //if (ushort.TryParse(m_PortString, out ushort port))
        //{
        //    m_Transport.SetConnectionData(m_ConnectAddress, port);
        //}
        //else
        //{
        //    m_Transport.SetConnectionData(m_ConnectAddress, 7777);
        //}
        return true;
    }

    bool SetConnectionData(ConnectionType type)
    {
        //Take inputs from the fields based on connection type
        switch (type)
        {
            case ConnectionType.HOST:
                {
                    m_ConnectAddress = SanitizeInput(m_HostIPAddressField.text);
                    m_PortString = SanitizeInput(m_HostPortField.text);
                }
                break;
            case ConnectionType.SERVER:
                {
                    m_ConnectAddress = SanitizeInput(m_ServerIPAddressField.text);
                    m_PortString = SanitizeInput(m_ServerPortField.text);
                }
                break;
            case ConnectionType.CLIENT:
                {
                    m_ConnectAddress = SanitizeInput(m_ClientIPAddressField.text);
                    m_PortString = SanitizeInput(m_ClientPortField.text);
                }
                break;
            default:
                break;
        }

        //m_ConnectAddress = SanitizeInput(m_IPAddressField.text);
        //m_PortString = SanitizeInput(m_PortField.text);

        if (m_ConnectAddress == "")
        {
            //m_MenuStatusText.text = "IP Address Invalid";
            GetStatusDisplayFromConnectionType(type).text = "IP Address Invalid";
            StopAllCoroutines();
            StartCoroutine(ShowInvalidInputStatus());
            return false;
        }

        if (m_PortString == "")
        {
            //m_MenuStatusText.text = "Port Invalid";
            GetStatusDisplayFromConnectionType(type).text = "Port Invalid";
            StopAllCoroutines();
            StartCoroutine(ShowInvalidInputStatus());
            return false;
        }


        if (ushort.TryParse(m_PortString, out ushort port))
        {
            m_Transport.SetConnectionData(m_ConnectAddress, port);
        }
        else
        {
            m_Transport.SetConnectionData(m_ConnectAddress, 7777);
        }
        return true;
    }

    static string SanitizeInput(string dirtyString)
    {
        // sanitize the input for the ip address
        return Regex.Replace(dirtyString, "[^0-9.]", "");
    }

    //Button Callbacks

    void StartHostButtonClicked()
    {
        if (SetConnectionData(ConnectionType.HOST))
        {
            NetworkManager.Singleton.StartHost();
        }
    }

    void StartClientButtonClicked()
    {
        if (SetConnectionData(ConnectionType.CLIENT))
        {
            NetworkManager.Singleton.StartClient();
            StopAllCoroutines();
            StartCoroutine(ShowConnectingStatus());
        }
    }

    void StartServerButtonClicked()
    {
        if (SetConnectionData(ConnectionType.SERVER))
        {
            m_NetworkManager.StartServer();
            ShowMainMenuUI(false);
            ShowInGameUI(true);
        }
    }

    void ShutdownButtonClicked()
    {
        m_NetworkManager.Shutdown();
        //ShowMainMenuUI(true);
        //ShowInGameUI(false);

        m_AppShutdownPopup.SetActive(true);
        Invoke(nameof(CloseApp), 7);
    }

    //Menu Transition Button Click event
    //Use this to change context

    public void OnHostMenuClicked()
    {
        //var ip = IPDetectorScript.GetLocalIPAddress();

        string ip = "";

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            ip = IPDetectorScript.GetIP(ADDRESSFAM.IPv4);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            ip = IPDetectorScript.GetLocalIPAddressAndroid();
        }

        if (ip.Length != 0 && ip != "127.0.0.1")
        {
            m_HostIPAddressField.text = ip;
        }

        connectionRequestType = ConnectionType.HOST;
    }

    public void OnServerMenuClicked()
    {
        string ip = "";

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            ip = IPDetectorScript.GetIP(ADDRESSFAM.IPv4);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            ip = IPDetectorScript.GetLocalIPAddressAndroid();
        }

        if (ip.Length != 0)
        {
            m_ServerIPAddressField.text = ip;
        }

        connectionRequestType = ConnectionType.SERVER;
    }

    public void OnClientMenuClicked()
    {
        connectionRequestType = ConnectionType.CLIENT;
    }

    //void ShowStatusText(bool visible)
    //{
    //    //m_MenuStatusText.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
    //    //m_MenuStatusText.gameObject.SetActive(visible);
    //}

    void ShowStatusText(bool visible, ConnectionType type)
    {
        switch (type)
        {
            case ConnectionType.HOST:
                {
                    m_HostMenuStatusText.gameObject.SetActive(visible);
                }
                break;
            case ConnectionType.SERVER:
                {
                    m_ServerMenuStatusText.gameObject.SetActive(visible);
                }
                break;
            case ConnectionType.CLIENT:
                {
                    m_ClientMenuStatusText.gameObject.SetActive(visible);
                }
                break;
            case ConnectionType.NONE:
                {
                    m_HostMenuStatusText.gameObject.SetActive(visible);
                    m_ServerMenuStatusText.gameObject.SetActive(visible);
                    m_ClientMenuStatusText.gameObject.SetActive(visible);
                }
                break;
            default:
                break;
        }
    }

    IEnumerator ShowInvalidInputStatus()
    {
        ShowStatusText(true, connectionRequestType);

        yield return new WaitForSeconds(3f);

        ShowStatusText(false, connectionRequestType);
    }

    IEnumerator ShowConnectingStatus()
    {
        //m_MenuStatusText.text = "Attempting to Connect...";
        GetStatusDisplayFromConnectionType(connectionRequestType).text = "Attempting to Connect...";
        ShowStatusText(true, connectionRequestType);

        //m_HostButton.SetEnabled(false);
        //m_ServerButton.SetEnabled(false);

        var unityTransport = m_NetworkManager.GetComponent<UnityTransport>();
        var connectTimeoutMs = unityTransport.ConnectTimeoutMS;
        var maxConnectAttempts = unityTransport.MaxConnectAttempts;

        yield return new WaitForSeconds(connectTimeoutMs * maxConnectAttempts / 1000f);

        // wait to verify connect status
        yield return new WaitForSeconds(1f);

        //m_MenuStatusText.text = "Connection Attempt Failed";
        GetStatusDisplayFromConnectionType(connectionRequestType).text = "Connection Attempt Failed";
        //m_HostButton.SetEnabled(true);
        //m_ServerButton.SetEnabled(true);

        yield return new WaitForSeconds(3f);

        ShowStatusText(false, connectionRequestType);
    }

    void ShowMainMenuUI(bool visible)
    {
        //m_MainMenuRootVisualElement.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        m_MainMenuRoot.SetActive(visible);
    }

    void ShowInGameUI(bool visible)
    {
        //m_InGameRootVisualElement.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        m_InGameRoot.SetActive(visible);

        if (m_NetworkManager.IsServer)
        {
            var mode = m_NetworkManager.IsHost ? "Host" : "Server";
            m_InGameIpText.text = ($"Ip: {m_Transport.ConnectionData.Address.ToString()}");
            m_InGamePortText.text = ($"Port: {m_Transport.ConnectionData.Port.ToString()}");
            //m_ShutdownButton.text = ($"Shutdown {mode}");
            m_ShutdownButton.GetComponentInChildren<TMP_Text>().text = ($"Shutdown {mode}");
        }
        else
        {
            if (m_NetworkManager.IsConnectedClient)
            {
                m_InGameStatusText.text = ($"Ip: {m_Transport.ConnectionData.Address.ToString()}");
                m_InGamePortText.text = ($"Port: {m_Transport.ConnectionData.Port.ToString()}");
                m_ShutdownButton.GetComponentInChildren<TMP_Text>().text = "Shutdown Client";
            }
        }
    }

    void ShowInGameAndroidUI(bool visible)
    {
        //m_AndroidUI.SetActive(visible);
    }

    void OnDestroy()
    {
        if (m_HostButton != null)
        {
            //m_HostButton.clickable.clickedWithEventInfo -= HostButtonClicked;
        }

        if (m_ServerButton != null)
        {
            //m_ServerButton.clickable.clickedWithEventInfo -= ServerButtonClicked;
        }

        if (m_ClientButton != null)
        {
            //m_ClientButton.clickable.clickedWithEventInfo -= ClientButtonClicked;
        }

        if (m_ShutdownButton != null)
        {
            //m_ShutdownButton.clickable.clickedWithEventInfo -= ShutdownButtonClicked;
        }
        m_NetworkManager.OnClientConnectedCallback -= OnOnClientConnectedCallback;
        m_NetworkManager.OnClientDisconnectCallback -= OnOnClientDisconnectCallback;
    }

    TMP_Text GetStatusDisplayFromConnectionType(ConnectionType type)
    {
        TMP_Text text = null;
        switch (type)
        {
            case ConnectionType.HOST:
                text = m_HostMenuStatusText;
                break;
            case ConnectionType.SERVER:
                text = m_ServerMenuStatusText;
                break;
            case ConnectionType.CLIENT:
                text = m_ClientMenuStatusText;
                break;
            default:
                break;
        }
        return text;
    }

    //Reset master menu to initial state
    void ResetMasterMenu()
    {
        m_HostMenuPanel.SetActive(false);
        m_ServerMenuPanel.SetActive(false);
        m_ClientMenuPanel.SetActive(false);
    }

    void CloseApp()
    {
        Application.Quit();
    }

}
