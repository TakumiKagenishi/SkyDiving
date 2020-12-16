using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObj;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - playerObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerObj != null)
        {
            transform.position = playerObj.transform.position + offset;
        }
    }
}
