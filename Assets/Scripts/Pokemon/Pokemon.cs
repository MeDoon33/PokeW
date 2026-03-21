using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pokemon
{
   public  PokemonBase Base;
   public  int Level;
   public int HP { get; set; }
    public List<Move> Moves { get; set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        HP = MaxHp;

        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.MoveBase));

            if (Moves.Count >= 4)
            {
                break;
            }
        }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }
    }
    public int SpAttack
    {
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; }
    }
    public int SpDefense
    {
        get { return Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }
    public int MaxHp
    {
        get { return Mathf.FloorToInt((Base.MaxHp * Level) / 100f) + 10 + Level; }
    }
    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        var details = new DamageDetails();

        // --------- Critical Hit ----------
        float critical = 1f;
        details.Critical = false;

        if (Random.value <= 0.0625f)   // 6.25% chance
        {
            critical = 2f;
            details.Critical = true;
        }

        // --------- Type Effectiveness ----------
        float type1 = TypeChart.GetEffectiveness(move.Base.Type, Base.Type1);
        float type2 = TypeChart.GetEffectiveness(move.Base.Type, Base.Type2);

        float typeEffect = type1 * type2;
        details.Type = typeEffect;

        // --------- Random Modifier ----------
        float random = Random.Range(0.85f, 1f);

        // --------- Damage Formula ----------
        float a = (2 * attacker.Level / 5f + 2);
        float d = ((a * move.Base.Power * ((float)attacker.Attack / Defense)) / 50) + 2;

        float modifiers = random * typeEffect * critical;

        int damage = Mathf.FloorToInt(d * modifiers);

        // --------- Apply Damage ----------
        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            details.Fainted = true;
        }
        else
        {
            details.Fainted = false;
        }

        return details;
    }
    
    public Move GetRanDomMove ()
    {
        if (Moves == null || Moves.Count == 0)
            return null;

        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }

}
public class DamageDetails
{
    public bool Fainted { get; set; }
    public bool Critical { get; set; }
    public float Type { get; set; }
}
