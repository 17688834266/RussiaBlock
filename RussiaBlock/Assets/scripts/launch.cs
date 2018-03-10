using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launch : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GameManager.Instacne.Init();
        QPoolSingleton.Instance.Init();
        BlockManager.Instance = gameObject.AddComponent<BlockManager>();
        BlockManager.Instance.Init();
        gameObject.AddComponent<CreatBlock>();
        gameObject.AddComponent<MoveContoler>();
    }
}
