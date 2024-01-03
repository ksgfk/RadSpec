using System.Numerics;

namespace RadSpec.Test;

[TestClass]
public class MatrixTest
{
    [TestMethod]
    public void Persp()
    {
        Transform4f a = Transform4f.Perspective(MathExt.Radian(30), 0.1f, 100f);
        Matrix4x4 b = Matrix4x4.CreatePerspectiveFieldOfViewLeftHanded(
            MathExt.Radian(30),
            1,
            0.1f,
            100);
        Assert.AreEqual(Matrix4x4.Transpose(b), a.Matrix.Value);
        Assert.IsTrue(Matrix4x4.Invert(b, out var c));
        Matrix4x4 d = Matrix4x4.Transpose(c);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Assert.AreEqual(d[i, j], a.InvMatrix.Value[i, j], 0.000001f);
            }
        }
    }

    [TestMethod]
    public void TransformApplyAffine()
    {
        Transform4f a = Transform4f.Perspective(MathExt.Radian(30), 0.1f, 100f);
        Vector3f b = new(2.333f, 5.1145f, -1.115f);
        Vector3f c = a.ApplyAffine(b);
        Assert.AreEqual(-7.808856f, c.X, 0.000001f);
        Assert.AreEqual(-17.118900f, c.Y, 0.000001f);
        Assert.AreEqual(1.090777f, c.Z, 0.000001f);
    }

    [TestMethod]
    public void TransformApplyNormal()
    {
        Transform4f a = Transform4f.Perspective(MathExt.Radian(30), 0.1f, 100f);
        Vector3f b = Vector3f.Normalize(new(2.333f, -5.1145f, 11.115f));
        Vector3f c = a.ApplyLinear(b);
        Assert.AreEqual(0.699028f, c.X, 0.000001f);
        Assert.AreEqual(-1.532438f, c.Y, 0.000001f);
        Assert.AreEqual(0.893256f, c.Z, 0.000001f);
    }
}
