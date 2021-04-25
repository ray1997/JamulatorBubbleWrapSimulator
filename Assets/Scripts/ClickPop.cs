using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPop : MonoBehaviour
{
    public int PopIndexX;
    public int PopIndexY;

    public bool IsPop;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!IsPop)
            IsPop = true;
        else if (IsPop)
            return;
        //Add popped wrinkle
        //Play pop sound
        WrapperManager.Instance.PlayPop();
        //Mark as popped
    }
}
