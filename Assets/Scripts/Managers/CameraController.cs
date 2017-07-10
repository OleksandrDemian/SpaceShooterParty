using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance
    {
        get;
        private set;
    }

    private bool isShaking = false;
    [SerializeField]
    [Range(0.1f, 1f)]
    private float shakeRadius = 0.6f;
    [SerializeField]
    [Range(2, 10)]
    private int shakePoints = 3;
    [SerializeField]
    [Range(1, 50)]
    private int shakeSpeed = 10;

    private void Awake ()
    {
        Instance = this;
	}

    public void Shake()
    {
        if (isShaking)
            return;

        isShaking = true;
        StartCoroutine(Shaking());
    }

    private IEnumerator Shaking()
    {
        Vector3[] positions = new Vector3[shakePoints];
        for (int i = 0; i < positions.Length - 1; i++)
        {
            positions[i] = new Vector3(Random.Range(-shakeRadius, shakeRadius), Random.Range(-shakeRadius, shakeRadius), transform.position.z);
        }
        positions[positions.Length - 1] = transform.position;

        int index = 0;

        while (index < positions.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, positions[index], GameTime.TimeScale * shakeSpeed);
            if (transform.position == positions[index])
                index++;

            yield return new WaitForEndOfFrame();
        }
        isShaking = false;
    }
}
