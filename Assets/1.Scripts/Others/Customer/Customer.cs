using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    private void Start()
    {
        LevelManager.Instance.OnLoadLevelComplete += SetStartPoint;
        GameManager.Instance.OnPickCustomer += GoToCar;
        GameManager.Instance.OnFinishTrace += GoOffCar;
    }
    private void OnDestroy()
    {
        LevelManager.Instance.OnLoadLevelComplete -= SetStartPoint;
        GameManager.Instance.OnPickCustomer -= GoToCar;
        GameManager.Instance.OnFinishTrace -= GoOffCar;

    }


    private void SetStartPoint()
    {
        Animator.SetTrigger("Restart");
        transform.SetParent(null);
        transform.localScale = Vector3.one;
        transform.position = LevelManager.Instance.CurrentLevelData.CustomerStartPoint;
        transform.LookAt(LevelManager.Instance.CurrentLevelData.GetPickupPoint());

    }

    private void GoToCar(Transform doorTf)
    {
        Animator.ResetTrigger("Restart");
        Animator.SetTrigger("Move");
        transform.LookAt(doorTf);
        transform.DOMove(doorTf.position, 0.7f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.localScale = Vector3.zero;
            transform.SetParent(doorTf);
        });
    }
    private void GoOffCar()
    {
        DOVirtual.DelayedCall(0.3f, () =>
        {
            transform.localScale = Vector3.one;
            transform.LookAt(LevelManager.Instance.CurrentLevelData.FinalCustomerPoint);
            transform.DOMove(LevelManager.Instance.CurrentLevelData.FinalCustomerPoint, 1.5f).SetDelay(0.2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.LookAt(LevelManager.Instance.CurrentLevelData.PlayerTrace.GetLastPoint());
                Animator.SetTrigger("Congrat");

            });
        });
    }
}
