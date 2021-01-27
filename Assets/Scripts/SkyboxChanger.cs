using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    [SerializeField]
    private Material[] skyboxMaterials;

    [SerializeField, Header("skybox設定用の設定値、999の場合、ランダムにする")]
    private int skyboxMaterialsIndex;

    /// <summary>
    /// Skyboxを変更
    /// </summary>
    public void ChangeSkybox()
    {
        if(skyboxMaterialsIndex == 999)
        {
            RenderSettings.skybox = skyboxMaterials[RandomSelectIndexOfSkyboxMaterials()];
        }

        else
        {
            RenderSettings.skybox = skyboxMaterials[skyboxMaterialsIndex];
        }

        Debug.Log("skybox変更");
    }


    public int RandomSelectIndexOfSkyboxMaterials()
    {
        return Random.Range(0, skyboxMaterials.Length);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
