using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField, Header("BGM用オーディオファイル")]
    private AudioClip[] bgms;

    private AudioSource audioSource;

    public enum BGMType
    {
        Main,
        GameClear,
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayBGM(BGMType.Main);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBGM(BGMType bgmType)
    {
        audioSource.Stop();
        audioSource.clip = bgms[(int)bgmType];
        audioSource.Play();
        //Debug.Log("再生中のBGM : " + bgmType);
    }
}
