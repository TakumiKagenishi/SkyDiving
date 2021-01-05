using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Camera fpsCamera;

    [SerializeField]
    private Camera selfishCamera;

    [SerializeField]
    private Button btnChangeCamera;

    private int cameraIndex;

    private Camera mainCamera;
    
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - playerController.transform.position;
        mainCamera = Camera.main;
        btnChangeCamera.onClick.AddListener(ChangeCamera);
        SetDefaultCamera();
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

    /// <summary>
    /// カメラを変更(ボタンを押すたびに呼び出される)
    /// </summary>
    private void ChangeCamera()
    {
        switch(cameraIndex)
        {
            case 0:
                cameraIndex++;
                mainCamera.enabled = false;
                fpsCamera.enabled = true;
                break;

            case 1:
                cameraIndex++;
                fpsCamera.enabled = false;
                selfishCamera.enabled = true;
                break;

            case 2:
                cameraIndex = 0;
                selfishCamera.enabled = false;
                mainCamera.enabled = true;
                break;
        }
    }

    /// <summary>
    /// カメラを初期カメラ(三人称カメラ)に戻す
    /// </summary>
    public void SetDefaultCamera()
    {
        cameraIndex = 0;
        mainCamera.enabled = true;
        fpsCamera.enabled = false;
        selfishCamera.enabled = false;
    }
}
