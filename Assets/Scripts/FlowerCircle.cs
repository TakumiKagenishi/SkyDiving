using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowerCircle : MonoBehaviour
{
    [Header("花輪通過時の得点")]
    public int point;
    [SerializeField]
    private BoxCollider boxCollider;
    [SerializeField]
    private GameObject effectPrefab;
    [SerializeField]
    private AudioClip flowerSE;
    [SerializeField, Header("移動させる場合スイッチ入れる")]
    private bool isMoving;
    [SerializeField, Header("移動時間")]
    private float duration;
    [SerializeField, Header("移動距離")]
    private float moveDistance;
    [SerializeField, Header("移動する時間と距離をランダムにする割合"), Range(0, 100)]
    private int randomMovingPercent;
    [SerializeField, Header("移動時間のランダム幅")]
    private Vector2 durationRange;
    [SerializeField, Header("移動距離のランダム幅")]
    private Vector2 moveDistanceRange;
    [SerializeField, Header("大きさの設定")]
    private float[] flowerSizes;
    [SerializeField, Header("点数の倍率")]
    private float[] pointRate;

    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 5.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        if(isMoving)
        {
            transform.DOMoveZ(transform.position.z + moveDistance, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Water" || other.gameObject.tag == "Obstacle")
        {
            return;
        }
        boxCollider.enabled = false;
        transform.SetParent(other.transform);
        StartCoroutine(PlayGetEffect(other.GetComponent<PlayerController>()));
    }
    /// <summary>
    /// 花輪をくぐった際の演出
    /// </summary>
    private IEnumerator PlayGetEffect(PlayerController playerController)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.zero, 1.0f));
        sequence.Join(transform.DOLocalMove(Vector3.zero, 1.0f));
        yield return new WaitForSeconds(1.0f);
        if (playerController.inWater == false)
        {
            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            effect.transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y -1.5f, playerController.transform.position.z);
            Destroy(effect, 1.0f);
            AudioSource.PlayClipAtPoint(flowerSE, transform.position);
        }
        Destroy(gameObject, 1.0f);
    }

    /// <summary>
    /// 移動する花輪の設定
    /// </summary>
    /// <param name="isMoving"></param>
    /// <param name="isScaleChanging"></param>
    public void SetUpMovingFlowerCircle(bool isMoving, bool isScaleChanging)
    {
        this.isMoving = isMoving;

        if(this.isMoving)
        {
            if(DetectRandomMovingFromPercent())
            {
                ChangeRandomMoveParameters();
            }
        }

        if(isScaleChanging)
        {
            ChangeRandomScales();
        }
    }

    /// <summary>
    /// 移動時間と距離をランダムにする判定、 true の場合はランダムとする
    /// </summary>
    /// <returns></returns>
    private bool DetectRandomMovingFromPercent()
    {
        return Random.Range(0, 100) <= randomMovingPercent;
    }

    /// <summary>
    /// ランダム値を取得して移動
    /// </summary>
    private void ChangeRandomMoveParameters()
    {
        duration = Random.Range(durationRange.x, durationRange.y);
        moveDistance = Random.Range(moveDistanceRange.x, moveDistanceRange.y);
    }

    /// <summary>
    /// 大きさを変更して点数に反映
    /// </summary>
    private void ChangeRandomScales()
    {
        int index = Random.Range(0, flowerSizes.Length);
        transform.localScale *= flowerSizes[index];
        point = Mathf.CeilToInt(point * pointRate[index]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
