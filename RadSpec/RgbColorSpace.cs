using System.Numerics;

namespace RadSpec;

public class RgbColorSpace
{
    private readonly Vector2 _r, _g, _b, _w;
    private readonly Matrix3x3 _toRgb, _toXyz;

    public Matrix3x3 ToRgbMatrix => _toRgb;
    public Matrix3x3 ToXyzMatrix => _toXyz;

    public RgbColorSpace(Vector2 r, Vector2 g, Vector2 b, ISpectrum illuminant)
    {
        _r = r;
        _g = g;
        _b = b;
        Xyz w = illuminant.ToXyz();
        _w = w.ChromaticityCoord();
        Xyz r_ = Xyz.FromChromaticityCoord(_r);
        Xyz g_ = Xyz.FromChromaticityCoord(_g);
        Xyz b_ = Xyz.FromChromaticityCoord(_b);
        Matrix3x3 rgb = new(
            r_.X, g_.X, b_.X,
            r_.Y, g_.Y, b_.Y,
            r_.Z, g_.Z, b_.Z);
        Matrix3x3 invRgb;
        {
            bool isSucc = Matrix3x3.Invert(rgb, out invRgb);
            if (!isSucc)
            {
                throw new InvalidOperationException();
            }
        }
        Xyz c = Matrix3x3.Mul(invRgb, w);
        _toXyz = Matrix3x3.Mul(rgb, Matrix3x3.FromDiagonal(c));
        {
            bool isSucc = Matrix3x3.Invert(_toXyz, out _toRgb);
            if (!isSucc)
            {
                throw new InvalidOperationException();
            }
        }
    }

    public Vector3 ToRgb(Xyz xyz) => Matrix3x3.Mul(_toRgb, xyz);

    public Xyz ToXyz(Vector3 rgb) => Matrix3x3.Mul(_toXyz, rgb);
}
