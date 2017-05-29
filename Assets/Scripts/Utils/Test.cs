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

    private void AttributeTest()
    {
        //DEBUG EXAMPLE
        AttributeModifier mod = new AttributeModifier(ModifierType.ADD, 5);
        AttributeModifier mult = new AttributeModifier(ModifierType.MULTIPLY, 2);
        Attribute attr = new Attribute(AttributeType.HEALTH, 15);
        Debug.Log(attr);
        attr.AddModifier(mod);
        Debug.Log(attr);
        attr.Value -= 5;
        Debug.Log(attr);
        attr.ResetValue();
        Debug.Log(attr);
        attr.Value += 10;
        Debug.Log(attr);
        attr.Value -= 10;
        Debug.Log(attr);
        attr.AddModifier(mult);
        Debug.Log(attr);
        attr.ResetValue();
        Debug.Log(attr);
        attr.Value -= 2;
        Debug.Log(attr);
        attr.RemoveModifier(mod);
        Debug.Log("After remove: " + attr);
        attr.ResetValue();
        attr.RemoveModifier(mult);
        Debug.Log("After remove2: " + attr);
        attr.ResetDefaultValue();
        Debug.Log(attr);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Trigger();
    }

    private void Trigger()
    {
        //AbilityTest();
        BonusTest();
        /*ExplosionManager manager = ObjectPool.Instance.Get<ExplosionManager>();
        manager.Initialize(Vector3.zero);*/
        /*BlackHole bh = ObjectPool.Instance.Get<BlackHole>();
        bh.Initialize(Vector3.zero);*/
        /*PowerUp b = ObjectPool.Instance.Get<PowerUp>();
        b.AddBonus(new SlowDownTimeBonus(1.8f));
        b.transform.position = Vector2.zero;*/
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

    private void BonusTest()
    {
        PowerUp bonus = ObjectPool.Instance.Get<PowerUp>();

        bonus.transform.position = Vector2.zero;

        bool negative = false;
        /*
        bonus.AddBonus(new GenericBonus(delegate (GameObject target)
        {
            Player player = target.GetComponent<ShipController>().GetPlayer();
            if (player == null)
                return;

            PopUp.ShowText(target.transform.position, "Disable players", 1);
            System.Collections.Generic.List<Player> players = GameManager.Instance.GetPlayers();
            players.Remove(player);

            for (int i = 0; i < players.Count; i++)
            {
                players[i].EnableControll(false);
            }

            GameTime.Instance.AddTimer(new Timer(5, delegate ()
            {
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].EnableControll(true);
                }
            }));
        }));
        */
        //bonus.AddBonus(new CircleFireBonus(15));
        bonus.AddBonus(new GenericBonus(delegate (GameObject target)
        {
            Vector3 position = target.transform.position;
            PopUp.ShowText(position, "Black hole", 2);
            GameTime.Instance.AddTimer(new Timer(3, delegate ()
            {
                BlackHole bh = ObjectPool.Instance.Get<BlackHole>();
                ShipController player = target.GetComponent<ShipController>();
                if (player != null)
                    bh.SetOnDamageListener(player.DamageListener);
                bh.Initialize(position);
            }));
        }));
        
        bonus.EnableFollowing(negative);
    }

    private void TextPopUpTest()
    {
        //GameObject test = GameManager.ObjectPooler.Get(EntityType.DAMAGEPOPUP);
        /*PopUp dpu = test.GetComponent<PopUp>();
        dpu.Initialize(new Vector3(Random.Range(-5, 5), 0, 0), Random.Range(0, 10).ToString(), Color.green);*/
        PopUp.ShowText(new Vector3(Random.Range(-5, 5), 0, 0), Random.Range(0, 10).ToString(), 0.5f, Color.green);
    }

    private void AbilityTest()
    {
        //Attribute damage = new Attribute(AttributeType.DAMAGE, 10);
        //Ability ability = new CircleFire(damage, 5, transform);
        //ability.Trigger();
    }
}