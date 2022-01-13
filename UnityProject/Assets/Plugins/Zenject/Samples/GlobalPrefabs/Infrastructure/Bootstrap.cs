using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(SceneContext))]
public class Bootstrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        
        
        ProjectContext.Instance.EnsureIsInitialized();
        GetComponent<SceneContext>().Run();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    
}
