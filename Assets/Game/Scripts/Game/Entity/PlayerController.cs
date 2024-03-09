using Mirror;
using UnityEngine;
using TMPro;
using GDLib.Comms;

public class PlayerController : NetworkBehaviour
{
    [Header("Sync Vars")]
    [SyncVar(hook = nameof(OnNameChanged))]
    [HideInInspector] public string playerName;
    [SyncVar(hook = nameof(OnColorChanged))]
    [HideInInspector] public Color playerColor = Color.white;

    [Header("Cam")]
    [SerializeField] Vector3 camOffset;
    Camera mainCam;

    [Header("Movement Mods")]
    [SerializeField] float speedMod = 10.0f;

    [Header("Managed")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] GameObject floatingInfo;

    [Header("Dependencies")]
    private MessageBroker msgBroker;

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
        if (ServiceLocator.RequestService(ServiceLibrary.MessageBroker, out IService outService))
            msgBroker = (MessageBroker)outService;

        mainCam = Camera.main;
        floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        Vector2 move = new Vector2
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical")
        };

        move *= speedMod * Time.deltaTime;

        transform.Translate(move);
    }
    #endregion LOCAL
}