using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemTrampoline : MonoBehaviour
{
    private BoxCollider boxCol;

    [SerializeField, Header("跳ねた時の空気抵抗値")]
    private float airResistance;

    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water" || other.gameObject.tag == "FlowerCircle")
        {
            return;
        }

        if (other.gameObject.TryGetComponent(out PlayerController player))
        {
            Bound(player);
        }
    }

    /// <summary>
    /// バウンドさせる
    /// </summary>
    /// <param name="player"></param>
    private void Bound(PlayerController player)
    {
        boxCol.enabled = false;
        player.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * Random.Range(800, 1000), ForceMode.Impulse);
        player.transform.DORotate(new Vector3(90, 1080, 0), 1.5f, RotateMode.FastBeyond360)
        .OnComplete(() =>
        {
            player.DampingDrag(airResistance);
        });

        Destroy(gameObject);
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
