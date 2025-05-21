using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShowroomUI_PagesMarker : MonoBehaviour
{

    [SerializeField] private RectTransform KnotHolder;
    [SerializeField] private Image KnotPrefab;
    [SerializeField] private Color NormalColor, PickedColor;
    private List<Image> CurrentKnots;
    private void Start()
    {
        ShowroomManager.Instance.OnPageChange += OnPageChange;
        ShowroomManager.Instance.OnLoadDataComplete += InitKnots;
    }
    private void OnDestroy()
    {
        ShowroomManager.Instance.OnPageChange -= OnPageChange;
        ShowroomManager.Instance.OnLoadDataComplete -= InitKnots;

    }
    private void OnEnable()
    {
        InitKnots();
    }

    private void InitKnots()
    {
        if (CurrentKnots != null)
        {
            for (int i = 0; i < CurrentKnots.Count; i++)
            {
                Destroy(CurrentKnots[i].gameObject);
            }
        }
        CurrentKnots = new List<Image>();

        for (int i = 0; i < ShowroomManager.Instance.GetPageCount(); i++)
        {
            Image newKnot = Instantiate(KnotPrefab, KnotHolder);
            CurrentKnots.Add(newKnot);
            newKnot.DOColor(ShowroomManager.Instance.CurrentPageIndex == i ? PickedColor : NormalColor, 0.2f);
            newKnot.rectTransform.sizeDelta = Vector2.one * (ShowroomManager.Instance.CurrentPageIndex == i ? 32 : 20);
        }

    }

    private void OnPageChange(bool isNextPage)
    {
        for (int i = 0; i < ShowroomManager.Instance.GetPageCount(); i++)
        {
            CurrentKnots[i].DOColor(ShowroomManager.Instance.CurrentPageIndex == i ? PickedColor : NormalColor, 0.2f);
            CurrentKnots[i].rectTransform.sizeDelta = Vector2.one * (ShowroomManager.Instance.CurrentPageIndex == i ? 32 : 20);
        }
    }

}
