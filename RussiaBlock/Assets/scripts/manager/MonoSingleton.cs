using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 继承自Mono的模板类单例
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	protected static T _instance= null;

	public static T Instance
	{
		get
		{ 
			if (_instance ==null)
			{             
				_instance = GameObject.FindObjectOfType<T>();
				if (1 < GameObject.FindObjectsOfType<T> ().Length) 
				{
					Debug.Log ("该类型存在不止一个");
					return _instance;
				}

				if (_instance == null) 
				{//游戏中不存在这个单例类 重新生成一个物体 
					string instanceName = typeof(T).Name;
					GameObject instanceGo = GameObject.Find (instanceName);
					if (instanceGo == null)
						instanceGo = new GameObject (instanceName);
					_instance = instanceGo.AddComponent <T>();
					DontDestroyOnLoad (instanceGo);
				}
			}
			return _instance;
		}
	}
}
