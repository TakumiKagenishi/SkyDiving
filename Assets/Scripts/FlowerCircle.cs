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

    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 5.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    private void OnTriggerEnter(Collider other)
    {
        boxCollider.enabled = false;
        transform.SetParent(other.transform);
        StartCoroutine(PlayGetEffect());
    }
    /// <summary>
    /// 花輪をくぐった際の演出
    /// </summary>
    private IEnumerator PlayGetEffect()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.zero, 1.0f));
        sequence.Join(transform.DOLocalMove(Vector3.zero, 1.0f));
        yield return new WaitForSeconds(1.0f);
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y -1.5f, effect.transform.position.z);
        Destroy(effect, 1.0f);
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
