using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orologio : MonoBehaviour
{
    public GameObject lancetteMinuti;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lancetteMinuti.transform.Rotate(Vector3.right, Time.deltaTime*10);
    }
}
