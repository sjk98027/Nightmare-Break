using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboSystem : MonoBehaviour {

    [SerializeField]
    private const float comboCheckTime = 1.5f;
    private Animator comboAnim;
    private Text comboText;
    private int currentComboCount;
    private CharacterManager characterManager;
    public static ComboSystem instance = null;

    void Start()
    {
        instance = this.gameObject.GetComponent<ComboSystem>();
        comboAnim = GameObject.Find("ComBoText").GetComponent<Animator>();
        comboText = comboAnim.transform.GetChild(0).GetComponent<Text>();
        characterManager = GameObject.Find("Player").GetComponent<CharacterManager>();
    }

    public void comboProcess(int count)
    {
        if(!comboAnim.gameObject.activeSelf)
        {
            comboAnim.gameObject.SetActive(true);
        }
        currentComboCount = count;
        comboText.text = count.ToString();
        StartCoroutine(ComboCheck());
    }

    IEnumerator ComboCheck()
    {
        float time = Time.time;
        while(Time.time - time < comboCheckTime)
        {
            if(currentComboCount == characterManager.ComboCount)
            {
                print("콤보카운트 체크중");
            } else
            {
                currentComboCount = 0;
                comboAnim.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
    }
}
