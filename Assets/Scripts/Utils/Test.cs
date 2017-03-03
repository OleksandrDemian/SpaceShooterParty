using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Trigger();
    }

    private void Trigger()
    {
        AbilityTest();
    }

    private void TextPopUpTest()
    {
        GameObject test = GameManager.ObjectPooler.Get(GOType.DAMAGEPOPUP);
        DamagePopUp dpu = test.GetComponent<DamagePopUp>();
        dpu.Initialize(new Vector3(Random.Range(-5, 5), 0, 0), Random.Range(0, 10).ToString(), Color.green);
    }

    private void AbilityTest()
    {
        Attribute damage = new Attribute(AttributeType.DAMAGE, 10);
        Ability ability = new CircleFire(damage, 5, transform);
        ability.Trigger();
    }
}