using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerMove : MonoBehaviour
{
    private FixedJoystick joystick;

    private CameraScript cameraMovement;
    private ItemScript itemScript;
    private SettingScript settingScript;
    private GameOverScore gameOverScore;
    private AudioManager audioManager;
    private PortalScript portalScript;

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

    // ���
    public int maxTailCount; // �ִ벿�� ����
    public int TailScore; // �ִ벿�� ���� * 50 
    public int itemCount; // ������ * 100
    public int itemScore;
    public float gameTime; // �ʴ� * 1
    public int finalScore; // ��������
    
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
        gameOverScore = GameObject.Find("Manager").GetComponent<GameOverScore>();
        portalScript = GameObject.Find("Manager").GetComponent<PortalScript>();
        cameraMovement = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        mainCamera = GameObject.Find("Main Camera");
        roll = GameObject.Find("RollButton").GetComponent<Button>();        
        rollText = GameObject.Find("RollTime").GetComponent<TMP_Text>();
        // roll.onClick.AddListener(Roll);
        roll.onClick.AddListener(() => Roll());

        tails.Add(gameObject);

        spdUP = 0.1f;
        moveSpd = 2f;
        tailRotateSpd = 200f;
        rollTime = 0f;
        maxTailCount = 0;
        itemCount = 0;
    }


    void Update()
    {
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
                settingScript.skillOn.SetActive(false);
                settingScript.skillOff.SetActive(true);
                rollTime -= Time.deltaTime;
            }
            else
            {
                isRoll = true;
                settingScript.skillOn.SetActive(true);
                settingScript.skillOff.SetActive(false);
            }
        }

        spdText.text = "�ӵ� : " + moveSpd.ToString("F1");
        tailText.text = "���� : " + (tails.Count - 1).ToString();
        if(rollTime > 0)
        {
            rollText.text = ((int)rollTime).ToString();
        }
        else
        {
            rollText.text = "";
        }
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

        // �ִ� ���� ���� ������Ʈ
        if (tails.Count > maxTailCount)
        {
            maxTailCount = tails.Count;
        }
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

    public void EndGame()
    {
        // ���� ���� ó��
        settingScript.gaming = false;

        // ���ھ� ���
        gameTime = settingScript.timer; // ���� �ð�
        TailScore = maxTailCount * 100; // �ִ� ���� ����
        itemScore = itemCount * 50; // ������ ����
        finalScore = (int)(gameTime + TailScore + itemScore); // ���� ���ھ�

        gameOverScore.finalScore.SetActive(true);

        // ������ Ű ����
        string key = "FinalScore_" + System.Guid.NewGuid().ToString();

        // ���� ���� ����
        PlayerPrefs.SetInt(key, finalScore);

        // Ű ����
        string existingKeys = PlayerPrefs.GetString("ScoreKeys", "");
        existingKeys += key + ",";
        PlayerPrefs.SetString("ScoreKeys", existingKeys);

        PlayerPrefs.Save();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            audioManager.GetItemSound();
            Destroy(collision.gameObject);
            itemScript.OnItemDestroyed(collision.gameObject);

            StartPlusTail();
            moveSpd += spdUP;

            itemCount += 1;
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
                EndGame();
            }
        }

        if (other.gameObject.CompareTag("Portal"))
        {
            if (portalScript.portalTime <= 0)
            {
                if (other.gameObject.name == "Portal1")
                {
                    transform.position = portalScript.portal2Point.transform.position;
                    transform.Rotate(0f, 180f, 0f);
                }
                if (other.gameObject.name == "Portal2")
                {
                    transform.position = portalScript.portal1Point.transform.position;
                    transform.Rotate(0f, 180f, 0f);
                }
                portalScript.UsePortal();
            }
        }
    }
}
