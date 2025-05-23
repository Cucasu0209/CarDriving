using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShowroomUI_CarGrid : MonoBehaviour
{
    [SerializeField] private List<RectTransform> Grid;
    [SerializeField] private ShowroomUI_CarGridElement ElementPrefab;
    private List<ShowroomUI_CarGridElement> Elements = new List<ShowroomUI_CarGridElement>();
    private int CurrentGridUsing = 0;
    private void Start()
    {
        ShowroomManager.Instance.OnPageChange += OnPageChange;
    }
    private void OnDestroy()
    {
        ShowroomManager.Instance.OnPageChange -= OnPageChange;

    }
    private void OnPageChange(bool isNext)
    {
        float destinationX = 2000;
        int targetIndex = (CurrentGridUsing + 1) % Grid.Count;

        //Cache last Element
        List<ShowroomUI_CarGridElement> LastElement = new List<ShowroomUI_CarGridElement>(Elements);

        //Create new Elements
        Elements = new List<ShowroomUI_CarGridElement>();
        List<CarData> elementDatas = ShowroomManager.Instance.GetElementsInPage(ShowroomManager.Instance.CurrentPageIndex);
        for (int i = 0; i < elementDatas.Count; i++)
        {
            ShowroomUI_CarGridElement el = PoolingSystem.Spawn(ElementPrefab.gameObject, Grid[targetIndex].position, Quaternion.identity).GetComponent<ShowroomUI_CarGridElement>();
            el.transform.SetParent(Grid[targetIndex]);
            el.transform.SetAsLastSibling();
            el.transform.localPosition = Vector3.zero;
            el.transform.localScale = Vector3.one;
            el.SetData(elementDatas[i]);
            Elements.Add(el);
        }

        //Move out - move in
        Grid[CurrentGridUsing].DOAnchorPosX(isNext ? -destinationX : destinationX, 0.4f).SetEase(Ease.Linear);
        Grid[targetIndex].anchoredPosition = new Vector2((isNext ? destinationX : -destinationX), Grid[targetIndex].anchoredPosition.y);
        Grid[targetIndex].DOAnchorPosX(0, 0.4f).SetEase(Ease.Linear);
        CurrentGridUsing = targetIndex;
        DOVirtual.DelayedCall(0.4f, () =>
        {
            for (int i = 0; i < LastElement.Count; i++)
            {
                PoolingSystem.Despawn(LastElement[i].gameObject);
            }
        });
    }

    private void OnEnable()
    {
        int targetIndex = (CurrentGridUsing + 1) % Grid.Count;
        float destinationX = 2000;

        //Cache last Element
        List<ShowroomUI_CarGridElement> LastElement = new List<ShowroomUI_CarGridElement>(Elements);

        //Create new Elements
        Elements = new List<ShowroomUI_CarGridElement>();
        List<CarData> elementDatas = ShowroomManager.Instance.GetElementsInPage(ShowroomManager.Instance.CurrentPageIndex);
        for (int i = 0; i < elementDatas.Count; i++)
        {
            ShowroomUI_CarGridElement el = PoolingSystem.Spawn(ElementPrefab.gameObject, Grid[targetIndex].position, Quaternion.identity).GetComponent<ShowroomUI_CarGridElement>();
            el.transform.SetParent(Grid[targetIndex]);
            el.transform.SetAsLastSibling();
            el.transform.localPosition = Vector3.zero;
            el.transform.localScale = Vector3.one;
            el.SetData(elementDatas[i]);
            Elements.Add(el);
        }

        //Move out - move in

        Grid[CurrentGridUsing].anchoredPosition = new Vector2(-destinationX, Grid[CurrentGridUsing].anchoredPosition.y);
        Grid[targetIndex].anchoredPosition = new Vector2(0, Grid[targetIndex].anchoredPosition.y);
        CurrentGridUsing = targetIndex;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            for (int i = 0; i < LastElement.Count; i++)
            {
                PoolingSystem.Despawn(LastElement[i].gameObject);
            }
        });
    }
}
