using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipCreatureScript : MonoBehaviour
{
    public Text cretureName;
    public Text HP;
    public Text MP;
    public Text ATK;
    public Text DEF;
    public Text SPEED;
    public Text RANGE;

    private void Start()
    {
        if (transform.position.y > Screen.height / 2) transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y * (-1));
        if (transform.position.x > Screen.width / 2) transform.localPosition = new Vector2(transform.localPosition.x * (-1), transform.localPosition.y);
        transform.SetAsFirstSibling();
    }

    public void GenerateInfo(CreatureStats creture)
    {
        cretureName.text = creture.creatureName;
        HP.text = creture.HP.ToString();
        MP.text = creture.MP.ToString();
        ATK.text = string.Format("{0} - {1}",creture.minATK,creture.maxATK);
        DEF.text = string.Format("{0} - {1}", creture.minDEF, creture.maxDEF);
        SPEED.text = creture.SPD.ToString();
        RANGE.text = creture.rangeAttack.ToString();
    }
}
