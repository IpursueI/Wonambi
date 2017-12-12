using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
{
	private static T _instance;
	private static bool applicationIsQutting = false;
	private static object _lock = new object();

	public static T Instance
	{
		get {
			if (applicationIsQutting) {
				return null;
			}
			lock(_lock)
			{
				if(_instance == null) {
					_instance = (T)FindObjectOfType(typeof(T));
					if(FindObjectsOfType(typeof(T)).Length > 1) {
						return _instance;
					}
					if (_instance == null) {
						//创建一个新的gameobject来放置此单例
						GameObject singleton = new GameObject();
						_instance = singleton.AddComponent<T>();
						singleton.name = "Singleton_" + typeof(T).ToString();
						DontDestroyOnLoad(singleton);
					}
					else {
						Debug.LogError("[Singleton] Using instance already created: " + _instance.gameObject.name);
					}
				}
				return _instance;
			}
		}

	}
	public void OnDestroy()
	{
		applicationIsQutting = true;
	}
}