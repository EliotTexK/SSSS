using UnityEngine;

public class GeneratePlanet : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale;

    void Start()
    {
        //Grabs texture from renderer
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.EnableKeyword ("_NORMALMAP");

        //Randomly Generated Properties
        scale = Random.Range(10, 20);  //Crater Scale
        renderer.material.color = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));  //Color
        renderer.material.SetTexture("_BumpMap", GenerateNoiseTexture());   //Crater Locations
    }

    Texture2D GenerateNoiseTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoordinate = (float)x / width * scale;
        float yCoordinate = (float)y / height * scale;

        float sample = Mathf.PerlinNoise(xCoordinate, yCoordinate);
        return new Color(sample, sample, sample);
    }
}
