using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class markerController : MonoBehaviour
{
    GameObject ImageTarget;
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "MazeWall")
        {
            UnityEngine.Debug.Log("Hit!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ImageTarget = transform.root.gameObject;
        Vector3 pos = transform.position;
        UnityEngine.Debug.Log("" + pos.x + pos.y + pos.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = ImageTarget.transform.position;
        //transform.position = new Vector3(pos.x, pos.y, -pos.z);
        UnityEngine.Debug.Log("" + pos.x + pos.y + pos.z);
    }
}
