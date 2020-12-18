using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - playerController.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.inWater == true)
        {
            return;
        }

        if(playerController != null)
        {
            transform.position = playerController.transform.position + offset;
        }
    }
}
