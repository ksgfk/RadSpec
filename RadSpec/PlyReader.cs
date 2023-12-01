namespace RadSpec;

public enum PlyFormat
{
    Ascii,
    BinaryLittleEndian,
    BinaryBigEndian
}

public enum PlyType
{
    Int8,
    UInt8,
    Int16,
    UInt16,
    Int32,
    UInt32,
    Float32,
    Float64,

    Char,
    UChar,
    Short,
    UShort,
    Int,
    UInt,
    Float,
    Double,

    List
}

public class PlyElement
{
    public string? Name { get; internal set; }
    public List<PlyProperty> Properties { get; } = [];
}

public class PlyProperty
{

}

public class PlyHeader
{
    public string? Magic { get; internal set; }
    public List<string> Commits { get; } = [];
    public PlyFormat? Format { get; internal set; }
    public string? Version { get; internal set; }
}

public class PlyReader
{
    private Stream _stream;
    private PlyHeader? _header;

    public PlyHeader? Header { get => _header; set => _header = value; }

    public PlyReader(Stream stream)
    {
        _stream = stream;
    }

    public void ReadHeader()
    {
        StreamReader sr = new(_stream);
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
                    header.Commits.Add(opts.Skip(1).Aggregate((work, next) => work + " " + next));
                    break;
                case "format":
                    if (opts.Length >= 3)
                    {
                        header.Format = opts[1] switch
                        {
                            "ascii" => PlyFormat.Ascii,
                            "binary_little_endian" => PlyFormat.BinaryLittleEndian,
                            "binary_big_endian" => PlyFormat.BinaryBigEndian,
                            _ => null
                        };
                        header.Version = opts[2];
                    }
                    break;
                case "end_header":
                    isEnd = true;
                    break;
            }
            Console.WriteLine(data);
        }
        Console.WriteLine(_stream.Position);
        _header = header;
    }
}
