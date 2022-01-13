using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Hierarchy window game object icon.
/// http://diegogiacomelli.com.br/unitytips-hierarchy-window-gameobject-icon/
/// </summary>
[InitializeOnLoad]
public static class HierarchyWindowGameObjectIcon
{
    const                   string  IgnoreIcons      = "GameObject Icon, Prefab Icon";
    private static readonly Texture _globalAssetIcon = EditorGUIUtility.IconContent("SceneAsset Icon").image;

    static HierarchyWindowGameObjectIcon()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var contentObject = EditorUtility.InstanceIDToObject(instanceID);
        if ((contentObject as GameObject)?.GetComponent<GlobalAssetBinding>())
        {
            var content = EditorGUIUtility.ObjectContent(contentObject, null);
            // var texture = EditorGUIUtility.FindTexture("d_TrackMarkerButtonEnabled");
            if (content.image != null && !IgnoreIcons.Contains(content.image.name))
                // GUI.DrawTexture(new Rect(selectionRect.xMax - 16, selectionRect.yMin, 16, 16), content.image);
                GUI.DrawTexture(new Rect(selectionRect.xMax - 16, selectionRect.yMin, 16, 16),_globalAssetIcon);
        }
    }
}