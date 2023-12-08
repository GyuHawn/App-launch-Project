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
    public Vector3 boxSize; // �÷��̾� ���� ����
    public Vector3 boxHitSize; // �÷��̾� ���� ����

    private Vector3 playerPosition; // �÷��̾� ��ġ ����
    private bool isPlayerCheck; // �÷��̾� �����ߴ��� Ȯ��
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
            if (!isPlayerCheck) // �÷��̾� ó�� �������� �� ��ġ�� ����
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
