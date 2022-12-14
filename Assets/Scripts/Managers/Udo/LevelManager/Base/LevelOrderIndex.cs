
[System.Serializable]
public class LevelOrderIndex
{
    public int LevelID;
    public string LevelName;

    public LevelOrderIndex()
    {

    }

    public LevelOrderIndex(int levelID, string levelName)
    {
        LevelID = levelID;
        LevelName = levelName;
    }
}
