using UnityEngine;
using TMPro;


namespace ScoreManagement
{
    //This class will only be in Rank_Scene and Menu_Scene to manage the score rank table
    public sealed class Score_Rank_Display : MonoBehaviour 
    {
        public TextMeshProUGUI[] players_Name;
        public TextMeshProUGUI[] players_Score;

        
        private void Start() 
        {
            /* Load_And_Save_Json.AddHighScore("", Score.ScorePlayer01);
            print(Score.ScorePlayer01); */

           

            //Showing players_attribues in Json when enter Rank_Scene
            for(int i = 0; i < Load_And_Save_Json.player_list.Count; i++)
            {
                players_Name[i].text = Load_And_Save_Json.player_list[i].Name;
            }
            for(int i = 0; i < Load_And_Save_Json.player_list.Count; i++)
            {
                if(Load_And_Save_Json.player_list[i].Score < 100000 && Load_And_Save_Json.player_list[i].Score >= 10000)
                {
                    players_Score[i].text = $"0{Load_And_Save_Json.player_list[i].Score}";
                }
                else if (Load_And_Save_Json.player_list[i].Score < 10000 && Load_And_Save_Json.player_list[i].Score >= 1000)
                {
                    players_Score[i].text = $"00{Load_And_Save_Json.player_list[i].Score}";
                }
                else if (Load_And_Save_Json.player_list[i].Score < 1000 && Load_And_Save_Json.player_list[i].Score >= 100)
                {
                    players_Score[i].text = $"000{Load_And_Save_Json.player_list[i].Score}";
                }
                else if (Load_And_Save_Json.player_list[i].Score < 100)
                {
                    players_Score[i].text= $"000000";
                }
            }
            
        }
    }
        
}