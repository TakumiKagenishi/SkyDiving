﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Coffee.UIExtensions;

public class PlayerController : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed;
    [Header("落下速度")]
    public float fallSpeed;
    [Header("着水判定用, trueなら着水済")]
    public bool inWater;
    public enum AttitudeType
    {
        Straight,
        Prone,
    }
    [Header("現在のキャラの姿勢")]
    public AttitudeType attitudeType;
    private Rigidbody rb;
    private float x;
    private float z;
    private Vector3 straightRotation = new Vector3(180, 0, 0);
    private int score;
    private Vector3 proneRotation = new Vector3(-90, 0, 0);
    private float attitudeTimer;
    private float chargeTime = 2.0f;
    private bool isCharge;
    private Animator anim;
    [SerializeField, Header("水しぶきのエフェクト")]
    private GameObject splashEffectPrefab = null;
    [SerializeField, Header("水しぶきのSE")]
    private AudioClip splashSE = null;
    [SerializeField]
    private Text txtScore;
    [SerializeField]
    private Button btnChangeAttitude;
    [SerializeField]
    private Image imgGauge;
    [SerializeField]
    private ShinyEffectForUGUI shinyEffect;
    [SerializeField]
    private Transform limitLeftBottom;
    [SerializeField]
    private Transform limitRightTop;
    [SerializeField]
    private GameManager gameManager;
    
    //private FloatingJoystick joystick;

    // Start is called before the first frame update
    public void SetUpPlayer()
    {
        rb = GetComponent<Rigidbody>();
        transform.eulerAngles = straightRotation;
        attitudeType = AttitudeType.Straight;
        btnChangeAttitude.onClick.AddListener(ChangeAttitude);
        btnChangeAttitude.interactable = false;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameManager.isGoal == true)
        {
            return;
        }

        if (inWater)
        {
            return;
        }
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        //x = joystick.Horizontal;
        //z = joystick.Vertical;
        //Debug.Log(x);
        //Debug.Log(z);
        rb.velocity = new Vector3(x * moveSpeed, -fallSpeed, z * moveSpeed);
        //Debug.Log(rb.velocity);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Water" && inWater == false)
        {
            inWater = true;
            GameObject effect = Instantiate(splashEffectPrefab, transform.position, Quaternion.identity);
            effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y, effect.transform.position.z - 0.5f);
            Destroy(effect, 2.0f);
            AudioSource.PlayClipAtPoint(splashSE, transform.position);
            StartCoroutine(OutOfWater());
            //Debug.Log("着水 :" + inWater);
        }

        if (col.gameObject.tag == "FlowerCircle")
        {
            //Debug.Log("花輪ゲット");
            score += col.transform.parent.GetComponent<FlowerCircle>().point;
            //Debug.Log("現在の得点 : " + score);
            txtScore.text = score.ToString();
        }
    }
    /// <summary>
    /// 水面に顔を出す
    /// </summary>
    /// <returns></returns>
    private IEnumerator OutOfWater()
    {
        yield return new WaitForSeconds(0.1f);
        if (attitudeType == AttitudeType.Prone)
        {
            ChangeAttitude();
        }
        yield return new WaitForSeconds(1.0f);
        rb.isKinematic = true;
        transform.eulerAngles = new Vector3(-30, 180, 0);
        transform.DOMoveY(4.7f, 1.0f);
    }

    private void Update()
    {
        if (inWater)
        {
            btnChangeAttitude.interactable = false;
            return;
        }

        LimitMoveArea();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeAttitude();
        }

        if (isCharge == false && attitudeType == AttitudeType.Straight)
        {
            attitudeTimer += Time.deltaTime;
            imgGauge.DOFillAmount(attitudeTimer / chargeTime, 0.1f);
            btnChangeAttitude.interactable = false;
            if (attitudeTimer >= chargeTime)
            {
                attitudeTimer = chargeTime;
                isCharge = true;
                btnChangeAttitude.interactable = true;
                shinyEffect.Play(0.5f);
                //Debug.Log("チャージ済");
            }
        }

        if (attitudeType == AttitudeType.Prone)
        {
            attitudeTimer -= Time.deltaTime;
            imgGauge.DOFillAmount(attitudeTimer / chargeTime, 0.1f);
            if (attitudeTimer <= 0)
            {
                attitudeTimer = 0;
                btnChangeAttitude.interactable = false;
                ChangeAttitude();
            }
        }
    }

    /// <summary>
    /// 姿勢の変更
    /// </summary>
    private void ChangeAttitude()
    {
        switch (attitudeType)
        {
            case AttitudeType.Straight:
                if (isCharge == false)
                {
                    return;
                }
                isCharge = false;
                attitudeType = AttitudeType.Prone;
                transform.DORotate(proneRotation, 0.25f, RotateMode.WorldAxisAdd);
                rb.drag = 25.0f;
                btnChangeAttitude.transform.GetChild(0).DORotate(new Vector3(0, 0, 180), 0.25f);
                gameManager.sliderAltimeter.transform.GetChild(2).GetChild(0).DORotate(new Vector3(0, 0, 90), 0.25f);
                anim.SetBool("Prone", true);
                break;

            case AttitudeType.Prone:
                attitudeType = AttitudeType.Straight;
                transform.DORotate(straightRotation, 0.25f);
                rb.drag = 0f;
                btnChangeAttitude.transform.GetChild(0).DORotate(new Vector3(0, 0, 90), 0.25f);
                gameManager.sliderAltimeter.transform.GetChild(2).GetChild(0).DORotate(new Vector3(0, 0, 180), 0.25f);
                anim.SetBool("Prone", false);
                break;
        }
    }

    /// <summary>
    /// 移動範囲の確認と制限
    /// </summary>
    private void LimitMoveArea()
    {
        float limitX = Mathf.Clamp(transform.position.x, limitLeftBottom.position.x, limitRightTop.position.x);
        float limitZ = Mathf.Clamp(transform.position.z, limitLeftBottom.position.z, limitRightTop.position.z);
        transform.position = new Vector3(limitX, transform.position.y, limitZ);
    }

    /// <summary>
    /// キャラの落下と移動を一時停止
    /// </summary>
    public void StopMove()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// キャラの落下と移動を再開
    /// </summary>
    public void ResumeMove()
    {
        rb.isKinematic = false;
        rb.velocity = new Vector3(0, -fallSpeed, 0);
    }

    /// <summary>
    /// スコアを半分にする
    /// </summary>
    public void HalveScore()
    {
        score = Mathf.CeilToInt(score * 0.5f);
        Debug.Log("スコア半分 : " + score);
        txtScore.text = score.ToString();
    }

    /// <summary>
    /// 落下速度を減衰させながら元に戻す
    /// </summary>
    /// <param name="airResistance"></param>
    public void DampingDrag(float airResistance)
    {
        rb.drag = airResistance;
        DOTween.To(() => rb.drag, (x) => rb.drag = x, 0, 3.0f).OnComplete(() =>
        {
            if (transform.rotation.x != 1)
            {
                transform.DORotate(straightRotation, 0.25f);
            }
        });
    }
}

