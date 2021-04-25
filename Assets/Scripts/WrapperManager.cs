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
        
    }

    public void PlayPop()
    {
        //Move pop player to that bubble
        //Then
        PopPlayer.PlayOneShot(PopSounds[Random.Range(0, PopSounds.Length)]);
    }


}
