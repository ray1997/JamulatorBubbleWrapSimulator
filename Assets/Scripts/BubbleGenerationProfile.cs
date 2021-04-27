using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Generation profile", menuName = "Game/Profile", order = 1)]
public class BubbleGenerationProfile : ScriptableObject
{
    [Range(5, 100)]
    public int BubbleRowAmount = 10;

    [Range(5, 100)]
    public int BubbleColumnAmount = 10;

    public Vector2 BubbleSize = Vector2.one;
    public Vector2 BubbleMargin = new Vector2(0.25f, 0.25f);
    public Vector2 BubbleBorder = new Vector2(0.25f, 0.25f);
    public bool Zigzag = true;
    /// <summary>
    /// Determine direction to zigzag,
    /// true to zigzag on row
    /// false to zigzag on column
    /// </summary>
    public bool ZigzagOnRow = true;
}