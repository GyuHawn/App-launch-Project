using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SettingScript : MonoBehaviour
{
    public float timer; // 게임 진행 시간
    public TMP_Text timeText;

    public int min;
    public int sec;

    private float spdUpTime = 30f; // 속도 증가 간격
    private float spdAmount = 0.1f; // 속도 증가량
    private float spdUpTimeReset = 0f; // 속도 증가 후 경과 시간

    // 플레이어 스킬 UI
    public GameObject skillOn;
    public GameObject skillOff;

    public bool gaming;

    void Start()
    {
        timer = 0f;
        gaming = true;
    }

    
    void Update()
    {
        if (gaming)
        {
            timer += Time.deltaTime;
            spdUpTimeReset += Time.deltaTime;

            // 일정 시간마다 속도 증가
            if (spdUpTimeReset >= spdUpTime)
            {
                MonsterSpdUP();
                spdUpTimeReset = 0f;
            }
        }

        min = (int)(timer / 60);
        sec = (int)(timer % 60);
        timeText.text = string.Format("{0:00} : {1:00}", min, sec);
    }

    private void MonsterSpdUP()
    {
        MonsterMovement[] monsters = FindObjectsOfType<MonsterMovement>();
        foreach (MonsterMovement monster in monsters)
        {
            monster.SpdUp(spdAmount);
        }
    }

}
