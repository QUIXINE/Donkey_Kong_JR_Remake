using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ScoreManagement
{
    public class Load_And_Save_Json : MonoBehaviour
    {
        public TextAsset ScoreRankJson;
        private static string filePath = "Assets/Scripts/Game/ScoreRankJson.txt";


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
            LoadHighScore();    
            player_list_in_inspector = player_list;
        }

        public void LoadHighScore()
        {
                print("Get access to json");
                string json = File.ReadAllText(filePath);
                // PlayerList playerList = JsonUtility.FromJson<PlayerList>(ScoreRankJson.text);
                PlayerList playerList = JsonUtility.FromJson<PlayerList>(json);
                player_list = playerList.ScoreRank;
                // Sort the high scores by score in descending order.
                player_list.Sort((x, y) => y.Score.CompareTo(x.Score));
            
        }
        public static void SaveHighScores()
        {
            PlayerList playerList = new PlayerList{ ScoreRank = player_list};
            string json = JsonUtility.ToJson(playerList);
            // File.WriteAllText(Application.persistentDataPath + "ScoreRankJson.txt", json);
            File.WriteAllText(filePath, json);
            
            //File.WriteAllText(ScoreRankJson.text, json);
        }

        public static void AddHighScore(string name, int score)
        {
            // Sort the high scores by score in descending order.
            player_list.Sort((ScoreRank x, ScoreRank y) => y.Score.CompareTo(x.Score)); //implicit version --> player_list.Sort((x, y) => y.Score.CompareTo(x.Score));

            // Keep only the top 5 scores.
            // Find the rank where the new score belongs.
            for (int i = 0; i < player_list.Count; i++)
            {
                if (score > player_list[i].Score)
                {
                    player_list.Insert(i, new ScoreRank { Name = name, Score = score });
                    player_list.RemoveAt(player_list.Count - 1); // Remove the lowest score.
                    SaveHighScores();
                    break;
                }
            }
        }
    }
}