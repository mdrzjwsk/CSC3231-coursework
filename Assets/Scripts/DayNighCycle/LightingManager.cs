using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    [SerializeField, Range(0,24)] private float TimeOfDay;

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying){
            TimeOfDay += Time.deltaTime*0.04f;
            TimeOfDay %= 24;
            UpdateLighting(TimeOfDay);
        }
    }
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColour.Evaluate(timePercent);
        DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
        DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
        RenderSettings.skybox.SetColor("_Tint", Preset.AmbientColour.Evaluate(timePercent));
    }
}
