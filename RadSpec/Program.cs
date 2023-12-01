using RadSpec;

string a = "f     1//1          2/2/33             3/4        ";
using StringReader reader = new(a);
using WavefrontObjReader obj = new(reader);
obj.Read();
Console.WriteLine(obj.ErrorMessage);
foreach (var i in obj.Faces)
{
    Console.WriteLine(i);
}
