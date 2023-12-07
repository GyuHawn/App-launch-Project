using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmMainMenu; // 메인메뉴
    public AudioSource bgmMainGame; // 인게임
    public AudioSource bgmAttack; // 몬스터 공격
    public AudioSource bgmButton; // 버튼클릭
    public AudioSource bgmGetItme; // 아이템 획득
    public AudioSource bgmSpwanMonster; // 몬스터 소환

    private AudioSource bgmCount; // 게임시작 카운트 (보류)

    public Slider audioSlider;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("Volume", 1.0f);
        audioSlider.value = volume;

        if (SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "Loding")
        {
            bgmMainMenu.volume = volume;
            bgmMainMenu.Play();
        }
        if (SceneManager.GetActiveScene().name == "Main")
        {
            bgmMainGame.volume = volume;
            bgmMainGame.Play();

            if (bgmAttack != null)
            {
                bgmAttack.volume = 0.0f;
            }
            if (bgmButton != null)
            {
                bgmButton.volume = 0.0f;
            }
            if (bgmGetItme != null)
            {
                bgmGetItme.volume = 0.0f;
            }
            if (bgmSpwanMonster != null)
            {
                bgmSpwanMonster.volume = 0.0f;
            }
            
            StartCoroutine(UnmuteSounds());
        }
    }

    IEnumerator UnmuteSounds()
    {
        
        yield return new WaitForSeconds(1);
       
        if (bgmAttack != null)
        {
            bgmAttack.volume = 1.0f;
        }
        if (bgmButton != null)
        {
            bgmButton.volume = 1.0f;
        }
        if (bgmGetItme != null)
        {
            bgmGetItme.volume = 1.0f;
        }
        if (bgmSpwanMonster != null)
        {
            bgmSpwanMonster.volume = 1.0f;
        }
    }

    void Update()
    {
        if(bgmMainGame != null) 
        { 
            bgmMainGame.volume = audioSlider.value;
        }
        if(bgmMainMenu != null)
        {
            bgmMainMenu.volume = audioSlider.value;
        }
        PlayerPrefs.SetFloat("Volume", audioSlider.value);
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