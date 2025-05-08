using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEditor.ShaderGraph.Serialization;
using System.Net;

public class Get_Data : MonoBehaviour
{
    public TextMeshProUGUI gamesPlayedNum;
    public TextMeshProUGUI maxEnemyNum;
    public TextMeshProUGUI avgEnemyNum;
    public TextMeshProUGUI maxCoinsNum;
    public TextMeshProUGUI avgCoinsNum;
    public TextMeshProUGUI maxLevelNum;
    public TextMeshProUGUI avgLevelNum;
    

    private void Start()
    {
        GetData();
    }


    public void GetData()
    {
        StartCoroutine(GetGamesPlayedSum("http://127.0.0.1:5000/games_played"));
        StartCoroutine(GetMaxEnemiesKilled("http://127.0.0.1:5000/max_enemies_killed"));
        StartCoroutine(GetAvgEnemiesKilled("http://127.0.0.1:5000/avg_enemies_killed"));
        StartCoroutine(GetMaxCoinsSpent("http://127.0.0.1:5000/max_coins_spent"));
        StartCoroutine(GetAvgCoinsSpent("http://127.0.0.1:5000/avg_coins_spent"));
        StartCoroutine(GetMaxLevelReached("http://127.0.0.1:5000/max_level_reached"));
        StartCoroutine(GetAvgLevelReached("http://127.0.0.1:5000/avg_level_reached"));
        
    }


    IEnumerator GetGamesPlayedSum(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                JObject jsonObject = JObject.Parse(webRequest.downloadHandler.text);
                string gamesPlayed = jsonObject["games_played"].ToString();
                gamesPlayedNum.text = gamesPlayed;
            }
        }
    }


    IEnumerator GetMaxEnemiesKilled(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                JObject jsonObject = JObject.Parse(webRequest.downloadHandler.text);
                string maxEnemiesKilled = jsonObject["max_enemies_killed"].ToString();
                maxEnemyNum.text = maxEnemiesKilled;
            }
        }
    }
    IEnumerator GetAvgEnemiesKilled(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                JObject jsonObject = JObject.Parse(webRequest.downloadHandler.text);
                int avgEnemiesKilled = Mathf.RoundToInt((float)jsonObject["avg_enemies_killed"]);
                avgEnemyNum.text = avgEnemiesKilled.ToString();
            }
        }
    }

    IEnumerator GetMaxCoinsSpent(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                JObject jsonObject = JObject.Parse(webRequest.downloadHandler.text);
                string maxCoinsSpent = jsonObject["max_coins_spent"].ToString();
                maxCoinsNum.text = maxCoinsSpent;
            }
        }
    }

    IEnumerator GetAvgCoinsSpent(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                JObject jsonObject = JObject.Parse(webRequest.downloadHandler.text);
                int avgCoinsSpent = Mathf.RoundToInt((float)jsonObject["avg_coins_spent"]);
                avgCoinsNum.text = avgCoinsSpent.ToString();
            }
        }
    }


    IEnumerator GetMaxLevelReached(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                JObject jsonObject = JObject.Parse(webRequest.downloadHandler.text);
                string maxLevel = jsonObject["max_level_reached"].ToString();
                maxLevelNum.text = maxLevel;
            }
        }

    }
    IEnumerator GetAvgLevelReached(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                JObject jsonObject = JObject.Parse(webRequest.downloadHandler.text);
                int avgLevel = Mathf.RoundToInt((float)jsonObject["avg_level_reached"]);
                avgLevelNum.text = avgLevel.ToString();
            }
        }
    }


}
