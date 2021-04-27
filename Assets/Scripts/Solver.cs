using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour
{

    int inputPointsNum;
    int n;
    int curN = 0;
    Vector3[] pp;
    Vector3[] pn;
    float eps = 0.5f;
    

    //Matrix 
    int nA;
    float[] MatA;

    public Mesh outerMesh;
    public Mesh innerMesh;



    private void Awake()
    {
        outerMesh = new Mesh();
        innerMesh = new Mesh();
    }
    void Start()
    {
        loadData();
        calcInnerValue();
        createMesh();
        initMatrix();
    }

    void Update()
    {
        
    }

    private void loadData()
    {
        //Vector3[] pos = new Vector3[8];
        //Vector3[] nor = new Vector3[8];
        inputPointsNum = 8;
        n = inputPointsNum * 2;

        pp = new Vector3[n];
        pn = new Vector3[n];

        
        pp[0] = new Vector3(1.0f, 1.0f, 1.0f);
        pp[1] = new Vector3(1.0f, -1.0f, 1.0f);
        pp[2] = new Vector3(-1.0f, -1.0f, 1.0f);
        pp[3] = new Vector3(-1.0f, 1.0f, 1.0f);
        pp[4] = new Vector3(1.0f, 1.0f, -1.0f);
        pp[5] = new Vector3(1.0f, -1.0f, -1.0f);
        pp[6] = new Vector3(-1.0f, -1.0f, -1.0f);
        pp[7] = new Vector3(-1.0f, 1.0f, -1.0f);


        for (int i = 0; i < inputPointsNum; ++i)
        {
            pn[i] = pp[i].normalized;
            //Debug.Log("Normalil :" + pn[i]);
        }

    }

    private void calcInnerValue()
    {
        for(int i = inputPointsNum; i < n; i++)
        {
            pp[i] = pp[i - inputPointsNum] - eps * pn[i - inputPointsNum];
        }
    }

    private void initMatrix()
    {
        int dim = 3;

        //行列の1辺
        nA = 2 * inputPointsNum + dim + 1;
        
        MatA = new float[nA * nA];


        //n is num of  input points
        n = inputPointsNum;
        for (int i = 0; i < 2*n; i++)
        {
            for (int j = 0; j < 2*n; j++)
            {
                MatA[j + i * nA] = phi(pp[i], pp[j]);
            }
        }
        
        // left bottom
        for (int i = 0; i < 2*n; i++)
        {
            MatA[i + 2 * n * nA] = pp[i].x;
            MatA[i + (2 * n + 1) * nA] = pp[i].y;
            MatA[i + (2 * n + 2) * nA] = pp[i].z;
            MatA[i + (2 * n + 3) * nA] = 1.0f;
        }

        
        // right top
        for (int j = 0; j < 2*n; j++)
        {
            MatA[2 * n + j * nA] = pp[j].x;
            MatA[2 * n + j * nA + 1] = pp[j].y;
            MatA[2 * n + j * nA + 2] = pp[j].z;
            MatA[2 * n + j * nA + 3] = 1.0f;
        }
        // other
        for (int i = 2 * n; i < nA; i++)
        {
            for (int j = 2 * n; j < nA; j++)
            {
                MatA[i + j * nA] = 0.0f;
            }
        }


        float[] b = new float[nA];
        for (int i = 0; i < n; i++)
        {
            b[i] = 0.0f;
        }
        for (int i = n; i < 2 * n; i++)
        {
            b[i] = eps;
        }
        for (int i = 2 * n; i < 2 * n + (dim+1); i++)
        {
            b[i] = 0.0f;
        }


        //--------------------------------------------

        //Preview
        for(int i = 0; i < nA; i++)
        {
            string s = "";
            for(int j = 0; j < nA; j++)
            {
                s += MatA[j + i * nA].ToString() + ", ";
            }
            Debug.Log(s);
        }


        Debug.Log("==========================================");
        Debug.Log("Swap======================================");

        
        for (int j = 0; j < nA; j++)
        {
            var sIdx = j + 0 * nA;
            var dIdx = j + 1 * nA;

            var temp = MatA[dIdx];
            MatA[dIdx] = MatA[sIdx];
            MatA[sIdx] = temp;

        }

        for (int i = 0; i < nA; i++)
        {
            string s = "";
            for (int j = 0; j < nA; j++)
            {
                s += MatA[j + i * nA].ToString() + ", ";
            }
            Debug.Log(s);
        }

        Debug.Log("==========================================");
        Debug.Log("Forward===================================");



        //前身消去
        
        for (int i = 0; i < nA - 1; i++)
        {
            Debug.Log("Mat-----------------------------------------");
            for (int j = i + 1; j < nA; j++)
            {
                //0になる
                var idx = i + j * nA;
                var s = MatA[idx] / MatA[i*nA+i];
                Debug.Log(s);
                //for (int k = i; k < nA; k++)
                //{
                //    MatA[k+j*nA] -= MatA[k+i*nA] * s;
                //}
                //b[j] -= b[i] * s;
                
            }
            Debug.Log("-----------------------------------------");
        }
        

        //Debug.Log("-----------------------------------------");
        //Debug.Log("-----------------------------------------");

        //for (int i = 0; i < nA; i++)
        //{
        //    string s = "";
        //    for (int j = 0; j < nA; j++)
        //    {
        //        s += MatA[j + i * nA].ToString() + ", ";
        //    }
        //    Debug.Log(s);
        //}
    }

    private float phi(Vector3 p, Vector3 points)
    {
        float d = Mathf.Sqrt((p.x - points.x) * (p.x - points.x) + (p.y - points.y) * (p.y - points.y) + (p.z - points.z) * (p.z - points.z));
        return Mathf.Pow(d, 3.0f);
    }



    private void createMesh()
    {
        Vector3[] vertices = new Vector3[inputPointsNum];
        int[] indices = new int[inputPointsNum];

        for(int i = 0; i < inputPointsNum; i++)
        {
            vertices[i] = pp[i];
            indices[i] = i;
        }
        outerMesh.SetVertices(vertices);
        outerMesh.SetIndices(indices, MeshTopology.Points, 0);


        for (int i = 0; i < inputPointsNum; i++)
        {
            vertices[i] = pp[i+ inputPointsNum];
            indices[i] = i;
        }
        innerMesh.SetVertices(vertices);
        innerMesh.SetIndices(indices, MeshTopology.Points, 0);
    }



    
}
