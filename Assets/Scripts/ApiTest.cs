using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;

[System.Serializable]
public class OcidRoot
{
    public string ocid;
}
[System.Serializable]
public class CharacterInfo
{
    public string date;
    public string character_name;
    public string world_name;
    public string character_gender;
    public string character_class;
    public string character_class_level;
    public int character_level;
    public int character_exp;
    public string character_exp_rate;
    public string character_guild_name;
    public string character_image;
    public string character_date_create;
    public string access_flag;
    public string liberation_quest_clear_flag;
}

[System.Serializable]
public class Error
{
    public string name;
    public string message;
}

[System.Serializable]
public class ErrorRoot
{
    public Error error;
}

public class ApiTest : MonoBehaviour
{
    public TMP_InputField characterName;
    public Button searchBtn;
    public RawImage characterImage;

    public RawImage[] testimage;
    string[] characterImages = new string[3]; 


    private const string API_KEY = "test_c7d79847ef111a5c9664f55e7fac90f120b3495a4a746be461030f045fb17329efe8d04e6d233bd35cf2fabdeb93fb0d";

    private void Awake()
    {
        searchBtn.onClick.AddListener(SearchButtonClick);
    }

    string URLEncoder(string name)
    {
        string encoded = Uri.EscapeDataString(name);
        return name;
    }

    IEnumerator CharacterRequest()
    {
        string character_name = characterName.text;
        string url = $"https://open.api.nexon.com/maplestory/v1/id?character_name={URLEncoder(character_name)}";

        UnityWebRequest www = UnityWebRequest.Get(url);

        www.SetRequestHeader("x-nxopen-api-key", API_KEY);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            print(www);
            OcidRoot root = JsonUtility.FromJson<OcidRoot>(www.downloadHandler.text);
            StartCoroutine(CharacterInfoRequest(root.ocid));
            print($"ocid : {root.ocid}");
        }
        else
        {
            print($"캐릭터 검색 실패 {www.error}");
            ErrorRoot serverData = JsonUtility.FromJson<ErrorRoot>(www.downloadHandler.text);
            string output = $"Error Name: {serverData.error.name}\nMessage: {serverData.error.message}";
            Debug.Log(output);
        }
    }

    IEnumerator CharacterInfoRequest(string ocid)
    {
        string url = $"https://open.api.nexon.com/maplestory/v1/character/basic?ocid={ocid}&date=2024-12-10";

        UnityWebRequest wwwC = UnityWebRequest.Get(url);

        wwwC.SetRequestHeader("x-nxopen-api-key", API_KEY);

        yield return wwwC.SendWebRequest();

        if (wwwC.error == null)
        {
            CharacterInfo cInfo = JsonUtility.FromJson<CharacterInfo>(wwwC.downloadHandler.text);
            print($"{cInfo.character_name}님의 캐릭터 정보");
            //print($"{cInfo.character_image}캐릭터 이미지 주소");
            characterImages[0] = $"{cInfo.character_image}?action=A02&width=200&height=200";
            characterImages[1] = $"{cInfo.character_image}?action=A03&width=200&height=200";
            characterImages[2] = $"{cInfo.character_image}?action=A06&width=200&height=200";

            //StartCoroutine(ImageRequest(cInfo.character_image));
            StartCoroutine(ImageRequest(characterImages));
        }
        else
        {
            print($"캐릭터 불러오기 실패 : {wwwC.error}");
            ErrorRoot serverData = JsonUtility.FromJson<ErrorRoot>(wwwC.downloadHandler.text);
            string output = $"Error Name: {serverData.error.name}\nMessage: {serverData.error.message}";
            Debug.Log(output);
        }
    }

    IEnumerator ImageRequest(string[] urls)
    {
        for (int i = 0; i < 3; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(urls[i]);

            www.SetRequestHeader("x-nxopen-api-key", API_KEY);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print($"이미지 오류{www.error}");
                ErrorRoot serverData = JsonUtility.FromJson<ErrorRoot>(www.downloadHandler.text);
                string output = $"Error Name: {serverData.error.name}\nMessage: {serverData.error.message}";
                Debug.Log(output);
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                testimage[i].texture = texture;

                characterImage.SetNativeSize();
            }
        }
        
    }

    void SearchButtonClick()
    {
        StartCoroutine(CharacterRequest());
    }

    /*
     IEnumerator ImageRequest(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        www.SetRequestHeader("x-nxopen-api-key", API_KEY);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            print($"이미지 오류{www.error}");
            ErrorRoot serverData = JsonUtility.FromJson<ErrorRoot>(www.downloadHandler.text);
            string output = $"Error Name: {serverData.error.name}\nMessage: {serverData.error.message}";
            Debug.Log(output);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            characterImage.texture = texture;

            characterImage.SetNativeSize();
        }
    }
     */

}
