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
    public AudioSource bgmMainMenu; // 메인메뉴
    public AudioSource bgmMainGame; // 인게임
    public AudioSource bgmAttack; // 몬스터 공격
    public AudioSource bgmButton; // 버튼클릭
    public AudioSource bgmGetItme; // 아이템 획득
    public AudioSource bgmSpwanMonster; // 몬스터 소환

    private AudioSource bgmCount; // 게임시작 카운트 (보류)

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