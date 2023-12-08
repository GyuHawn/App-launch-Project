using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private MosterSpwan mosterSpwan;
    private AudioManager audioManager;

    public float spd;

    public GameObject player;

    public GameObject pos;
    public Vector3 boxSize; // 플레이어 감지 범위
    public Vector3 boxHitSize; // 플레이어 공격 범위

    private Vector3 playerPosition; // 플레이어 위치 저장
    private bool isPlayerCheck; // 플레이어 감지했는지 확인
    public GameObject enemy;
    public GameObject enemyPrifab;

    private bool isDead = false;

    private Animator anim;

    void Start()
    {
        mosterSpwan = GameObject.Find("Manager").GetComponent<MosterSpwan>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        player = GameObject.FindWithTag("Player");
        enemy = GameObject.Find("Enermy");

        anim = GetComponent<Animator>();

        spd = 1.5f;
    }

    void Update()
    {
        Die();

        if (Physics.CheckBox(pos.transform.position, boxSize / 2, Quaternion.identity, LayerMask.GetMask("Player")))
        {
            Vector3 playerCheck = (player.transform.position - transform.position).normalized;
            transform.position += playerCheck * spd * Time.deltaTime;

            Quaternion lookRotation = Quaternion.LookRotation(playerCheck);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * spd);
        }

        if (Physics.CheckBox(pos.transform.position, boxHitSize / 2, Quaternion.identity, LayerMask.GetMask("Player")))
        {
            if (!isPlayerCheck) // 플레이어 처음 감지했을 때 위치를 저장
            {
                playerPosition = player.transform.position;
                spd = 5f;
                isPlayerCheck = true;
            }
        }

        if (isPlayerCheck)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            transform.position += direction * spd * Time.deltaTime;

            if (Vector3.Distance(transform.position, playerPosition) < 3f)
            {
                spd = 0f;
            }
        }
    }

    public void SpdUp(float amount)
    {
        spd += amount;
    }

    void Die()
    {
        if (spd != 0)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            if (!isDead) 
            {
                isDead = true;
                enemy.GetComponent<MeshRenderer>().enabled = true;
                anim.SetBool("Walk", false);
                anim.SetTrigger("Attack");

                mosterSpwan.monsters.Remove(gameObject);
                StartCoroutine(DestroyMonster());
            }
        }
    }
    
    IEnumerator DestroyMonster()
    {
        yield return new WaitForSeconds(2f);
        audioManager.MonsterAttackSound();
        enemy.GetComponent<SphereCollider>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
