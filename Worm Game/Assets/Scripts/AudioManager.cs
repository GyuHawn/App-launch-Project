using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    /*
     
      private AudioManager audioManager;
      audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

     */
    public AudioSource bgmMainMenu; // ���θ޴�
    public AudioSource bgmMainGame; // �ΰ���
    public AudioSource bgmAttack; // ���� ����
    public AudioSource bgmButton; // ��ưŬ��
    public AudioSource bgmGetItme; // ������ ȹ��
    public AudioSource bgmSpwanMonster; // ���� ��ȯ

    private AudioSource bgmCount; // ���ӽ��� ī��Ʈ (����)

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            bgmMainMenu.Play();
        }
        if (SceneManager.GetActiveScene().name == "Main")
        {
            bgmMainGame.Play();
        }
    }

    public void MonsterAttackSound()
    {
        bgmAttack.Play();
    }
    public void ButtonSound()
    {
        bgmButton.Play();
    }
    public void GetItemSound()
    {
        bgmGetItme.Play();
    }

    public void MonsterSpwanSound()
    {
        bgmSpwanMonster.Play();
    }



}