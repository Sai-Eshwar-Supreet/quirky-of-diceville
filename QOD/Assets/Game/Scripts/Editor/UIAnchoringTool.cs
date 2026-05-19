using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class UIAnchoringTool : EditorWindow
{
    private RectTransform _root;
    [MenuItem("Tools/UI Anchoring Tool")]
    public static void ShowWindow()
    {
        GetWindow<UIAnchoringTool>(nameof(UIAnchoringTool));
    }

    private void OnGUI()
    {
        GUILayout.Label("UI Anchoring", EditorStyles.boldLabel);

        _root = (RectTransform) EditorGUILayout.ObjectField("Root", _root, typeof(RectTransform), true);

        if(_root == null)
        {
            EditorGUILayout.HelpBox("Assign a root RectTransform",  MessageType.Error);
            return;
        }

        if(GUILayout.Button("Set Anchors"))
        {
            SetAnchors();
        }
    }

    [ContextMenu("Set Anchors")]
    void SetAnchors()
    {
        // Find all RectTransform components in the scene

        RectTransform[] rectTransforms = _root.GetComponentsInChildren<RectTransform>(true);

        foreach (RectTransform rt in rectTransforms)
        {
            Debug.Log($"Processing RectTransform: {rt.name}");
            // Check if RectTransform is not part of any layout group or content
            if (!IsInLayoutGroup(rt))
            {
                SetAnchorsToCorners(rt);
            }
        }
    }

    bool IsInLayoutGroup(RectTransform rt)
    {
        // Check if RectTransform or any of its parents have a LayoutGroup component
        Transform current = rt;
        while (current != null)
        {
            if ((current.parent != null && current.parent.GetComponent<LayoutGroup>() != null) || 
                current.GetComponent<ContentSizeFitter>() != null || 
                current.GetComponent<AspectRatioFitter>() != null)
            {
                return true;
            }
            current = current.parent;
        }
        return false;
    }

    void SetAnchorsToCorners(RectTransform rt)
    {
        if (rt == null)
        {
            Debug.LogError("RectTransform is null");
            return;
        }

        // Calculate new anchor positions
        RectTransform parent = rt.parent as RectTransform;
        if (parent == null)
        {
            Debug.LogError("Parent RectTransform is null");
            return;
        }

        if (Mathf.Approximately(parent.rect.width, 0) ||
            Mathf.Approximately(parent.rect.height, 0))
        {
            Debug.LogWarning($"Skipping {rt.name} because parent size is zero.");
            return;
        }

        Vector2 newAnchorMin = new (
            rt.anchorMin.x + rt.offsetMin.x / parent.rect.width,
            rt.anchorMin.y + rt.offsetMin.y / parent.rect.height);

        Vector2 newAnchorMax = new (
            rt.anchorMax.x + rt.offsetMax.x / parent.rect.width,
            rt.anchorMax.y + rt.offsetMax.y / parent.rect.height);

        // Set new anchor values
        rt.anchorMin = newAnchorMin;
        rt.anchorMax = newAnchorMax;

        // Reset offsets to zero
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}