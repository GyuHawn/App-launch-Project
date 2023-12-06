using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChangeColor : MonoBehaviour
{
    public Image colorPicker;
    public Material mapColor;
    public Material map2Color;
    public EventTrigger trigger;

    void Start()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnPointerClick((PointerEventData) data); });
        trigger.triggers.Add(entry);
    }

    

    public void OnPointerClick(PointerEventData data)
    {
        StartCoroutine(SpoidClick());
    }

    IEnumerator SpoidClick()
    {
        yield return new WaitForEndOfFrame();
        
        if(SceneManager.GetActiveScene().name == "Main")
        {
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply();

            Vector3 mPos = Input.mousePosition;
            Color color = tex.GetPixel((int)mPos.x, (int)mPos.y);

            mapColor.SetColor("_TintColor", color);
        }

        if (SceneManager.GetActiveScene().name == "Main1")
        {
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply();

            Vector3 mPos = Input.mousePosition;
            Color color = tex.GetPixel((int)mPos.x, (int)mPos.y);

            map2Color.SetColor("_EmissionColor", color);
            map2Color.EnableKeyword("_EMISSION");
        }

    }
}
