using UnityEngine;
namespace ScoreMilk{

public class Singleton<T> : MonoBehaviour where T : Component
{
	protected static T _instance;

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<T>();
				if (_instance == null)
				{
					GameObject obj = new GameObject();
					_instance = obj.AddComponent<T>();
				}
			}
			return _instance;
		}
	}
	private void Awake() {
		
		if (!Application.isPlaying)
		{
			return;
		}

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this as T;
			StartUp();
        }
		DontDestroyOnLoad(this.gameObject);
	}

	protected virtual void StartUp(){

	}
}

}