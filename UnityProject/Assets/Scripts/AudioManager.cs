using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmMainMenu; // ���θ޴�
    public AudioSource bgmMainGame; // �ΰ���
    public AudioSource bgmAttack; // ���� ����
    public AudioSource bgmButton; // ��ưŬ��
    public AudioSource bgmGetItme; // ������ ȹ��
    public AudioSource bgmSpwanMonster; // ���� ��ȯ

    private AudioSource bgmCount; // ���ӽ��� ī��Ʈ (����)

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