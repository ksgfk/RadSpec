using System.Numerics;

namespace RadSpec;

public class Mesh
{
    public required Vector3[] Positions { get; set; }
    public Vector3[]? Normals { get; set; }
    public Vector2[]? UV { get; set; }
    public Vector4[]? Tangent { get; set; }
    public required int[] Indices { get; set; }
}
