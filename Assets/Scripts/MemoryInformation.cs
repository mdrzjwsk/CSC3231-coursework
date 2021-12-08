using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;

public class MemoryInformation : MonoBehaviour
{
    string stats;
    ProfilerRecorder totalMemory;
    public float deltaTime;

    void OnEnable()
    {
        totalMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
    }
    void OnDisable()
    {
        totalMemory.Dispose();
    }
    private void Update()
    {
        var sb = new StringBuilder(500);

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        if (totalMemory.Valid)
        {
            sb.AppendLine($"Memory Usage: {totalMemory.LastValue} ");
            sb.AppendLine($"Frame rate:  { Mathf.Ceil(fps).ToString()} fps");
            stats = sb.ToString();
        }

    }
    private void OnGUI()
    {
        GUI.TextArea(new Rect(10, 20, 150, 70), stats);
    }
}
