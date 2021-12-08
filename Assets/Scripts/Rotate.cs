using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    GameObject cube;

    public float speed;

    public bool aroundY;
    public bool aroundZ;
    private void Update()
    {
        Vector3 rotDir = new Vector3();
        if (aroundY)
        {
            rotDir = cube.transform.up;
        }
        else if(aroundZ)
        {
            rotDir = cube.transform.forward;
        }
       
        transform.RotateAround(cube.transform.position, rotDir, speed * Time.deltaTime);
    }
}
