using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
	private const float fadeValue = 0.4f;
	[SerializeField]private float fadeTime;
	[SerializeField]private int sceneIndex;
	[SerializeField]private Image fadePanel;
	[SerializeField]private LoadingSceneUI loadingScene;
   
		public enum SceneName{
		TitleScene,
	//	waitingScene,
		InGameScene,
		LoadingScene
	}
    private SceneName currentScene;
    public SceneName CurrentScene { get { return currentScene; } }


	private static SceneChanger instance = null;
	public static SceneChanger Instance {
		get{
			return instance;
		}
	}
		

	void Awake()
	{
		if (instance) {
			Destroy (gameObject);

			return;
		}
		instance = this;
		DontDestroyOnLoad (gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }

    public void SceneChange(SceneName sceneName)
	{
		sceneIndex = (int)sceneName;
		StartCoroutine (FadeOut ());
	}

	IEnumerator FadeOut(){
		{
			GameObject fadeCanvas =  Instantiate (Resources.Load<GameObject> ("UI/FadeCanvas"));
			fadePanel = fadeCanvas.transform.GetChild (0).GetComponent<Image>();
			fadeTime = Time.deltaTime; 
			while(fadePanel.color.a < 1)
			{
				fadePanel.color += new Color(0, 0, 0, (float)fadeValue * fadeTime);
				yield return null;
			}
			fadeTime = 0;
			fadePanel = null;
			SceneManager.LoadScene ((int)SceneName.LoadingScene);
		}
	}

	IEnumerator FadeIn()
	{
		GameObject fadeCanvas = Instantiate (Resources.Load<GameObject> ("UI/FadeCanvas"));
		fadePanel = fadeCanvas.transform.GetChild (0).GetComponent<Image> ();
		fadeTime = Time.deltaTime; 
		fadePanel.color = new Color (0, 0, 0, 1);
		while (fadePanel.color.a > 0) {
			fadePanel.color -= new Color (0, 0, 0, (float)fadeValue * fadeTime);
			yield return null;
		}
		fadePanel = null;
		Destroy (fadeCanvas);
		fadeTime = 0;
		if (SceneManager.GetActiveScene ().name == "LoadingScene") {
			if (!loadingScene) {
				loadingScene = GameObject.Find ("LoadingSceneManager").GetComponent<LoadingSceneUI> ();
			}
			AsyncOperation asyncScene = SceneManager.LoadSceneAsync (sceneIndex);
		
				while(!asyncScene.isDone)
				{
					loadingScene.LoadingProcess (asyncScene.progress);
					yield return null;
				}
		}
	}
}
