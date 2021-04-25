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

    public class BubbleGenerationProfile
    {
        public int RowXAmount = 10;
        public int RowYAmount = 10;
        public Vector2 BubbleSize = Vector2.one;
        public Vector2 BubbleMargin = new Vector2(0.25f, 0.25f);
        public Vector2 BubbleBorder = new Vector2(0.25f, 0.25f);
        public bool Zigzag = true;
    }

    [SerializeField]
    public BubbleGenerationProfile CurrentProfile = new BubbleGenerationProfile();

    public bool[,] PopMarker;

    public AudioClip[] PopSounds;
    public AudioSource PopPlayer;
    public GameObject BubblePrototype;
    public List<GameObject> ChildBubbles = new List<GameObject>();
    public GameObject BubbleSheet;
    public Transform BubbleSheetBackground;
    public Transform BubbleMask;

    // Start is called before the first frame update
    void Start()
    {
        if (PopPlayer is null)
        {
            PopPlayer = GameObject.Find(nameof(PopPlayer)).GetComponent<AudioSource>();
        }
        GenerateBubbleSheet();
        ResizeSheetBackground();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
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
        PopMarker = new bool[CurrentProfile.RowXAmount, CurrentProfile.RowYAmount];
        for (int x = 0;x < CurrentProfile.RowXAmount; x++)
        {
            for (int y = 0;y < CurrentProfile.RowYAmount; y++)
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
        //Move X and Y half from where it was
        x -= CurrentProfile.RowXAmount / 2;
        y -= CurrentProfile.RowYAmount / 2;
        //Add half a size of bubble size as a padding for even amount of bubble
        if (CurrentProfile.RowXAmount % 2 == 0)
            x += CurrentProfile.BubbleSize.x / 4;
        if (CurrentProfile.RowYAmount % 2 == 0)
            y += CurrentProfile.BubbleSize.y / 4;
        float axisX = ((CurrentProfile.BubbleSize.x + CurrentProfile.BubbleMargin.x) * x) + CurrentProfile.BubbleBorder.x;
        float axisY = ((CurrentProfile.BubbleSize.y + CurrentProfile.BubbleMargin.y) * y) + CurrentProfile.BubbleBorder.y;
        //Subtract half wrapper size
        //float axisX = 
        //float axisX = ((CurrentProfile.BubbleSize.x + CurrentProfile.BubbleMargin.x) * x) - 
        //    (CurrentProfile.BubbleSize.x + CurrentProfile.BubbleMargin.x + 
        //    (CurrentProfile.RowXAmount % 2 == 0 ? CurrentProfile.BubbleSize.x / 2 : 0));
        //float axisY = 0;
        //axisY = ((CurrentProfile.BubbleSize.y + CurrentProfile.BubbleMargin.y) * y) - ((CurrentProfile.BubbleSize.y + CurrentProfile.BubbleMargin.y) * halfY);
        
        return new Vector3(axisX, axisY);

    }

    private void ResizeSheetBackground()
    {
        //X Scale
        float XScale = (CurrentProfile.RowXAmount * CurrentProfile.BubbleSize.x) + 
            (CurrentProfile.RowXAmount * CurrentProfile.BubbleMargin.x) + 
            CurrentProfile.BubbleBorder.x;
        //Y Scale
        float YScale = (CurrentProfile.RowYAmount * CurrentProfile.BubbleSize.y) +
            (CurrentProfile.RowYAmount * CurrentProfile.BubbleMargin.y) +
            CurrentProfile.BubbleBorder.y;
        //Set
        BubbleSheetBackground.transform.localScale = new Vector3(XScale, YScale);
        BubbleMask.transform.localScale = new Vector3(XScale, YScale);
    }

}
