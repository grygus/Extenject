using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GlobalAssetBinding)), InitializeOnLoad]
public class GlobalAssetEditor : Editor
{
    #region Static Context

    static GlobalAssetEditor()
    {
        // Debug.Log("#Engine# Initialized Editor");
        EditorApplication.hierarchyChanged     -= OnHierarchyChanged;
        EditorApplication.hierarchyChanged     += OnHierarchyChanged;
        EditorSceneManager.sceneOpened         += EditorSceneManagerOnsceneOpened;
        EditorApplication.update               += OnInitialized;
        EditorApplication.playModeStateChanged += ModeChanged;
    }


    static void ModeChanged(PlayModeStateChange playModeState)
    {
        if (playModeState == PlayModeStateChange.EnteredEditMode)
        {
            Debug.Log("Exiting playmode.");
            // RecreateFlags();
            foreach (var globalAsset in GlobalAssetBinding._assets)
            {
                globalAsset.hideFlags = HideFlags.DontSave;
            }
        }

        if (playModeState == PlayModeStateChange.ExitingEditMode)
        {
            foreach (var globalAsset in GlobalAssetBinding._assets)
            {
                globalAsset.hideFlags = HideFlags.None;
            }
        }
    }

    private static void OnInitialized()
    {
        // RescanCurrentHierarchy();
        EditorApplication.update -= OnInitialized;
    }

    private static void RescanCurrentHierarchy()
    {
        // var allHiddenAssets = GameObject.FindObjectsOfType<GlobalAsset>();
        // var allHiddenObjects = GameObject.FindObjectsOfType<GameObject>();
        // var scene = SceneManager.GetActiveScene();
        // foreach (var asset in GameObject.FindObjectsOfType<GlobalAsset>())
        // {
        // _assets.Add(asset.gameObject);
        // }
    }

    private static void EditorSceneManagerOnsceneOpened(Scene scene, OpenSceneMode mode)
    {
        RecreateFlags();
    }

    private static void RecreateFlags()
    {
        foreach (var globalAsset in GlobalAssetBinding._assets)
        {
            globalAsset.hideFlags = HideFlags.DontSave;
        }
    }


    //Callback handler:
    private static void OnHierarchyChanged()
    {
        // Debug.Log("#Engine# Hierarchy Changed");
        if (Selection.activeGameObject                              != null &&
            PrefabUtility.GetPrefabType(Selection.activeGameObject) == PrefabType.PrefabInstance)
        {
            var asset = Selection.activeGameObject.GetComponent<GlobalAssetBinding>();
            if (asset)
            {
                // GlobalAsset._assets.Add(Selection.activeGameObject);
                // Selection.activeGameObject.hideFlags = HideFlags.DontSave;
                // asset.name = asset.name + " | Global |";
                Debug.Log(Selection.activeGameObject);
            }
        }
    }

    #endregion

    private int                _index;
    private GlobalAssetBinding _assetBinding;
    private PrefabStage        _prefabStage;

    private void OnEnable()
    {
        _assetBinding = (GlobalAssetBinding) target;
        _index        = GlobalAssetBinding._assets.IndexOf(_assetBinding.gameObject);
        _prefabStage  = PrefabStageUtility.GetPrefabStage(_assetBinding.gameObject);
        if (_prefabStage)
        {
            Debug.Log("Enable Edit Gloabl Asset");
            _prefabStage.prefabContentsRoot.hideFlags = HideFlags.None;
        }

        // PrefabStage.prefabSaving += PrefabStageOnprefabSaving;
        // PrefabStage.prefabSaved += PrefabStageOnprefabSaving;
    }

    private void PrefabStageOnprefabSaving(GameObject go)
    {
        if (_prefabStage)
        {
            go           = _prefabStage.prefabContentsRoot;
            go.hideFlags = HideFlags.None;
            PrefabUtility.ApplyPrefabInstance(go, InteractionMode.AutomatedAction);
            go.hideFlags = HideFlags.DontSave;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Flags: " + _assetBinding.gameObject.hideFlags);
        // Event.current.mousePosition.
        if (GUILayout.Button("Save Prefab"))
        {
            if (_prefabStage)
            {
            }
            else
            {
                GameObject root = null;
                root           = PrefabUtility.GetOutermostPrefabInstanceRoot(_assetBinding.gameObject);
                root.hideFlags = HideFlags.None;
                PrefabUtility.ApplyPrefabInstance(root, InteractionMode.AutomatedAction);
                root.hideFlags = HideFlags.DontSave;
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    public void OnDisable()
    {
        if (_prefabStage)
        {
            Debug.Log("Distable Edit Gloabl Asset");
            _prefabStage.prefabContentsRoot.hideFlags = HideFlags.DontSave;
        }
        // PrefabStage.prefabSaving -= PrefabStageOnprefabSaving;
    }

    public void OnDestroy()
    {
        if (Application.isEditor)
        {
            if (((GlobalAssetBinding) target) == null)
            {
                // Debug.Log("Object Destroyed");
                // GlobalAsset._assets.RemoveAt(_index);
            }

            //Do your code here
        }
    }
}