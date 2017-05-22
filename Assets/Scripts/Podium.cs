using UnityEngine;
using UnityEngine.UI;

public class Podium : MonoBehaviour
{
    [SerializeField]
    private Text txtName;
    [SerializeField]
    private Image imgPosition;

    public void SetUp(int position, string name)
    {
        imgPosition.sprite = GameManager.ImagePooler.GetPositionImage(position);
        txtName.text = name;
        GetComponent<RectTransform>().localPosition = new Vector2(0, 100 - (60*position));
    }

}
