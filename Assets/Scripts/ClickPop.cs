using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPop : MonoBehaviour
{
    public SpriteMask WrinkleMask;
    public SpriteRenderer Wrinkle;
    public int PopIndexX;
    public int PopIndexY;

    public bool IsPop;

    private void OnMouseDown()
    {
        if (PopIndexX < 0 || PopIndexY < 0)
            return;
        if (!IsPop)
            IsPop = true;
        else if (IsPop)
            return;
        //Add popped wrinkle
        //Play pop sound
        WrapperManager.Instance.PlayPop(this);
        //Mark as popped
        TurnPopped();
    }

    public void ValidatePop()
    {
        if (WrapperManager.Instance.PopMarker[PopIndexX, PopIndexY])
        {
            //Turn pop without sound
            //Mark as popped
            TurnPopped();
        }
    }

    /// <summary>
    /// Select other wrinkle randomly
    /// </summary>
    /// <param name="maxAlpha">Give maximum alpha (255/1) instead of randomize it.</param>
    public void RandomizeWrinkle(bool maxAlpha = false)
    {
        //Select wrinkle
        Wrinkle.sprite = WrapperManager.Instance.BubbleWrinkles[Random.Range(0, WrapperManager.Instance.BubbleWrinkles.Length)];
        //Randomize alpha range
        var color = Wrinkle.color;
        if (maxAlpha)
            color.a = 1;
        else
            color.a = Random.Range(0.05f, 0.8f);
        Wrinkle.color = color;
        //Randomize flip
        Wrinkle.flipX = Random.value < 0.5f;
        Wrinkle.flipY = Random.value < 0.5f;
        //Randomize offset
        float x = Random.Range(-0.4f, 0.4f);
        float y = Random.Range(-0.4f, 0.4f);
        Wrinkle.transform.localPosition = new Vector3(x, y, 0);
    }

    //turn sprite to indicate that this bubble is bursted
    public void TurnPopped()
    {
        //Darken alpha channel
        SpriteRenderer render = GetComponent<SpriteRenderer>();
        var bcolor = render.color;
        bcolor.a = 1;
        render.color = bcolor;
        //Randomize other wrinkle
        RandomizeWrinkle(true);
    }
}
