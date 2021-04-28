using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPop : MonoBehaviour
{
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
        //As a test, for now mark color red onto sprite
    }

    public void ValidatePop()
    {
        if (WrapperManager.Instance.PopMarker[PopIndexX, PopIndexY])
        {
            //Turn pop without sound
            //Mark as popped
            //As a test, for now mark color red onto sprite
        }
    }

    public void RandomizeWrinkle()
    {
        //Select wrinkle
        Wrinkle.sprite = WrapperManager.Instance.BubbleWrinkles[Random.Range(0, WrapperManager.Instance.BubbleWrinkles.Length)];
        //Randomize alpha range
        var color = Wrinkle.color;
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
}
