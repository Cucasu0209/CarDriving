using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShowroomUI_CarGrid : MonoBehaviour
{
    [SerializeField] private List<RectTransform> Grid;
    [SerializeField] private ShowroomUI_CarGridElement ElementPrefab;
    private List<ShowroomUI_CarGridElement> Elements= new List<ShowroomUI_CarGridElement>();
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
            ShowroomUI_CarGridElement el = Instantiate(ElementPrefab, Grid[targetIndex]);
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
                Destroy(LastElement[i].gameObject);
            }
        });
    }
}
