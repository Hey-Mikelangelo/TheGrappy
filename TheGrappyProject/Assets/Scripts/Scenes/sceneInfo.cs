[System.Serializable]
public struct sceneInfo : System.IEquatable<sceneInfo>
{
	public string name;
	public string path;
	public int buildIndex;

	public sceneInfo(string name, string path, int buildIndex)
	{
		this.name = name;
		this.path = path;
		this.buildIndex = buildIndex;
	}
	public bool Equals(sceneInfo other)
	{
		return path == other.path;
	}
}