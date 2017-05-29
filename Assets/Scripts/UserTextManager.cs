using UnityEngine;
using UnityEngine.UI;

public class UserTextManager : MonoBehaviour
{
    private Text text;
    //private Animation anim;

	private void Awake ()
    {
        text = GetComponentInChildren<Text>();
        //anim = GetComponent<Animation>();

        transform.localScale = Vector3.one;
	}

    public void SetUserName(string name)
    {
        /*if (name == text.text)
            return;
            
        if (name == "")
        {
            if (transform.localScale != Vector3.zero)
                anim.Play("FadeOut", PlayMode.StopAll);
        }
        else
        {
            text.text = name;
            //if (transform.position != Vector3.one)
            anim.Play("FadeIn", PlayMode.StopAll);
        }*/

        text.text = name;
    }
}
