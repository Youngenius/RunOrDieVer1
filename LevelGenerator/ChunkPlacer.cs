using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class ChunkPlacer : MonoBehaviour
{
    private Chunk _previousChunk;
    private Background _previousBackground;
    private int chunksSpawned = 0;

    public static Transform PointForSpiderToStop;

    [SerializeField] private int _possibleChunksNum;
    [SerializeField] private Transform Player;
    [SerializeField] private Background _background;
    [SerializeField] private Background _firstBackground;
    [SerializeField] private Chunk _firstChunk;
    [SerializeField] private Chunk[] _asphaltRoad_Prefabs;
    [SerializeField] private Chunk[] _grassRoad_Prefabs;
    [SerializeField] private Chunk[] _openingChunk;
    [SerializeField] private Chunk[] _closingChunk;

    private PoolMono<Chunk> openingChunkPool, closingChunkPool, asphaltChunkPool, grassChunkPool;

    public List<Chunk> _availableChunks = new List<Chunk>(); 
    private List<Chunk> spawnedChunks = new List<Chunk>();
    private List<Background> spawnedBackgrounds = new List<Background>();

    public string NewPlacedChunkName;

    public event EventHandler OnPrecipiceCreateEvent;

    private void Awake()
    {
        #region Pool Initialisation
        openingChunkPool = new PoolMono<Chunk>(_openingChunk, _openingChunk.Length, this.transform);
        closingChunkPool = new PoolMono<Chunk>(_closingChunk, _closingChunk.Length, this.transform);
        asphaltChunkPool = new PoolMono<Chunk>(_asphaltRoad_Prefabs, _asphaltRoad_Prefabs.Length, this.transform);
        grassChunkPool = new PoolMono<Chunk>(_grassRoad_Prefabs, _grassRoad_Prefabs.Length, this.transform);
        #endregion
        _availableChunks = _asphaltRoad_Prefabs.ToList();
        
        _previousBackground = _firstBackground;
        spawnedChunks.Add(_firstChunk);
    }

    private void Update()
    {
        float distanceToSpawn = 13f;

        _previousChunk = spawnedChunks[spawnedChunks.Count - 1];
        if (Player.position.x > _previousChunk.End.position.x - distanceToSpawn)
        {
            if (_previousChunk.typeOfChunk == "Start" || _previousChunk.typeOfChunk == "Asphalt") 
            {
                if (chunksSpawned < _possibleChunksNum)
                {
                    SpawnChunk(asphaltChunkPool);
                    chunksSpawned++;
                }

                else if (chunksSpawned >= _possibleChunksNum)
                {
                    SpawnChunk(closingChunkPool);
                    chunksSpawned = 0;
                }
            }

            else if (_previousChunk.typeOfChunk == "End" || _previousChunk.typeOfChunk == "Grass")
            {
                if (chunksSpawned < _possibleChunksNum)
                {
                    SpawnChunk(grassChunkPool);
                    chunksSpawned++;
                }

                else if (chunksSpawned >= _possibleChunksNum)
                {
                    SpawnChunk(openingChunkPool);
                    chunksSpawned = 0;

                    _availableChunks = _asphaltRoad_Prefabs.ToList();
                }
            }
        }

        if (Player.position.x > _previousBackground.End.position.x - distanceToSpawn)
        {
            SpawnBackground();
        }
    }


    private void SpawnChunk(PoolMono<Chunk> pool)
    {
        var chunk = pool.GetFreeElement();

        chunk.transform.position = new Vector2(_previousChunk.End.position.x - chunk.Start.localPosition.x - 1.8f, 
                                                    _previousChunk.transform.position.y);

        spawnedChunks.Add(chunk);

        if (spawnedChunks.Count > 3)
        {
            _availableChunks.Add(spawnedChunks[3]);
 
            spawnedChunks[0].gameObject.SetActive(false);
            spawnedChunks.RemoveAt(0);
        }

        if (chunk.name == "Precipice_Road")
        {
            OnPrecipiceCreateEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SpawnBackground()
    {
        Quaternion prevRot = _previousBackground.transform.rotation;
        Background nextBackground = Instantiate(_background, _previousBackground.transform.position, new Quaternion(prevRot.x,
                                                prevRot.y + 180, prevRot.z, prevRot.w));

        nextBackground.transform.position = new Vector2(_previousBackground.End.position.x - nextBackground.Start.localPosition.x + 4.1f,
                                                        _previousBackground.transform.position.y);
        spawnedBackgrounds.Add(nextBackground);
        
        Vector2 startPosition = nextBackground.Start.position;
        nextBackground.Start.position = nextBackground.End.position;
        nextBackground.End.position = startPosition;

        _previousBackground = nextBackground;

        if (spawnedBackgrounds.Count > 3)
        {
            DestroyImmediate(spawnedBackgrounds[0].gameObject);
            spawnedBackgrounds.RemoveAt(0);
        }
    }
}