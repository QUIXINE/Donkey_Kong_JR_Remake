using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class OptionSelection : MonoBehaviour
{
    public static int Player_Amount;

    //Option Selecting
    [SerializeField] private Image highLightImg;
    [SerializeField] private GameObject scoreBoardObj;
    [SerializeField] private RectTransform scoreBoardObjOldPos;
    [SerializeField] private Vector3 scoreBoardObjTargetedPos;
    [SerializeField] private TextMeshProUGUI[] Options_Text; 
    [SerializeField] int CurrentIndex = 0;



    void Start()
    {
        scoreBoardObjOldPos.position =   scoreBoardObj.transform.position;

        //used to check each player's current scene
        PlayerPrefs.GetInt("Player01_Scene", 0);
        PlayerPrefs.GetInt("Player02_Scene", 0); 

        //used to check which player is playing
        PlayerPrefs.GetInt("Current_Player", 0);

        //Player amount
        PlayerPrefs.GetInt("Player_Amount", 0);
        Player_Amount = PlayerPrefs.GetInt("Player_Amount", 0);  //Player amount of previous game
        PlayerPrefs.SetInt("Player_Amount", 0);
    }

    void Update()
    {
        //SelectPlayerAmount();
        //GetInGame();
        NavigateOption();
        SelectOption();
        CloseScoreBoard();
    }

  
    private void HighlightOption()
    {
        float xOffset = Options_Text[CurrentIndex].rectTransform.position.x;
        float yOffset = Options_Text[CurrentIndex].rectTransform.position.y;
        highLightImg.rectTransform.position = new Vector2(xOffset, yOffset);
    }

    private void NavigateOption()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                CurrentIndex = (CurrentIndex - 1 + Options_Text.Length) % Options_Text.Length;
                HighlightOption();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                CurrentIndex = (CurrentIndex + 1 + Options_Text.Length) % Options_Text.Length;
                HighlightOption();
            }
    }
    
    
    private void SelectOption()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //Use currentIndex to check which option player select
            switch(CurrentIndex)
            {
                case 0:
                    PlayerPrefs.SetInt("Player_Amount",  1);
                    PlayerPrefs.SetInt("Current_Player", 1);     //Set Player01 to play first
                    PlayerPrefs.SetInt("Player01_Scene", 1);     //Set Player01 Scene index 1 is Opening scene
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    break;
                case 1:
                    PlayerPrefs.SetInt("Player_Amount",  2);
                    PlayerPrefs.SetInt("Current_Player", 1);     //Set Player01 to play first
                    PlayerPrefs.SetInt("Player01_Scene", 1);     //Set Player01 Scene index 1 is Opening scene
                    PlayerPrefs.SetInt("Player02_Scene", 2);     //Set Player02 Scene index 2 is _Level01 scene, set as 2 because when it's Player02 turn, they will get to _Level01 first not Opening
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    break;
                case 2:
                    scoreBoardObj.transform.position    =   scoreBoardObjTargetedPos;
                    break;

                    //Move rank score display gameObj w/ black panel
            }
        }
    }

    private void CloseScoreBoard()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && scoreBoardObj.transform.position == scoreBoardObjTargetedPos)
        {
            scoreBoardObj.transform.position    =   new Vector3(20, 0, 0);
        }
    }
}


/* Insert Coin to increase player amount way  
    private int coin;
    [SerializeField] private TextMeshProUGUI amountTxtUI;
    [SerializeField] private TextMeshProUGUI insertCoinTxtUI;
    [SerializeField] private TextMeshProUGUI coinTxtUI;
    [SerializeField] private GameObject player_Coin_Num_UI_Parent;

  private void SelectPlayerAmount()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            AudioPlayerTest.PlayAudio(AudioReferences.CoinInsertSound);
            coin++;
            if (coin > 0)
            {
                if (coin == 1)
                {
                    PlayerPrefs.SetInt("Player_Amount", 1);
                    coinTxtUI.text = $"CREDIT  0{coin}";
                    player_Coin_Num_UI_Parent.SetActive(false);
                    insertCoinTxtUI.text = "PUSH";
                    amountTxtUI.text = "ONLY 1 PLAYER BUTTON";
                }
                else if (coin > 1)
                {
                    PlayerPrefs.SetInt("Player_Amount", 2);
                    if(coin > 9)
                    {
                        coinTxtUI.text = $"CREDIT  {coin}";
                    }
                    else if (coin >= 99)
                    {
                        coinTxtUI.text = $"CREDIT  99";
                        coin = 99;
                    }
                    else 
                    {
                        coinTxtUI.text = $"CREDIT  0{coin}";
                    }
                    amountTxtUI.text = "1 OR 2 PLAYERS BUTTON";
                }
            }
        }
    }

    
private static void GetInGame()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && PlayerPrefs.GetInt("Player_Amount") > 0)
        {
            PlayerPrefs.SetInt("Current_Player", 1);     //Set Player01 to play first
            PlayerPrefs.SetInt("Player01_Scene", 1);     //Set Player01 Scene index 1 is Opening scene
            PlayerPrefs.SetInt("Player02_Scene", 2);     //Set Player02 Scene index 2 is _Level01 scene, set as 2 because when it's Player02 turn, they will get to _Level01 first not Opening
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

*/