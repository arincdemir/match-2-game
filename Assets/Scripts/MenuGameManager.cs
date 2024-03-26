using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGameManager : MonoBehaviour
{
    public TMP_Text buttonText;
    public int maxLevels = 10;

    private int level;

    void Start()
    {

        // set the level number to 1 if the game hasnt been started before
        if(!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 1);
        }

        // show the level count on the button
        level = PlayerPrefs.GetInt("level");
        buttonText.text = "Level " + level;
        if (level > maxLevels)
        {
            buttonText.text = "Finished!";
        }
    }

    public void startGame()
    {
        if (level > maxLevels) { return; }
        SceneManager.LoadScene(1);
    }
}
