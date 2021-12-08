using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
    Mesh mesh;
    public AnimationCurve heightCurve;
    public AnimationCurve edgeControl;
    private Vector3[] vertices;
    private int[] triangles;
    Color[] colors;

    public int width;
    public int height;

    public float scale;
    public int octaves;
    public float lacunarity;
    public int seed;

    public bool autoUpdate;


    public Gradient gradient;

    float minHeight;
    float maxHeight;
    
    //the tree prefab
    [SerializeField]
    GameObject tree;
    [SerializeField]
    GameObject iceLevel;
    [SerializeField]
    GameObject vegetation;

    static int treeNum = 200;
    List<Vector3> positionsList = new List<Vector3>();

    public void GenerateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();
    }

    public void CreateMesh()
    {
        CreateMeshShape();
        CreateTriangles();
        UpdateMesh();
    }

    private void CreateMeshShape()
    {
        maxHeight = 0;
        minHeight = 0;
        Vector2[] octaveOffsets = GetOffsetSeed();

        if (scale <= 0)
            scale = 0.0001f;

        vertices = new Vector3[(width + 1) * (height + 1)];

        for (int i = 0, z = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                float noiseHeight = GenerateNoiseHeight(z, x, octaveOffsets);
                //making sure the vertices near mesh edges are flat
                float yVal = noiseHeight * edgeControl.Evaluate(z) * edgeControl.Evaluate(x);
                vertices[i] = new Vector3(x, yVal, z);

                //finding possible tree placements
                if (yVal > iceLevel.transform.localPosition.y && treeNum!=0 &&Random.Range(1, 100) > 1)
                {
                    print(vertices[i]);
                    positionsList.Add(vertices[i]);
                }

                //Get maximum and minimum height of the terrain
                if (yVal> maxHeight)
                {
                    maxHeight = noiseHeight;
                }
                if (yVal < minHeight)
                {
                    minHeight = noiseHeight;
                }

                i++;
            }
        }
        print(maxHeight);
        print(iceLevel.transform.localPosition.y);
        SpawnTrees();
    }

    private Vector2[] GetOffsetSeed()
    {
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int o = 0; o < octaves; o++)
        {
            float offsetX = prng.Next(-100000, 100000);
            float offsetY = prng.Next(-100000, 100000);
            octaveOffsets[o] = new Vector2(offsetX, offsetY);
        }
        return octaveOffsets;
    }

    private float GenerateNoiseHeight(int mapHeight, int mapWidth, Vector2[] octaveOffsets)
    {

        float amplitude = 12;
        float frequency = 1;
        float persistence = 0.5f;
        float noiseHeight = 0;
                for (int y = 0; y < octaves; y++)
                {
                    float mapZ = mapHeight / scale * frequency + octaveOffsets[y].y;
                    float mapX = mapWidth / scale * frequency + octaveOffsets[y].x;

                    float perlinValue = (Mathf.PerlinNoise(mapZ, mapX)) * 2 - 1;
                    noiseHeight += heightCurve.Evaluate(perlinValue) * amplitude;
                    frequency *= lacunarity;
                    amplitude *= persistence;

                }
        return noiseHeight;
    }

    private void CreateTriangles()
    {
        triangles = new int[width * height * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + width+ 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert +width + 1;
                triangles[tris + 5] = vert + width + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float vertexHeight = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(vertexHeight);
               i++;
            }
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2((float)vertices[i].x / width, (float)vertices[i].z/height);
        }
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        GetComponent<MeshCollider>().sharedMesh = mesh;

        gameObject.transform.localScale = new Vector3(10,10,10);

    }
    private List<GameObject> GenerateTrees() {
        List<GameObject> trees = new List<GameObject>();
        for (int i =0; i < treeNum; i++)
        {
            GameObject newTree = Instantiate(tree, Vector3.zero, Quaternion.Euler(new Vector3(-90, 0, 0)));
            newTree.transform.parent = vegetation.transform;
            trees.Add(newTree);
            newTree.SetActive(false);
        }
        return trees;

    }
    private void SpawnTrees()
    {
        List<GameObject> trees = GenerateTrees();
        for (int t = 0; t < trees.Count; t++)
        {
            int positionIndex = Random.Range(1, positionsList.Count);
            trees[t].transform.localPosition = positionsList[positionIndex];
            trees[t].SetActive(true);
            }
        }

}
