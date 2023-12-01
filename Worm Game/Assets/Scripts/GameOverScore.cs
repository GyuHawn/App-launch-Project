using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverScore : MonoBehaviour
{
    private PlayerMove playerMove;
    private SettingScript settingScript;
    private AudioManager audioManager;

    public int min;
    public int sec;

    public GameObject finalScore;
    public TMP_Text scores;
    
    void Start()
    {
        playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();
        settingScript = GameObject.Find("Manager").GetComponent<SettingScript>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    
    void Update()
    {
        scores.text = string.Format("{0:00}분 {1:00}초 = ", settingScript.min, settingScript.sec) + (int)playerMove.gameTime + "점" + "\n\n" + playerMove.maxTailCount + " * 100 = " + playerMove.TailScore + "점" + "\n\n" + playerMove.itemCount + " * 50 = " + playerMove.itemScore + "점" + "\n\n" + playerMove.finalScore + "점";
    }

    public void OffScore()
    {
        audioManager.ButtonSound();
        finalScore.SetActive(false);
        settingScript.timer = 0;
        SceneManager.LoadScene("MainMenu");
    }
}
