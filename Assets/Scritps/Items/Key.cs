using UnityEngine;

[CreateAssetMenu(fileName = "Key", menuName = "ScriptableObjects/Key")]
public class Key : ScriptableObject
{
    [SerializeField]
    private Color color;

    public Color ColorValue => color;
}
