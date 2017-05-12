using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TimePanelController : MonoBehaviour
{
    private List<Transform> currentColliding = new List<Transform>();
    [SerializeField]
    private Image backgroundImage;

    private void Start()
    {
        backgroundImage = GetComponent<Image>();
    }

    private void ManageColliders()
    {
        //backgroundImage = GetComponent<Image>();
        if (currentColliding.Count > 0)
            backgroundImage.enabled = false;
        else
            backgroundImage.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag != "Player")
            return;
        currentColliding.Add(col.transform);
        ManageColliders();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag != "Player")
            return;
        currentColliding.Remove(col.transform);
        ManageColliders();
    }
}