using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pSender {
    #region public variables

    public Vector3 fromPoint;
    public Vector3 toPoint;
    public Vector3 position;
    public float limitTime;
    public float startTime;
    public Cell targetCell;
    public Axon fromAxon;
    public bool active;
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	public void Update () {
        if (active)
        {
            if ((Time.time - startTime) > limitTime)
            {
                targetCell.getSignal = true;
                targetCell.getSignalTime = Time.time;
                targetCell.fromAxon = fromAxon;
                BrainNetWork.numberOfSignals--;
                active = false;
                return;
            }
            position = getPosition(fromPoint, toPoint, (Time.time - startTime) / limitTime);
        }
	}

    Vector3 getPosition(Vector3 a, Vector3 b, float x)
    {
        return (a + (b - a) * x);
    }
}
