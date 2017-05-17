public class GameInfo
{
    private int gameTime = 60;
    private int mapSize = 11;
    private bool enableAsteroids = true;
    private bool enableShields = true;
    private bool enableBonuses = true;

    public GameInfo()
    {
        Instance = this;
    }

    public static GameInfo Instance
    {
        get;
        private set;
    }

    public int MatchTime
    {
        get
        {
            return gameTime;
        }
        set
        {
            gameTime = value;
        }
    }

    public int MapSize
    {
        get
        {
            return mapSize;
        }
        set
        {
            mapSize = value;
        }
    }

    public bool AsteroidsEnabled
    {
        get
        {
            return enableAsteroids;
        }
        set
        {
            enableAsteroids = value;
        }
    }

    public bool ShieldsEnabled
    {
        get
        {
            return enableShields;
        }
        set
        {
            enableShields = value;
        }
    }

    public bool BonusesEnbled
    {
        get
        {
            return enableBonuses;
        }
        set
        {
            enableBonuses = value;
        }
    }
}
