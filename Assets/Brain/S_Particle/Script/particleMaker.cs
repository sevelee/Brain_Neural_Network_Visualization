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
    public class particleMaker : MonoBehaviour
    {
        #region Setting Variables

        public Texture2D ParticleTexture;

        public Material ParticleMaterial;

        public int ParticleAmount = 1000;
        #endregion

        #region Public Variables

        private ComputeBuffer VertexBuffer;
        private ComputeBuffer UVBuffer;
        private ComputeBuffer ParticlePropertyBuffer;

        protected struct ParticleProperty
        {
            public Vector3 position;
            public bool active;
        }
        #endregion

        #region Private Variables

        ParticleProperty[] pProper;

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
            pProper[newid].position = pos;
            pProper[newid].active = true;
            ParticlePropertyBuffer.SetData(pProper);
            return newid;
        }

        public void setFree(int pid)
        {
            pProper[pid].position = disapperPos;
            pProper[pid].active = false;
            ParticlePropertyBuffer.SetData(pProper);
        }

        public void setParticlePosition(int pid, Vector3 pos)
        {
            if (pid < 0 || pid >= ParticleAmount) return;
            pProper[pid].position = pos;
        }

        public void disappear(int pid)
        {
            pProper[pid].position = disapperPos;
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
            for (int i = 0; i < ParticleAmount; ++i)
            {
                if (pProper[i].active == false)
                    return i;
            }
            return -1;
        }

        private void InitVertices()
        {
            VertexBuffer = new ComputeBuffer(6, Marshal.SizeOf(typeof(Vector2)));
            UVBuffer = new ComputeBuffer(6, Marshal.SizeOf(typeof(Vector2)));
            Vector2[] verticesData = new Vector2[6];
            Vector2[] uvData = new Vector2[6];

            verticesData[0] = new Vector2(-1, -1);
            verticesData[1] = new Vector2(-1,  1);
            verticesData[2] = new Vector2( 1,  1);
            verticesData[3] = new Vector2( 1,  1);
            verticesData[4] = new Vector2( 1, -1);
            verticesData[5] = new Vector2(-1, -1);

            uvData[0] = new Vector2(0, 0);
            uvData[1] = new Vector2(0, 1);
            uvData[2] = new Vector2(1, 1);
            uvData[3] = new Vector2(1, 1);
            uvData[4] = new Vector2(1, 0);
            uvData[5] = new Vector2(0, 0);

            VertexBuffer.SetData(verticesData);
            UVBuffer.SetData(uvData);

            ParticlePropertyBuffer = new ComputeBuffer(ParticleAmount, Marshal.SizeOf(typeof(ParticleProperty)));
            pProper = new ParticleProperty[ParticleAmount];
            //make sure to set false
            for (int i = 0; i < ParticleAmount; ++i)
            {
                pProper[i].active = false;
                pProper[i].position = disapperPos;
            }
            ParticlePropertyBuffer.SetData(pProper);
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
                    if (pProper[i].active)
                    {
                        setFree(i);
                        break;
                    }
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
            ParticlePropertyBuffer.SetData(pProper);
            ParticleMaterial.SetVector("_Transform", pos);
            ParticleMaterial.SetFloat("_Scale", maxScale);
            ParticleMaterial.SetBuffer("_VertexBuffer", VertexBuffer);
            ParticleMaterial.SetBuffer("_UVBuffer", UVBuffer);
            ParticleMaterial.SetBuffer("_ParticlePropertyBuffer", ParticlePropertyBuffer);
            ParticleMaterial.SetPass(0);
            Graphics.DrawProcedural(MeshTopology.Triangles, 6, ParticleAmount);
        }

        private void OnRenderObject()
        {
            debugText.text = "no1 pos " + pProper[0].position + " no2 pos " + pProper[1].position;
            updateAndDraw();
        }

        private void OnDestroy()
        {
            VertexBuffer.Release();
            UVBuffer.Release();
            ParticlePropertyBuffer.Release();
        }
    }
}