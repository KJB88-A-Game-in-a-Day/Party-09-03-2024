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
    [SerializeField] Vector3 camOffset;

    [Header("Managed")]
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] GameObject floatingInfo;
    [SerializeField] SpriteRenderer emotionDisplay;

    [Header("Dependencies")]
    private MessageBroker msgBroker;
    private Camera mainCam;
    FSM fsm;
    Dictionary<string, object> blackboard;

    [Header("Data")]
    [SerializeField] PlayerData playerData;

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

        // Populate the blackboard
        blackboard = new Dictionary<string, object>()
        {
            {"messageBroker", msgBroker },
            { "currentHealth", playerData.MaxHealth },
            {"maxHealth", playerData.MaxHealth },
            {"maxPower", playerData.MaxPower },
            //{"idleTimeMax", playerData.IdleTimeMax },
            {"thisTransform", this.transform },
            {"spriteRenderer", spriteRenderer },
            {"emotionDisplay", emotionDisplay },
            {"speedMod", playerData.SpeedMod },
            {"recoveryTime", playerData.RecoveryTime },
            {"rigidbody2D", rigidbody2D }
        };

        fsm = new FSM();
        fsm.SetState(new PlayerState_Idle(fsm), blackboard);
    }

    public override void OnStartLocalPlayer()
    {
        // Set up follow cam
        mainCam.transform.SetParent(transform);
        mainCam.transform.localPosition = camOffset;

        // Update player context info
        floatingInfo.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);

        // Assign core vars
        string name = "Player" + Random.Range(100, 999);
        Color color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        CmdSetupPlayer(name, color);

        // Update status text
        msgBroker.SendMessage(new MSG_ClientConnected(MessageLibrary.ClientConnected, name));
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        fsm.UpdateFSM(blackboard);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
    #endregion LOCAL
}