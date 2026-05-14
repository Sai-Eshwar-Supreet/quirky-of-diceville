
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class UiHelpers
{
    public static IEnumerator RefreshLayout(RectTransform parent)
    {
        yield return null;

        Canvas.ForceUpdateCanvases();

        LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
    }
}
