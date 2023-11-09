using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace ScoreManagement
{
    public class Name_Registration : MonoBehaviour
    {
        Score_Rank_Display score_Rank_Display;
        public TextMeshProUGUI[] Input_Letters;
        public TextMeshProUGUI[] SelectedLetter;
        public Image HighLightImg;
        public int CurrentIndex = 0;

        [Header("Registration Time")]
        [SerializeField] private float regisTime = 30;
        [SerializeField] private TextMeshProUGUI regisTimeNumUI;
        private bool canResetTime;


        // Start is called before the first frame update
        void Start()
        {
            score_Rank_Display = FindObjectOfType<Score_Rank_Display>(); 
        }

        // Update is called once per frame
        void Update()
        {
            
            Select_Letter_Input();
            TimeCount();
            
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
            if (SelectedLetter[2].text != "" && SelectedLetter[1].text != "" && SelectedLetter[0].text != "")
            {
                SelectedLetter[0].text = "";
                SelectedLetter[1].text = "";
                SelectedLetter[2].text = "";
            }
            else if(SelectedLetter[2].text != "")
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
            //use current player to set which player to put score in

            //Save Json
            if(PlayerPrefs.GetInt("Player_Amount") == 1)
            {
                for(int i = 0; i < Load_And_Save_Score.player_list.Count; i++)
                {
                    if(PlayerPrefs.GetInt("Player01_Score") == Load_And_Save_Score.player_list[i].Score)  
                    {
                        score_Rank_Display.players_Name[i].text = $"{SelectedLetter[0].text}{SelectedLetter[1].text}{SelectedLetter[2].text}";
                        Load_And_Save_Score.player_list[i].Name = score_Rank_Display.players_Name[i].text;       // change name of player in list in Json w/ input name  
                        Load_And_Save_Score.SaveHighScores();
                        SceneManager.LoadScene(0);
                        break;
                    }
                }
            }
            else
            {
                if(PlayerPrefs.GetInt("Current_Player") == 1)
                {
                    for(int i = 0; i < Load_And_Save_Score.player_list.Count; i++)
                    {
                        if(PlayerPrefs.GetInt("Player01_Score") == Load_And_Save_Score.player_list[i].Score)  
                        {
                            score_Rank_Display.players_Name[i].text = $"{SelectedLetter[0].text}{SelectedLetter[1].text}{SelectedLetter[2].text}";
                            Load_And_Save_Score.player_list[i].Name = score_Rank_Display.players_Name[i].text;
                            Load_And_Save_Score.SaveHighScores();
                            if(Load_And_Save_Score.player_list.Any(player => PlayerPrefs.GetInt("Player02_Score") == player.Score))
                            {
                                EraseLetter();
                                PlayerPrefs.SetInt("Current_Player", 2);
                                CurrentIndex = 0;
                                HighlightLetter();
                                canResetTime = true;
                                break;
                            }
                            print("Load scene");
                            SceneManager.LoadScene(0);
                            break;
                        }
                    }
                    return;
                }
                else if (PlayerPrefs.GetInt("Current_Player") == 2)
                {
                    for(int i = 0; i < Load_And_Save_Score.player_list.Count; i++)
                    {
                        if(PlayerPrefs.GetInt("Player02_Score") == Load_And_Save_Score.player_list[i].Score)  
                        {
                            
                            if(PlayerPrefs.GetInt("Player02_Score") == PlayerPrefs.GetInt("Player01_Score") && i <= 3)
                            {
                                i++;
                            }
                            else if(PlayerPrefs.GetInt("Player02_Score") == PlayerPrefs.GetInt("Player01_Score") && i == 4)
                            {
                                SceneManager.LoadScene(0);
                                break;
                            }
                            
                            score_Rank_Display.players_Name[i].text = $"{SelectedLetter[0].text}{SelectedLetter[1].text}{SelectedLetter[2].text}";
                            Load_And_Save_Score.player_list[i].Name = score_Rank_Display.players_Name[i].text;       // change name of player in list in Json w/ input name  
                            Load_And_Save_Score.SaveHighScores();
                            SceneManager.LoadScene(0);
                            break;
                        }
                    }
                }
            }
        }
    
        //Registration time
        private void TimeCount()
        {
            if(canResetTime)
            {
                regisTime = 30;
                canResetTime = false;
            }

            if(regisTime > 0)
            {
                regisTime -= Time.deltaTime;
                regisTimeNumUI.text = $"{regisTime.ToString("00")}";
            }
            else
            {
                regisTime = 0;
                SubmitLetter();
                canResetTime = true;
            }
        }    
    }
}
/* Way to round num
    //regisTimeNumUI.text = String.Format("{0 : 00}", regisTime); 
    //if use <> can't print 24, 14 like 
    //- regisTimeNumUI.text = $"<{Math.Round(regisTime, 0)}>"; 
    //- regisTimeNumUI.text = $"0 : 00{Mathf.FloorToInt(regisTime)}";
    //- int intNum = Mathf.RoundToInt(regisTime);
    //- regisTimeNumUI.text = $"<{intNum}>";  
*/