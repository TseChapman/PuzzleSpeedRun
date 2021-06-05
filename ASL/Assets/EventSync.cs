using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using ASL;

public class EventSync : MonoBehaviour
{

    [Serializable]
    public class EventSyncEvent
    {
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public UnityEvent OnEvent;
        public enum TriggerMode { OnReachedByAny, OnReachedByAll, OnActiveOnAll };
        public TriggerMode triggerMode {
            get
            {
                return _triggerMode;
            }
        }

        [SerializeField]
        private string _name;

        [SerializeField]
        private TriggerMode _triggerMode;

        private int id;
        private ASLObject syncHandler;
        private HashSet<int> activatedIDs;
        private HashSet<int> reachedByIDs;
        private bool neverActivated = true;

        public EventSyncEvent()
        {
            activatedIDs = new HashSet<int>();
            reachedByIDs = new HashSet<int>();
        }
        public void Activate()
        {
            float[] f = new float[3];
            f[0] = this.id;
            f[1] = 1;
            f[2] = GameLiftManager.GetInstance().m_PeerId;
            syncHandler.SendAndSetClaim(() =>
            {
                syncHandler.SendFloatArray(f);
            });
        }

        public void Deactivate()
        {
            float[] f = new float[3];
            f[0] = this.id;
            f[1] = 0;
            f[2] = GameLiftManager.GetInstance().m_PeerId;
            syncHandler.SendAndSetClaim(() =>
            {
                syncHandler.SendFloatArray(f);
            });
        }

        static int n;

        internal void activatedBy(int peerId)
        {
            activatedIDs.Add(peerId);
            reachedByIDs.Add(peerId);
            switch (triggerMode)
            {
                case TriggerMode.OnReachedByAny:
                    OnEvent.Invoke();
                    neverActivated = false;
                    break;
                case TriggerMode.OnReachedByAll:
                    if (neverActivated)
                    {
                        bool reachedByAll = true;
                        foreach (int i in GameLiftManager.GetInstance().m_Players.Keys)
                        {
                            if (!reachedByIDs.Contains(i))
                            {
                                reachedByAll = false;
                            }
                        }
                        if (reachedByAll)
                        {
                            OnEvent.Invoke();
                            neverActivated = false;
                        }
                    }
                    break;
                case TriggerMode.OnActiveOnAll:
                    bool activeOnAll = true;
                    foreach (int i in GameLiftManager.GetInstance().m_Players.Keys)
                    {
                        if (!activatedIDs.Contains(i))
                        {
                            activeOnAll = false;
                        }
                    }
                    if (activeOnAll)
                    {
                        OnEvent.Invoke();
                        neverActivated = false;
                    }
                    break;
            }
        }

        internal void deactivatedBy(int peerId)
        {
            activatedIDs.Remove(peerId);
        }

        internal void setSyncHandler(int myID, ASLObject syncHandler)
        {
            this.id = myID;
            this.syncHandler = syncHandler;
        }
    }

    [SerializeField]
    private List<EventSyncEvent> Events;

    private Dictionary<string, int> eventNameIDMapping;

    private ASLObject syncHandler;

    public EventSync() {
        eventNameIDMapping = new Dictionary<string, int>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Events.Count; ++i)
        {
            if (eventNameIDMapping.ContainsKey(Events[i].Name))
                Debug.Log("EventSync: Start(): Event name = " + Events[i].Name);
            eventNameIDMapping.Add(Events[i].Name, i);
        }
        if (eventNameIDMapping.Count != Events.Count)
        {
            throw new Exception("EventSync: Event names must be unique within EventSync instance.");
        }
        syncHandler = GetComponent<ASLObject>();
        syncHandler._LocallySetFloatCallback(floatCallback);
        for (int i = 0; i < Events.Count; ++i)
        {
            Events[i].setSyncHandler(i, syncHandler);
        }
    }

    public void Activate(string name)
    {
        Events[eventNameIDMapping[name]].Activate();
    }

    public void Deactivate(string name)
    {
        Events[eventNameIDMapping[name]].Deactivate();
    }

    private void floatCallback(string id, float[] data)
    {
        if (data[1] == 1) {
            Events[(int)data[0]].activatedBy((int)data[2]);
        } else
        {
            Events[(int)data[0]].deactivatedBy((int)data[2]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
