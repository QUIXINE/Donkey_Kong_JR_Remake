using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

namespace ScoreManagement
{
    public class Letter_Selection : MonoBehaviour
    {
        Score_Rank_Display score_Rank_Display;
        public TextMeshProUGUI[] Input_Letters;
        public TextMeshProUGUI[] SelectedLetter;
        public Image HighLightImg;
        public int CurrentIndex = 0;
        // Start is called before the first frame update
        void Start()
        {
            score_Rank_Display = FindObjectOfType<Score_Rank_Display>(); 
        }

        // Update is called once per frame
        void Update()
        {
            
            Select_Letter_Input();
            
        }

        private void Select_Letter_Input()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CurrentIndex = (CurrentIndex - 1 + Input_Letters.Length) % Input_Letters.Length;
                HighlightLetter();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                CurrentIndex = (CurrentIndex + 1 + Input_Letters.Length) % Input_Letters.Length;
                HighlightLetter();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                switch (CurrentIndex)
                {
                    default:
                        if (SelectedLetter[0].text == "")
                        {
                            SelectedLetter[0].text = Input_Letters[CurrentIndex].text;
                        }
                        else if (SelectedLetter[1].text == "")
                        {
                            SelectedLetter[1].text = Input_Letters[CurrentIndex].text;
                        }
                        else if (SelectedLetter[2].text == "")
                        {
                            SelectedLetter[2].text = Input_Letters[CurrentIndex].text;
                        }
                        break;

                    case 28:
                        EraseLetter();
                        break;

                    case 29:
                        SubmitLetter();
                        break;
                }
            }
        }

        private void HighlightLetter()
        {
            float xOffset = Input_Letters[CurrentIndex].rectTransform.position.x;
            float yOffset = Input_Letters[CurrentIndex].rectTransform.position.y;
            HighLightImg.rectTransform.position = new Vector2(xOffset, yOffset);
        }

        private void EraseLetter()
        {
            if(SelectedLetter[2].text != "")
            {
                SelectedLetter[2].text = "";
            }
            else if (SelectedLetter[1].text != "")
            {
                SelectedLetter[1].text = "";
            }
            else if (SelectedLetter[0].text != "")
            {
                SelectedLetter[0].text = "";
            }
        }
    
        private void SubmitLetter()
        {

            //Save Json
            for(int i = 0; i < Score_Singleton.player_list.Count; i++)
            {
                if(PlayerPrefs.GetInt("Player01_Score") == Score_Singleton.player_list[i].Score)  
                {
                    score_Rank_Display.players_Name[i].text = $"{SelectedLetter[0].text}{SelectedLetter[1].text}{SelectedLetter[2].text}";
                    Score_Singleton.player_list[i].Name = score_Rank_Display.players_Name[i].text;
                    Score_Singleton.SaveHighScores();
                    SceneManager.LoadScene(0);
                    break;
                }
            }
            /* if (Score.ScorePlayer01 == Score_Singleton.player_list[0].Score)
            {
                score_Rank_Display.players_Name[0].text = $"{SelectedLetter[0].text}{SelectedLetter[1].text}{SelectedLetter[2].text}";
                Score_Singleton.player_list
                Score_Singleton.SaveHighScores();
                print("Save");
            }
            else if (Score.ScorePlayer01 == Score_Singleton.player_list[1].Score)
            {
                score_Rank_Display.players_Name[1].text = $"{SelectedLetter[0].text}{SelectedLetter[1].text}{SelectedLetter[2].text}";
                Score_Singleton.SaveHighScores();
                print("Save");

            }
            else if (Score.ScorePlayer01 == Score_Singleton.player_list[2].Score)
            {
                score_Rank_Display.players_Name[2].text = $"{SelectedLetter[0].text}{SelectedLetter[1].text}{SelectedLetter[2].text}";
                Score_Singleton.SaveHighScores();
                print("Save");
             
            }
            else if (Score.ScorePlayer01 == Score_Singleton.player_list[3].Score)
            {
                score_Rank_Display.players_Name[3].text = $"{SelectedLetter[0].text}{SelectedLetter[1].text}{SelectedLetter[2].text}";
                Score_Singleton.SaveHighScores();
                print("Save");

            }
            else if (Score.ScorePlayer01 == Score_Singleton.player_list[4].Score)
            {
                score_Rank_Display.players_Name[4].text = $"{SelectedLetter[0].text}{SelectedLetter[1].text}{SelectedLetter[2].text}";
                Score_Singleton.SaveHighScores();
                print("Save");

            } */
        }
    
    }
}