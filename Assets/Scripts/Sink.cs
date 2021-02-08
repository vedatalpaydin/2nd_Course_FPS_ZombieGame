using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
    private float destroyHeight;
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.tag=="Ragdoll")
            Invoke(nameof(StartSink),5);
    }

    public void StartSink()
    {
        destroyHeight = Terrain.activeTerrain.SampleHeight(transform.position )-5;
        Collider[] colList = transform.GetComponentsInChildren<Collider>();
        foreach (Collider c in colList)
        {
            Destroy(c);
        }
        InvokeRepeating(nameof(SinkIntoGround),5,0.5f);
    }

    void SinkIntoGround()
    {
        transform.Translate(0,-0.0001f,0);
        if (transform.position.y < destroyHeight)
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
