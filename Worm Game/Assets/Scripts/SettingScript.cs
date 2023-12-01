using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SettingScript : MonoBehaviour
{
    public float timer; // ���� ���� �ð�
    public TMP_Text timeText;

    public int min;
    public int sec;

    private float spdUpTime = 30f; // �ӵ� ���� ����
    private float spdAmount = 0.1f; // �ӵ� ������
    private float spdUpTimeReset = 0f; // �ӵ� ���� �� ��� �ð�

    // �÷��̾� ��ų UI
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

            // ���� �ð����� �ӵ� ����
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
