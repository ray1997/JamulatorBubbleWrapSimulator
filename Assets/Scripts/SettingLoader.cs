using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingLoader : MonoBehaviour
{
    public Slider RowSize;
    public Slider ColumnSize;
    private void OnEnable()
    {
        RowSize.SetValueWithoutNotify(WrapperManager.Instance.CurrentProfile.RowXAmount);
        ColumnSize.SetValueWithoutNotify(WrapperManager.Instance.CurrentProfile.RowYAmount); 
    }
}
