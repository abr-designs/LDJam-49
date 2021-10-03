using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    private float speed;
    

    // Update is called once per frame
    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0,0,speed);
    }
}
