using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MosterSpwan : MonoBehaviour
{
    public GameObject monsterPrifabs;
    public int monsterNum;

    public GameObject[] monsterSpwanPoint;

    void Start()
    {
        monsterNum = 1;
        Invoke("MonsterSpwan", 5f);
    }

    void MonsterSpwan()
    {
        if(monsterNum >0)
        {
            Vector3 spwanPosition = SpwanPoint();

            GameObject monster = Instantiate(monsterPrifabs, spwanPosition, Quaternion.identity);
            monsterNum--;

        }
    }

    public IEnumerator RespawnMonster()
    {

        yield return new WaitForSeconds(3f);
        monsterNum++;
        MonsterSpwan();
    }

    private Vector3 SpwanPoint()
    {
        int randomPoint = Random.Range(0, monsterSpwanPoint.Length);
        return monsterSpwanPoint[randomPoint].transform.position;
    }
}
