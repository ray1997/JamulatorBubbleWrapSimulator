using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPop : MonoBehaviour
{
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
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();
        render.color = Color.red;
    }
}
