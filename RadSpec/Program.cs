using RadSpec;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;

ColorSystem color = new()
{
    xRed = 0.64,
    yRed = 0.33,
    xGreen = 0.30,
    yGreen = 0.60,
    xBlue = 0.15,
    yBlue = 0.06,
    xWhite = 0.3127,
    yWhite = 0.3291,
    gamma = 0
};

// int row = 0;
// for (int t = 90000; t <= 99900; t += 20, row++)
// {
//     // double lambdaMax = 2.8977721e-3 / t;
//     // double normalizationFactor = 1 / Blackbody(lambdaMax * 1e9f, t);
//     double x = 0, y = 0, z = 0;
//     for (int lambda = Spectra.CIE1931LambdaMin; lambda <= Spectra.CIE1931LambdaMax; lambda++)
//     {
//         // double l = Blackbody(lambda, t) * normalizationFactor;
//         double l = Blackbody(lambda, t);
//         x += l * Spectra.CIE1931X[lambda - Spectra.CIE1931LambdaMin];
//         y += l * Spectra.CIE1931Y[lambda - Spectra.CIE1931LambdaMin];
//         z += l * Spectra.CIE1931Z[lambda - Spectra.CIE1931LambdaMin];
//     }
//     double xyz = x + y + z;
//     x /= xyz;
//     y /= xyz;
//     z /= xyz;
//     xyz_to_rgb(color, x, y, z, out double r, out double g, out double b);
//     gamma_correct(color, ref r);
//     gamma_correct(color, ref g);
//     gamma_correct(color, ref b);
//     Console.WriteLine($"{row} <{x} {y} {z}> <{r} {g} {b}>");

//     image.ProcessPixelRows(p =>
//     {
//         foreach (ref var i in p.GetRowSpan(row))
//         {
//             i.R = (ushort)(Math.Clamp(r, 0, 1) * ushort.MaxValue);
//             i.G = (ushort)(Math.Clamp(g, 0, 1) * ushort.MaxValue);
//             i.B = (ushort)(Math.Clamp(b, 0, 1) * ushort.MaxValue);
//             i.A = ushort.MaxValue;
//         }
//     });
// }

// image.SaveAsPng("/Users/admin/Desktop/test.png");

static double Blackbody(double lambda, double t)
{
    if (t <= 0) return 0;
    const double c = 299792458.0;
    const double h = 6.62606957e-34;
    const double kb = 1.3806488e-23;
    double l = lambda * 1e-9f;
    double le = (2 * h * c * c) / (Math.Pow(l, 5) * (Math.Exp((h * c) / (l * kb * t)) - 1));
    return le;
}

void xyz_to_rgb(ColorSystem cs,
                double xc, double yc, double zc,
               out double r, out double g, out double b)
{
    double xr, yr, zr, xg, yg, zg, xb, yb, zb;
    double xw, yw, zw;
    double rx, ry, rz, gx, gy, gz, bx, by, bz;
    double rw, gw, bw;

    xr = cs.xRed; yr = cs.yRed; zr = 1 - (xr + yr);
    xg = cs.xGreen; yg = cs.yGreen; zg = 1 - (xg + yg);
    xb = cs.xBlue; yb = cs.yBlue; zb = 1 - (xb + yb);

    xw = cs.xWhite; yw = cs.yWhite; zw = 1 - (xw + yw);

    /* xyz -> rgb matrix, before scaling to white. */

    rx = (yg * zb) - (yb * zg); ry = (xb * zg) - (xg * zb); rz = (xg * yb) - (xb * yg);
    gx = (yb * zr) - (yr * zb); gy = (xr * zb) - (xb * zr); gz = (xb * yr) - (xr * yb);
    bx = (yr * zg) - (yg * zr); by = (xg * zr) - (xr * zg); bz = (xr * yg) - (xg * yr);

    /* White scaling factors.
       Dividing by yw scales the white luminance to unity, as conventional. */

    rw = ((rx * xw) + (ry * yw) + (rz * zw)) / yw;
    gw = ((gx * xw) + (gy * yw) + (gz * zw)) / yw;
    bw = ((bx * xw) + (by * yw) + (bz * zw)) / yw;

    /* xyz -> rgb matrix, correctly scaled to white. */

    rx = rx / rw; ry = ry / rw; rz = rz / rw;
    gx = gx / gw; gy = gy / gw; gz = gz / gw;
    bx = bx / bw; by = by / bw; bz = bz / bw;

    /* rgb of the desired point */

    r = (rx * xc) + (ry * yc) + (rz * zc);
    g = (gx * xc) + (gy * yc) + (gz * zc);
    b = (bx * xc) + (by * yc) + (bz * zc);
}

void gamma_correct(ColorSystem cs, ref double c)
{
    double gamma;

    gamma = cs.gamma;

    if (gamma == 0)
    {
        /* Rec. 709 gamma correction. */
        double cc = 0.018;

        if (c < cc)
        {
            c *= ((1.099 * Math.Pow(cc, 0.45)) - 0.099) / cc;
        }
        else
        {
            c = (1.099 * Math.Pow(c, 0.45)) - 0.099;
        }
    }
    else
    {
        /* Nonlinear colour = (Linear colour)^(1/gamma) */
        c = Math.Pow(c, 1.0 / gamma);
    }
}

struct ColorSystem
{
    public double xRed, yRed,              /* Red x, y */
            xGreen, yGreen,          /* Green x, y */
            xBlue, yBlue,            /* Blue x, y */
            xWhite, yWhite,          /* White point x, y */
            gamma;                   /* Gamma correction for system */
};
