using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainNetWork : MonoBehaviour {
    #region world parameters
    static public int numberOfSignals = 0;
    #endregion

    #region setting parameters

    public int MaxSendSignalTimes = 5;
    public int MaxActiveCell = 5000;
    public int MaxSignalNumber = 1000;
    public float cellLiveTime = 4f;
    public float holdSignalTime = 2f;
    public float maxAxonDistance = 10f;
    public bool ActiveRandom = true;
    public int senderNumber = 2;
    #endregion

    List<Cell> cells;
    List<Axon> axons;

    int liveCellNumber = 0;

    Vector3[] meshVertex;
    Mesh mesh;

    ObjImporter objImporter;

    Mesh axonLineMesh;

    #region public paramaters
    public int jumpVertex = 1;

    public GameObject objImporterGameObject;

    public int maxAxonNumber = 5;

    public bool showCell = true;

    public GameObject senderMakerGO;
    pSenderMaker senderMaker;
    #endregion

    #region debug
    public GameObject debugCell;
    public Mesh debugCellMesh;
    public GameObject debugTextObject;
    TextMesh debugText;
    #endregion

    void otherInit()
    {
    }

    void loadVertexData()
    {
        objImporter = objImporterGameObject.GetComponent<ObjImporter>();
        mesh = new Mesh();
        mesh = objImporter.inputMesh;
        if (mesh != null)
        {
            meshVertex = new Vector3[mesh.vertexCount];
            meshVertex = mesh.vertices;
            return;
        }
        else
        {
            Debug.LogError("MESH IS NULL!");
        }
    }

    // Use this for initialization
    void Start () {
        senderMaker = senderMakerGO.GetComponent<pSenderMaker>();
        loadVertexData();
        InitCells();
        InitAxons();
        //debugShowCells();
        makeAxonLineMesh();
        debugText = debugTextObject.GetComponent<TextMesh>();
    }

    /// <summary>
    /// Init the Axons
    /// </summary>
    private void InitAxons()
    {
        //双向边
        axons = new List<Axon>();
        for (int i = 0; i < cells.Count; ++i)
        {
            for (int j = i + 1; j < cells.Count; ++j)
            {
                Cell cell_a = cells[i];
                Cell cell_b = cells[j];
                Vector3 pa = cell_a.Position;
                Vector3 pb = cell_b.Position;
                if (Vector3.Distance(pa, pb) < maxAxonDistance)
                {
                    if (cell_a.AxonNumber < maxAxonNumber && cell_b.AxonNumber < maxAxonNumber)
                    {
                        //创建新的axon
                        //分别存到cell a， cell b， network里
                        Axon newA = new Axon(cell_a, cell_b);
                        newA.senderMaker = senderMaker;
                        cell_a.selfAxons.Add(newA);
                        cell_b.selfAxons.Add(newA);
                        axons.Add(newA);
                    }
                }
            }
        }
        Debug.Log("Number of axons : " + axons.Count);
    }

    /// <summary>
    /// draw lines for Axons
    /// </summary>
    void makeAxonLineMesh()
    {
        axonLineMesh = new Mesh();
        Axon axon;

        int[] indexs = new int[axons.Count * 2];
        int indexIndex = 0;

        for (int i = 0; i < axons.Count; ++i)
        {
            axon = axons[i];
            indexs[indexIndex] = axon.CellA.meshIndex;
            ++indexIndex;
            indexs[indexIndex] = axon.CellB.meshIndex;
            ++indexIndex;
        }

        axonLineMesh.vertices = meshVertex;
        axonLineMesh.SetIndices(indexs, MeshTopology.Lines, 0);

        gameObject.GetComponent<MeshFilter>().mesh = axonLineMesh;
    }

    /// <summary>
    /// Init Cells from obj mesh data
    /// </summary>
    private void InitCells()
    {
        cells = new List<Cell>();
        for (int i = 0; i < meshVertex.Length; i += jumpVertex)
        {
            Cell newCell = new Cell(meshVertex[i]);
            cells.Add(newCell);
            //newCell.debugCube = Instantiate(debugCell, newCell.Position, Quaternion.identity);
            //newCell.debugCube.SetActive(false);
            newCell.meshIndex = i;
        }
        Debug.Log("THE number of Cells : " + cells.Count);
    }

    /// <summary>
    /// Debug mode. Draw cells as a given GameObject.
    /// </summary>
    void debugShowCells()
    {
        foreach (var c in cells)
        {
            GameObject g = Instantiate(debugCell, c.Position, Quaternion.identity);
            g.transform.parent = gameObject.transform;
            //Graphics.DrawMeshNow(debugCellMesh, c.Position, Quaternion.identity);
        }
    }

    /// <summary>
    /// update the status of cells
    /// </summary>
    void Update()
    {
        liveCellNumber = 0;
        //for (int i = 0; i <)
        for (int i = 0; i < cells.Count; ++i)
        {
            var c = cells[i];
            if (c.actived) continue;
            if (liveCellNumber < maxAxonNumber && numberOfSignals < MaxSignalNumber)
            {
                //Debug.Log("NUMBER OF SIGNAL" + numberOfSignals);
                if (c.getSignal)
                {
                    if (c.sendCount < MaxSendSignalTimes)
                    {
                        numberOfSignals += c.SendSignal();
                        c.actived = true;
                        if (c.sendCount > 1) Debug.Log("SEND COUNT ERROR" + c.sendCount);
                    }
                }
            }
            else
            {
                c.getSignal = false;
            }
            //c.debugCube.SetActive(c.active);
        }
        if (numberOfSignals == 0)
        {
            resetAllCells();
            if (ActiveRandom)
            {
                ActiveRadomCell();
            }
        }
        debugText.text = "Signal Number" + numberOfSignals;
    }

    private void ActiveRadomCell()
    {
        int num = (int)(UnityEngine.Random.value * cells.Count);
        cells[num].getSignal = true;
        cells[num].getSignalTime = Time.time;
    }

    void resetAllCells()
    {
        foreach (var c in cells)
        {
            c.reset();
        }
    }
}
