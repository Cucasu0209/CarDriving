using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameplayEffectManager : MonoBehaviour
{
    [SerializeField] private GameObject PickUpCustomerFx;
    [SerializeField] private GameObject ConfetiFinishTraceFx;

    private void Start()
    {
        GameManager.Instance.OnPickCustomer += OnPickupCustomer;
        GameManager.Instance.OnFinishTrace += OnFinishTrace;


    }
    private void OnDestroy()
    {
        GameManager.Instance.OnPickCustomer -= OnPickupCustomer;
        GameManager.Instance.OnFinishTrace -= OnFinishTrace;
    }

    private void OnPickupCustomer(Transform doorpos)
    {
        GameObject newfx = Instantiate(PickUpCustomerFx, LevelManager.Instance.CurrentLevelData.PickupPoint, Quaternion.identity);
        DOVirtual.DelayedCall(3, () => Destroy(newfx));
    }

    private void OnFinishTrace()
    {
        GameObject newfx = Instantiate(ConfetiFinishTraceFx, LevelManager.Instance.CurrentLevelData.PlayerTrace.GetLastPoint(), Quaternion.identity);
        DOVirtual.DelayedCall(3, () => Destroy(newfx));
    }
}
