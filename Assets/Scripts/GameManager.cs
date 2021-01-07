using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private Transform goal;

    [SerializeField]
    private Text txtDistance;

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private ResultPopUp resultPopUp;

    private float distance;

    private bool isGoal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoal == true)
        {
            return;
        }
        
        distance = player.transform.position.y - goal.transform.position.y;
        //Debug.Log(distance.ToString("F2"));

        txtDistance.text = distance.ToString("F2");

        if (distance <= 0)
        {
            isGoal = true;
            txtDistance.text = 0.ToString("F2");
            cameraController.SetDefaultCamera();
            resultPopUp.DisplayResult();
        }

    }
}
