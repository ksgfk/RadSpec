using System.Numerics;

namespace RadSpec;

/// <summary>
/// 定义色彩空间不需要完整的光谱响应曲线, 可以使用色度坐标(chromaticity coordinates)
/// 三个色度坐标在色度图(chromaticity diagram)上, 组成一个三角形, 分别代表三原色
/// 
/// 除了三原色, 还需要一个白色点, 代表RGB三个值都拉满的情况, CIE标准照明D65是常用的白色光源值
/// 
/// 从XYZ转换RGB的矩阵可以预计算, 这个矩阵通过考虑三原色和白点之间的关系来找到
/// </summary>
public class RgbColorSpace
{
    private readonly Matrix3x3 _toRgb, _toXyz;

    public Matrix3x3 ToRgbMatrix => _toRgb;
    public Matrix3x3 ToXyzMatrix => _toXyz;

    public static RgbColorSpace Srgb { get; } = new(
        new(0.64f, 0.33f),
        new(0.3f, 0.6f),
        new(0.15f, 0.06f),
        Spectra.CieIllumD65);
    public static RgbColorSpace DciP3 { get; } = new(
        new(0.68f, 0.32f),
        new(0.265f, 0.69f),
        new(0.15f, 0.06f),
        Spectra.CieIllumD65);

    public RgbColorSpace(Vector2 r, Vector2 g, Vector2 b, ISpectrum illuminant)
    {
        Xyz w = illuminant.ToXyz();
        Xyz r_ = Xyz.FromChromaticityCoord(r);
        Xyz g_ = Xyz.FromChromaticityCoord(g);
        Xyz b_ = Xyz.FromChromaticityCoord(b);
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
