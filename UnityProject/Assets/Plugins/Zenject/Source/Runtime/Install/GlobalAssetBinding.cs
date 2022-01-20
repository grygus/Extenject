

// #define GLOBAL_ASSET_DEBUG

using System;
using System.Collections.Generic;
using ModestTree;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using Zenject;
using Assert = UnityEngine.Assertions.Assert;

#if UNITY_EDITOR
[ExecuteInEditMode, InitializeOnLoad]
// [ExecuteAlways, InitializeOnLoad]
#endif
public class GlobalAssetBinding : MonoBehaviour, ISerializationCallbackReceiver
{
    public static List<GameObject> _assets = new List<GameObject>();

    private static void Log(string message)
    {
        #if GLOBAL_ASSET_DEBUG
        Debug.Log(message);
        #endif
    }
    

    public static void RemoveAssets(GameObject assetObject)
    {
        _assets.Remove(assetObject);
    }

    public static void AddAsset(GameObject assetObject)
    {
        _assets.Add(assetObject);
    }

    static GlobalAssetBinding()
    {
        // Debug.Log("#Engine# Initialized");
    }

    public static List<GameObject> GetActiveObjects()
    {
        return _assets;
    }

    [NonSerialized] private bool _editorAwaked = false;

    [Tooltip("The component to add to the Zenject container")] [SerializeField]
    Component[] _components = null;

    [Tooltip(
        "Note: This value is optional and can be ignored in most cases.  This can be useful to differentiate multiple bindings of the same type.  For example, if you have multiple cameras in your scene, you can 'name' them by giving each one a different identifier.  For your main camera you might call it 'Main' then any class can refer to it by using an attribute like [Inject(Id = 'Main')]")]
    [SerializeField]
    string _identifier = string.Empty;


    [Tooltip(
        "This value is used to determine how to bind this component.  When set to 'Self' is equivalent to calling Container.FromInstance inside an installer. When set to 'AllInterfaces' this is equivalent to calling 'Container.BindInterfaces<MyMonoBehaviour>().ToInstance', and similarly for InterfacesAndSelf")]
    [SerializeField]
    ZenjectBinding.BindTypes _bindType = ZenjectBinding.BindTypes.Self;

    public Context Context { get; set; }

    public Component[] Components
    {
        get { return _components; }
    }

    public string Identifier
    {
        get { return _identifier; }
    }

    public ZenjectBinding.BindTypes BindType
    {
        get { return _bindType; }
    }


#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void StartInitializeOnLoadMethod()
    {
        PrefabUtility.prefabInstanceUpdated += instance => { };
    }
#endif


    private void Awake()
    {
        Log("Global Asset Awake");
        if (Application.isPlaying)
        {
            Assert.IsFalse(_assets.Contains(gameObject));
            AddAsset(gameObject);
        }
    }

    private void Start()
    {
        Log("Global Asset Start");
    }


    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

        if (!_editorAwaked && !Application.isPlaying && !PrefabStageUtility.GetPrefabStage(gameObject))
        {   
            Assert.IsFalse(_assets.Contains(gameObject));
            gameObject.hideFlags = HideFlags.DontSave;
            AddAsset(gameObject);
            _editorAwaked = true;
        }
#endif
    }

    private void OnDestroy()
    {
        RemoveAssets(gameObject);
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
    }
    
}