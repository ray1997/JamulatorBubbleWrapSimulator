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

    [SerializeField]
    public BubbleGenerationProfile CurrentProfile;

    public bool[,] PopMarker;

    public AudioClip[] PopSounds;
    public AudioClip[] BassBoostedPopSounds;
    public AudioSource PopPlayer;
    public GameObject BubblePrototype;
    public List<GameObject> ChildBubbles = new List<GameObject>();
    public GameObject BubbleSheet;
    public Transform BubbleSheetBackground;
    public Transform BubbleMask;
    public Sprite[] BubbleWrinkles;

    void Start()
    {
        if (PopPlayer is null)
            PopPlayer = GameObject.Find(nameof(PopPlayer)).GetComponent<AudioSource>();

        RegenerateAllBubbles();
    }

    public float GlobalNoSoundCooldown = 0;
    public float GlobalBassBoostedCooldown = 0;
    public float PassCooldownThreshold = 30;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ClearAllBubbles();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RegenerateAllBubbles();
        }
        GlobalBassBoostedCooldown += Time.deltaTime;
        GlobalNoSoundCooldown += Time.deltaTime;
    }

    void ClearAllBubbles()
    {
        foreach (var obj in ChildBubbles)
        {
            Destroy(obj);
        }
        ChildBubbles.Clear();
    }

    public void RegenerateAllBubbles()
    {
        //Recreate bubbles
        if (ChildBubbles.Count > 0)
        {
            //Destroy all first
            ClearAllBubbles();
        }
        GenerateBubbleSheet();
        ResizeSheetBackground();
    }

    [Range(0,1)]
    public float UseEffectThreshold = 0.01f;
    [Range(0,1)]
    public float UseSilentEffectThreshold = 0.9f;
    public void PlayPop(ClickPop info)
    {
        //Mark as pop on pop marker
        PopMarker[info.PopIndexX, info.PopIndexY] = true;
        //Randomize a chance to quite or bassboosted
        var random = Random.value;
        Debug.Log($"Random chance got {random} | {(random < UseEffectThreshold ? "randomize effect" : "no effect")}");
        if (random < UseEffectThreshold) //1% chance?
        {
            var effect = Random.value;
            bool silentornot = effect < UseSilentEffectThreshold; //90% chance of silent | 10% chance of bass boosted
            Debug.LogWarning($"random effect got {effect} | {(silentornot ? "No sound" : "Bass boosted")}");
            if (silentornot)
            {
                if (GlobalNoSoundCooldown > PassCooldownThreshold)
                {
                    GlobalNoSoundCooldown = 0;
                    return;
                }
            }
            else
            {
                if (GlobalBassBoostedCooldown > PassCooldownThreshold)
                {
                    GlobalBassBoostedCooldown = 0;
                    //Bass boosted
                    PopPlayer.PlayOneShot(BassBoostedPopSounds[Random.Range(0, BassBoostedPopSounds.Length)]);
                    return;
                }
            }
        }
        //Move pop player to that bubble
        //Then
        PopPlayer.PlayOneShot(PopSounds[Random.Range(0, PopSounds.Length)]);
    }

    public void GenerateBubbleSheet()
    {
        PopMarker = new bool[CurrentProfile.BubbleRowAmount, CurrentProfile.BubbleColumnAmount];
        for (int x = 0;x < CurrentProfile.BubbleRowAmount; x++)
        {
            for (int y = 0;y < CurrentProfile.BubbleColumnAmount; y++)
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
                bubbleInfo.RandomizeWrinkle();
                ChildBubbles.Add(bubble);
                if (CurrentProfile.Zigzag)
                {
                    if (CurrentProfile.ZigzagOnRow)
                    {
                        if (x % 2 == 0 && y == 0)
                        {
                            PopMarker[x, y] = true;
                            bubbleInfo.ValidatePop();
                        }
                        //else if (x % 2 == 1 && y == CurrentProfile.BubbleColumnAmount - 1)
                        //{
                        //    PopMarker[x, y] = true;
                        //    bubbleInfo.ValidatePop();
                        //}
                    }
                }
            }
        }
    }

    public Vector3 GetPositionForBubble(float x, float y)
    {
        bool shouldzigzag = (CurrentProfile.ZigzagOnRow ? Mathf.RoundToInt(x) : Mathf.RoundToInt(y)) % 2 == 0;
        //Move X and Y half from where it was
        x -= CurrentProfile.BubbleRowAmount / 2;
        y -= CurrentProfile.BubbleColumnAmount / 2;
        //Add half a size of bubble size as a padding for even amount of bubble
        if (CurrentProfile.BubbleRowAmount % 2 == 0)
            x += CurrentProfile.BubbleSize.x / 4;
        if (CurrentProfile.BubbleColumnAmount % 2 == 0)
            y += CurrentProfile.BubbleSize.y / 4;
        float axisX = ((CurrentProfile.BubbleSize.x + CurrentProfile.BubbleMargin.x) * x) + CurrentProfile.BubbleBorder.x;
        float axisY = ((CurrentProfile.BubbleSize.y + CurrentProfile.BubbleMargin.y) * y) + CurrentProfile.BubbleBorder.y;
        //Zigzag consideration
        Debug.Log($"Currently using zigzag generation: {CurrentProfile.Zigzag} |" +
            $"Current position is consider zigzag?: {shouldzigzag}");
        if (CurrentProfile.Zigzag && shouldzigzag)
        {
            Debug.Log($"Currently zigzag on: {(CurrentProfile.ZigzagOnRow ? "row" : "column")}");
            Debug.Log($"x = {x} | x % 2 = {x % 2}");
            Debug.Log($"y = {y} | y % 2 = {y % 2}");
            if (CurrentProfile.ZigzagOnRow && shouldzigzag)
            {
                Debug.Log($"Current AxisX = {axisX} | Current AxisY = {axisY}");
                axisY -= (CurrentProfile.BubbleSize.x / 2) + (CurrentProfile.BubbleMargin.x / 2);
                Debug.Log($"After alteration: {axisX} : {axisY}");
            }

            if (!CurrentProfile.ZigzagOnRow && shouldzigzag)
            {
                Debug.Log($"Current AxisX = {axisX} | Current AxisY = {axisY}");
                axisX -= (CurrentProfile.BubbleSize.y / 2) + (CurrentProfile.BubbleMargin.y / 2);
                Debug.Log($"After alteration: {axisX} : {axisY}");
            }

        }

        return new Vector3(axisX, axisY);

    }

    private void ResizeSheetBackground()
    {
        //X Scale
        float XScale = (CurrentProfile.BubbleRowAmount * CurrentProfile.BubbleSize.x) + 
            (CurrentProfile.BubbleRowAmount * CurrentProfile.BubbleMargin.x) + 
            CurrentProfile.BubbleBorder.x;
        //Y Scale
        float YScale = (CurrentProfile.BubbleColumnAmount * CurrentProfile.BubbleSize.y) +
            (CurrentProfile.BubbleColumnAmount * CurrentProfile.BubbleMargin.y) +
            CurrentProfile.BubbleBorder.y;
        //Set
        BubbleSheetBackground.transform.localScale = new Vector3(XScale, YScale);
        BubbleMask.transform.localScale = new Vector3(XScale, YScale);
    }

}
