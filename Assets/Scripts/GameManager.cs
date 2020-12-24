using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private Transform goal;

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
        Debug.Log(distance.ToString("F2"));

        if (distance <= 0)
        {
            isGoal = true;
        }
    }
}
