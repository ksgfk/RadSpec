namespace RadSpec;

public interface ICamera
{
    Ray3f SampleRay(float time, float xi1, Vector2f xi2, Vector2f xi3);

    SampledWavelength SampleWavelengths(float xi);
}
