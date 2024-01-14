using UnityEngine;
using TMPro;


namespace ScoreManagement
{
    //This class will only be in Rank_Scene and Menu_Scene to manage the score rank table
    public sealed class ScoreRankDisplay : MonoBehaviour 
    {
        public TextMeshProUGUI[] players_Name;
        public TextMeshProUGUI[] players_Score;

        private void Awake() 
        {
            //Load_And_Save_Score.LoadHighScore();    //used to load json and show on UI, used this to Test, will UI show if use this code
        }
        private void Start() 
        {
            /* Load_And_Save_Json.AddHighScore("", Score.ScorePlayer01);
            print(Score.ScorePlayer01); */

           

            //Showing players_attribues in Json when enter Rank_Scene
            for(int i = 0; i < LoadAndSaveScore.player_list.Count; i++)
            {
                players_Name[i].text = LoadAndSaveScore.player_list[i].Name;
            }
            for(int i = 0; i < LoadAndSaveScore.player_list.Count; i++)
            {
                if(LoadAndSaveScore.player_list[i].Score < 999999 && LoadAndSaveScore.player_list[i].Score >= 10000)
                {
                    players_Score[i].text = $"0{LoadAndSaveScore.player_list[i].Score}";
                }
                else if (LoadAndSaveScore.player_list[i].Score < 10000 && LoadAndSaveScore.player_list[i].Score >= 1000)
                {
                    players_Score[i].text = $"00{LoadAndSaveScore.player_list[i].Score}";
                }
                else if (LoadAndSaveScore.player_list[i].Score < 1000 && LoadAndSaveScore.player_list[i].Score >= 100)
                {
                    players_Score[i].text = $"000{LoadAndSaveScore.player_list[i].Score}";
                }
                else if (LoadAndSaveScore.player_list[i].Score < 100)
                {
                    players_Score[i].text= $"000000";
                }
            }
            
        }
    }
        
}