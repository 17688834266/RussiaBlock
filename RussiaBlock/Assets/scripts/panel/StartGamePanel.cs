using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGamePanel : MonoBehaviour {

    Button startBt;
	// Use this for initialization
	void Start () {
        Init();
	}
	void Init()
    {
        GameManager.Instacne.gameState = GameState.UnStarte;
        startBt = transform.Find("startBt").GetComponent<Button>();
        startBt.onClick.AddListener(() => SceneManager.LoadScene(1));
    }


}
