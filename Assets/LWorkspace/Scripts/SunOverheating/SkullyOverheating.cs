using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkullyOverheating : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> overheatingMeshRendererList;
    [SerializeField] private List<Color> originalColorList;

    [SerializeField] private SkinnedMeshRenderer skullyMeshRenderer;
    [SerializeField] private Color skullyOverheatingColor;
    [SerializeField] private Color skullyOriginalColor;

    [SerializeField] private float overheatingProgress;

    [SerializeField] private float overheatingSpeed = 0.1f;

    private bool isOverheating = false;
    private bool isOverheated = false;

    public event OnOverheat OnOverheatEvent;
    public delegate void OnOverheat();

    [SerializeField] private string warningText;
    [SerializeField] private Image overheatingFilter;

    private bool messageSent = false;
    private Color overheatingFilterColor = Color.white;

    [SerializeField] private float overheatingFilterSpeed = 10f;

    private void Start()
    {
        foreach (MeshRenderer mr in overheatingMeshRendererList)
        {
            originalColorList.Add(mr.material.color);
        }
    }

    private void Update()
    {
        if (isOverheating && !isOverheated)
        {
            overheatingProgress += Time.deltaTime * overheatingSpeed;

            overheatingProgress = Mathf.Clamp01(overheatingProgress);

            float currentAlpha = overheatingFilter.color.a;            

            overheatingFilterColor.a = Mathf.MoveTowards(currentAlpha, overheatingProgress ,Time.deltaTime * overheatingFilterSpeed);

            overheatingFilter.color = overheatingFilterColor;

            if (overheatingProgress > 0.75f && !messageSent)
            {
                InstructionManager.ShowText(warningText);
                messageSent = true;
            }

            if (overheatingProgress >= 1 && !isOverheated)
            {
                isOverheated = true;
                OnOverheatEvent?.Invoke();
            }
        }

        SetOverheatingProgress(overheatingProgress);
    }

    public void SetOverheatingProgress(float progress)
    {
        for (int i = 0; i < originalColorList.Count; i++)
        {
            Color normalColor = originalColorList[i];
            Color color = Color.Lerp(normalColor, skullyOverheatingColor, progress);
            overheatingMeshRendererList[i].material.color = color;
            overheatingMeshRendererList[i].material.SetColor("_EmissionColor", color);
        }

        foreach (Material mat in skullyMeshRenderer.materials)
        {
            Color color = Color.Lerp(skullyOriginalColor, skullyOverheatingColor, progress);
            mat.color = color;
        }
    }

    public void StartOverheating()
    {
        Debug.Log("start overheating");
        isOverheating = true;
    }

    public void StopOverheating()
    {
        Debug.Log("stop overheating");
        isOverheating = false;
        overheatingProgress = 0;
        SetOverheatingProgress(overheatingProgress);
    }

    public void ReduceOverheatingProgress(float progress)
    {
        overheatingProgress -= progress;
    }
}
