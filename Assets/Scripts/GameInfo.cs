public class GameInfo {

    public static GameInfo Instance
    {
        get;
        private set;
    }

    public int gameTime = 60;
    public bool enableAsteroids = true;
    //public bool oneShotMode = false;

    public GameInfo()
    {
        Instance = this;
    }

    
}
