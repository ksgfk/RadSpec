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
    private readonly Matrix3x3f _toRgb, _toXyz;

    public Matrix3x3f ToRgbMatrix => _toRgb;
    public Matrix3x3f ToXyzMatrix => _toXyz;

    /// <summary>
    /// ITU-R Rec. BT.709 linear RGB
    /// </summary>
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

    public RgbColorSpace(Vector2f r, Vector2f g, Vector2f b, ISpectrum illuminant)
    {
        Xyz w = illuminant.ToXyz();
        Xyz r_ = Xyz.FromChromaticityCoord(r);
        Xyz g_ = Xyz.FromChromaticityCoord(g);
        Xyz b_ = Xyz.FromChromaticityCoord(b);
        Matrix3x3f rgb = new(
            r_.X, g_.X, b_.X,
            r_.Y, g_.Y, b_.Y,
            r_.Z, g_.Z, b_.Z);
        Matrix3x3f invRgb;
        {
            bool isSucc = Invert(rgb, out invRgb);
            if (!isSucc)
            {
                throw new InvalidOperationException();
            }
        }
        Xyz c = Multiply(invRgb, w);
        _toXyz = Multiply(rgb, FromDiagonal(c));
        {
            bool isSucc = Invert(_toXyz, out _toRgb);
            if (!isSucc)
            {
                throw new InvalidOperationException();
            }
        }
    }

    public Vector3f ToRgb(Xyz xyz) => Multiply(_toRgb, xyz);

    public Xyz ToXyz(Vector3 rgb) => Multiply(_toXyz, rgb);
}
