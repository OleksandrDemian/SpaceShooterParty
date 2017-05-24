using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlackHoleAttractable
{
    void Attract(Vector3 toPosition);
}

public class BlackHole : MonoBehaviour, IPoolable
{
    private List<IBlackHoleAttractable> subjected;
    private Animation anim;

    private void Start ()
    {
        anim = GetComponent<Animation>();
        AnimationEvent ev = new AnimationEvent();

        ev.time = anim.GetClip("FadeOut").length;
        ev.functionName = "Disable";

        anim.GetClip("FadeOut").AddEvent(ev);
    }
	
	private void Update ()
    {
        Attract();
	}

    private void Attract()
    {
        for (int i = 0; i < subjected.Count; i++)
        {
            subjected[i].Attract(transform.position);
        }
    }

    private void NewObjectCreated(GameObject go)
    {
        IBlackHoleAttractable at = go.GetComponent<IBlackHoleAttractable>();
        if (at != null)
        {
            if (!subjected.Contains(at))
                subjected.Add(at);
        }
    }

    public void Initialize(Vector3 position)
    {
        transform.localScale = Vector2.zero;

        anim = GetComponent<Animation>();
        transform.position = position;
        ObjectPool.Instance.AddOnObjectCreateListener(NewObjectCreated);
        subjected = new List<IBlackHoleAttractable>();

        anim.Play("FadeIn");

        GameTime.Instance.AddTimer(new Timer(5, delegate()
        {
            StartCoroutine(DisableWait());
        }));

        GameObject[] all = FindObjectsOfType<GameObject>();
        for (int i = 0; i < all.Length; i++)
        {
            IBlackHoleAttractable at = all[i].GetComponent<IBlackHoleAttractable>();
            if (at != null)
            {
                subjected.Add(at);
            }
        }
    }

    private IEnumerator DisableWait()
    {
        anim.Play("FadeOut");
        while (anim.isPlaying)
            yield return new WaitForEndOfFrame();
        Disable();
    }

    private void Disable()
    {
        subjected.Clear();
        Debug.Log("Disable");
        ObjectPool.Instance.RemoveOnObjectCreateListener(NewObjectCreated);
        GameManager.ObjectPooler.Add(this);
        gameObject.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D collider)
    {
        IDamagable damagable = collider.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
            damagable.Damage(666, null);
    }

    public GameObject GetGameObject
    {
        get
        {
            return gameObject;
        }
    }
}
