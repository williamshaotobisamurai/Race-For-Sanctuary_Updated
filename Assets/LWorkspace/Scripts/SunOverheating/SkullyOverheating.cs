using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            if (overheatingProgress >= 1)
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
        isOverheating = true;
    }

    public void ReduceOverheatingProgress(float progress)
    {
        overheatingProgress -= progress;
    }
}
