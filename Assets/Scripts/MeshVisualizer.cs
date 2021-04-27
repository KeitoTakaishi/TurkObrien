using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class MeshVisualizer : MonoBehaviour
{
    [SerializeField] Solver solver;
    void Start()
    {
        
        GetComponent<MeshFilter>().mesh = solver.outerMesh;
        var mf = GetComponent<MeshFilter>();
        mf.mesh.SetIndices(mf.mesh.GetIndices(0), MeshTopology.Points, 0);

    }

    void Update()
    {
        
    }
}
