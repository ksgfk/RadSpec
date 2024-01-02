namespace RadSpec.Test;

[TestClass]
public class VectorTest
{
    [TestMethod]
    public void Vec2fFloor()
    {
        Vector2f a = new(-1.414f, 2.732f);
        Vector2f b = new(-2, 2);
        Vector2f c = Vector2f.Floor(a);
        Assert.AreEqual(b, c);
    }

    [TestMethod]
    public void Vec2fCeiling()
    {
        Vector2f a = new(-1.414f, 2.732f);
        Vector2f b = new(-1, 3);
        Vector2f c = Vector2f.Ceiling(a);
        Assert.AreEqual(b, c);
    }

    [TestMethod]
    public void Vec2fHasNaN()
    {
        Vector2f a = new(-1.414f, 2.732f);
        Assert.IsFalse(Vector2f.HasNaN(a));
        Vector2f b = new(-1.414f, float.NaN);
        Assert.IsTrue(Vector2f.HasNaN(b));
    }

    [TestMethod]
    public void Vec2fHasInf()
    {
        Vector2f a = new(-1.414f, 2.732f);
        Assert.IsFalse(Vector2f.HasInf(a));
        Vector2f b = new(float.PositiveInfinity, float.NaN);
        Assert.IsTrue(Vector2f.HasInf(b));
    }

    [TestMethod]
    public void Vec2fSum()
    {
        Vector2f a = new(-1.414f, 2.732f);
        float b = -1.414f + 2.732f;
        Assert.AreEqual(b, Vector2f.Sum(a));
    }

    [TestMethod]
    public void Vec2fFma()
    {
        Vector2f a = new(1.414f, -1.732f);
        Vector2f b = new(2.333f, 1.312f);
        Vector2f c = new(-1.001f, 2.221f);
        Vector2f d = Vector2f.Fma(a, b, c);
        Vector2f e = a * b + c;
        Assert.AreEqual(e.X, d.X, 0.000001f);
        Assert.AreEqual(e.Y, d.Y, 0.000001f);
    }

    [TestMethod]
    public void Vec3fFloor()
    {
        Vector3f a = new(-1.414f, 2.732f, 4.115f);
        Vector3f b = new(-2, 2, 4);
        Vector3f c = Vector3f.Floor(a);
        Assert.AreEqual(b, c);
    }

    [TestMethod]
    public void Vec3fCeiling()
    {
        Vector3f a = new(-1.414f, 2.732f, 4.115f);
        Vector3f b = new(-1, 3, 5);
        Vector3f c = Vector3f.Ceiling(a);
        Assert.AreEqual(b, c);
    }

    [TestMethod]
    public void Vec3fHasNaN()
    {
        Vector3f a = new(-1.414f, 2.732f, 4.115f);
        Assert.IsFalse(Vector3f.HasNaN(a));
        Vector3f b = new(-1.414f, float.NaN, 4.115f);
        Assert.IsTrue(Vector3f.HasNaN(b));
    }

    [TestMethod]
    public void Vec3fHasInf()
    {
        Vector3f a = new(-1.414f, 2.732f, 4.115f);
        Assert.IsFalse(Vector3f.HasInf(a));
        Vector3f b = new(0, float.NaN, float.PositiveInfinity);
        Assert.IsTrue(Vector3f.HasInf(b));
    }

    [TestMethod]
    public void Vec3fSum()
    {
        Vector3f a = new(-1.414f, 2.732f, 4.115f);
        float b = -1.414f + 2.732f + 4.115f;
        Assert.AreEqual(b, Vector3f.Sum(a));
    }

    [TestMethod]
    public void Vec3fFma()
    {
        Vector3f a = new(1.414f, -1.732f, 0.004f);
        Vector3f b = new(2.333f, 1.312f, 1.212f);
        Vector3f c = new(-1.001f, 2.221f, 1.214f);
        Vector3f d = Vector3f.Fma(a, b, c);
        Vector3f e = a * b + c;
        Assert.AreEqual(e.X, d.X, 0.000001f);
        Assert.AreEqual(e.Y, d.Y, 0.000001f);
        Assert.AreEqual(e.Z, d.Z, 0.000001f);
    }

    [TestMethod]
    public void Vec4fFloor()
    {
        Vector4f a = new(-1.414f, 2.732f, 4.115f, 0.125f);
        Vector4f b = new(-2, 2, 4, 0);
        Vector4f c = Vector4f.Floor(a);
        Assert.AreEqual(b, c);
    }

    [TestMethod]
    public void Vec4fCeiling()
    {
        Vector4f a = new(-1.414f, 2.732f, 4.115f, 0.125f);
        Vector4f b = new(-1, 3, 5, 1);
        Vector4f c = Vector4f.Ceiling(a);
        Assert.AreEqual(b, c);
    }

    [TestMethod]
    public void Vec4fHasNaN()
    {
        Vector4f a = new(-1.414f, 2.732f, 4.115f, 0.125f);
        Assert.IsFalse(Vector4f.HasNaN(a));
        Vector4f b = new(-1.414f, float.NaN, 4.115f, float.NaN);
        Assert.IsTrue(Vector4f.HasNaN(b));
    }

    [TestMethod]
    public void Vec4fHasInf()
    {
        Vector4f a = new(-1.414f, 2.732f, 4.115f, 0.125f);
        Assert.IsFalse(Vector4f.HasInf(a));
        Vector4f b = new(0, float.NaN, float.PositiveInfinity, 0.125f);
        Assert.IsTrue(Vector4f.HasInf(b));
    }

