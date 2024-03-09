using GDLib.Comms;
using Mirror;
using UnityEngine;

namespace Party
{
    public class GameManager : NetworkBehaviour, ISubscriber
    {
        MessageBroker msgBroker;

        private void Awake()
        {
            msgBroker = new MessageBroker();
            msgBroker.RegisterSubscriber(MessageLibrary.ClientConnected, this);

            ServiceLocator.AddService(ServiceLibrary.MessageBroker, msgBroker);
        }

        [Header("Sync Vars")]
        [SyncVar(hook = nameof(OnStatusTextChanged))]
        private string statusText;

        #region SYNC VARS
        void OnStatusTextChanged(string _old, string _new)
        {
            statusText = _new;
            msgBroker.SendMessage(new MSG_StatusUpdated(MessageLibrary.StatusUpdated, statusText));
        }
        #endregion SYNC VARS

        #region LOCAL
        public bool Receive(Message msg)
        {
            if (msg.MessageType == MessageLibrary.ClientConnected)
            {
                MSG_ClientConnected cc = (MSG_ClientConnected)msg;
                UpdateStatusText(cc.clientName + " connected!");
            }

            return false;
        }

        private void UpdateStatusText(string newStatusText)
        {
            statusText = newStatusText;
            msgBroker.SendMessage(new MSG_StatusUpdated(MessageLibrary.StatusUpdated, statusText));
        }
        #endregion LOCAL
    }
}
