using UnityEngine;

public class ArtifactController : Item
{
    [SerializeField]
    private int point = 1;

    protected override void TouchPlayer(PlayerController player)
    {
        player.Points += point;
        GameObject.Destroy(gameObject);
    }
}
