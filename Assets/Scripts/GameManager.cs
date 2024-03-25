using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public TextAsset[] levelFiles;
    public GameObject boardObject;
    Board boardComponent;

    public Image boxImage;
    public Image stoneImage;
    public Image vaseImage;
    public TMP_Text boxText;
    public TMP_Text stoneText;
    public TMP_Text vaseText;
    public Image boxCheck;
    public Image stoneCheck;
    public Image vaseCheck;
    public TMP_Text movesText;

    public GameObject losePanel;
    public Image loseImage;
    public GameObject winPanel;
    public Image winStar;
    public GameObject winRibbon;

    int curLevelNo;
    public Level curLevel = new Level();
    int boxCount;
    int stoneCount;
    int vaseCount;
    int moves;

    bool playing = true;
    // Start is called before the first frame update
    void Start()
    {
        curLevelNo = PlayerPrefs.GetInt("level");
        TextAsset levelFile = levelFiles[curLevelNo - 1];
        curLevel = JsonUtility.FromJson<Level>(levelFile.text);
        boardComponent = boardObject.GetComponent<Board>();
        boardComponent.Initialize(curLevel);

        boxCheck.enabled = false;
        stoneCheck.enabled = false;
        vaseCheck.enabled = false;

        int[] obstacleCounts = boardComponent.GetObstacleCounts();
        boxCount = obstacleCounts[0];
        stoneCount = obstacleCounts[1];
        vaseCount = obstacleCounts[2];

        moves = boardComponent.getMoveCount();
        boxText.text = boxCount.ToString();
        stoneText.text = stoneCount.ToString();
        vaseText.text = vaseCount.ToString();
        movesText.text = moves.ToString();

        if (boxCount == 0)
        {
            boxImage.gameObject.SetActive(false);
        }
        if (stoneCount == 0)
        {
            stoneImage.gameObject.SetActive(false);
        }
        if (vaseCount == 0)
        {
            vaseImage.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playing) { return; }

        int[] obstacleCounts = boardComponent.GetObstacleCounts();
        if (boxCount != obstacleCounts[0])
        {
            boxCount = obstacleCounts[0];
            boxText.text = boxCount.ToString();
            if (boxCount == 0)
            {
                boxCheck.enabled = true;
                boxText.enabled = false;
            }
        }
        if (stoneCount != obstacleCounts[1])
        {
            stoneCount = obstacleCounts[1];
            stoneText.text = stoneCount.ToString();
            if (stoneCount == 0)
            {
                stoneCheck.enabled = true;
                stoneText.enabled = false;
            }
        }
        if (vaseCount != obstacleCounts[2])
        {
            vaseCount = obstacleCounts[2];
            vaseText.text = vaseCount.ToString();
            if (vaseCount == 0)
            {
                vaseCheck.enabled = true;
                vaseText.enabled = false;
            }
        } 

        if (boxCount + stoneCount + vaseCount == 0)
        {
            Debug.Log("Won!");
            playing = false;
            Win();
        }

        if(boardComponent.getMoveCount() != moves)
        {
            moves = boardComponent.getMoveCount();
            movesText.text = moves.ToString();
            if (moves == 0)
            {
                playing = false;
                Debug.Log("Lost");
                Invoke("Lose", 0.4f);
            }
        }
    }


    public void Lose()
    {
        loseImage.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        losePanel.SetActive(true);
        loseImage.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        
    }

    public void Win()
    {
        winPanel.SetActive(true);
        winRibbon.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        winRibbon.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        winStar.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        winStar.transform.DOScale(new Vector3(1, 1, 1), 1f).SetLoops(4, LoopType.Yoyo);
        winStar.transform.DORotate(new Vector3(0, 0, 45), 1f).SetLoops(4, LoopType.Yoyo);
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        Invoke("returnToMainMenu", 2);
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Replay() {
        SceneManager.LoadScene(1);
    }
}
