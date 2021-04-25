using UnityEngine;

[CreateAssetMenu(fileName = "Generation profile", menuName = "Game/Profile", order = 1)]
public class BubbleGenerationProfile : ScriptableObject
{
    [Range(5,100)]
    public int RowXAmount = 10;
    [Range(5, 100)]
    public int RowYAmount = 10;
    public Vector2 BubbleSize = Vector2.one;
    public Vector2 BubbleMargin = new Vector2(0.25f, 0.25f);
    public Vector2 BubbleBorder = new Vector2(0.25f, 0.25f);
    public bool Zigzag = true;
}