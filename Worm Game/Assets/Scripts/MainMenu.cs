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

    public List<int> scores = new List<int>();

    public TMP_Text scoreText;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

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
        LodingController.LoadScene("Main");
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
}
