//*************************************************************
// by Seve. 2017/02/06
// Thanks for Jasper Degens @Teamlab, Sakato Yoshiaki@Teamlab
//*************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace seve.QuadParticle
{
    public class particleMakerHoloTest : MonoBehaviour
    {
        #region Setting Variables

        public Texture2D ParticleTexture;

        public Material ParticleMaterial;

        public int ParticleAmount = 2;
        #endregion

        #region Public Variables

        private ComputeBuffer VertexBuffer;
        private ComputeBuffer UVBuffer;
        private ComputeBuffer PositionBuffer;

        #endregion

        #region Private Variables

        Vector3[] posData;

        Vector3 disapperPos = new Vector3(10000000, 10000000, 10000000);
        #endregion

        #region debug variables
        public GameObject debugTextObject;
        TextMesh debugText;
        #endregion

        /// <summary>
        /// Call a new particle. Return the index of particle. "-1" to overflow.
        /// </summary>
        /// <param name="pos">Particle init position</param>
        /// <returns></returns>
        public int newParticle(Vector3 pos)
        {
            int newid = findNewIndex();
            if (newid > ParticleAmount - 1) return -1;
            posData[newid] = pos;
            PositionBuffer.SetData(posData);
            return newid;
        }

        public void setFree(int pid)
        {
            posData[pid] = disapperPos;
            PositionBuffer.SetData(posData);
        }

        public void setParticlePosition(int pid, Vector3 pos)
        {
            if (pid < 0 || pid >= ParticleAmount) return;
            posData[pid] = pos;
        }

        public void disappear(int pid)
        {
            posData[pid] = disapperPos;
        }

        // Use this for initialization
        void Start()
        {
            InitVertices();
            debugText = debugTextObject.GetComponent<TextMesh>();
        }

        /// <summary>
        /// find a new id for new particle. if -1, is overflow
        /// </summary>
        /// <returns></returns>
        int findNewIndex()
        {
            return -1;
        }

        private void InitVertices()
        {
            VertexBuffer = new ComputeBuffer(6, Marshal.SizeOf(typeof(Vector2)));
            UVBuffer = new ComputeBuffer(6, Marshal.SizeOf(typeof(Vector2)));
            PositionBuffer = new ComputeBuffer(ParticleAmount, Marshal.SizeOf(typeof(Vector3)));
            Vector2[] verticesData = new Vector2[6];
            Vector2[] uvData = new Vector2[6];
            posData = new Vector3[ParticleAmount];

            verticesData[0] = new Vector2(-1.5f, -1.5f);
            verticesData[1] = new Vector2(-1.5f,  1.5f);
            verticesData[2] = new Vector2( 1.5f,  1.5f);
            verticesData[3] = new Vector2( 1.5f,  1.5f);
            verticesData[4] = new Vector2( 1.5f, -1.5f);
            verticesData[5] = new Vector2(-1.5f, -1.5f);

            uvData[0] = new Vector2(0, 0);
            uvData[1] = new Vector2(0, 1);
            uvData[2] = new Vector2(1, 1);
            uvData[3] = new Vector2(1, 1);
            uvData[4] = new Vector2(1, 0);
            uvData[5] = new Vector2(0, 0);

            VertexBuffer.SetData(verticesData);
            UVBuffer.SetData(uvData);

            for (int i = 0; i < ParticleAmount; ++i)
            {
                posData[i] = disapperPos;
            }
            PositionBuffer.SetData(posData);
        }

        void DebugFunction()
        {
            if (Input.GetMouseButton(0))
            {
                newParticle(new Vector3(UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(0, 10)));
            }
            if (Input.GetMouseButton(1))
            {
                for (int i = 0; i < ParticleAmount; ++i)
                {
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            //DebugFunction();
        }

        void updateAndDraw()
        {
            Vector3 scale = gameObject.transform.lossyScale;
            float maxScale = Mathf.Max((Mathf.Max(scale.x, scale.y)), scale.z);
            Vector3 pos = gameObject.transform.position;
            PositionBuffer.SetData(posData);
            ParticleMaterial.SetVector("_Transform", pos);
            ParticleMaterial.SetFloat("_Scale", maxScale);
            ParticleMaterial.SetBuffer("_VertexBuffer", VertexBuffer);
            ParticleMaterial.SetBuffer("_UVBuffer", UVBuffer);
            ParticleMaterial.SetBuffer("_PositionBuffer", PositionBuffer);
            ParticleMaterial.SetPass(0);
            Graphics.DrawProcedural(MeshTopology.Triangles, 6, ParticleAmount);
        }

        private void OnRenderObject()
        {
            debugText.text = "no1 pos " + posData[0] + " no2 pos " + posData[1];
            updateAndDraw();
        }

        private void OnDestroy()
        {
            VertexBuffer.Release();
            UVBuffer.Release();
            PositionBuffer.Release();
        }
    }
}