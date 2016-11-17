using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneFadeInOut : MonoBehaviour {

    public Image panel;

    void Awake()
    {

    }

    IEnumerator FadeInOut(int i)
    {
        if(i == 0)
        {
            while(panel.color.a > i)
            {
                panel.color -= new Color(0, 0, 0, (float)0.4 * Time.deltaTime);
                yield return null;
            }
            
        } else
        {
            while(panel.color.a < i)
            {
                panel.color += new Color(0, 0, 0, (float)0.4 * Time.deltaTime);
                yield return null;
            }
        }
       // SceneManager.LoadScene("");
    }
}
