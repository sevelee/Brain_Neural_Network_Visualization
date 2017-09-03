using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using seve.QuadParticle;

public class ParticleSender {

    #region public variables

    public Vector3 fromPoint;
    public Vector3 toPoint;

    public int particleIndex = -1;

    public float limitTime = 0;
    public float startTime = 0;

    public bool active = false;

    public Cell targetCell;

    public Axon fromAxon;

    public particleMaker pMaker;
    #endregion

    #region new method

    public ParticleSender(Vector3 fpos, Vector3 tpos, int pIn, float limi_time, Cell tcell, Axon faxon, particleMaker pm)
    {
        fromPoint = fpos;
        toPoint = tpos;
        particleIndex = pIn;
        limitTime = limi_time;
        startTime = Time.time;
        targetCell = tcell;
        fromAxon = faxon;
        pMaker = pm;
        active = true;
    }

    public void delete()
    {
        pMaker.setFree(particleIndex);
        targetCell.getSignal = true;
        targetCell.getSignalTime = Time.time;
        targetCell.fromAxon = fromAxon;
        BrainNetWork.numberOfSignals--;
        active = false;
    }
    #endregion
    
    void Start () {
	}

	// Update is called once per frame
	public void Update () {
        if (active)
        {
            //reach the target
            if ((Time.time - startTime) > limitTime)
            {
                delete();
            }
            pMaker.setParticlePosition(particleIndex, getPosition(fromPoint, toPoint, (Time.time - startTime) / limitTime));
        }
    }

    Vector3 getPosition(Vector3 a, Vector3 b, float x)
    {
        return (a + (b - a) * x);
    }
}
