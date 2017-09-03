using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using seve.QuadParticle;

public class SenderMaker : MonoBehaviour {
    #region public variables

    public GameObject senderGameObject;
    #endregion

    #region private variables
    
    #endregion

    private void Awake()
    {
    }

    // Use this for initialization
    void Start () {
	}

    public void MakeNewSender(Vector3 pa, Vector3 pb, float limitTime, Cell targetCell, Axon axon)
    {
        Debug.Log("Make a new sender");
        GameObject sendergo = Instantiate(senderGameObject, pa, Quaternion.identity);
        Sender se = sendergo.GetComponent<Sender>();
        se.fromPoint = pa;
        se.toPoint = pb;
        se.limitTime = limitTime;
        se.startTime = Time.time;
        se.targetCell = targetCell;
        se.fromAxon = axon;
        se.active = true;
    }

    // Update is called once per frame
    void Update () {
	}
}
