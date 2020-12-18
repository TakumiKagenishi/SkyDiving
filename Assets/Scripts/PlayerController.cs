using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed;
    [Header("落下速度")]
    public float fallSpeed;
    [Header("着水判定用, trueなら着水済")]
    public bool inWater;
    private Rigidbody rb;
    private float x;
    private float z;
    [SerializeField, Header("水しぶきのエフェクト")]
    private GameObject splashEffectPrefab = null;
    [SerializeField, Header("水しぶきのSE")]
    private AudioClip splashSE = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        //Debug.Log(x);
        //Debug.Log(z);
        rb.velocity = new Vector3(x * moveSpeed, -fallSpeed, z * moveSpeed);
        //Debug.Log(rb.velocity);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Water" && inWater == false)
        {
            inWater = true;
            GameObject effect = Instantiate(splashEffectPrefab, transform.position, Quaternion.identity);
            effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y, effect.transform.position.z -0.5f);
            Destroy(effect, 2.0f);
            AudioSource.PlayClipAtPoint(splashSE, transform.position);
            //Debug.Log("着水 :" + inWater);
        }
    }
}
