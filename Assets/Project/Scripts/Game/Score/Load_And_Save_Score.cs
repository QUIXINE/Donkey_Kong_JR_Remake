using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ScoreManagement
{
    public class Load_And_Save_Score : MonoBehaviour
    {
        public TextAsset ScoreRankJson;
        private static string originalPath = "Assets/Project/Resources/ScoreRankJson.txt";
        private static string newPath;

        [System.Serializable]
        public class ScoreRank
        {
            public string Name;
            public int Score;
        }

        [System.Serializable]
        public class PlayerList
        {
            public List<ScoreRank> ScoreRank;
        }
        public static List<ScoreRank> player_list = new List<ScoreRank>();
        public  List<ScoreRank> player_list_in_inspector;


        private void Awake() 
        {
            newPath = Path.Combine(Application.persistentDataPath, "ScoreRank.json");
            LoadHighScore();    
            player_list_in_inspector = player_list;
        }

        public static void LoadHighScore()
        {
            TextAsset json = null;
            PlayerList playerList = null;
            //string json = File.ReadAllText(filePath);
            if(File.Exists(newPath))
            {
                print("new File exits");
                print(newPath);
                string jsonFile = File.ReadAllText(newPath);
                playerList = JsonUtility.FromJson<PlayerList>(jsonFile);
            }
            else if(File.Exists(originalPath) && !File.Exists(newPath))
            {
                json = Resources.Load<TextAsset>("ScoreRankJson");
                playerList = JsonUtility.FromJson<PlayerList>(json.text);
            }
            else
            {
                Debug.LogWarning("File not found: " + originalPath);
            }
            // print("Get access to json");
            // PlayerList playerList = JsonUtility.FromJson<PlayerList>(ScoreRankJson.text);
            print(json);
            player_list = playerList.ScoreRank;
            // Sort the high scores by score in descending order.
            player_list.Sort((x, y) => y.Score.CompareTo(x.Score));
        
        }
        public static void SaveHighScores()
        {
            PlayerList playerList = new PlayerList{ ScoreRank = player_list};
            string json = JsonUtility.ToJson(playerList);
            File.WriteAllText(newPath, json);
            
            
            // File.WriteAllText(Application.persistentDataPath + "ScoreRankJson.txt", json);
            
            // File.WriteAllText(ScoreRankJson.text, json);
        }

        public static void AddHighScore(string name, int score)
        {
            // Sort the high scores by score in descending order.
            // Sort the list
            player_list.Sort((ScoreRank x, ScoreRank y) => y.Score.CompareTo(x.Score)); //implicit version --> player_list.Sort((x, y) => y.Score.CompareTo(x.Score));

            // Keep only the top 5 scores.
            // Find the rank where the new score belongs.
            for (int i = 0; i < player_list.Count; i++)
            {
                if (score > player_list[i].Score)
                {
                    player_list.Insert(i, new ScoreRank { Name = name, Score = score });
                    player_list.RemoveAt(player_list.Count - 1); // Remove the lowest score, index 4
                    SaveHighScores();
                    break;
                }
            }
        }
    }
}