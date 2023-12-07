using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 현재 이동은 가능 벽에 막혔을때 다른 길 찾도록

public class NavMonsterMovement : MonoBehaviour
{
    private MosterSpwan monsterSpawn;
    private AudioManager audioManager;
    private NavMeshAgent navMeshAgent;

    public float spd;
    public float chaseSpeed = 5f;

    public GameObject player;
    public GameObject pos;
    public Vector3 boxHitSize;

    private Vector3 playerPosition;
    private bool isPlayerCheck;
    public GameObject enemy;
    public GameObject enemyPrefab;

    private bool isDead = false;

    private Animator anim;

    public NavMeshData navMeshData;

    void Start()
    {
        monsterSpawn = GameObject.Find("Manager").GetComponent<MosterSpwan>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        player = GameObject.FindWithTag("Player");
        enemy = GameObject.Find("Enermy");

        anim = GetComponent<Animator>();

        spd = 1.5f;

        navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        navMeshAgent.speed = spd;
        navMeshAgent.enabled = true;
    }

    void Update()
    {
        Die();

        if (Physics.CheckBox(pos.transform.position, boxHitSize / 2, Quaternion.identity, LayerMask.GetMask("Player")))
        {
            if (!isPlayerCheck)
            {
                isPlayerCheck = true;
                SetPlayerDestination();
            }
        }

        if (isPlayerCheck && navMeshAgent.remainingDistance < 3f)
        {
            navMeshAgent.speed = 0f;
        }
        else
        {
            navMeshAgent.speed = spd;
            SetPlayerDestination();
        }
    }

    void SetPlayerDestination()
    {
        if (player != null)
        {
            playerPosition = player.transform.position;
            navMeshAgent.SetDestination(playerPosition);
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

                monsterSpawn.monsters.Remove(gameObject);
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
