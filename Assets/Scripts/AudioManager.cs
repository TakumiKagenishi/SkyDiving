using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField, Header("BGM用オーディオファイル")]
    private AudioClip[] bgms = null;

    private AudioSource audioSource;

    public enum BgmType
    {
        Main,
        GameClear,
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayBGM(BgmType.Main);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBGM(BgmType bgmType)
    {
        audioSource.Stop();
        audioSource.clip = bgms[(int)bgmType];
        audioSource.Play();
        //Debug.Log("再生中のBGM : " + bgmType);
    }
}
