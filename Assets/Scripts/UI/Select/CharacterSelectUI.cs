using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour {
   
    private const int maxImage = 3;
    [SerializeField]
    private Button createButton;
    [SerializeField]
    private Button deleteButton;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private GameObject[] selectImage;
    [SerializeField]
    private bool[] selectActive;
   

    void Start()
    {
        createButton = GameObject.Find("CreateButton").GetComponent<Button>();
        deleteButton = GameObject.Find("DeleteButton").GetComponent<Button>();
        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        selectImage = new GameObject[maxImage];
        for(int i = 0; i < selectImage.Length; i++)
        {
            selectImage[i] = GameObject.Find("SelectEdge" + (i+1));
            selectImage[i].SetActive(false);
        }
    }

    public void ImageActive(int Imageindex)
    {
        selectActive[Imageindex] = true;
        selectImage[Imageindex].SetActive(true);
    }

    

}
