using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class KeyController : Item
{
    [SerializeField]
    private Key key;

    private void Awake()
    {
        if (key == null)
            throw new System.Exception(name + ": key is missing.");

        GetComponent<SpriteRenderer>().color = key.ColorValue;
    }

    protected override void TouchPlayer(PlayerController player)
    {
        player.AddKey(key);
        GameObject.Destroy(gameObject);
    }
}
