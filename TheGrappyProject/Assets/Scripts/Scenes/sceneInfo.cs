
[System.Serializable]
public struct sceneInfo : System.IEquatable<sceneInfo>
{
	public string name;
	public string path;
	public int buildIndex;
	public bool needsInitialisation;

	public sceneInfo(string name, string path, int buildIndex, bool needsInitialisation = false)
	{
		this.name = name;
		this.path = path;
		this.buildIndex = buildIndex;
		this.needsInitialisation = needsInitialisation;
	}
	//need to manualy set "needsInitialisation" paramater
	public static explicit operator sceneInfo(UnityEngine.SceneManagement.Scene scene)
	{
		return new sceneInfo
		{
			name = scene.name,
			path = scene.path,
			buildIndex = scene.buildIndex,
			needsInitialisation = false
		};
	}
	public bool Equals(sceneInfo other)
	{
		return path == other.path;
	}
}