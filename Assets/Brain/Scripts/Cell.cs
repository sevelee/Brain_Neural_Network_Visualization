using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {

    Vector3 positon = new Vector3(1, 1, 1);
    
    public int meshIndex = -1;
    public int sendCount = 0;
    public bool getSignal = false;
    public bool actived = false;
    public List<Axon> selfAxons;
    public Axon fromAxon;
    public float getSignalTime = 0;
    public float sendSignalTime = 0;
    public float holdSignalTime = 1;
    public GameObject debugCube;

    #region get set
    public Vector3 Position
    {
        get
        {
            return positon;
        }
    }

    public int AxonNumber
    {
        get
        {
            return selfAxons.Count;
        }
    }
    #endregion

    public Cell(Vector3 p)
    {
        positon = p;
        selfAxons = new List<Axon>();
    }

    public Axon setAxonTo(Cell b)
    {
        Axon axon = new Axon(this, b);
        selfAxons.Add(axon);
        b.selfAxons.Add(axon);
        return axon;
    }

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void reset()
    {
        sendCount = 0;
        getSignal = false;
        actived = false;
        getSignalTime = 0;
        sendSignalTime = 0;
        holdSignalTime = 0;
    }

    internal int SendSignal()
    {
        int numOfSig = 0;
        sendSignalTime = Time.time;
        getSignal = false;
        foreach (var ax in selfAxons)
        {
            if (ax != fromAxon)
            {
                numOfSig += ax.sendSignal(this);
            }
        }
        ++sendCount;
        return numOfSig;
    }
}
