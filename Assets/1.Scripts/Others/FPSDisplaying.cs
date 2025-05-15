using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSDisplaying : MonoBehaviour
{
    private TextMeshProUGUI txt;

    private void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
        StartCoroutine(DisplayFPS());
    }
    private IEnumerator DisplayFPS()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            txt.SetText("FPS: " + (Mathf.FloorToInt(1f / Time.deltaTime)).ToString());
        }
    }
}
