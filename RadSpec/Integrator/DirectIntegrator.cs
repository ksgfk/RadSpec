namespace RadSpec.Integrator;

public class DirectIntegrator : IIntegrator
{
    public bool IsComplete => true;

    public void Render(IScene scene, ICamera camera)
    {
        throw new NotImplementedException();
    }
}
