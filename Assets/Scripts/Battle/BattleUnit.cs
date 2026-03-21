using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour 
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }

    public void Setup()
    { 
        Pokemon = new Pokemon(_base, level);

        if (Pokemon == null || Pokemon.Base == null)
        {
            Debug.LogError($"BattleUnit '{name}': Pokemon or Pokemon.Base is null. Check the '_base' field in inspector.");
            return;
        }

        var image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError($"BattleUnit '{name}': no Image component found on this GameObject.");
            return;
        }

        var sprite = isPlayerUnit ? Pokemon.Base.BackSprite : Pokemon.Base.FrontSprite;
        if (sprite == null)
        {
            Debug.LogError($"BattleUnit '{name}': sprite is null for '{Pokemon.Base.Name}'. Check PokemonBase sprites.");
            return;
        }

        image.sprite = sprite;
    }
}
