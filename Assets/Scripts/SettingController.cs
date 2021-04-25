using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingController : MonoBehaviour
{
    public static SettingController Instance;
    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    ScrollView settingView;

    Slider amountSliderX;
    Slider amountSliderY;

    TextField bubbleWidth;
    TextField bubbleHeight;

    TextField bubbleMarginWidth;
    TextField bubbleMarginHeight;

    TextField bubbleBorderWidth;
    TextField bubbleBorderHeight;

    Toggle useZigzag;

    Button regenerateBubble;
    private void OnEnable()
    {
        if (CurrentProfile is null)
            CurrentProfile = WrapperManager.Instance.CurrentProfile;
        //Gather UI
        var root = GetComponent<UIDocument>().rootVisualElement;

        settingView = root.Q<ScrollView>("settingView");

        amountSliderX = root.Q<Slider>("amount-x");
        amountSliderX.SetValueWithoutNotify(CurrentProfile.RowXAmount);
        amountSliderX.RegisterValueChangedCallback((update) => UpdateValue(update, nameof(amountSliderX)));
        amountSliderY = root.Q<Slider>("amount-y");
        amountSliderY.SetValueWithoutNotify(CurrentProfile.RowYAmount);
        amountSliderY.RegisterValueChangedCallback((update) => UpdateValue(update, nameof(amountSliderY)));

        bubbleWidth = root.Q<TextField>("bubble-size-x");
        bubbleWidth.SetValueWithoutNotify(CurrentProfile.BubbleSize.x.ToString());
        bubbleWidth.RegisterValueChangedCallback((update) => UpdateText(update, nameof(bubbleWidth)));
        bubbleHeight = root.Q<TextField>("bubble-size-y");
        bubbleHeight.SetValueWithoutNotify(CurrentProfile.BubbleSize.y.ToString());
        bubbleHeight.RegisterValueChangedCallback((update) => UpdateText(update, nameof(bubbleHeight)));

        bubbleMarginWidth = root.Q<TextField>("bubble-margin-x");
        bubbleMarginWidth.SetValueWithoutNotify(CurrentProfile.BubbleMargin.x.ToString());
        bubbleMarginWidth.RegisterValueChangedCallback((update) => UpdateText(update, nameof(bubbleMarginWidth)));
        bubbleMarginHeight = root.Q<TextField>("bubble-margin-y");
        bubbleMarginHeight.SetValueWithoutNotify(CurrentProfile.BubbleMargin.y.ToString());
        bubbleMarginHeight.RegisterValueChangedCallback((update) => UpdateText(update, nameof(bubbleMarginHeight)));

        bubbleBorderWidth = root.Q<TextField>("bubble-border-x");
        bubbleBorderWidth.SetValueWithoutNotify(CurrentProfile.BubbleBorder.x.ToString());
        bubbleBorderWidth.RegisterValueChangedCallback((update) => UpdateText(update, nameof(bubbleBorderWidth)));
        bubbleBorderHeight = root.Q<TextField>("bubble-border-y");
        bubbleBorderHeight.SetValueWithoutNotify(CurrentProfile.BubbleBorder.y.ToString());
        bubbleBorderHeight.RegisterValueChangedCallback((update) => UpdateText(update, nameof(bubbleBorderHeight)));

        useZigzag = root.Q<Toggle>("zigzag");
        useZigzag.SetValueWithoutNotify(CurrentProfile.Zigzag);
        useZigzag.RegisterValueChangedCallback((update) => UpdateToggle(update, nameof(useZigzag)));

        regenerateBubble = root.Q<Button>("bubble-regen");
        regenerateBubble.RegisterCallback<ClickEvent>(click => CallRegenerateBubble(click));
    }

    BubbleGenerationProfile CurrentProfile;
    void CallRegenerateBubble(ClickEvent click) => WrapperManager.Instance.RegenerateAllBubbles();

    private void UpdateToggle(ChangeEvent<bool> update, string name)
    {
        switch (name)
        {
            case nameof(useZigzag):
                CurrentProfile.Zigzag = update.newValue;
                break;
        }
    }

    private void UpdateText(ChangeEvent<string> update, string name)
    {
        float parsed;
        switch (name)
        {
            case nameof(bubbleWidth):
                float.TryParse(update.newValue, out parsed);
                CurrentProfile.BubbleSize = new Vector2(parsed, CurrentProfile.BubbleSize.y);
                break;
            case nameof(bubbleHeight):
                float.TryParse(update.newValue, out parsed);
                CurrentProfile.BubbleSize = new Vector2(CurrentProfile.BubbleSize.x, parsed);
                break;

            case nameof(bubbleMarginWidth):
                float.TryParse(update.newValue, out parsed);
                CurrentProfile.BubbleMargin = new Vector2(parsed, CurrentProfile.BubbleMargin.y);
                break;
            case nameof(bubbleMarginHeight):
                float.TryParse(update.newValue, out parsed);
                CurrentProfile.BubbleMargin = new Vector2(CurrentProfile.BubbleMargin.x, parsed);
                break;

            case nameof(bubbleBorderWidth):
                float.TryParse(update.newValue, out parsed);
                CurrentProfile.BubbleBorder = new Vector2(parsed, CurrentProfile.BubbleBorder.y);
                break;
            case nameof(bubbleBorderHeight):
                float.TryParse(update.newValue, out parsed);
                CurrentProfile.BubbleBorder = new Vector2(CurrentProfile.BubbleBorder.x, parsed);
                break;
        }
    }

    private void UpdateValue(ChangeEvent<float> update, string name)
    {
        switch (name)
        {
            case nameof(amountSliderX):
                CurrentProfile.RowXAmount = Mathf.RoundToInt(update.newValue);
                break;
            case nameof(amountSliderY):
                CurrentProfile.RowYAmount = Mathf.RoundToInt(update.newValue);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Toggle setting
            settingView.visible = !settingView.visible;
        }
    }

    public bool IsSettingShow
    {
        get
        {
            return settingView.visible;
        }
    }
}
