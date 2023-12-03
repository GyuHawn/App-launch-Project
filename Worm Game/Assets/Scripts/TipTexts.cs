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
        textList.Add("이 게임은 테스트용 게임입니다.");
        textList.Add("소리 조절은 BGM에만 적용됩니다.");
        textList.Add("이 게임의 업데이트는 없을수도 있습니다.");
        textList.Add("아이템 하나는 0.1씩 속도를 올립니다.");
        textList.Add("혹시 맵이탈 오류 발생시 재시작 해주세요.");

        tipText.text = GetRandomTip();
    }

    public string GetRandomTip()
    {
        int index = random.Next(textList.Count);
        return textList[index];
    }
}
