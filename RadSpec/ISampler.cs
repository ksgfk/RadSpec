namespace RadSpec;

/**
 * https://pbr-book.org/4ed/Sampling_and_Reconstruction/Sampling_Theory
 * 为了计算数字图像中的离散像素值，需要对原始连续定义的图像函数进行采样。获取样本值集合并将其转换回连续函数的过程称为重建。
 * 采样和重建过程涉及近似，因此会引入称为走样的误差
 * 这种误差可以通过多种方式表现出来，包括锯齿状边缘或动画中的闪烁。
 * 出现这些错误的原因是采样过程无法从连续定义的图像函数中捕获所有信息。傅里叶分析可用于评估重构函数与原始函数之间的匹配质量
 * 傅里叶分析的基础之一是傅里叶变换，它表示频域（frequency domain）中的函数。（我们会说函数通常在空间域（spatial domain）中表达。）
 * 傅里叶变换描述了这样一个事实：大多数函数可以分解为移位正弦曲线的加权和。函数的这种频率空间表示可以深入了解其一些特征——正弦函数中的频率分布对应于原始函数中的频率分布
 * 总之傅里叶变换和逆变换是两个积分方程，描述函数的频域和时域互相转换。
 * 有一类特殊的频域空间表示，叫做狄拉克（Dirac）delta分布，这种分布在整个积分域上积分结果是1，但是任何自变量不是0的地方，对应函数值都是0
 * 因此，根据这种特性，将delta分布乘以一个函数f(x)再积分，结果就是f(0)
 * 这种delta分布不是初等函数，通常被认为是以原点为中心且宽度接近 0 的单位面积盒函数（box function）的极限
 *
 * 
 */
public interface ISampler
{
}
