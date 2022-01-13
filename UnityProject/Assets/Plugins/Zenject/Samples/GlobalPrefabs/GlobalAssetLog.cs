using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GlobalAssetLog : MonoBehaviour
{
    // Start is called before the first frame update
    public int GlobalAssetCount;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GlobalAssetCount = GlobalAssetBinding.GetActiveObjects().Count;
    }
}
