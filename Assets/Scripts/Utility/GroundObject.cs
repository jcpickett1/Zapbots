using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObject : MonoBehaviour
{
    void Start()
    {
        RaycastHit hit;
 
        Vector3 origin = new Vector3(transform.position.x, 100, transform.position.z);
 
        Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity);
        // Debug.Log(transform.position - hit.point);
        transform.position = hit.point;
    }
}
