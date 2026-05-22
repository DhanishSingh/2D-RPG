using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PartyMemberUI : MonoBehaviour
{

    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HpBar hpBar;

    [SerializeField] Color highlightedColor;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        nameText.text = pokemon.Base.Name;
        Debug.Log("UI Pokemon Level: " + pokemon.Level);
        levelText.text = "Lvl: " + pokemon.Level;
        hpBar.SetHp((float)pokemon.HP / pokemon.MaxHp);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
            nameText.color = highlightedColor;
        else
            nameText.color = Color.black;

     
    }
}
