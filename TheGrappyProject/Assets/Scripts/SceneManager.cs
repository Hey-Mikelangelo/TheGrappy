using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu (fileName = "SceneManager", menuName = "Game/SceneManager")]
public class SceneManager : ScriptableObject {
    public string scenesFolder;
    public string enumFolder;
    public Scene scenes;
    public Dictionary<Scene, string> SceneName; 
    List<string> ScenesNames;
    private void OnEnable () {
        ScenesNames = new List<string>();
        // DirectoryInfo dirInfo = new DirectoryInfo (scenesFolder);
        // FileInfo[] fileInfo = dirInfo.GetFiles ();
        // foreach (var file in fileInfo) {
        //     Debug.Log (file);
        // }
    }
    public void RefreshScenes () {
		List<SceneAccessHolderSO.SceneInfo> allScene = new List<SceneAccessHolderSO.SceneInfo>();

		DirectoryInfo mainDir = new DirectoryInfo(scenesFolder);
		List<DirectoryInfo> DirInfos = mainDir.GetDirectories().ToList();
		DirInfos.Add(mainDir);
		int j = 0;
		//search in every subDirectory
		while (j < DirInfos.Count)
		{
			DirectoryInfo dirInfo = DirInfos.ElementAt(j);
			//get subDirectories from subDirectories
			DirectoryInfo[] SubDirs = dirInfo.GetDirectories();
			foreach (var subDir in SubDirs)
			{
				//add subDirectories to the list so they will be searched for scenes 
				DirInfos.Add(subDir);
			}
			FileInfo[] FileInfos = dirInfo.GetFiles();
			foreach (FileInfo fileInfo in FileInfos)
			{
				string name = fileInfo.Name;
				//if file is .unity and not a .unity.meta
				if (!name.Contains(".meta") && name.Contains(".unity"))
				{
					allScene.Add(
						new SceneAccessHolderSO.SceneInfo
						{
							name = Path.GetFileNameWithoutExtension(fileInfo.FullName),
							path = Path.GetFullPath(fileInfo.FullName),
							visible = true
						});

				}

			}
			j++;
		}
		//EnumGenerator.GenerateEnum("Scene", enumFolder, ScenesNames);
	}
}