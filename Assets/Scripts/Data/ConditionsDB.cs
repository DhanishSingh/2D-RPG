using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConditionsDB
{

    public static void Init()
    {
        foreach (var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }
    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.psn,
            new Condition()
            {
                Name ="Poison",
                StartMessage = "has been poisoned",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.MaxHp / 8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt due to poison");
                }
            }
        },

        {
            ConditionID.brn,
            new Condition()
            {
                Name ="Burn",
                StartMessage = "has been burned",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.MaxHp / 16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt due to burn");
                }
            }
        },
        {
            ConditionID.par,
            new Condition()
            {
                Name ="Paralyzed",
                StartMessage = "has been paralyzed",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (UnityEngine.Random.Range(1, 5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}'s paralyzed and can't move. ");
                        return false;
                    }
                    return true;
                }
            }
        },
        {
           ConditionID.frz,
           new Condition()
           {
               Name ="Freeze",
               StartMessage = "has been frozen",
               OnBeforeMove = (Pokemon pokemon) =>
               {
                   if (UnityEngine.Random.Range(1, 6) == 1)
                   {
                       pokemon.CureStatus();
                       pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}'s is not frozen anymore ");
                       return true;
                   }
                   return false;
               }
           }
        },
        {
            ConditionID.slp,
            new Condition()
            {
                Name ="Sleep",
                StartMessage = "has fallen asleep",
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.StatusTime = UnityEngine.Random.Range(1,5);
                    Debug.Log($"Will be asleep for{pokemon.StatusTime} moves");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {

                    if (pokemon.StatusTime <= 0)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} woke up! ");
                        return true;
                    }
                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is sleeping");
                    return false;
                }
            }
        },

        //Volatile Status
        {
            ConditionID.confuion,
            new Condition()
            {
                Name = "Confusion",
                StartMessage = "has been confused",
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.VolatileStatusTime = UnityEngine.Random.Range(1,5);
                    Debug.Log($"Will be confused for {pokemon.VolatileStatusTime} moves");
                },
                OnBeforeMove =(Pokemon pokemon) =>
                {
                    if(pokemon.VolatileStatusTime <= 0)
                    {
                        pokemon.CureVolatileStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} kicked out of confusion!");
                        return true;
                    }
                    pokemon.VolatileStatusTime--;

                    if(UnityEngine.Random.Range(1,3) == 1)
                        return true;

                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is confused");
                    pokemon.UpdateHP(pokemon.MaxHp / 8);
                    pokemon.StatusChanges.Enqueue($"It hurt itself due to confuion");
                    return false;
                }
            }
        }

    };

}

public enum ConditionID
{
    none, psn, brn, slp, par, frz,
    confuion
}