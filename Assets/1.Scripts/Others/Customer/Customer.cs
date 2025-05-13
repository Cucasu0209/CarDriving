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
        transform.SetParent(null);
        transform.position = LevelManager.Instance.CurrentLevelData.CustomerStartPoint;
    }

    private void GoToCar(Transform doorTf)
    {

        Animator.SetTrigger("Move");
        transform.LookAt(doorTf);
        transform.DOMove(doorTf.position, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.3f, () => transform.localScale = Vector3.zero);
            transform.SetParent(doorTf);
        });
    }
    private void GoOffCar()
    {
        DOVirtual.DelayedCall(0.3f, () =>
        {
            transform.localScale = Vector3.one * 2;
            transform.LookAt(LevelManager.Instance.CurrentLevelData.FinalCustomerPoint);
            transform.DOMove(LevelManager.Instance.CurrentLevelData.FinalCustomerPoint, 2.8f).SetDelay(0.2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.LookAt(transform.parent);
                Animator.SetTrigger("Congrat");

            });
        });
    }
}
