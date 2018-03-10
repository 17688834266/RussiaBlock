using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class root : MonoBehaviour {
    string gamestarPath = "GameStartPl";
	// Use this for initialization
	void Start () {

        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(gamestarPath));
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.AddComponent<StartGamePanel>();
	}
	
	
}
