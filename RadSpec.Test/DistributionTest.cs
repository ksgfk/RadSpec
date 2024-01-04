namespace RadSpec.Test;

[TestClass]
public class DistributionTest
{
    [TestMethod]
    public void Piecewise1D()
    {
        PiecewiseConstant1D dist = new([1, 1, 2, 4, 8], 0, 1);
        {
            var (value, pdf, offset) = dist.Sample(0.5f);
            Assert.AreEqual(0.8f, value);
            Assert.AreEqual(2.5f, pdf);
            Assert.AreEqual(4, offset);
            Assert.AreEqual(pdf, dist.Pdf(value), 0.000001f);
        }
        {
            var (value, pdf, offset) = dist.Sample(0.11255f);
            Assert.AreEqual(0.36016f, value, 0.000001f);
            Assert.AreEqual(0.3125f, pdf, 0.000001f);
            Assert.AreEqual(1, offset, 0.000001f);
            Assert.AreEqual(pdf, dist.Pdf(value), 0.000001f);
        }
        {
            var (value, pdf, offset) = dist.Sample(0.41522f);
            Assert.AreEqual(0.732176f, value, 0.000001f);
            Assert.AreEqual(1.25f, pdf, 0.000001f);
            Assert.AreEqual(3, offset, 0.000001f);
            Assert.AreEqual(pdf, dist.Pdf(value), 0.000001f);
        }
        {
            var (value, pdf, offset) = dist.Sample(0.91526f);
            Assert.AreEqual(0.966104f, value, 0.000001f);
            Assert.AreEqual(2.5f, pdf, 0.000001f);
            Assert.AreEqual(4, offset, 0.000001f);
            Assert.AreEqual(pdf, dist.Pdf(value), 0.000001f);
        }
    }

    [TestMethod]
    public void Piecewise2D()
    {
        const float epsilon = 0.000001f;
        VectorEqualityComparer<Vector2f, float> cmp = new(epsilon);
        float[] f = [
            1,2,4,7,10,15,22,14,9,9,2,1,
            2,1,4,5,6,10,11,12,6,2,1,1,
            1,1,1,1,1,2,1,1,1,1,1,1,
            2,2,2,2,2,1,2,2,2,2,2,2,
            2,1,3,4,5,6,5,4,3,2,1,1];
        PiecewiseConstant2D dist = new(f, 12, 5, Vector2f.Zero, Vector2f.One);
        {
            var (value, pdf, offset) = dist.Sample(new(0.5f, 0.5f));
            Assert.AreEqual(new Vector2f(0.51893944f, 0.26229507f), value, cmp);
            Assert.AreEqual(2.869565f, pdf, epsilon);
            Assert.AreEqual(new Vector2i(6, 1), offset);
            Assert.AreEqual(pdf, dist.Pdf(value), epsilon);
        }
        {
            var (value, pdf, offset) = dist.Sample(new(0, 0));
            Assert.AreEqual(new Vector2f(0, 0), value, cmp);
            Assert.AreEqual(0.260870f, pdf, epsilon);
            Assert.AreEqual(new Vector2i(0, 0), offset);
            Assert.AreEqual(pdf, dist.Pdf(value), epsilon);
        }
        {
            var (value, pdf, offset) = dist.Sample(new(0.999999f, 0.999999f));
            Assert.AreEqual(new Vector2f(0.9999969f, 0.99999875f), value, cmp);
            Assert.AreEqual(0.260870f, pdf, epsilon);
            Assert.AreEqual(new Vector2i(11, 4), offset);
            Assert.AreEqual(pdf, dist.Pdf(value), epsilon);
        }
        {
            var (value, pdf, offset) = dist.Sample(new(0.114514f, 0.810f));
            Assert.AreEqual(new Vector2f(0.109742574f, 0.74173915f), value, cmp);
            Assert.AreEqual(0.521739f, pdf, epsilon);
            Assert.AreEqual(new Vector2i(1, 3), offset);
            Assert.AreEqual(pdf, dist.Pdf(value), epsilon);
        }
        {
            var (value, pdf, offset) = dist.Sample(new(0.212121f, 0.121212f));
            Assert.AreEqual(new Vector2f(0.38636348f, 0.05808075f), value, cmp);
            Assert.AreEqual(2.608696f, pdf, epsilon);
            Assert.AreEqual(new Vector2i(4, 0), offset);
            Assert.AreEqual(pdf, dist.Pdf(value), epsilon);
        }
        {
            var (value, pdf, offset) = dist.Sample(new(0.375f, 0.112f));
            Assert.AreEqual(new Vector2f(0.48333335f, 0.05366667f), value, cmp);
            Assert.AreEqual(3.913043f, pdf, epsilon);
            Assert.AreEqual(new Vector2i(5, 0), offset);
            Assert.AreEqual(pdf, dist.Pdf(value), epsilon);
        }
    }
}
