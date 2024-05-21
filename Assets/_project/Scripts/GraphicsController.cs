using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class GraphicsController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown Quality;
    [SerializeField] private TMP_Dropdown AntiAliasing;
    [SerializeField] private TMP_Dropdown VSync;
    [SerializeField] private TMP_Dropdown TextureQuality;
    [SerializeField] private TMP_Dropdown ShadowQuality;

    public UnityEvent changedSetting;

    [SerializeField] private int[] QualityInts;

    private void Start()
    {
        QualityInts = new int[5];
    }

    public void SettingInvoke()
    {
        changedSetting.Invoke();
    }

    public void CollectSettings()
    {
        //print(Quality.value);
        QualityInts[0] = Quality.value;
        //print(AntiAliasing.value);
        QualityInts[1] = AntiAliasing.value;
        //print(VSync.value);
        QualityInts[2] = VSync.value;
        //print(TextureQuality.value);
        QualityInts[3] = TextureQuality.value;
        //print(ShadowQuality.value);
        QualityInts[4] = ShadowQuality.value;
    }
}
