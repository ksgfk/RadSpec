using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace RadSpec;

public enum PlyFormat
{
    Unknown,
    Ascii,
    BinaryLittleEndian,
    BinaryBigEndian
}

public enum PlyType
{
    Unknown,
    Int8,
    UInt8,
    Int16,
    UInt16,
    Int32,
    UInt32,
    Float32,
    Float64,

    Char = Int8,
    UChar = UInt8,
    Short = Int16,
    UShort = UInt16,
    Int = Int32,
    UInt = UInt32,
    Float = Float32,
    Double = Float64,

    List
}

public class PlyElement
{
    public string Name { get; internal set; } = null!;
    public int Length { get; internal set; }
    public List<PlyProperty> Properties { get; } = [];
    public byte[] Data { get; internal set; } = null!;
}

public class PlyProperty
{
    public string Name { get; internal set; } = null!;
    public PlyType DataType { get; internal set; }
    public string UserDataType { get; internal set; } = null!;
    public PlyType ListIndexType { get; internal set; }
    public PlyType ListElementType { get; internal set; }
    public int ListLengthHint { get; set; } = 0;
}

public class PlyHeader
{
    public string Magic { get; internal set; } = null!;
    public List<string> Commits { get; } = [];
    public PlyFormat Format { get; internal set; } = PlyFormat.Unknown;
    public string Version { get; internal set; } = null!;
    public List<PlyElement> Elements { get; } = [];
}

public class PlyReadException : Exception
{
    public PlyReadException(string? message = null) : base(message) { }
}

/// <summary>
/// 假设ply的list, 同一element呢的所有是list的property长度完全相等
/// List花在扩容上的时间很多
/// 
/// 样例:
/// using FileStream f = File.OpenRead("");
/// PlyReader r = new(f);
/// PlyHeader h = r.ReadHeader();
/// h.Elements.Find(i => i.Name == "face")!.Properties[0].ListLengthHint = 3;
/// r.ReadData();
/// </summary>
public class PlyReader : IDisposable
{
    private Stream _stream;
    private PlyHeader? _header;
    private bool _disposedValue;

    public PlyHeader? Header { get => _header; set => _header = value; }

    public PlyReader(Stream stream)
    {
        _stream = stream;
    }

    private string? ReadLine(List<byte> buffer)
    {
        buffer.Clear();
        int ch;
        while ((ch = _stream.ReadByte()) != -1)
        {
            if (ch == '\r' || ch == '\n')
            {
                long nowPos = _stream.Position;
                int next = _stream.ReadByte();
                if (next != '\n')
                {
                    _stream.Position = nowPos;
                }
                break;
            }
            buffer.Add((byte)ch);
        }
        return ch == -1 ? null : Encoding.UTF8.GetString(CollectionsMarshal.AsSpan(buffer));
    }

    private ReadOnlySpan<char> ReadNext(List<char> buffer)
    {
        buffer.Clear();
        int ch;
        while ((ch = _stream.ReadByte()) != -1)
        {
            if (ch != ' ' && ch != '\n' && ch != '\r')
            {
                buffer.Add((char)ch);
                break;
            }
        }
        while ((ch = _stream.ReadByte()) != -1)
        {
            if (ch == ' ' || ch == '\n' || ch == '\r')
            {
                break;
            }
            buffer.Add((char)ch);
        }
        return CollectionsMarshal.AsSpan(buffer);
    }

