using Mirror;
using UnityEngine;
using TMPro;
using GDLib.Comms;
using GDLib.State;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerController : NetworkBehaviour, IHittable
{
    [Header("Sync Vars")]
    [SyncVar(hook = nameof(OnNameChanged))]
    [HideInInspector] public string playerName;
    [SyncVar(hook = nameof(OnColorChanged))]
    [HideInInspector] public Color playerColor = Color.white;

    [Header("Modifiers")]
    [SerializeField] Vector3 camOffset;

    [Header("Managed")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] GameObject floatingInfo;
    [SerializeField] SpriteRenderer emotionDisplay;

    [Header("Dependencies")]
    private MessageBroker globalMsgBroker;
    private MessageBroker localMsgBroker;
    private Camera mainCam;
    FSM fsm;
    Dictionary<string, object> blackboard;
    VirtualInput virtualInput;

    [Header("Data")]
    [SerializeField] PlayerData playerData;

    float currentHealth;

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
            globalMsgBroker = (MessageBroker)outService;

        mainCam = Camera.main;
        // Update managed resources
        localMsgBroker = new MessageBroker();
        floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        virtualInput = new VirtualInput();
        currentHealth = playerData.MaxHealth;

        // Populate the blackboard
        blackboard = new Dictionary<string, object>()
        {
            { "currentHealth", currentHealth },
            {"maxPower", playerData.MaxPower },
            {"bumpedTimeMax", playerData.BumpedTimeMax },
            {"dizzyTimeMax", playerData.DizzyTimeMax },
            {"speedMod", playerData.SpeedMod },
            {"thrustPower", playerData.ThrustPower },
            {"thrustStep", playerData.ThrustStep },

            {"currentLayer", gameObject.layer },
            {"thisTransform", this.transform },
            {"spriteRenderer", spriteRenderer },
            {"emotionDisplay", emotionDisplay },
            {"localMsgBroker", localMsgBroker },
            {"virtualInput", virtualInput }
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
        globalMsgBroker.SendMessage(new MSG_ClientConnected(MessageLibrary.ClientConnected, name));
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        virtualInput.inputAxisVector = new Vector2(x, y);

        fsm.UpdateFSM(blackboard);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            localMsgBroker.SendMessage(
                new MSG_Collision2D(
                    MessageLibrary.Collision2DEvent, 
                    MSG_Collision2D.COLL_TYPE.ENTER, 
                    collision
                    ));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            localMsgBroker.SendMessage(
                new MSG_Collision2D(
                    MessageLibrary.Collision2DEvent,
                    MSG_Collision2D.COLL_TYPE.EXIT,
                    collision
                    ));
    }

    public bool OnHit(int damage)
    {
        if (gameObject.layer != LayerMask.NameToLayer("Hurtable"))
            return false;

        currentHealth--;

        if (blackboard.ContainsKey("currentHealth"))
            blackboard["currentHealth"] = currentHealth;
        else
            blackboard.Add("currentHealth", currentHealth);

        fsm.SetState(new PlayerState_Bumped(fsm), blackboard);
        return true;
    }
    #endregion LOCAL
}