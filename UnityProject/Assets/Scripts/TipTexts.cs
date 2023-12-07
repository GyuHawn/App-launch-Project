using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipTexts : MonoBehaviour
{
    public TMP_Text tipText;

    private List<string> textList = new List<string>();
    private System.Random random = new System.Random();

    void Start()
    {
        textList.Add("�� ������ �׽�Ʈ�� �����Դϴ�.");
        textList.Add("�Ҹ� ������ BGM���� ����˴ϴ�.");
        textList.Add("�� ������ ������Ʈ�� �������� �ֽ��ϴ�.");
        textList.Add("������ �ϳ��� 0.1�� �ӵ��� �ø��ϴ�.");
        textList.Add("Ȥ�� ����Ż ���� �߻��� ����� ���ּ���.");

        tipText.text = GetRandomTip();
    }

    public string GetRandomTip()
    {
        int index = random.Next(textList.Count);
        return textList[index];
    }
}
