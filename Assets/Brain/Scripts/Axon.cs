using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axon {
    Cell cell_a;
    Cell cell_b;

    public pSenderMaker senderMaker;

    #region setting parameters

    public float maxTime = 0.8f;
    public float minTime = 0.3f;
    #endregion

    #region get set
    public Cell CellA
    {
        get
        {
            return cell_a;
        }
    }

    public Cell CellB
    {
        get
        {
            return cell_b;
        }
    }

    public Axon(Cell a, Cell b)
    {
        cell_a = a;
        cell_b = b;
    }
    #endregion

    // Use this for initialization
    void Start () {
        senderMaker = new pSenderMaker();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Use Axon to send signal
    /// </summary>
    /// <param name="fromCell"> the Cell which send the signal</param>
    internal int sendSignal(Cell fromCell)
    {
        if (fromCell == cell_a)
        {
            //cell_b.getSignal = true;
            //cell_b.active = true;
            //cell_b.getSignalTime = Time.time;
            //cell_b.fromAxon = this;
            //cell_b.holdSignalTime = UnityEngine.Random.Range(0.5f, 1.5f);
            senderMaker.MakeNewSender(cell_a.Position, cell_b.Position, UnityEngine.Random.Range(minTime, maxTime), cell_b, this);
            return 1;
        }
        else
        {
            //cell_a.getSignal = true;
            //cell_a.active = true;
            //cell_a.getSignalTime = Time.time;
            //cell_a.fromAxon = this;
            //cell_a.holdSignalTime = UnityEngine.Random.Range(0.5f, 1.5f);
            senderMaker.MakeNewSender(cell_b.Position, cell_a.Position, UnityEngine.Random.Range(minTime, maxTime), cell_a, this);
            return 1;
        }
    }
}
