using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TreeEditor;
using UnityEngine;

public class CarCustomer : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private Transform PickupPin;
    [SerializeField] private Transform EndPin;
    private void Start()
    {
        DoAnimLoopPin(PickupPin);
        DoAnimLoopPin(EndPin);
        GameManager.Instance.OnPickCustomer += GoToCar;
        GameManager.Instance.OnFinishTrace += GoOffCar;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnPickCustomer -= GoToCar;
        GameManager.Instance.OnFinishTrace -= GoOffCar;

    }

    private void GoToCar(Transform doorTf)
    {
        HidePin(PickupPin);

        Animator.SetTrigger("Move");
        transform.LookAt(doorTf);
        transform.DOMove(doorTf.position, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.3f, () => transform.localScale = Vector3.zero);
            transform.SetParent(doorTf);
        });
    }
    private void GoOffCar(Transform endPos)
    {
        HidePin(EndPin);

        DOVirtual.DelayedCall(0.3f, () =>
        {
            transform.localScale = Vector3.one * 2;
            transform.LookAt(endPos);
            transform.DOMove(endPos.position, 2.8f).SetDelay(0.2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.LookAt(transform.parent);
                Animator.SetTrigger("Congrat");

            });
        });
    }
    private void DoAnimLoopPin(Transform Pin)
    {
        Pin.DORotateQuaternion(Quaternion.Euler(0, 180, 0), 0.8f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        Pin.DOLocalMoveY(1, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        Pin.DOScale(1.1f, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
    private void HidePin(Transform Pin)
    {
        Pin.DOKill();
        Pin.DOLocalMoveY(2.5f, 0.3f);
        Pin.DOScale(1.6f, 0.3f);
        Pin.DORotateQuaternion(Quaternion.Euler(0, Pin.eulerAngles.y + 180, 0), 0.2f).SetEase(Ease.Linear).SetLoops(5, LoopType.Incremental);
        Pin.DOScale(0, 0.3f).SetDelay(0.7f);
        Pin.DOLocalMoveY(1, 0.3f).SetDelay(0.7f);
    }
  
}
