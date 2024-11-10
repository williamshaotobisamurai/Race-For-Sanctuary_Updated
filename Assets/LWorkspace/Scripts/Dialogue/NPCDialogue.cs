using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private Transform uiAnchor;

    [SerializeField] private Image dialogueBG;
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private TMP_Text dialogueText;

    public void Show(string text)
    {
        dialogueCanvas.gameObject.SetActive(true);
        dialogueText.text = text;
    }

    public void Hide()
    {
        dialogueCanvas.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(uiAnchor.transform.position);
        dialogueBG.GetComponent<RectTransform>().anchoredPosition = screenPoint / dialogueCanvas.scaleFactor;
    }
}
