using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGameManager : MonoBehaviour
{
    public TMP_Text buttonText;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("level", 1);

        int level = PlayerPrefs.GetInt("level");
        buttonText.text = "Level " + level;
    }

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }
}
