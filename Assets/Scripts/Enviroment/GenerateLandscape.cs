using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLandscape : MonoBehaviour
{
    public GameObject planePrefab;


    [SerializeField]
    private int _rols;
    [SerializeField]
    private int _cols;


    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        List<GameObject> list = new List<GameObject>();
        float factorX = planePrefab.transform.localScale.x * 10;
        float factorZ = planePrefab.transform.localScale.z * 10;
        for (float obj = 0, x = 0 - factorX * _rols / 2; obj <= _rols; obj++, x += factorX)
        {
            for (float objZ = 0, z = 0 - factorZ * _cols / 2; objZ <= _cols; objZ++, z += factorZ)
            {
                Transform plane = Instantiate(planePrefab).transform;
                
                plane.position = new Vector3(x, 0, z);

                plane.SetParent(transform);

                list.Add(plane.gameObject);
            }
        }
        transform.Rotate(new Vector3(0, 45, 0));
    }
}
