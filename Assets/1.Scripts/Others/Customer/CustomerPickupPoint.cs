using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CustomerPickupPoint : MonoBehaviour
{
    [SerializeField] private AudioClip PickupSound;
    void Start()
    {
        GameManager.Instance.OnPickCustomer += HidePoint;
        LevelManager.Instance.OnLoadLevelComplete += SetupPoint;
    }

    void OnDestroy()
    {
        GameManager.Instance.OnPickCustomer -= HidePoint;
        LevelManager.Instance.OnLoadLevelComplete -= SetupPoint;
        transform.DOKill();

    }
    private void DoAnimLoopPoint()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localPosition -= Vector3.up * transform.localPosition.y;
        transform.localScale = Vector3.one;
        transform.DORotateQuaternion(Quaternion.Euler(0, 180, 0), 0.8f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        transform.DOLocalMoveY(1, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        transform.DOScale(1.1f, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
    private void SetupPoint()
    {
        transform.position = LevelManager.Instance.CurrentLevelData.GetPickupPoint();
        DoAnimLoopPoint();
    }
    private void HidePoint(Transform door)
    {
        SoundManager.Instance.PlayEffect(PickupSound);
        transform.DOKill();
        transform.DOLocalMoveY(3f, 0.3f);
        transform.DOScale(1.4f, 0.3f);
        transform.DORotateQuaternion(Quaternion.Euler(0, transform.eulerAngles.y + 180, 0), 0.2f).SetEase(Ease.Linear).SetLoops(5, LoopType.Incremental);
        transform.DOScale(0, 0.3f).SetDelay(0.7f);
        transform.DOLocalMoveY(1, 0.3f).SetDelay(0.7f);
    }
}
