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
}
