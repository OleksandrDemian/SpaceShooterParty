using UnityEngine;

public class Test : MonoBehaviour
{
    /*private class Ciao
    {
        public int c;

        public Ciao(int i)
        {
            c = i;
        }
    }*/

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
        /*Ciao[] ciaos = new Ciao[5];
        ciaos[0] = new Ciao(5);
        ciaos[1] = new Ciao(7);
        ciaos[2] = new Ciao(2);
        ciaos[3] = new Ciao(4);
        ciaos[4] = new Ciao(9);

        System.Array.Sort(ciaos, delegate (Ciao x, Ciao y) {
            return x.c.CompareTo(y.c);
        });
        for (int i = 0; i < ciaos.Length; i++)
            Debug.Log("Ciao: " + ciaos[i].c);*/
    }

    private void TextPopUpTest()
    {
        GameObject test = GameManager.ObjectPooler.Get(EntityType.DAMAGEPOPUP);
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