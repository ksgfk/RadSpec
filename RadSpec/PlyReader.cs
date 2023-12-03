using System.Runtime.InteropServices;

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
}

public class PlyHeader
{
    public string Magic { get; internal set; } = null!;
    public List<string> Commits { get; } = [];
    public PlyFormat Format { get; internal set; } = PlyFormat.Unknown;
    public string Version { get; internal set; } = null!;
    public List<PlyElement> Elements { get; } = [];
}

public class PlyReader : IDisposable
{
    private Stream _stream;
    private StreamReader _headReader;
    private PlyHeader? _header;
    private bool disposedValue;

    public PlyHeader? Header { get => _header; set => _header = value; }

    public PlyReader(Stream stream)
    {
        _stream = stream;
        _headReader = new(_stream);
    }

    public void ReadHeader()
    {
        var sr = _headReader;
        string? data = sr.ReadLine();
        if (data == null)
        {
            return;
        }
        bool isEnd = false;
        PlyHeader header = new();
        header.Magic = data;
        while (!isEnd && (data = sr.ReadLine()) != null)
        {
            string[] opts = data.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (opts.Length == 0)
            {
                continue;
            }
            switch (opts[0])
            {
                case "comment":
                    header.Commits.Add(data[(data.IndexOf(' ') + 1)..]);
                    break;
                case "format":
                    if (opts.Length >= 3)
                    {
                        header.Format = opts[1] switch
                        {
                            "ascii" => PlyFormat.Ascii,
                            "binary_little_endian" => PlyFormat.BinaryLittleEndian,
                            "binary_big_endian" => PlyFormat.BinaryBigEndian,
                            _ => throw new ArgumentOutOfRangeException($"unknown ply format {opts[1]}")
                        };
                        header.Version = opts[2];
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException($"invalid ply header line: {data}");
                    }
                    break;
                case "element":
                    if (opts.Length >= 3)
                    {
                        PlyElement e = new()
                        {
                            Name = opts[1],
                            Length = int.Parse(opts[2])
                        };
                        header.Elements.Add(e);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException($"invalid ply header line: {data}");
                    }
                    break;
                case "property":
                    if (opts.Length >= 3)
                    {
                        PlyType type = StringToType(opts[1].ToLower());
                        PlyProperty p = new();
                        if (type == PlyType.Unknown)
                        {
                            p.DataType = PlyType.Unknown;
                            p.UserDataType = opts[1];
                        }
                        else
                        {
                            p.DataType = type;
                        }
                        if (type == PlyType.List)
                        {
                            if (opts.Length < 5)
                            {
                                throw new ArgumentOutOfRangeException($"invalid ply header line: {data}");
                            }
                            p.ListIndexType = StringToType(opts[2].ToLower());
                            p.ListElementType = StringToType(opts[3].ToLower());
                            p.Name = opts[4];
                        }
                        else
                        {
                            p.Name = opts[2];
                        }
                        if (header.Elements.Count == 0)
                        {
                            throw new ArgumentOutOfRangeException($"invalid ply header line: {data}");
                        }
                        header.Elements[^1].Properties.Add(p);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException($"unknown ply header line: {data}");
                    }
                    break;
                case "end_header":
                    isEnd = true;
                    break;
            }
        }
        _header = header;
    }

    public void ReadData()
    {
        if (_header == null)
        {
            throw new InvalidOperationException();
        }
        switch (_header.Format)
        {
            case PlyFormat.Ascii:
                break;
            case PlyFormat.BinaryLittleEndian:
                ReadLittleEndian();
                break;
            case PlyFormat.BinaryBigEndian:
                break;
        }
    }

    private void ReadLittleEndian()
    {
        Span<byte> buffer = stackalloc byte[8];
        foreach (var i in _header!.Elements)
        {
            bool hasList = false;
            int stride = 0;
            foreach (var j in i.Properties)
            {
                hasList = j.DataType == PlyType.List;
                stride += TypeToByteCount(j.DataType);
            }
            if (hasList)
            {
                List<byte> data = new(4096);
                for (int j = 0; j < i.Length; j++)
                {
                    foreach (var k in i.Properties)
                    {
                        if (k.DataType == PlyType.List)
                        {
                            int length;
                            {
                                int lengthByte = TypeToByteCount(k.ListIndexType);
                                Span<byte> lengthData = buffer[..lengthByte];
                                if (_stream.Read(lengthData) != lengthByte)
                                {
                                    throw new ArgumentOutOfRangeException($"ply data error, expect:{lengthByte}");
                                }
                                length = k.ListIndexType switch
                                {
                                    PlyType.Int8 => lengthData[0],
                                    PlyType.UInt8 => lengthData[0],
                                    PlyType.Int16 => MemoryMarshal.Cast<byte, short>(lengthData)[0],
                                    PlyType.UInt16 => MemoryMarshal.Cast<byte, ushort>(lengthData)[0],
                                    PlyType.Int32 => MemoryMarshal.Cast<byte, int>(lengthData)[0],
                                    PlyType.UInt32 => (int)MemoryMarshal.Cast<byte, uint>(lengthData)[0],
                                    _ => throw new ArgumentOutOfRangeException($"ply unknown list index type: {k.ListIndexType}")
                                };
                            }
                            for (int l = 0; l < length; l++)
                            {
                                int elementType = TypeToByteCount(k.ListElementType);
                                if (_stream.Read(buffer[..elementType]) != elementType)
                                {
                                    throw new ArgumentOutOfRangeException($"ply data error, expect:{elementType}");
                                }
                                data.AddRange(buffer[..elementType]);
                            }
                        }
                        else
                        {
                            int byteCount = TypeToByteCount(k.DataType);
                            int readCount = _stream.Read(buffer[..byteCount]);
                            if (byteCount != readCount)
                            {
                                throw new ArgumentOutOfRangeException($"ply data error, expect:{byteCount}, actully:{readCount}");
                            }
                            data.AddRange(buffer[..byteCount]);
                        }
                    }
                }
                i.Data = [.. data];
            }
            else
            {
                long byteCount = (long)stride * i.Length;
                if (byteCount >= int.MaxValue)
                {
                    throw new ArgumentOutOfRangeException($"ply data too large: {byteCount}");
                }
                int allLength = (int)byteCount;
                byte[] data = new byte[allLength];
                int readLength = _stream.Read(data);
                if (allLength != readLength)
                {
                    throw new ArgumentOutOfRangeException($"ply data error, expect:{allLength}, actully:{readLength}");
                }
                i.Data = data;
            }
        }
        Console.WriteLine($"now pos: {_stream.Position}, len: {_stream.Length}");
    }

    private static PlyType StringToType(string str)
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
        if (!disposedValue)
        {
            if (disposing)
            {
                try
                {
                    _headReader.Dispose();
                    _stream.Dispose();
                }
                finally
                {
                    _headReader = null!;
                    _stream = null!;
                }
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
