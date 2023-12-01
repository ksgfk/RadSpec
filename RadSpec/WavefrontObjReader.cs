using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace RadSpec;

public struct WavefrontObjFace
{
    public int V1, V2, V3;
    public int Vt1, Vt2, Vt3;
    public int Vn1, Vn2, Vn3;

    public override readonly string ToString()
    {
        return $"f {V1}/{Vt1}/{Vn1} {V2}/{Vt2}/{Vn2} {V3}/{Vt3}/{Vn3}";
    }
}

public class WavefrontObjReader : IDisposable
{
    private TextReader _reader;
    private bool _disposedValue;
    private readonly List<Vector3> _positions = [];
    private readonly List<Vector3> _normals = [];
    private readonly List<Vector2> _uvs = [];
    private readonly List<WavefrontObjFace> _faces = [];
    private string? _errorMessage;

    public List<Vector3> Positions => _positions;
    public List<Vector3> Normals => _normals;
    public List<Vector2> UVs => _uvs;
    public List<WavefrontObjFace> Faces => _faces;
    public string? ErrorMessage => _errorMessage;

    public WavefrontObjReader(Stream stream)
    {
        _reader = new StreamReader(stream);
    }

    public WavefrontObjReader(StringReader reader)
    {
        _reader = reader;
    }

    public void Read()
    {
        string str = _reader.ReadToEnd();
        ReadOnlySpan<char> data = str;
        int line = 0;
        StringBuilder errBuilder = new();
        do
        {
            int lineIdx = data.IndexOfAny('\r', '\n');
            ReadOnlySpan<char> now;
            if (lineIdx == -1)
            {
                now = data;
            }
            else
            {
                int nextIdx = lineIdx;
                char matchedChar = data[nextIdx];
                if (matchedChar == '\r')
                {
                    if (nextIdx < data.Length - 1)
                    {
                        if (data[nextIdx + 1] == '\n')
                        {
                            nextIdx++;
                        }
                    }
                }
                now = data[0..lineIdx];
                data = data[(nextIdx + 1)..];
            }
            line++;
            Parse(now, line, errBuilder);
            if (lineIdx == -1)
            {
                break;
            }
        } while (true);
        if (errBuilder.Length != 0)
        {
            _errorMessage = errBuilder.ToString();
        }
    }

    private void Parse(ReadOnlySpan<char> line, int lineNumber, StringBuilder errBuilder)
    {
        line = line.Trim();
        if (line.Length == 0 || (line.Length > 0 && line[0] == '#'))
        {
            return;
        }
        ReadOnlySpan<char> cmd;
        ReadOnlySpan<char> data;
        {
            int cmdEnd = line.IndexOf(' ');
            if (cmdEnd == -1)
            {
                errBuilder.Append("at ").Append(lineNumber).AppendLine(": cannot parse any data");
                return;
            }
            cmd = line[..cmdEnd];
            data = line[cmdEnd..].TrimStart();
        }
        switch (cmd)
        {
            case "v":
                {
                    Vector3 pos;
                    if (TrySplitSpace3Float(data, out float x, out float y, out float z))
                    {
                        pos = new(x, y, z);
                    }
                    else
                    {
                        errBuilder.Append("at ").Append(lineNumber).Append(": cannot parse position ").Append(data).AppendLine();
                        pos = Vector3.Zero;
                    }
                    _positions.Add(pos);
                    break;
                }
            case "vn":
                {
                    Vector3 nor;
                    if (TrySplitSpace3Float(data, out float x, out float y, out float z))
                    {
                        nor = new(x, y, z);
                    }
                    else
                    {
                        errBuilder.Append("at ").Append(lineNumber).Append(": cannot parse normal ").Append(data).AppendLine();
                        nor = Vector3.Zero;
                    }
                    _normals.Add(nor);
                    break;
                }
            case "vt":
                {
                    Vector2 uv;
                    if (TrySplitSpace2Float(data, out float x, out float y))
                    {
                        uv = new(x, y);
                    }
                    else
                    {
                        errBuilder.Append("at ").Append(lineNumber).Append(": cannot parse uv ").Append(data).AppendLine();
                        uv = Vector2.Zero;
                    }
                    _uvs.Add(uv);
                    break;
                }
            case "f":
                {
                    if (!TryParseFace(data, out var face))
                    {
                        errBuilder.Append("at ").Append(lineNumber).Append(": cannot parse face ").Append(data).AppendLine();
                        face = new();
                    }
                    _faces.Add(face);
                    break;
                }
            case "vp":
            case "l":
            case "mtllib":
            case "usemtl":
            case "o":
            case "g":
            case "s":
                break;
            default:
                errBuilder.Append("at ").Append(lineNumber).Append(": unknown cmd ").Append(cmd).AppendLine();
                break;
        }
    }

