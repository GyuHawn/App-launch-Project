using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private MosterSpwan mosterSpwan;

    public float spd;

    public GameObject player;

    public GameObject pos;
    public Vector3 boxSize; // �÷��̾� ���� ����
    public Vector3 boxHitSize; // �÷��̾� ���� ����

    private Vector3 playerPosition; // �÷��̾� ��ġ ����
    private bool isPlayerCheck; // �÷��̾� �����ߴ��� Ȯ��

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
