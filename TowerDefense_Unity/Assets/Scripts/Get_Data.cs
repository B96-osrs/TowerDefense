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
    [System.Serializable]
    public struct APIEndpoint
    {
        public string uri;
        public string jsonKey;
        public TextMeshProUGUI targetField;
        public bool roundToInt;
    }

    public APIEndpoint[] endpoints;


    private void Start()
    {
        foreach (APIEndpoint endpoint in endpoints)
        {
            StartCoroutine(displayData(endpoint));
        }
    }

    /// Coroutine to fetch and display data from the API
    /// If rounding is checked in the configuration, the data will be rounded to the nearest integer
    IEnumerator displayData(APIEndpoint endpoint)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(endpoint.uri))
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
                string data = jsonObject[endpoint.jsonKey].ToString();
                if (endpoint.roundToInt)
                {
                    int roundedData = Mathf.RoundToInt(float.Parse(data));
                    endpoint.targetField.text = roundedData.ToString();
                }
                else
                {
                    endpoint.targetField.text = data;
                }
            }
        }
    }

}
