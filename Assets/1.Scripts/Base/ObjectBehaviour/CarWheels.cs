using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarWheels : MonoBehaviour
{
    [SerializeField] private bool IsRuning = false;
    private void Start()
    {
        // transform.DOLocalRotateQuaternion(Quaternion.Euler(180, 0, 0), 0.3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        if (IsRuning) Run();
    }
    public void Run()
    {
        IsRuning = true;
        transform.DOLocalRotateQuaternion(Quaternion.Euler(180, 0, 0), 0.1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
    public void Stop()
    {    
        IsRuning = false;
        transform.DOKill();
        transform.localEulerAngles = Vector3.zero;
    }
    private void OnDestroy()
    {
         transform.DOKill();
    }
}
