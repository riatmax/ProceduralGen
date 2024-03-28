using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Gen : MonoBehaviour
{
    int xWidth;
    int zWidth;
    float xOrg;
    float zOrg;
    private Mesh mesh;
    private Vector3[] vertices;
    private MeshCollider meshCollider;

    float scale = 4.0F; 
    float heightScale = 5.0f; 

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;
    private MeshFilter myMeshFilter;
    private float[,] heigthMap;

    float[] hTerrain = new float[] { 1.0f, 0.6f, 0.3f };
    Color[] cTerrain = new Color[] { Color.gray, Color.white, Color.cyan };

    [SerializeField]
    private GameObject treePrefab, rockPrefab; 

    void Start()
    {
        xWidth = 40;
        zWidth = xWidth;
        xOrg = zOrg = 0;
        Generate(); 

        rend = GetComponent<Renderer>();
        heigthMap = new float[xWidth + 1, zWidth + 1];
        pix = new Color[(xWidth + 1) * (zWidth + 1)];
        noiseTex = new Texture2D(xWidth + 1, zWidth + 1);
        rend.material.mainTexture = noiseTex;

        CalcNoise();

        DeformMyGrid();

        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        mesh.RecalculateTangents();

        AddThings();

        gameObject.AddComponent<MeshCollider>();
    }

    void CalcNoise()
    {
        for (int i = 0, z = 0; z <= zWidth; z++)
        {
            for (int x = 0; x <= xWidth; x++, i++)
            {
                float xCoord = xOrg + (float)x / xWidth * scale;
                float zCoord = zOrg + (float)z / zWidth * scale;
                float sample = Mathf.PerlinNoise(xCoord, zCoord);
                heigthMap[x, z] = GetMyHeight(sample);
                pix[i] = GetMyColor(sample);

            }
        }
    }
    void AddThings()
    {
        Vector3 treeSize = treePrefab.GetComponent<MeshRenderer>().bounds.size;
        Vector3 rockSize = rockPrefab.GetComponent<MeshRenderer>().bounds.size;
        float treeHeight = treeSize.y;
        float rockHeight = rockSize.y;

        vertices = new Vector3[(xWidth + 1) * (zWidth + 1)];
        vertices = mesh.vertices;

        Vector3 thisPos;
        for (int i = 0, z = 0; z <= zWidth; z++)
        {
            for (int x = 0; x <= xWidth; x++, i++)
            {
                if (noiseTex.GetPixel(x, z) == Color.white) 
                {
                    if (Random.value < 0.1f) 
                    {
                        thisPos = vertices[i]; 
                        GameObject tree= Instantiate(treePrefab, thisPos, Quaternion.identity);
                        tree.transform.position = vertices[i] + 0.45f * Vector3.up * treeHeight / 4;
                    }
                    if (Random.value > .8f)
                    {
                        thisPos = vertices[i];
                        GameObject rock = Instantiate(rockPrefab, thisPos, Quaternion.identity);
                        rock.transform.localScale = .5f * Vector3.one;
                        rock.transform.position = vertices[i] + .45f * Vector3.up * rockHeight / 2;
                    }
                }
            }
        }
    }

    Color GetMyColor(float sample)
    {
        Color colorNow = new Color(0, 0, 0);
        int nTerrains = hTerrain.Length;
        for (int i = 0; i < nTerrains; i++)
        {
            if (sample < hTerrain[i])
            {
                colorNow = cTerrain[i];
            }

        }
        return colorNow;
    }

    float GetMyHeight(float sample)
    {
        float heightNow = 0;
        int nterrains = hTerrain.Length;
        for (int i = 0; i < nterrains - 1; i++)
        {
            if (sample < hTerrain[i])
            {
                heightNow = heightScale * sample;
            }
        }
        if (sample < hTerrain[nterrains - 1])
        {
            heightNow = heightScale * hTerrain[nterrains - 1];
        }
        return heightNow;
    }


    void DeformMyGrid()
    {
        vertices = mesh.vertices;
        float y = 0;
        for (int i = 0, z = 0; z <= zWidth; z++)
        {
            for (int x = 0; x <= xWidth; x++, i++)
            {
                vertices[i].y = 0;
                y = heigthMap[x, z];
                vertices[i] += new Vector3(0, y, 0);
            }
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

    private void Generate()
    {
        int xSize = xWidth;
        int zSize = zWidth;
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, 0, z);
                uv[i] = new Vector2((float)x / xSize, (float)z / zSize);
                tangents[i] = tangent;
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

        int[] triangles = new int[xSize * zSize * 6];
        for (int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
                mesh.triangles = triangles;
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
