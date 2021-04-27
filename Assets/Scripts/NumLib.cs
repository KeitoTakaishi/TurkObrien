using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

public class NumLib : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        var A = Matrix<double>.Build.DenseOfArray(new double[,]
        {
           { 3, 2, -1 },
           { 2, -2, 4 },
           { -1, 0.5, -1 }
        });
        var b = Vector<double>.Build.Dense(new double[] { 1, -2, 0 });
        var x = A.Solve(b);

        Vector3 t;

        t.x = (float)(x.ToArray())[0];
        t.y = (float)(x.ToArray())[1];
        t.z = (float)(x.ToArray())[2];

        Debug.Log(t); // (1.0, -2.0, -2.0)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
