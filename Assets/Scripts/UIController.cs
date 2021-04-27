using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentPage == Page.Main)
                CurrentPage = Page.Setting;
            else
                CurrentPage = Page.Main;
        }
    }

    public GameObject[] Pages;

    public enum Page
    {
        Main,
        Setting
    }
    Page _cp;
    public Page CurrentPage
    {
        get => _cp;
        set
        {
            if (!Equals(_cp, value))
            {
                _cp = value;
                foreach (var page in Pages)
                {
                    if (page.name == value.ToString())
                        page.SetActive(true);
                    else
                        page.SetActive(false);
                }
            }
        }
    }
    
    public void ActivateSettingPage()
    {
        if (CurrentPage != Page.Setting)
            CurrentPage = Page.Setting;
    }

    public void LeaveSettingPage()
    {
        if (CurrentPage == Page.Setting)
            CurrentPage = Page.Main;
    }

    public float BubbleRowAmount
    {
        get => WrapperManager.Instance.CurrentProfile.BubbleRowAmount;
        set => WrapperManager.Instance.CurrentProfile.BubbleRowAmount = Mathf.RoundToInt(value);
    }

    public float BubbleColumnAmount
    {
        get => WrapperManager.Instance.CurrentProfile.BubbleColumnAmount;
        set => WrapperManager.Instance.CurrentProfile.BubbleColumnAmount = Mathf.RoundToInt(value);
    }
}
