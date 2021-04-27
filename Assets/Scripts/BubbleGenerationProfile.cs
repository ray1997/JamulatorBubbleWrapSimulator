using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Generation profile", menuName = "Game/Profile", order = 1)]
public class BubbleGenerationProfile : ScriptableObject, INotifyPropertyChanged
{
    [SerializeField]
    [Range(5, 100)]
    int _row = 10;
    public int RowXAmount
    {
        get => _row;
        set => Set(ref _row, value);
    }
    [SerializeField]
    [Range(5, 100)]
    int _column = 10;
    public int RowYAmount
    {
        get => _column;
        set => Set(ref _column, value);
    }

    [SerializeField]
    Vector2 _size = Vector2.one;
    public Vector2 BubbleSize
    {
        get => _size;
        set => Set(ref _size, value);
    }

    [SerializeField]
    Vector2 _margin = new Vector2(0.25f, 0.25f);
    public Vector2 BubbleMargin
    {
        get => _margin;
        set => Set(ref _margin, value);
    }

    [SerializeField]
    Vector2 _border = new Vector2(0.25f,0.25f);
    public Vector2 BubbleBorder
    {
        get => _border;
        set => Set(ref _border, value);
    }

    [SerializeField]
    bool _zigzag = true;
    public bool Zigzag
    {
        get => _zigzag;
        set => Set(ref _zigzag, value);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void Set<T>(ref T storage, T value, [CallerMemberName]string name = null)
    {
        if (!Equals(storage, value))
        {
            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}