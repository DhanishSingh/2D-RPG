using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
public class PokemonPartyMain : MonoBehaviour
{ [SerializeField] List<Pokemon> pokemons;
    public List<Pokemon> Pokemons
    { get
        {
            return pokemons;
        }
    }
    private void Start()
    {
        foreach (var pokemon in pokemons)
        {
            if (pokemon == null)
            {
                Debug.LogError("Pokemon is NULL");
                continue;
            }

            if (pokemon.Base == null)
            {
                Debug.LogError("Pokemon Base is NULL");
                continue;
            }

            pokemon.Init();
            Debug.Log("Trying to save: " + pokemon.Base.Name + " Level: " + pokemon.Level);
            Debug.Log("Pokemon: " + pokemon.Base.Name + " Level: " + pokemon.Level);

            if (pokemon.Level > 0)
            {
                if (DatabaseManager.Instance != null)
                {
                    DatabaseManager.Instance.SavePokemon(
                        pokemon.Base.Name,
                        pokemon.Level
                    );
                }
                else
                {
                    Debug.LogError("DatabaseManager is NULL");
                }
            }
            else
            {
                Debug.LogError("Level is 0 for " + pokemon.Base.Name);
            }
        }
    }
    public Pokemon GetHealthyPokemon() 
    {
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();
    }
}