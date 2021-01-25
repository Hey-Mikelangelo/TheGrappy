using UnityEngine;

public class SceneInfoSO : ScriptableObject
{
	public new string name;
	public string path;
	public int buildIndex;

	public void setValues(string name, string path, int buildIndex)
	{
		this.name = name;
		this.path = path;
		this.buildIndex = buildIndex;
	}
	public SceneInfoSO() { }
	public SceneInfoSO(string name, string path, int buildIndex)
	{
		this.name = name;
		this.path = path;
		this.buildIndex = buildIndex;
	}
	public static explicit operator sceneInfo(SceneInfoSO info)
	{
		return new sceneInfo
		{
			name = info.name,
			path = info.path,
			buildIndex = info.buildIndex
		};
	} 
	public sceneInfo getInfo()
	{
		return new sceneInfo
		{
			name = name,
			path = path,
			buildIndex = buildIndex
		};
	}
	public bool Equals(SceneInfoSO other)
	{
		return path == other.path;
	}
}
