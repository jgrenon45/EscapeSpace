using UnityEngine;

public class Draw : MonoBehaviour
{
    public int textureWidth = 28;  // Resolution of the drawing texture
    public int textureHeight = 28;
    public Color drawColor = Color.black;  // Brush color
    public int brushSize = 5;  // Brush size
    public bool canDraw;
    public Texture2D drawingTexture;

    private Renderer meshRenderer;
    private Vector2? lastDrawPosition = null;

    void Start()
    {
        // Get the mesh renderer
        meshRenderer = GetComponent<Renderer>();

        // Create a blank texture
        drawingTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        drawingTexture.wrapMode = TextureWrapMode.Clamp;
        drawingTexture.filterMode = FilterMode.Point;

        // Fill it with white
        ClearTexture();

        // Assign the texture to the material
        meshRenderer.material.SetTexture("_MainTex", drawingTexture);

    }

    void Update()
    {
        if (canDraw)
        {
            if (Input.GetMouseButton(0))  // Left mouse button to draw
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layerMask = LayerMask.GetMask("DrawingSurface");
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    Renderer renderer = hit.collider.GetComponent<Renderer>();
                    MeshCollider meshCollider = hit.collider as MeshCollider;

                    if (renderer != null && renderer.material.mainTexture != null && meshCollider != null)
                    {
                        // Get the mesh's scale to adjust UVs
                        Vector3 scale = hit.transform.localScale;

                        // Get local hit position
                        Vector3 localHit = hit.transform.InverseTransformPoint(hit.point);

                        // Adjust UVs based on the mesh's scale (since mesh is scaled, we need to adjust the texture mapping)
                        float uvX = Mathf.InverseLerp(-0.5f * scale.x, 0.5f * scale.x, localHit.x);
                        float uvY = Mathf.InverseLerp(-0.5f * scale.y, 0.5f * scale.y, localHit.y);

                        // Now map these UV coordinates to pixel coordinates on the texture
                        int x = Mathf.FloorToInt(uvX * drawingTexture.width);
                        int y = Mathf.FloorToInt(uvY * drawingTexture.height);

                        if (lastDrawPosition != null)
                        {
                            DrawLine((Vector2)lastDrawPosition, new Vector2(x, y)); // Fill in gaps
                        }
                        else
                        {
                            DrawAt(x, y); // First frame, just draw the point
                        }

                        lastDrawPosition = new Vector2(x, y); // Update last drawn position
                    }                 
                }
            }
            else
            {
                lastDrawPosition = null; // Reset when mouse is released
            }

            if (Input.GetKeyDown(KeyCode.C)) // Press "C" to clear the drawing
            {
                ClearTexture();
            }
        }
    }

    void DrawAt(int x, int y)
    {
        for (int i = -brushSize; i < brushSize; i++)
        {
            for (int j = -brushSize; j < brushSize; j++)
            {
                int drawX = Mathf.Clamp(x + i, 0, drawingTexture.width - 1);
                int drawY = Mathf.Clamp(y + j, 0, drawingTexture.height - 1);
                drawingTexture.SetPixel(drawX, drawY, drawColor);
            }
        }
        drawingTexture.Apply();
    }

    void DrawLine(Vector2 start, Vector2 end)
    {
        int x0 = (int)start.x;
        int y0 = (int)start.y;
        int x1 = (int)end.x;
        int y1 = (int)end.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = (x0 < x1) ? 1 : -1;
        int sy = (y0 < y1) ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            DrawAt(x0, y0); // Draw the pixel at (x0, y0)

            if (x0 == x1 && y0 == y1) break;
            int e2 = err * 2;

            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

    public void ClearTexture()
    {
        Color[] pixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.black; // White background
        }
        drawingTexture.SetPixels(pixels);
        drawingTexture.Apply();
    }
}
