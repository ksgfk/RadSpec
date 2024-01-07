namespace RadSpec;

/// <summary>
/// https://pbr-book.org/4ed/Cameras_and_Film/Film_and_Imaging#FilteringImageSamples
/// https://pbr-book.org/4ed/Sampling_and_Reconstruction/Image_Reconstruction
/// 
/// 滤波后的图像等于滤波函数乘以图像函数对图像坐标的积分 (https://pbr-book.org/4ed/Cameras_and_Film/Film_and_Imaging#eq:pixel-contribution-integral)
/// 该积分使用蒙特卡洛法可以重要性采样，无偏估计滤波后的图像
/// 
/// 图像重建的职责：
/// * 从图像样本集重建连续图像函数。
/// * 消除任何超过像素间距奈奎斯特极限的频率。
/// * 在像素位置对预过滤函数进行采样以计算最终像素值。
/// 
/// pbrt仅在像素位置对函数进行重采样，所以没有必要构造函数的显式表示。相反，可以使用单个过滤器函数组合前两个步骤。
/// 回想一下，如果原始函数以大于奈奎斯特频率的频率均匀采样并使用 sinc 滤波器重建，那么第一步中的重建函数将与原始图像函数完美匹配——这是一项壮举，因为我们只有点样本。 
/// 但由于图像函数几乎总是具有比采样率所能解释的频率更高的频率（由于边缘等），因此我们选择对其进行非均匀采样，来权衡噪音和走样。
/// 
/// 理想重建背后的理论取决于均匀间隔的样本。 尽管已经进行了许多尝试将该理论扩展到非均匀采样，但尚未找到解决此问题的公认方法。 此外，由于已知采样率不足以捕获函数，因此完美的重建是不可能的。
/// 采样理论领域的最新研究重新审视了重建问题，并明确承认在实践中通常无法实现完美的重建。这种观点的轻微转变催生了强大的新重建技术。
/// 特别是，重建理论的研究目标已经从完美重建转向开发重建技术，无论原始函数是否是带限的，都可以最小化重建函数和原始函数之间的误差。
/// （说人话就是，无法完美的重建原函数，但可以缩小误差）
/// 不存在单一的最佳过滤函数。为特定场景选择最佳方案需要结合定量评估和定性判断。因此pbrt提供了多种选择。
/// （说人话就是每种滤波函数都有局限性，针对特定场景要自己试）（啊这）
/// </summary>
public interface IImageReconstruction
{
    Vector2f Radius { get; }
    float Integral { get; }

    float Eval(Vector2f x);

    (Vector2f value, float weight) Sample(Vector2f xi);
}
