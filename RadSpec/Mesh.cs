namespace RadSpec;

public class Mesh
{
    public required Vector3f[] Positions { get; set; }
    public Vector3f[]? Normals { get; set; }
    public Vector2f[]? UV { get; set; }
    public Vector4f[]? Tangent { get; set; }
    public required int[] Indices { get; set; }
}
