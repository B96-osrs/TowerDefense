using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class Post_Data : MonoBehaviour
{
    // Posts game data to the server
    // The server should be running on localhost:5000, endpoint /add_game
    public IEnumerator Upload(int play_time_seconds, int coins_spent, int max_level_reached, int enemies_killed)
    {
        GameData data = new GameData
        {
            play_time_seconds = play_time_seconds,
            coins_spent = coins_spent,
            max_level_reached = max_level_reached,
            enemies_killed = enemies_killed
        };

        string jsonString = JsonUtility.ToJson(data);
        using (UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/add_game", jsonString, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log($"Error {www.responseCode}: {www.error}\nResponse: {www.downloadHandler.text}");
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log($"Error {www.responseCode}: {www.error}\nResponse: {www.downloadHandler.text}");
                Debug.Log("Form upload complete!");
            }
        }
    }

    private class GameData
    {
        public int play_time_seconds;
        public int coins_spent;
        public int max_level_reached;
        public int enemies_killed;
    }

}
