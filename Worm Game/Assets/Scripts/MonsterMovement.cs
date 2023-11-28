using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private MosterSpwan mosterSpwan;

    public float spd;

    public GameObject player;

    public GameObject pos;
    public Vector3 boxSize; // 플레이어 감지 범위
    public Vector3 boxHitSize; // 플레이어 공격 범위

    private Vector3 playerPosition; // 플레이어 위치 저장
    private bool isPlayerCheck; // 플레이어 감지했는지 확인

    private Animator anim;

    void Start()
    {
        mosterSpwan = GameObject.Find("Manager").GetComponent<MosterSpwan>();
        player = GameObject.FindWithTag("Player");

        anim = GetComponent<Animator>();

        spd = 2f;
    }

    void Update()
    {
        Anime();

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

    void Anime()
    {
        if(spd != 0)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetTrigger("Attack");
        }
    }
    /* private void OnCollisionEnter(Collision collision)
     {
         if (collision.gameObject.CompareTag("Player"))
         {
             mosterSpwan.StartCoroutine(mosterSpwan.RespawnMonster());
             Destroy(gameObject);
         }
     }*/

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.transform.position, boxHitSize);
    }*/
}
