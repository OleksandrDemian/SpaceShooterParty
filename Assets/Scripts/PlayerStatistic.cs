public class PlayerStatistic
{
    private int death = 0;
    private int kill = 0;
    private int miss = 0;
    private int hit = 0;
    private int bonuses = 0;

    public void Dead()
    {
        death++;
    }

    public void Kill()
    {
        kill++;
    }

    public int KillCounter()
    {
        return kill;
    }

    public void Miss()
    {
        miss++;
    }

    public void Hit()
    {
        hit++;
    }

    public void Bonus()
    {
        bonuses++;
    }

    public float Precision()
    {
        try
        {
            return (kill + hit) / (kill + miss + hit);
        }
        catch
        {
            return 0f;
        }
        
    }
}