using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sender : MonoBehaviour {
    public Vector3 fromPoint;
    public Vector3 toPoint;
    public float limitTime = 0;
    public float startTime = 0;
    public bool active = false;

    #region test parameter
    public Cell targetCell;
    public Axon fromAxon;
    #endregion

    // Use this for initialization
    void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (active)
        {
            //reach the target
            if ((Time.time - startTime) > limitTime)
            {
                targetCell.getSignal = true;
                targetCell.getSignalTime = Time.time;
                targetCell.fromAxon = fromAxon;
                BrainNetWork.numberOfSignals--;
                Destroy(gameObject);
            }
            gameObject.transform.position = getPosition(fromPoint, toPoint, (Time.time - startTime) / limitTime);
        }
	}

    Vector3 getPosition(Vector3 a, Vector3 b, float x)
    {
        return (a + (b - a) * x);
    }
}
