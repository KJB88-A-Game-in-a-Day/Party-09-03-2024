using GDLib.Comms;
using TMPro;
using UnityEngine;

public class StatusView : MonoBehaviour, ISubscriber
{
    [Header("Managed")]
    [SerializeField] TMP_Text canvasStatusText;

    MessageBroker msgBroker;
    private void Awake()
    {
        if (ServiceLocator.RequestService(ServiceLibrary.MessageBroker, out IService service))
            msgBroker = (MessageBroker)service;

        msgBroker.RegisterSubscriber(MessageLibrary.StatusUpdated, this);
    }
    public bool Receive(Message msg)
    {
        if (msg.MessageType == MessageLibrary.StatusUpdated)
        {
            MSG_StatusUpdated su = (MSG_StatusUpdated)msg;
            canvasStatusText.text = su.statusText;
        }

        return false;
    }
}
