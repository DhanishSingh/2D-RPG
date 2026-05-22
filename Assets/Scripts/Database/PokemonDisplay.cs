using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PokemonDisplay : MonoBehaviour
{
    public TMP_Text pokemonText;
    public GameObject pokemonTextObject;

    void Start()
    {
        List<PokemonData> pokemonList = DatabaseManager.Instance.GetPokemonForCurrentUser();

        string display = "";

        foreach (var p in pokemonList)
        {
            display += p.PokemonName + " - Level " + p.Level + "\n";
        }

        pokemonText.text = display;
    }

    public void ShowUI(bool show)
    {
        pokemonTextObject.SetActive(show);
    }
}