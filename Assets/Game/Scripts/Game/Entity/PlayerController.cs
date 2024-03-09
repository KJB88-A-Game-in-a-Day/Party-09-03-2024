using Mirror;
using UnityEngine;
using TMPro;
using GDLib.Comms;
using GDLib.State;
using System.Collections.Generic;

public class PlayerController : NetworkBehaviour
{
    [Header("Sync Vars")]
    [SyncVar(hook = nameof(OnNameChanged))]
    [HideInInspector] public string playerName;
    [SyncVar(hook = nameof(OnColorChanged))]
    [HideInInspector] public Color playerColor = Color.white;

    [Header("Modifiers")]
    [SerializeField] float speedMod = 10.0f;
    [SerializeField] Vector3 camOffset;

    [Header("Managed")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] GameObject floatingInfo;

    [Header("Dependencies")]
    private MessageBroker msgBroker;
    private Camera mainCam;

    [Header("Data")]
    [SerializeField] PlayerData playerData;

    [Header("Working / Deltas")]
    float currentHealth;
    float idleCounter = 0.0f;
    float cooldownCounter = 0.0f;

    FSM fsm;
    VirtualInput vInput;
    Dictionary<string, object> blackboard;

    #region NETWORKED
    // Send player info to server to update SyncVars for other clients
    [Command]
    public void CmdSetupPlayer(string _name, Color _col)
    {
        playerName = _name;
        playerColor = _col;


    }

    #endregion NETWORKED
    #region SYNC VAR
    void OnNameChanged(string _old, string _new)
        => playerNameText.text = playerName;

    void OnColorChanged(Color _old, Color _new)
    {
        playerNameText.color = _new;
        spriteRenderer.color = _new;
    }
    #endregion SYNC VAR
    #region LOCAL
    private void Awake()
    {
        // Get dependencies
        if (ServiceLocator.RequestService(ServiceLibrary.MessageBroker, out IService outService))
            msgBroker = (MessageBroker)outService;

        mainCam = Camera.main;

        // Update managed resources
        floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        vInput = new VirtualInput();

        // Populate the blackboard
        blackboard = new Dictionary<string, object>()
        {
            {"messageBroker", msgBroker },
            { "currentHealth", currentHealth },
            {"maxHealth", playerData.MaxHealth },
            {"maxPower", playerData.MaxPower },
            {"idleTimeMax", playerData.IdleTimeMax },
            {"thisTransform", this.transform },
            {"virtualInput", vInput },
            {"spriteRenderer", spriteRenderer }
        };

        fsm = new FSM();
        fsm.SetState(new PlayerState_Idle(fsm), blackboard);
    }

    public override void OnStartLocalPlayer()
    {
        //sceneScript.LocalPlayerScript = this;
        mainCam.transform.SetParent(transform);
        mainCam.transform.localPosition = camOffset;

        floatingInfo.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);

        string name = "Player" + Random.Range(100, 999);
        Color color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        CmdSetupPlayer(name, color);

        msgBroker.SendMessage(new MSG_ClientConnected(MessageLibrary.ClientConnected, name));
    }

    private void GetInput()
    {
        vInput.move = new Vector2
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical")
        };
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;
    }
    #endregion LOCAL
}