using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.magenta);
    }
}
