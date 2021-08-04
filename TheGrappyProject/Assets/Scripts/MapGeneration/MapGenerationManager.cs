using UnityEngine;
using Zenject;

public class MapGenerationManager : MonoBehaviour
{
    public MapGenerator MapGenerator => mapGenerator;
    
    [SerializeField] private Vector2 clearStartAreaExtends;
    
    [Inject] private MapGenerator mapGenerator;
    [Inject] private Player player;
    [Inject] private SceneLoadingChannelSO sceneLoadingChannel;


    private Vector2Int prevPlayerChunk;
    private bool isInitialMapGenerated;
   
    public void Init()
    {
        isInitialMapGenerated = false;
        mapGenerator.onMapGenerated += MapGenerator_onMapGenerated;
        prevPlayerChunk = GetCurrentChunk();
        mapGenerator.UpdateChunks(GetCurrentChunk());
    }

    public void Finish()
    {
        mapGenerator.onMapGenerated -= MapGenerator_onMapGenerated;
    }

    private void MapGenerator_onMapGenerated()
    {
        if(isInitialMapGenerated == false)
        {
            isInitialMapGenerated = true;
            mapGenerator.ClearAreaBox(player.transform.position, clearStartAreaExtends);
            int sceneIndex = player.gameObject.scene.buildIndex;
            sceneLoadingChannel.SetSceneInited(sceneIndex);
        }
    }

    public void UpdateTick()
    {
        Vector2Int currentPlayerChunk = GetCurrentChunk();
        if(currentPlayerChunk != prevPlayerChunk)
        {
            mapGenerator.UpdateChunks(GetCurrentChunk());
        }
        prevPlayerChunk = currentPlayerChunk;
    }

    public void GenerateNewMapSeed()
    {
        MapGenerator.GenerateRandomPerlinOffset();    
    }

    private Vector2Int GetCurrentChunk()
    {
        return mapGenerator.WorldToChunk(player.transform.position);
    }
}
