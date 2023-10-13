using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;


namespace ScoreManagement
{
    //Don't destroyed
    public class Score_Singleton : MonoBehaviour 
    {

        private static Score_Singleton instance;
        public static Score_Singleton Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Score_Singleton();
                }
                return instance;
            }
        }


        #region Variables
        [SerializeField] private Canvas score_Count_Canvas;
        [SerializeField] private Canvas bonus_Count_Canvas;
        [SerializeField] private TextMeshProUGUI player01ScoreTextUI;
        [SerializeField] private TextMeshProUGUI bonusScoreTextUI;
        [SerializeField] private TextMeshProUGUI lapAmountTextUI;
        [SerializeField] private TextMeshProUGUI highScoreTextUI;
        public static TextMeshProUGUI Player01ScoreTextUI;
        public static TextMeshProUGUI BonusScoreTextUI;
        public static TextMeshProUGUI LapAmountTextUI;
        public static TextMeshProUGUI HighScoreTextUI;
        public TextAsset ScoreRankJson;
        #endregion

        static string filePath = "Assets/Scripts/Game/ScoreRankJson.txt";



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
            DontDestroyOnLoad(gameObject);
            Player01ScoreTextUI =   player01ScoreTextUI;
            BonusScoreTextUI    =   bonusScoreTextUI;
            HighScoreTextUI     =   highScoreTextUI;
            LapAmountTextUI     =   lapAmountTextUI;
            
            //Singleton
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                 print("Destrot");
                Destroy(gameObject);
            }
        
        }
        private void Start() 
        {
            player_list_in_inspector = player_list;
            print(player_list[0].Name);
        }

        private void Update()
        {
            Canvas_Check();
        }

        private void Canvas_Check()
        {
            if (score_Count_Canvas.worldCamera == null && bonus_Count_Canvas.worldCamera == null)
            {
                score_Count_Canvas.worldCamera = Camera.main;
                bonus_Count_Canvas.worldCamera = Camera.main;
            }
            if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 5)
            {
                bonus_Count_Canvas.gameObject.SetActive(false);
            }
            else
            {
                bonus_Count_Canvas.gameObject.SetActive(true);
            }
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