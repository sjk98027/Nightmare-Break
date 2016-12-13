using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public enum SceneName
    {
        TitleScene,
        LoadingScene,
        SelectScene,
        CreateScene,
        WaitingScene,
        RoomScene,
        InGameScene,
    }

    public enum SelectLoadingData
    {
        CharacterList = 0,
    }

    private const float fadeValue = 0.4f;
    private float fadeTime;
    private int sceneIndex;
    private Image fadePanel;
    private LoadingSceneUI loadingScene;
    private SceneName currentScene;

    private bool[] loadingCheck;

    public bool[] LoadingCheck { get { return loadingCheck; } set { loadingCheck = value; } }
    public SceneName CurrentScene { get { return currentScene; } }

    private static SceneChanger instance = null;
    public static SceneChanger Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TitleScene")
        {
            currentScene = SceneName.TitleScene;
        }
        else if (scene.name == "LoadingScene")
        {
            currentScene = SceneName.LoadingScene;

            if (sceneIndex == (int)SceneName.SelectScene)
            {
                DataSender.Instance.RequestCharacterList();
            }

            StartCoroutine(CheckLoading());
        }
        else if (scene.name == "SelectScene")
        {
            if(currentScene == SceneName.LoadingScene)
            {
                StartCoroutine(FadeIn());
            }            

            currentScene = SceneName.SelectScene;
            UIManager.Instance.SetSelectUIManager();
        }
        else if (scene.name == "CreateScene")
        {
            currentScene = SceneName.CreateScene;

            UIManager.Instance.SetCreateUIManager();
        }
    }

    public void SceneChange(SceneName sceneName, bool needLoadingScene)
    {
        sceneIndex = (int)sceneName;

        if (needLoadingScene)
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            SceneManager.LoadScene((int)sceneName);
        }
    }

    private IEnumerator FadeOut()
    {
        {
            GameObject fadeCanvas = Instantiate(Resources.Load<GameObject>("UI/FadeCanvas"));
            fadePanel = fadeCanvas.transform.GetChild(0).GetComponent<Image>();
            fadeTime = Time.deltaTime;
            while (fadePanel.color.a < 1)
            {
                fadePanel.color += new Color(0, 0, 0, (float)fadeValue * fadeTime);
                yield return null;
            }
            fadeTime = 0;
            fadePanel = null;
            SceneManager.LoadScene((int)SceneName.LoadingScene);
        }
    }

    private IEnumerator FadeIn()
    {
        GameObject fadeCanvas = Instantiate(Resources.Load<GameObject>("UI/FadeCanvas"));
        fadePanel = fadeCanvas.transform.GetChild(0).GetComponent<Image>();
        fadeTime = Time.deltaTime;
        fadePanel.color = new Color(0, 0, 0, 1);
        while (fadePanel.color.a > 0)
        {
            fadePanel.color -= new Color(0, 0, 0, (float)fadeValue * fadeTime);
            yield return null;
        }
        fadePanel = null;
        Destroy(fadeCanvas);
        fadeTime = 0;
        
    }

    private IEnumerator CheckLoading()
    {
        if(sceneIndex == (int)SceneName.SelectScene)
        {
            loadingCheck = new bool[1];
        }

        bool checkComplete = false;

        while (!checkComplete)
        {
            yield return new WaitForSeconds(1.0f);

            for (int i = 0; i < loadingCheck.Length; i++)
            {
                if (loadingCheck[i])
                {
                    checkComplete = true;
                }
                else
                {
                    checkComplete = false;
                    break;
                }
            }
        }

        SceneManager.LoadScene(sceneIndex);
    }
}