    public PlyHeader ReadHeader()
    {
        if (_header != null)
        {
            return _header;
        }
        List<byte> buffer = new(1024);
        PlyHeader header = new();
        bool hasNextLine = true;
        int lineNumber = 0;
        do
        {
            string? line = ReadLine(buffer);
            if (line == null)
            {
                break;
            }
            lineNumber++;
            ReadOnlySpan<char> cmd;
            ReadOnlySpan<char> data;
            {
                int cmdIdx = line.IndexOf(' ');
                ReadOnlySpan<char> ld = line.AsSpan();
                if (cmdIdx == -1)
                {
                    cmd = ld;
                    data = new();
                }
                else
                {
                    cmd = ld[..cmdIdx];
                    data = ld[(cmdIdx + 1)..].Trim();
                }
            }
            switch (cmd)
            {
                case "ply" or "PLY":
                    if (header.Magic == null)
                    {
                        header.Magic = "ply";
                    }
                    else
                    {
                        throw new PlyReadException($"at line {lineNumber}: magic number is not first line");
                    }
                    break;
                case "comment":
                    header.Commits.Add(data.ToString());
                    break;
                case "end_header":
                    hasNextLine = false;
                    break;
                case "format":
                    {
                        if (header.Version != null || header.Format != PlyFormat.Unknown)
                        {
                            throw new PlyReadException($"at line {lineNumber}: duplicate format");
                        }
                        int formatTypeIdx = data.IndexOf(' ');
                        if (formatTypeIdx == -1)
                        {
                            throw new PlyReadException($"at line {lineNumber}: invalid header format");
                        }
                        ReadOnlySpan<char> format = data[..formatTypeIdx];
                        ReadOnlySpan<char> ver = data[(formatTypeIdx + 1)..].TrimStart();
                        header.Format = format switch
                        {
                            "ascii" => PlyFormat.Ascii,
                            "binary_little_endian" => PlyFormat.BinaryLittleEndian,
                            "binary_big_endian" => PlyFormat.BinaryBigEndian,
                            _ => throw new PlyReadException($"at line {lineNumber}: unknown ply format")
                        };
                        header.Version = new string(ver);
                        break;
                    }
                case "element":
                    {
                        int nextIdx = data.IndexOf(' ');
                        if (nextIdx == -1)
                        {
                            throw new PlyReadException($"at line {lineNumber}: invalid header element");
                        }
                        ReadOnlySpan<char> name = data[..nextIdx];
                        ReadOnlySpan<char> num = data[(nextIdx + 1)..];
                        if (!int.TryParse(num, out int length))
                        {
                            throw new PlyReadException($"at line {lineNumber}: invalid length number");
                        }
                        PlyElement e = new()
                        {
                            Name = new string(name),
                            Length = length
                        };
                        header.Elements.Add(e);
                        break;
                    }
                case "property":
                    {
                        int a = data.IndexOf(' ');
                        if (a == -1)
                        {
                            throw new PlyReadException($"at line {lineNumber}: invalid header property");
                        }
                        ReadOnlySpan<char> typeD = data[..a];
                        ReadOnlySpan<char> ad = data[(a + 1)..];
                        PlyProperty p = new()
                        {
                            DataType = StringToType(typeD)
                        };
                        if (p.DataType == PlyType.List)
                        {
                            int b = ad.IndexOf(' ');
                            if (b == -1)
                            {
                                throw new PlyReadException($"at line {lineNumber}: invalid header property");
                            }
                            ReadOnlySpan<char> idxType = ad[..b];
                            ReadOnlySpan<char> bd = ad[(b + 1)..].TrimStart();
                            p.ListIndexType = StringToType(idxType);
                            if (p.ListIndexType == PlyType.Unknown)
                            {
                                throw new PlyReadException($"at line {lineNumber}: invalid header property");
                            }
                            int c = bd.IndexOf(' ');
                            if (c == -1)
                            {
                                throw new PlyReadException($"at line {lineNumber}: invalid header property");
                            }
                            ReadOnlySpan<char> listElemType = bd[..c];
                            ReadOnlySpan<char> listName = bd[(c + 1)..].TrimStart();
                            p.ListElementType = StringToType(listElemType);
                            if (p.ListElementType == PlyType.Unknown)
                            {
                                throw new PlyReadException($"at line {lineNumber}: invalid header property");
                            }
                            p.Name = new string(listName);
                        }
                        else
                        {
                            if (p.DataType == PlyType.Unknown)
                            {
                                p.UserDataType = new string(typeD);
                            }
                            p.Name = new string(ad.TrimStart());
                        }
                        if (header.Elements.Count == 0)
                        {
                            throw new PlyReadException($"at line {lineNumber}: invalid header property");
                        }
                        header.Elements[^1].Properties.Add(p);
                        int listCount = header.Elements[^1].Properties.Count(tmp => tmp.DataType == PlyType.List);
                        if (listCount > 1)
                        {
                            throw new PlyReadException($"at line {lineNumber}: element can only have one list");
                        }
                        break;
                    }
            }
        } while (hasNextLine);
        _header = header;
        return _header;
    }

    public void ReadData()
    {
        if (_header == null)
        {
            throw new PlyReadException("use ReadHeader() first");
        }
        switch (_header.Format)
        {
            case PlyFormat.Ascii:
                ReadAscii();
                break;
            case PlyFormat.BinaryLittleEndian:
                ReadBinary(PlyFormat.BinaryLittleEndian);
                break;
            case PlyFormat.BinaryBigEndian:
                ReadBinary(PlyFormat.BinaryBigEndian);
                break;
        }
    }

