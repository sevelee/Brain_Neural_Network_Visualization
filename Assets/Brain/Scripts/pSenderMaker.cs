using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using seve.QuadParticle;

public class pSenderMaker : MonoBehaviour {
    #region public variables

    public GameObject pManagerGameObject;
    public GameObject BrainGameObject;

    particleMakerHoloTest pMaker;
    #endregion

    #region private variables

    pSender[] _pSender;
    BrainNetWork bnw;
    #endregion

    private void Awake()
    {
        pMaker = pManagerGameObject.GetComponent<particleMakerHoloTest>();
        bnw = BrainGameObject.GetComponent<BrainNetWork>();
        _pSender = new pSender[bnw.MaxSignalNumber];
        for (int i = 0; i < _pSender.Length; ++i)
        {
            _pSender[i] = new pSender();
            _pSender[i].active = false;
        }
    }

    // Use this for initialization
    void Start () {
    }

    int findEmptyId()
    {
        for (int i = 0; i < _pSender.Length; ++i)
        {
            if (_pSender[i].active == false) return i;
        }
        return -1;
    }

    public void MakeNewSender(Vector3 pa, Vector3 pb, float limitTime, Cell targetCell, Axon axon)
    {
        //Debug.Log("Make a new sender");
        //GameObject sendergo = Instantiate(senderGameObject, pa, Quaternion.identity);
        //Sender se = sendergo.GetComponent<Sender>();
        //se.fromPoint = pa;
        //se.toPoint = pb;
        //se.limitTime = limitTime;
        //se.startTime = Time.time;
        //se.targetCell = targetCell;
        //se.fromAxon = axon;
        //se.active = true;

        int emptyId = findEmptyId();
        if (emptyId == -1) return;
        _pSender[emptyId].fromPoint = pa;
        _pSender[emptyId].toPoint = pb;
        _pSender[emptyId].position = pa;
        _pSender[emptyId].limitTime = limitTime;
        _pSender[emptyId].startTime = Time.time;
        _pSender[emptyId].targetCell = targetCell;
        _pSender[emptyId].fromAxon = axon;
        _pSender[emptyId].active = true;
    }

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < _pSender.Length; ++i)
        {
            if (_pSender[i].active)
            {
                _pSender[i].Update();
                pMaker.setParticlePosition(i, _pSender[i].position);
            }
            else
            {
                pMaker.disappear(i);
            }
        }
	}
}
