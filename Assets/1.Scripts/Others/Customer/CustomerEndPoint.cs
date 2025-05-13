using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CustomerEndPoint : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnFinishTrace += HidePoint;
        LevelManager.Instance.OnLoadLevelComplete += SetupPoint;
    }

    void OnDestroy()
    {
        GameManager.Instance.OnFinishTrace -= HidePoint;
        LevelManager.Instance.OnLoadLevelComplete -= SetupPoint;

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
        transform.position = LevelManager.Instance.CurrentLevelData.PlayerTrace.GetLastPoint();
        DoAnimLoopPoint();
    }
    private void HidePoint()
    {
        transform.DOKill();
        transform.DOLocalMoveY(2.5f, 0.3f);
        transform.DOScale(1.6f, 0.3f);
        transform.DORotateQuaternion(Quaternion.Euler(0, transform.eulerAngles.y + 180, 0), 0.2f).SetEase(Ease.Linear).SetLoops(5, LoopType.Incremental);
        transform.DOScale(0, 0.3f).SetDelay(0.7f);
        transform.DOLocalMoveY(1, 0.3f).SetDelay(0.7f);
    }
}