    private void ReadAscii()
    {
        List<char> buffer = new(128);
        foreach (PlyElement e in _header!.Elements)
        {
            List<byte> byteData = new();
            for (int i = 0; i < e.Length; i++)
            {
                foreach (PlyProperty p in e.Properties)
                {
                    ReadOnlySpan<char> txtData = ReadNext(buffer);
                    if (txtData.Length == 0)
                    {
                        throw new PlyReadException();
                    }
                    if (p.DataType == PlyType.List)
                    {
                        int length = p.ListIndexType switch
                        {
                            PlyType.Int8 => byte.Parse(txtData),
                            PlyType.UInt8 => byte.Parse(txtData),
                            PlyType.Int16 => short.Parse(txtData),
                            PlyType.UInt16 => ushort.Parse(txtData),
                            PlyType.Int32 => int.Parse(txtData),
                            PlyType.UInt32 => checked((int)uint.Parse(txtData)),
                            _ => throw new PlyReadException("bad list index type")
                        };
                        for (int j = 0; j < length; j++)
                        {
                            ReadOnlySpan<char> listData = ReadNext(buffer);
                            if (listData.Length == 0)
                            {
                                throw new PlyReadException();
                            }
                            ParseData(listData, p.ListElementType, byteData);
                        }
                    }
                    else
                    {
                        ParseData(txtData, p.DataType, byteData);
                    }
                }
            }
            e.Data = byteData.ToArray();
        }

        static void ParseData(ReadOnlySpan<char> txtData, PlyType type, List<byte> byteData)
        {
            switch (type)
            {
                case PlyType.Int8:
                    {
                        byteData.Add(byte.Parse(txtData));
                        break;
                    }
                case PlyType.UInt8:
                    {
                        byteData.Add(byte.Parse(txtData));
                        break;
                    }
                case PlyType.Int16:
                    {
                        short t = short.Parse(txtData);
                        byteData.AddRange(BitConverter.GetBytes(t));
                        break;
                    }
                case PlyType.UInt16:
                    {
                        ushort t = ushort.Parse(txtData);
                        byteData.AddRange(BitConverter.GetBytes(t));
                        break;
                    }
                case PlyType.Int32:
                    {
                        int t = int.Parse(txtData);
                        byteData.AddRange(BitConverter.GetBytes(t));
                        break;
                    }
                case PlyType.UInt32:
                    {
                        uint t = uint.Parse(txtData);
                        byteData.AddRange(BitConverter.GetBytes(t));
                        break;
                    }
                case PlyType.Float32:
                    {
                        float t = float.Parse(txtData);
                        byteData.AddRange(BitConverter.GetBytes(t));
                        break;
                    }
                case PlyType.Float64:
                    {
                        double t = double.Parse(txtData);
                        byteData.AddRange(BitConverter.GetBytes(t));
                        break;
                    }
                case PlyType.List:
                    break;
                default:
                    throw new PlyReadException();
            }
        }
    }

