using UnityEngine;
using TMPro;


namespace ScoreManagement
{
    //This class will only be in Rank_Scene and Menu_Scene to manage the score rank table
    public sealed class Score_Rank_Display : MonoBehaviour 
    {
        /* //Score Rank
        public TextAsset ScoreRankJson;

        [System.Serializable]
        public class ScoreRank
        {
            public string Name;
            public int Score;
        }

        [System.Serializable]
        public class ScoreRankList
        {
            public ScoreRank[] ScoreRank;
        }

        public ScoreRankList scoreRankList = new ScoreRankList();

        private void Start() 
        {
            //Score Rank
            scoreRankList = JsonUtility.FromJson<ScoreRankList>(ScoreRankJson.text);    
        }

        private void Update() 
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            print(highScore);
            if(Input.GetKeyDown(KeyCode.E))
            {
                if (highScore > scoreRankList.ScoreRank[0].Score)
                {
                    ScoreRank scoreRank = new ScoreRank
                    {
                        Score = highScore
                    };
                    scoreRankList.ScoreRank[0].Score = highScore;

                    string toJson = JsonUtility.ToJson(scoreRank);
                    print(toJson);
                    File.WriteAllText(Application.dataPath + "/Scripts/Game/ScoreRankJson", toJson);
                    scoreRankList = JsonUtility.FromJson<ScoreRankList>(ScoreRankJson.text);    
                    
                }    

            }
        } */
        public TextMeshProUGUI[] players_Name;
        public TextMeshProUGUI[] players_Score;

        
        private void Start() 
        {
            Score_Singleton.AddHighScore("", Score.ScorePlayer01);
            print(Score.ScorePlayer01);

           

            //Showing players_attribues in Json when enter Rank_Scene
            for(int i = 0; i < Score_Singleton.player_list.Count; i++)
            {
                players_Name[i].text = Score_Singleton.player_list[i].Name;
            }
            for(int i = 0; i < Score_Singleton.player_list.Count; i++)
            {
                if(Score_Singleton.player_list[i].Score < 100000 && Score_Singleton.player_list[i].Score >= 10000)
                {
                    players_Score[i].text = $"0{Score_Singleton.player_list[i].Score}";
                }
                else if (Score_Singleton.player_list[i].Score < 10000 && Score_Singleton.player_list[i].Score >= 1000)
                {
                    players_Score[i].text = $"00{Score_Singleton.player_list[i].Score}";
                }
                else if (Score_Singleton.player_list[i].Score < 1000 && Score_Singleton.player_list[i].Score >= 100)
                {
                    players_Score[i].text = $"000{Score_Singleton.player_list[i].Score}";
                }
                else if (Score_Singleton.player_list[i].Score < 100)
                {
                    players_Score[i].text= $"000000";
                }
            }
            /* if(PlayerPrefs.GetString("Player01_Name", "") == "")
            {
                Player_Attributes player01 = new Player_Attributes{Name = "", Score = 0};
                Player_Attributes player02 = new Player_Attributes{Name = "", Score = 0};
                Player_Attributes player03 = new Player_Attributes{Name = "", Score = 0};
                Player_Attributes player04 = new Player_Attributes{Name = "", Score = 0};
                Player_Attributes player05 = new Player_Attributes{Name = "", Score = 0};
                scoreRankList.player_Attributes = new Player_Attributes[5] {player01, player02, player03, player04, player05};    
                PlayerPrefs.GetString("Player01_Name", player01.Name);
                PlayerPrefs.GetString("Player02_Name", player02.Name);
                PlayerPrefs.GetString("Player03_Name", player03.Name);
                PlayerPrefs.GetString("Player04_Name", player04.Name);
                PlayerPrefs.GetString("Player05_Name", player05.Name);      
            } */
            
        }
    }
        
}