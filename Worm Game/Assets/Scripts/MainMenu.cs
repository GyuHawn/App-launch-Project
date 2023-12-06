using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;

    public GameObject uiMenu;
    public GameObject scoreMenu;
    public GameObject mapMenu;

    // 점수
    public List<int> scores = new List<int>();
    public TMP_Text scoreText;

    // 맵
    public GameObject[] maps;
    public int selectMap; // 맵 선택중
    public int currentMap; // 맵 선택
    public TMP_Text currentMapText;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        selectMap = 0;

        // 저장된 모든 최종 점수 불러오기
        string existingKeys = PlayerPrefs.GetString("ScoreKeys", "");
        if (!string.IsNullOrEmpty(existingKeys))
        {
            string[] keys = existingKeys.Split(',');
            foreach (var key in keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    int score = PlayerPrefs.GetInt(key);
                    scores.Add(score);
                }
            }
        }

        // 점수 정렬 (내림차순)
        scores.Sort((a, b) => b.CompareTo(a));

        // 상위 5개 점수만 유지
        if (scores.Count > 5)
        {
            scores.RemoveRange(5, scores.Count - 5);
        }

        // 점수 표시
        UpdateScoreText();
    }

    void Update()
    {
        if (currentMap == 0)
        {
            currentMapText.text = "현재 : 트레이닝";
        }
        else if (currentMap == 1)
        {
            currentMapText.text = "현재 : 연구실";
        }
    }

    public void UpdateScoreText()
    {
        scoreText.text = "";
        for (int i = 0; i < scores.Count; i++)
        {
            scoreText.text += scores[i].ToString() + "\n";
        }
    }

    public void GameStart()
    {
        audioManager.ButtonSound();

        if(currentMap == 0)
        {
            LodingController.LoadScene("Main");
        }
        else if (currentMap == 1)
        {
            LodingController.LoadScene("Main1");
        }
    }

    public void OnScoreMenu()
    {
        audioManager.ButtonSound();
        uiMenu.SetActive(false);
        scoreMenu.SetActive(true);
    }

    public void OffScoreMenu()
    {
        audioManager.ButtonSound();
        uiMenu.SetActive(true);
        scoreMenu.SetActive(false);
    }

    public void ResetScore()
    {
        audioManager.ButtonSound();
        // PlayerPrefs에서 키 삭제
        string existingKeys = PlayerPrefs.GetString("ScoreKeys", "");
        if (!string.IsNullOrEmpty(existingKeys))
        {
            string[] keys = existingKeys.Split(',');
            foreach (var key in keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    PlayerPrefs.DeleteKey(key);
                }
            }
        }

        // 리스트 비우기
        scores.Clear();

        // 텍스트 업데이트
        UpdateScoreText();
    }

    public void OnMapMenu()
    {
        audioManager.ButtonSound();
        mapMenu.SetActive(!mapMenu.activeSelf);
    }

    public void NextMap()
    {
        audioManager.ButtonSound();

        if (selectMap < maps.Length - 1)
        {
            maps[selectMap].SetActive(false);
            selectMap++;
            maps[selectMap].SetActive(true);
        }    
    }

    public void BeforeMap()
    {
        audioManager.ButtonSound();

        if (selectMap > 0)
        {
            maps[selectMap].SetActive(false);
            selectMap--;
            maps[selectMap].SetActive(true);
        }       
    }

    public void SelectMap()
    {
        currentMap = selectMap;
        mapMenu.SetActive(false);
    }
}
