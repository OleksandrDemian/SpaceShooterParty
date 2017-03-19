using UnityEngine;
using UnityEngine.UI;

public class ShipGUI : MonoBehaviour
{

    private Text nameT;
    private Text healthT;
    private Text shieldT;
    private Transform target;

    private void Start()
    {
        nameT = transform.FindChild("Name").GetComponent<Text>();
        healthT = transform.FindChild("Health").GetComponent<Text>();
        shieldT = transform.FindChild("Shield").GetComponent<Text>();
        target = this.transform;
    }

    private void Update()
    {
        transform.position = target.position;
    }

    public void SetTargetTransform(Transform target)
    {
        this.target = target;
    }

    public void SetHealth(string value)
    {
        healthT.text = "Health: " + value;
    }

    public void SetShield(string value)
    {
        shieldT.text = "Shield: " + value;
    }

    public void SetName(string value)
    {
        nameT.text = value;
    }
}