    private static bool TrySplitSpace2Float(ReadOnlySpan<char> data, out float x, out float y)
    {
        Unsafe.SkipInit(out x);
        Unsafe.SkipInit(out y);
        int a = data.IndexOf(' ');
        if (a == -1 || a == data.Length - 1)
        {
            return false;
        }
        ReadOnlySpan<char> m = data[..a];
        ReadOnlySpan<char> n = data[(a + 1)..].TrimStart();
        {
            int b = n.IndexOf(' ');
            if (b != -1)
            {
                n = n[..b];
            }
        }
        if (!float.TryParse(m, out x) || !float.TryParse(n, out y))
        {
            return false;
        }
        return true;
    }

    private static bool TrySplitSpace3Float(ReadOnlySpan<char> data, out float x, out float y, out float z)
    {
        Unsafe.SkipInit(out x);
        Unsafe.SkipInit(out y);
        Unsafe.SkipInit(out z);
        int a = data.IndexOf(' ');
        if (a == -1 || a == data.Length - 1)
        {
            return false;
        }
        ReadOnlySpan<char> m = data[..a];
        ReadOnlySpan<char> n = data[(a + 1)..].TrimStart();
        ReadOnlySpan<char> o;
        {
            int b = n.IndexOf(' ');
            if (b == -1 || b == n.Length - 1)
            {
                return false;
            }
            ReadOnlySpan<char> t = n[..b];
            o = n[(b + 1)..].TrimStart();
            n = t;
            int c = o.IndexOf(' ');
            if (c != -1)
            {
                o = o[..c];
            }
        }
        if (!float.TryParse(m, out x) || !float.TryParse(n, out y) || !float.TryParse(o, out z))
        {
            return false;
        }
        return true;
    }

    private static bool TryParseFace(ReadOnlySpan<char> data, out WavefrontObjFace face)
    {
        Unsafe.SkipInit(out face);
        int a = data.IndexOf(' ');
        if (a == -1 || a == data.Length - 1)
        {
            return false;
        }
        ReadOnlySpan<char> m = data[..a];
        ReadOnlySpan<char> n = data[(a + 1)..].TrimStart();
        ReadOnlySpan<char> o;
        {
            int b = n.IndexOf(' ');
            if (b == -1 || b == n.Length - 1)
            {
                return false;
            }
            ReadOnlySpan<char> t = n[..b];
            o = n[(b + 1)..].TrimStart();
            n = t;
            int c = o.IndexOf(' ');
            if (c != -1)
            {
                o = o[..c];
            }
        }
        {
            if (TryParseFaceElement(m, out var p, out var uv, out var n_))
            {
                face.V1 = p; face.Vt1 = uv; face.Vn1 = n_;
            }
            else
            {
                return false;
            }
        }
        {
            if (TryParseFaceElement(n, out var p, out var uv, out var n_))
            {
                face.V2 = p; face.Vt2 = uv; face.Vn2 = n_;
            }
            else
            {
                return false;
            }
        }
        {
            if (TryParseFaceElement(o, out var p, out var uv, out var n_))
            {
                face.V3 = p; face.Vt3 = uv; face.Vn3 = n_;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private static bool TryParseFaceElement(ReadOnlySpan<char> data, out int p, out int uv, out int n)
    {
        int a = data.IndexOf('/');
        p = 0;
        uv = 0;
        n = 0;
        bool result;
        if (a == -1)
        {
            //f v1 v2 v3
            result = int.TryParse(data, out p);
        }
        else
        {
            if (a == data.Length - 1)
            {
                result = false;
            }
            else
            {
                ReadOnlySpan<char> x = data[..a];
                ReadOnlySpan<char> y = data[(a + 1)..].TrimStart();
                int b = y.IndexOf('/');
                if (b == -1)
                {
                    //f v1/vt1 v2/vt2 v3/vt3
                    result = int.TryParse(x, out p) && int.TryParse(y, out uv);
                }
                else if (b == 0)
                {
                    //f v1//vn1 v2//vn2 v3//vn3
                    result = int.TryParse(x, out p) && int.TryParse(y[1..], out n);
                }
                else
                {
                    //f v1/vt1/vn1 v2/vt2/vn2 v3/vt3/vn3
                    ReadOnlySpan<char> z = y[(b + 1)..];
                    y = y[..b];
                    result = int.TryParse(x, out p) && int.TryParse(y, out uv) && int.TryParse(z, out n);
                }
            }
        }
        return result;
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                try
                {
                    _reader.Dispose();
                }
                finally
                {
                    _reader = null!;
                }
            }
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
