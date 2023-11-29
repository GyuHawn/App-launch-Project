using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    private FixedJoystick joystick;

    private CameraScript cameraMovement;
    private ItemScript itemScript;
    private SettingScript settingScript;

    // �̵�
    public float moveSpd;
    public float spdUP; // ���� �ӵ�
    public float tailRotateSpd;

    public Button roll;
    public float rollTime;
    private bool isRoll = true;
    private TMP_Text rollText;

    public float hAxis;
    private float vAxis;

    private Vector3 moveVec;

    // �̴ϸ�
    public GameObject mainCamera;

    // ����
    public List<GameObject> tails = new List<GameObject>();
    public GameObject tailObj;
    public GameObject tailPoint;

    // UI �ؽ�Ʈ
    public TMP_Text spdText;
    public TMP_Text tailText;

    private Rigidbody rigid;
    private Animator anim;

    void Awake()
    {
        rigid = GetComponentInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        itemScript = GameObject.Find("Manager").GetComponent<ItemScript>();
        settingScript = GameObject.Find("Manager").GetComponent<SettingScript>();
        cameraMovement = GameObject.Find("Main Camera").GetComponent<CameraScript>();

        mainCamera = GameObject.Find("Main Camera");
        roll = GameObject.Find("RollButton").GetComponent<Button>();
        rollText = GameObject.Find("RollTime").GetComponent<TMP_Text>();
        roll.onClick.AddListener(Roll);

        tails.Add(gameObject);

        spdUP = 0.1f;
        moveSpd = 2f;
        tailRotateSpd = 200f;
        rollTime = 0f;
    }

    void Update()
    {
        Debug.Log(rollTime);

        GetInput();
        UseAnimator();
        Invoke("Move", 4); // �̵� �غ� �ִϸ��̼� �� ���
        Rotate();
        CameraMove();
        UpdateTails();

        if (!isRoll)
        {
            if (rollTime > 0)
            {
                rollTime -= Time.deltaTime;
            }
            else
            {
                isRoll = true;
            }
        }

        spdText.text = "�ӵ� : " + moveSpd.ToString("F1");
        tailText.text = "���� : " + (tails.Count - 1).ToString();
        rollText.text = ((int)rollTime).ToString();
    }

    private void GetInput()
    {
        hAxis = joystick.Horizontal;
        //hAxis = Input.GetAxisRaw("Horizontal");
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, moveRotate, tailRotateSpd * Time.deltaTime);
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
            Roll();
        }
    }

    public void Roll()
    {
        if (isRoll)
        {
            isRoll = false;
            rollTime = 15f;
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


    public void OnTailCollision(GameObject tail)
    {
        int tailIndex = tails.IndexOf(tail); // �⵹�� ����

        // ���� Ȯ��
        if (tailIndex >= 0)
        {
            // ���� ��ü ���� ����
            for (int i = tails.Count - 1; i >= tailIndex; i--)
            {
                Destroy(tails[i]);
                tails.RemoveAt(i);
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
            moveSpd += spdUP; // ������ ȹ��� �̵� �ӵ� ����
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enermy"))
        {
            if (settingScript.gaming)
            {
                int tailIndex = tails.IndexOf(other.gameObject); // IndexOf - ����Ʈ, �迭���� ������ ����� �ε����� �˻��ϴ� �޼���

                if (tailIndex >= 0)
                {
                    for (int i = tails.Count - 1; i >= tailIndex; i--)
                    {
                        Destroy(tails[i]);
                        tails.RemoveAt(i);
                    }
                }

                Destroy(gameObject);
                settingScript.gaming = false;
            }
        }
    }
}
