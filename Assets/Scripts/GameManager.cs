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

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField, Header("ステージをランダム生成する場合にはチェックする")]
    private bool isRandomStaging;

    [SerializeField, Header("移動する花輪の割合"), Range(0, 100)]
    private int movingFlowerCirclePercent;

    [SerializeField, Header("大きさが変化する花輪の割合"), Range(0, 100)]
    private int scalingFlowerCirclePercent;

    [SerializeField]
    private FlowerCircle flowerCirclePrefab;

    [SerializeField]
    private Transform limitLeftBottom;

    [SerializeField]
    private Transform limitRightTop;

    private float distance;

    public bool isGoal;

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
            audioManager.PlayBGM(AudioManager.BGMType.GameClear);
        }
    }

    IEnumerator Start()
    {
        isGoal = true;
        
        if(isRandomStaging)
        {
            yield return StartCoroutine(CreateRandomStage());
        }

        isGoal = false;
        Debug.Log(isGoal);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateRandomStage()
    {
        float flowerHeight = goal.position.y;
        int count = 0;
        Debug.Log("初期の花輪のスタート位置 : " + flowerHeight);
        while (flowerHeight <= player.transform.position.y -15)
        {
            flowerHeight += Random.Range(5.0f, 10.0f);
            Debug.Log("現在の花輪の生成位置 : " + flowerHeight);
            FlowerCircle flowerCircle = Instantiate(flowerCirclePrefab, new Vector3(Random.Range(limitLeftBottom.position.x, limitRightTop.position.x), flowerHeight, Random.Range(limitLeftBottom.position.z, limitRightTop.position.z)), Quaternion.identity);
            flowerCircle.SetUpMovingFlowerCircle(Random.Range(0, 100) <= movingFlowerCirclePercent, Random.Range(0, 100) <= scalingFlowerCirclePercent);
            count++;
            Debug.Log("花輪の合計生成数 : " + count);
            yield return null;
        }

        Debug.Log("ランダムステージ完成");
    }
}