    [TestMethod]
    public void Vec4fSum()
    {
        Vector4f a = new(-1.414f, 2.732f, 4.115f, 0.125f);
        float b = -1.414f + 2.732f + 4.115f + 0.125f;
        Assert.AreEqual(b, Vector4f.Sum(a));
    }

    [TestMethod]
    public void Vec4fFma()
    {
        Vector4f a = new(1.414f, -1.732f, 0.004f, -0.0015f);
        Vector4f b = new(2.333f, 1.312f, 1.212f, 0.125f);
        Vector4f c = new(-1.001f, 2.221f, 1.214f, 1.212f);
        Vector4f d = Vector4f.Fma(a, b, c);
        Vector4f e = a * b + c;
        Assert.AreEqual(e.X, d.X, 0.000001f);
        Assert.AreEqual(e.Y, d.Y, 0.000001f);
        Assert.AreEqual(e.Z, d.Z, 0.000001f);
        Assert.AreEqual(e.W, d.W, 0.000001f);
    }

    [TestMethod]
    public void Vec2dDot()
    {
        Vector2f a = new(-1.414f, 2.732f);
        Vector2f b = new(-2.2f, 4.121f);
        float c = Vector2f.Dot(a, b);
        Vector2d d = new(-1.414, 2.732);
        Vector2d e = new(-2.2, 4.121);
        double f = Vector2d.Dot(d, e);
        Assert.AreEqual(c, f, 0.000001);
    }

    [TestMethod]
    public void Vec2dLength()
    {
        Vector2f a = new(-1.414f, 2.732f);
        float b = Vector2f.Length(a);
        Vector2d c = new(-1.414, 2.732);
        double d = Vector2d.Length(c);
        Assert.AreEqual(b, d, 0.000001);
    }

    [TestMethod]
    public void Vec2dNormalize()
    {
        Vector2f a = new(-1.414f, 2.732f);
        Vector2f b = Vector2f.Normalize(a);
        Vector2d c = new(-1.414, 2.732);
        Vector2d d = Vector2d.Normalize(c);
        Assert.AreEqual(b.X, d.X, 0.000001);
        Assert.AreEqual(b.Y, d.Y, 0.000001);
    }

    [TestMethod]
    public void Vec3dDot()
    {
        Vector3f a = new(-1.414f, 2.732f, 0.015f);
        Vector3f b = new(-2.2f, 4.121f, 2.141f);
        float c = Vector3f.Dot(a, b);
        Vector3d d = new(-1.414, 2.732, 0.015);
        Vector3d e = new(-2.2, 4.121, 2.141);
        double f = Vector3d.Dot(d, e);
        Assert.AreEqual(c, f, 0.000001);
    }

    [TestMethod]
    public void Vec3dLength()
    {
        Vector3f a = new(-1.414f, 2.732f, 0.015f);
        float b = Vector3f.Length(a);
        Vector3d c = new(-1.414, 2.732, 0.015);
        double d = Vector3d.Length(c);
        Assert.AreEqual(b, d, 0.000001);
    }

    [TestMethod]
    public void Vec3dNormalize()
    {
        Vector3f a = new(-1.414f, 2.732f, 0.015f);
        Vector3f b = Vector3f.Normalize(a);
        Vector3d c = new(-1.414, 2.732, 0.015);
        Vector3d d = Vector3d.Normalize(c);
        Assert.AreEqual(b.X, d.X, 0.000001);
        Assert.AreEqual(b.Y, d.Y, 0.000001);
        Assert.AreEqual(b.Z, d.Z, 0.000001);
    }

    [TestMethod]
    public void Vec3fCross()
    {
        Vector3f a = new(1.414f, -1.732f, 3.445f);
        Vector3f b = new(2.333f, 1.312f, 2.141f);
        Vector3f ab = Vector3f.Cross(a, b);
        Vector3d c = new(1.414f, -1.732f, 3.445f);
        Vector3d d = new(2.333f, 1.312f, 2.141f);
        Vector3d cd = Vector3d.Cross(c, d);
        Assert.AreEqual(ab.X, cd.X, 0.000001);
        Assert.AreEqual(ab.Y, cd.Y, 0.000001);
        Assert.AreEqual(ab.Z, cd.Z, 0.000001);
    }

    [TestMethod]
    public void Vec4dDot()
    {
        Vector4f a = new(-1.414f, 2.732f, 0.015f, -2.145f);
        Vector4f b = new(-2.2f, 4.121f, 2.141f, 1.666f);
        float c = Vector4f.Dot(a, b);
        Vector4d d = new(-1.414, 2.732, 0.015, -2.145);
        Vector4d e = new(-2.2, 4.121, 2.141, 1.666);
        double f = Vector4d.Dot(d, e);
        Assert.AreEqual(c, f, 0.000001);
    }

    [TestMethod]
    public void Vec4dLength()
    {
        Vector4f a = new(-1.414f, 2.732f, 0.015f, -2.145f);
        float b = Vector4f.Length(a);
        Vector4d c = new(-1.414, 2.732, 0.015, -2.145);
        double d = Vector4d.Length(c);
        Assert.AreEqual(b, d, 0.000001);
    }

    [TestMethod]
    public void Vec4dNormalize()
    {
        Vector4f a = new(-1.414f, 2.732f, 0.015f, -2.145f);
        Vector4f b = Vector4f.Normalize(a);
        Vector4d c = new(-1.414, 2.732, 0.015, -2.145);
        Vector4d d = Vector4d.Normalize(c);
        Assert.AreEqual(b.X, d.X, 0.000001);
        Assert.AreEqual(b.Y, d.Y, 0.000001);
        Assert.AreEqual(b.Z, d.Z, 0.000001);
        Assert.AreEqual(b.W, d.W, 0.000001);
    }
}
