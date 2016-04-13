// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.PixelsArray
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Utils;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace DevExpress.Xpf.Gauges.Native
{
  public class PixelsArray
  {
    private const string symbolsImageFileNamePref = "DevExpress.Xpf.Gauges.Images.";
    private readonly byte[] pixels;
    private readonly int stride;

    public byte this[int x, int y]
    {
      get
      {
        return this.pixels[y * this.stride + x];
      }
    }

    private PixelsArray(byte[] pixels, int stride)
    {
      this.pixels = pixels;
      this.stride = stride;
    }

    [SecuritySafeCritical]
    public static PixelsArray LoadFromCpecificImage(int symbolWidth, int symbolHeight)
    {
      byte[] numArray;
      int width;
      using (Bitmap bitmapFromResources = ResourceImageHelper.CreateBitmapFromResources("DevExpress.Xpf.Gauges.Images." + ("Symbols" + symbolWidth.ToString() + "x" + symbolHeight.ToString() + ".png"), Assembly.GetExecutingAssembly()))
      {
        BitmapData bitmapdata = bitmapFromResources.LockBits(new Rectangle(0, 0, bitmapFromResources.Width, bitmapFromResources.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
        IntPtr scan0 = bitmapdata.Scan0;
        int length = bitmapFromResources.Width * bitmapFromResources.Height;
        numArray = new byte[length];
        width = bitmapFromResources.Width;
        Marshal.Copy(scan0, numArray, 0, length);
        bitmapFromResources.UnlockBits(bitmapdata);
      }
      return new PixelsArray(numArray, width);
    }
  }
}
