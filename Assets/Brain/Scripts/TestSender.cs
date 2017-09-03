using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSender : MonoBehaviour {
    public GameObject sender;
    public Vector3 pa = new Vector3(0, 0, 0);
    public Vector3 pb = new Vector3(10, 10, 10);
    public float limitTime = 10f;

	// Use this for initialization
	void Start () {
        GameObject g = Instantiate(sender, pa, Quaternion.identity);
        Sender se = g.GetComponent<Sender>();
        se.limitTime = limitTime;
        se.fromPoint = pa;
        se.toPoint = pb;
        se.startTime = Time.time;
        se.active = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