    private void ReadBinary(PlyFormat format)
    {
        Span<byte> buffer = stackalloc byte[8];
        bool needSwap = BitConverter.IsLittleEndian != (format == PlyFormat.BinaryLittleEndian);
        foreach (PlyElement e in _header!.Elements)
        {
            bool hasList = false;
            foreach (PlyProperty p in e.Properties)
            {
                if (p.DataType == PlyType.List)
                {
                    hasList = true;
                    break;
                }
            }
            if (hasList)
            {
                long stride = 0;
                foreach (PlyProperty p in e.Properties)
                {
                    if (p.DataType == PlyType.List)
                    {
                        long length;
                        if (p.ListLengthHint == 0)
                        {
                            long startPos = _stream.Position;
                            int idxSize = TypeToByteCount(p.ListIndexType);
                            Span<byte> bytes = buffer[..idxSize];
                            int readSize = _stream.Read(bytes);
                            if (idxSize != readSize)
                            {
                                throw new PlyReadException($"at {startPos} bad file");
                            }
                            if (needSwap)
                            {
                                bytes.Reverse();
                            }
                            length = DataToLength(p, bytes);
                            _stream.Position = startPos;
                            p.ListLengthHint = (int)length;
                        }
                        else
                        {
                            length = p.ListLengthHint;
                        }
                        int elemSize = TypeToByteCount(p.ListElementType);
                        long allSize = length * elemSize;
                        stride += allSize;
                    }
                    else if (p.DataType == PlyType.Unknown)
                    {
                        throw new PlyReadException("data type cannot be unknown");
                    }
                    else
                    {
                        stride += TypeToByteCount(p.DataType);
                    }
                }
                long size = e.Length * stride;
                if (size > int.MaxValue)
                {
                    throw new PlyReadException("file too large");
                }
                int intSize = (int)size;
                byte[] data = new byte[intSize];
                int ptr = 0;
                for (int i = 0; i < e.Length; i++)
                {
                    foreach (PlyProperty p in e.Properties)
                    {
                        if (p.DataType == PlyType.List)
                        {
                            int length = p.ListLengthHint;
                            int elemSize = TypeToByteCount(p.ListElementType);
                            int listByte = elemSize * length;
                            int idxSize = TypeToByteCount(p.ListIndexType);
                            {
                                Span<byte> bytes = buffer[..idxSize];
                                long startPos = _stream.Position;
                                int readSize = _stream.Read(bytes);
                                if (idxSize != readSize)
                                {
                                    throw new PlyReadException($"at {startPos} bad file");
                                }
                                if (needSwap)
                                {
                                    bytes.Reverse();
                                }
                                long t = DataToLength(p, bytes);
                                if (length != t)
                                {
                                    throw new PlyReadException($"at {startPos} All elements of the list must have the same length");
                                }
                            }
                            {
                                Span<byte> bytes = data.AsSpan(ptr, listByte);
                                long startPos = _stream.Position;
                                int readSize = _stream.Read(bytes);
                                if (listByte != readSize)
                                {
                                    throw new PlyReadException($"at {startPos} bad file");
                                }
                                if (needSwap)
                                {
                                    for (int j = 0; j < listByte; j += elemSize)
                                    {
                                        Span<byte> ele = bytes[j..(j + elemSize)];
                                        ele.Reverse();
                                    }
                                }
                            }
                            ptr += listByte;
                        }
                        else
                        {
                            long startPos = _stream.Position;
                            int elemSize = TypeToByteCount(p.DataType);
                            Span<byte> bytes = data.AsSpan(ptr, elemSize);
                            int readSize = _stream.Read(bytes);
                            if (elemSize != readSize)
                            {
                                throw new PlyReadException($"at {startPos} bad file");
                            }
                            if (needSwap)
                            {
                                bytes.Reverse();
                            }
                            ptr += elemSize;
                        }
                    }
                }
                e.Data = data;
            }
            else
            {
                int stride = 0;
                foreach (PlyProperty p in e.Properties)
                {
                    if (p.DataType == PlyType.Unknown)
                    {
                        throw new PlyReadException("data type cannot be unknown");
                    }
                    stride += TypeToByteCount(p.DataType);
                }
                long size = (long)e.Length * stride;
                if (size > int.MaxValue)
                {
                    throw new PlyReadException("file too large");
                }
                int intSize = (int)size;
                byte[] data = new byte[intSize];
                long startPos = _stream.Position;
                int readSize = _stream.Read(data);
                if (intSize != readSize)
                {
                    throw new PlyReadException($"at {startPos} bad file");
                }
                e.Data = data;
            }
        }

        static long DataToLength(PlyProperty p, Span<byte> bytes)
        {
            return p.ListIndexType switch
            {
                PlyType.Int8 => bytes[0],
                PlyType.UInt8 => bytes[0],
                PlyType.Int16 => Unsafe.ReadUnaligned<short>(ref bytes[0]),
                PlyType.UInt16 => Unsafe.ReadUnaligned<ushort>(ref bytes[0]),
                PlyType.Int32 => Unsafe.ReadUnaligned<int>(ref bytes[0]),
                PlyType.UInt32 => Unsafe.ReadUnaligned<uint>(ref bytes[0]),
                _ => throw new PlyReadException("bad list index type")
            };
        }
    }

    private static PlyType StringToType(ReadOnlySpan<char> str)
    {
        return str switch
        {
            "char" => PlyType.Char,
            "uchar" => PlyType.UChar,
            "short" => PlyType.Short,
            "ushort" => PlyType.UShort,
            "int" => PlyType.Int,
            "uint" => PlyType.UInt,
            "float" => PlyType.Float,
            "double" => PlyType.Double,
            "int8" => PlyType.Int8,
            "uint8" => PlyType.UInt8,
            "int16" => PlyType.Int16,
            "uint16" => PlyType.UInt16,
            "int32" => PlyType.Int32,
            "uint32" => PlyType.UInt32,
            "float32" => PlyType.Float32,
            "float64" => PlyType.Float64,
            "list" => PlyType.List,
            _ => PlyType.Unknown
        };
    }

    private static int TypeToByteCount(PlyType type)
    {
        return type switch
        {
            PlyType.Int8 => 1,
            PlyType.UInt8 => 1,
            PlyType.Int16 => 2,
            PlyType.UInt16 => 2,
            PlyType.Int32 => 4,
            PlyType.UInt32 => 4,
            PlyType.Float32 => 4,
            PlyType.Float64 => 8,
            _ => 0
        };
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                try
                {
                    _stream.Dispose();
                }
                finally
                {
                    _stream = null!;
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
