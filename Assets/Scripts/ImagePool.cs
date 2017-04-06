using UnityEngine;

public class ImagePool : MonoBehaviour
{
    [SerializeField]
    private Sprite[] positionImages;
    [SerializeField]
    private Sprite[] shipSkins;
    [SerializeField]
    private Sprite[] laserSkins;
    [SerializeField]
    private Sprite[] asteroidsSkin;

    public Sprite GetPositionImage(int position)
    {
        if (position >= positionImages.Length)
            position = 0;
        return positionImages[position];
    }

    public Sprite GetShipSkin(int index)
    {
        Debug.Log(index);
        if (index > 15)
            return shipSkins[16];
        return shipSkins[index];
    }

    public Sprite GetLaserSkin(int index)
    {
        if (index >= laserSkins.Length)
            index = 0;
        return laserSkins[index];
    }

    public Sprite GetAsteroidSkin(int index)
    {
        if (index >= asteroidsSkin.Length)
            index = 0;
        return asteroidsSkin[index];
    }

    public Sprite GetAsteroidSkin()
    {
        int rand = Random.Range(0, asteroidsSkin.Length);
        return asteroidsSkin[rand];
    }
}