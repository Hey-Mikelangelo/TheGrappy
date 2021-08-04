public class PersistentGameData : System.ICloneable
{
    public int version { get; }
    public int HighScore { get; set; }
    public int LastScore { get; set; }
    public string PlayerNickname { get; set; }

    public object Clone()
    {
        return (PersistentGameData)this.MemberwiseClone();
    }
}
