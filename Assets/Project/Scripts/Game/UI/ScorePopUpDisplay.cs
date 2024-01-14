using TMPro;
using UnityEngine;

public class ScorePopUpDisplay : MonoBehaviour {
    public static TextMeshProUGUI PopUpScoreUI;
    [SerializeField] private TextMeshProUGUI popUpScoreUI;

    [SerializeField] private Animator popUpScoreAnimator;

    public static Animator  PopUpScoreAnimator;

    private void Start() 
    {
        AssignVariables();
    }

    private void AssignVariables()
    {
        PopUpScoreUI        =   popUpScoreUI;
        PopUpScoreAnimator  =   popUpScoreAnimator;
    }

    public static void PopUpScore(Transform objectPos, int score)
    {
        PopUpScoreUI.text = $"{score}";
        PopUpScoreUI.gameObject.transform.position = objectPos.position;
        PopUpScoreAnimator.Play("Pop_Up", -1, 0f);
    }
}