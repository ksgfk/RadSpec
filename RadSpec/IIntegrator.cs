namespace RadSpec;

public interface IIntegrator
{
    bool IsComplete { get; }

    void Render(IScene scene, ICamera camera, int seed, int spp);
}
