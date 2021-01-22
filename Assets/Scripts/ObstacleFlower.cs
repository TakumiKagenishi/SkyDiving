using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ObstacleFlower : MonoBehaviour
{
    private Animator anim;

    private BoxCollider boxCol;

    [SerializeField]
    private AudioClip octableFlowerSE;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water" || other.gameObject.tag == "FlowerCircle")
        {
            return;
        }

        if(other.gameObject.TryGetComponent(out PlayerController player))
        {
            StartCoroutine(EatingTarget(player));
        }
    }

    /// <summary>
    /// 対象を食べて吐き出す
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private IEnumerator EatingTarget(PlayerController player)
    {
        boxCol.enabled = false;
        player.transform.SetParent(transform);
        player.transform.localPosition = new Vector3(0, -2.0f, 0);
        player.transform.SetParent(null);
        anim.SetTrigger("attack");
        player.StopMove();
        yield return new WaitForSeconds(0.75f);
        player.ResumeMove();
        player.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 300, ForceMode.Impulse);
        player.transform.DORotate(new Vector3(180, 0, 1080), 0.5f, RotateMode.FastBeyond360);
        AudioSource.PlayClipAtPoint(octableFlowerSE, transform.position);
        player.HalveScore();
        transform.DOScale(Vector3.zero, 0.5f);
        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
