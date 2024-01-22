namespace RadSpec;

/// <summary>
/// https://pbr-book.org/4ed/Cameras_and_Film/Camera_Interface
/// https://pbr-book.org/4ed/Cameras_and_Film/Film_and_Imaging
/// 
/// 相机捕获光线后，传感器对其进行测量。
/// 大多数现代相机使用被划分为像素的固态传感器，每个像素计算一定时间内针对某个波长范围到达的光子数量
/// 
/// 由于光路可逆，可以从相机出发逆向追踪，因此相机具有SampleRay函数来生成光线
/// 光栅化渲染中有这些空间：模型空间、世界空间、相机空间、齐次空间和NDC，这些坐标系在这里也是通用的
/// 传统光栅化渲染在相机空间中进行大部分计算，因为很多主流游戏引擎都是三角形一路从模型空间转换到相机空间，再进行着色计算
/// 但是光线追踪渲染器大多是在世界空间中执行求交和着色（虽然着色时会变换到法线空间）
/// 
/// camera measurement equation，描述了像素捕获的能量取决于入射辐亮度、像素面积、出瞳面积和曝光时间。不过大多数相机不会用这个公式建模，太复杂了
/// 对于数字传感器，像素的颜色测量可以使用谱响应曲线来建模，叫做sensor response function
/// </summary>
public interface ICamera
{
    IFilm Film { get; }

    ISampler Sampler { get; }

    Ray3f SampleRay(float time, float xi1, Vector2f xi2, Vector2f xi3);

    SampledWavelength SampleWavelengths(float xi);
}
