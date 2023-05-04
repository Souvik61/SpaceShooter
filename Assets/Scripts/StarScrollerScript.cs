using UnityEngine;

public class StarScrollerScript : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Material mat = meshRenderer.material;
        Vector2 offset = mat.mainTextureOffset;
        offset.x += Time.deltaTime * xSpeed;
        offset.y += Time.deltaTime * ySpeed;
        mat.mainTextureOffset = offset;
    }
}
