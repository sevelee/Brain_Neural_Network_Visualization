using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ObjImporter : MonoBehaviour {

    public Vector3[] vertices;

    public string modelFilePath;

    public TextAsset txtFile;

    public Mesh inputMesh
    {
        get
        {
            return mesh;
        }
    }

    public Mesh mesh;

    //thanks to http://blog.csdn.net/qinyuanpei/article/details/49991607
    Vector3[] LoadFromTxt(string objText)
    {
        if (objText.Length <= 0) return null;

        string[] allLines = objText.Split('\n');
        Vector3[] vertices = new Vector3[allLines.Length];
        Debug.Log(allLines.Length);
        int index = 0;
        foreach (string line in allLines)
        {
            string[] chars = line.Split(' ');
            if (chars[0] == "v")
            {
                vertices[index] = new Vector3(
                    ConvertToFloat(chars[1]),
                    ConvertToFloat(chars[2]),
                    ConvertToFloat(chars[3])
                    );
                ++index;
            }
        }
        return vertices;
    }

    private float ConvertToFloat(string s)
    {
        return (float)System.Convert.ToDouble(s);
    }

    private void Awake()
    {

        string content = txtFile.text;

        mesh = new Mesh();
        mesh.vertices = LoadFromTxt(content);
        Debug.Log(mesh.vertexCount);
        int[] indexs = new int[mesh.vertexCount];
        for (int i = 0; i < indexs.Length; ++i)
        {
            indexs[i] = i;
        }
        mesh.SetIndices(indexs, MeshTopology.Lines, 0);
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
    }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}
}
