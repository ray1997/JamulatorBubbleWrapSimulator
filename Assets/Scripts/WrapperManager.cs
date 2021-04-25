using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapperManager : MonoBehaviour
{
    public static WrapperManager Instance { get; set; }

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public AudioClip[] PopSounds;
    public AudioSource PopPlayer;
    public bool[,] PopMarker;
    public GameObject BubblePrototype;
    public int RowXAmount = 10;
    public int RowYAmount = 10;
    public Vector2 RowMargin;
    public Vector2 BubbleSize;
    public List<GameObject> ChildBubbles = new List<GameObject>();
    public GameObject BubbleSheet;

    // Start is called before the first frame update
    void Start()
    {
        if (PopPlayer is null)
        {
            PopPlayer = GameObject.Find(nameof(PopPlayer)).GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateBubbleSheet();
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            foreach (var obj in ChildBubbles)
            {
                Destroy(obj);
            }
            ChildBubbles.Clear();
        }
    }

    public void PlayPop(ClickPop info)
    {
        //Move pop player to that bubble
        //Then
        PopPlayer.PlayOneShot(PopSounds[Random.Range(0, PopSounds.Length)]);
        //Mark as pop
        PopMarker[info.PopIndexX, info.PopIndexY] = true;
    }

    public void GenerateBubbleSheet()
    {
        PopMarker = new bool[10, 10];
        int rowX = PopMarker.GetUpperBound(0);
        int rowY = PopMarker.GetUpperBound(1);
        for (int x = 0;x < rowX; x++)
        {
            for (int y = 0;y < rowY; y++)
            {
                //Generate bubble for the row
                var bubble = Instantiate(BubblePrototype, transform);
                //Set parent
                bubble.transform.parent = BubbleSheet.transform;
                //Set position
                bubble.transform.position = GetPositionForBubble(x, y);
                //Set name and bubble info
                bubble.name = $"Bubble {x}-{y}";
                ClickPop bubbleInfo = bubble.GetComponent<ClickPop>();
                bubbleInfo.PopIndexX = x;
                bubbleInfo.PopIndexY = y;
                ChildBubbles.Add(bubble);
            }
        }
    }

    public Vector3 GetPositionForBubble(float x, float y)
    {
        float axisX = 0;
        float axisY = 0;
        float halfX = RowXAmount / 2.0f;
        float halfY = RowYAmount / 2.0f;
        axisX = ((BubbleSize.x + RowMargin.x) * x) - ((BubbleSize.x + RowMargin.x) * halfX);
        axisY = ((BubbleSize.y + RowMargin.y) * y) - ((BubbleSize.y + RowMargin.y) * halfY);
        //if (x > 0)
        //    axisX = (BubbleSize.x + RowMargin.x) * x;
        //if (y > 0)
        //    axisY = (BubbleSize.y + RowMargin.y) * y;
        return new Vector3(axisX, axisY);

    }
}
