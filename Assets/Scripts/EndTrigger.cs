using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public event OnSkullyEnter OnSkullyEnterEvent;
    public delegate void OnSkullyEnter();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Skully>() != null)
        {
            OnSkullyEnterEvent?.Invoke();
        }
    }
}
