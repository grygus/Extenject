using System.Collections;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using Zenject;

public class CameraSerivceTest : MonoBehaviour,IInitializable
{
    [Inject] private ICameraService _service;
    [Inject] private DiContainer _container;
    // Start is called before the first frame update
    void Start()
    {
        Assert.That(_service != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        Assert.That(_service != null);
    }
}
