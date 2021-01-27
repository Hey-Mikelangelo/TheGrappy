using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneInfoSO : ScriptableObject
{
	public new string name;
	public string path;
	public int buildIndex;
	public bool needsInitialisation;

	public void setValues(string name, string path, int buildIndex, bool needsInitialisation = false)
	{
		this.name = name;
		this.path = path;
		this.buildIndex = buildIndex;
		this.needsInitialisation = needsInitialisation;
	}
	public SceneInfoSO() { }
	public SceneInfoSO(string name, string path, int buildIndex, bool needsInitialisation = false)
	{
		this.name = name;
		this.path = path;
		this.buildIndex = buildIndex;
		this.needsInitialisation = needsInitialisation;
	}
	public static explicit operator sceneInfo(SceneInfoSO info)
	{
		return new sceneInfo
		{
			name = info.name,
			path = info.path,
			buildIndex = info.buildIndex,
			needsInitialisation = info.needsInitialisation
		};
	}
	//need to manualy set "needsInitialisation" paramater
	public static explicit operator SceneInfoSO(Scene scene)
	{
		return new SceneInfoSO
		{
			name = scene.name,
			path = scene.path,
			buildIndex = scene.buildIndex,
			needsInitialisation = false
		};
	}
	public sceneInfo getInfo()
	{
		return new sceneInfo
		{
			name = name,
			path = path,
			buildIndex = buildIndex,
			needsInitialisation = needsInitialisation
		};
	}
	public bool Equals(SceneInfoSO other)
	{
		return (name == other.name && path == other.path && buildIndex == other.buildIndex);
	}
}
