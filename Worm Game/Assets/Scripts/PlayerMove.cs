using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.SceneView;

public class PlayerMove : MonoBehaviour
{
    private CameraMovement cameraMovement;
    private ItemScript itemScript;

    public float moveSpd;
    public float spdUP; // ���� �ӵ�
    public float rotateSpd;

    private float hAxis;
    private float vAxis;

    private Vector3 moveVec;

    public GameObject mainCamera;

    private List<GameObject> tails = new List<GameObject>();
    public GameObject tailObj;
    public GameObject tailPoint;

    private Rigidbody rigid;
    private Animator anim;

    void Awake()
    {
        rigid = GetComponentInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        itemScript = GameObject.Find("Manager").GetComponent<ItemScript>();
        cameraMovement = GameObject.Find("Main Camera").GetComponent<CameraMovement>();

        mainCamera = GameObject.Find("Main Camera");

        tails.Add(gameObject);

        spdUP = 0.01f;
        moveSpd = 2f;
        rotateSpd = 150f;
    }

    void Update()
    {
        GetInput();
        UseAnimator();
        Invoke("Move", 4); // �̵� �غ� �ִϸ��̼� �� ���
        Rotate();
        CameraMove();
        UpdateTails();
    }

    private void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = 1;
        //vAxis = Input.GetAxisRaw("Vertical");

        // �ڷ� �̵� ����
        if (vAxis < 0)
        {
            vAxis = 0;
        }

        Vector3 vCam = mainCamera.transform.forward;
        Vector3 hCam = mainCamera.transform.right;

        vCam.y = 0f;
        hCam.y = 0f;

        vCam.Normalize();
        hCam.Normalize();

        moveVec = (vCam * vAxis + hCam * hAxis).normalized;
    }

    private void Move()
    {
        transform.position += moveVec * moveSpd * Time.deltaTime;
    }

    private void Rotate()
    {
        if (moveVec != Vector3.zero && hAxis != 0)
        {
            Quaternion moveRotate = Quaternion.LookRotation(new Vector3(moveVec.x, 0, moveVec.z), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, moveRotate, rotateSpd * Time.deltaTime);
        }
    }

    void CameraMove()
    {
        mainCamera.transform.position = transform.position + transform.rotation * cameraMovement.offset;

        mainCamera.transform.LookAt(transform);
        Vector3 curAngles = mainCamera.transform.rotation.eulerAngles;
        mainCamera.transform.rotation = Quaternion.Euler(15, curAngles.y, curAngles.z);
    }

    private void UseAnimator()
    {
        // �ȱ�
        if (vAxis == 1)
        {
            anim.SetBool("Walk_Anim", true);
        }
        /*else if (hAxis == 0)
        {
            anim.SetBool("Walk_Anim", false);
        }*/

        // ���� (�ִϸ��̼� �߰� �ؾ���)
        /*if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!anim.GetBool("Open_Anim"))
            {
                anim.SetBool("Open_Anim", true);
            }
            else
            {
                anim.SetBool("Open_Anim", false);
            }
        }*/

        // ������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Roll_Anim", true);
            moveSpd += 2f;

            StartCoroutine(StopRoll());
        }
    }

    IEnumerator StopRoll()
    {
        yield return new WaitForSeconds(5f);

        anim.SetBool("Roll_Anim", false);
        moveSpd -= 2f;
    }

    public void StartPlusTail()
    {
        GameObject tail = Instantiate(tailObj, tailPoint.transform.position, Quaternion.identity);
        tails.Add(tail);
    }

    private void UpdateTails()
    {
        if (tails.Count > 1)
        {
            for (int i = 1; i < tails.Count; i++)
            {
                GameObject tailPoint = tails[i - 1].transform.Find("TailPoint").gameObject;
                float followSpd = 3f * Time.deltaTime; // ���� �ӵ�
                tails[i].transform.position = Vector3.Lerp(tails[i].transform.position, tailPoint.transform.position, followSpd);
                tails[i].transform.rotation = Quaternion.Lerp(tails[i].transform.rotation, tails[i - 1].transform.rotation, followSpd);
            }
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
            itemScript.OnItemDestroyed(collision.gameObject);

            StartPlusTail();
            moveSpd += spdUP; // �������� ȹ���� ������ �̵� �ӵ� ����
        }
    }
}
