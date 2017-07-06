public class GameInfo
{
    private int gameTime = 60;
    private int mapSize = 12;
    private bool enableAsteroids = true;
    private bool enableShields = true;
    private bool enableBonuses = true;

    public GameInfo()
    {
        Instance = this;
    }

    public GameInfo(int time, int size, bool asteroids, bool shields, bool bonuses)
    {
        Instance = this;
        gameTime = time;
        mapSize = size;
        enableAsteroids = asteroids;
        enableBonuses = bonuses;
        enableShields = shields;
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
