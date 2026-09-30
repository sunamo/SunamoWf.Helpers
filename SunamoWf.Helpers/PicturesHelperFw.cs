namespace SunamoWf.Helpers;

/// <summary>
/// WinForms (System.Drawing) image helpers - format detection, thumbnail creation, jpeg saving.
/// </summary>
public class PicturesHelperFw
{
    /// <summary>
    /// Returns the file extension (without dot) matching the image's RawFormat.
    /// </summary>
    public static string ExtensionFromImage(Image image)
    {
        Guid formatGuid = image.RawFormat.Guid;
        if (formatGuid == ImageFormat.Jpeg.Guid)
        {
            return ImageExtensions.jpg;
        }
        else if (formatGuid == ImageFormat.Gif.Guid)
        {
            return ImageExtensions.gif;
        }
        else if (formatGuid == ImageFormat.Bmp.Guid)
        {
            return ImageExtensions.bmp;
        }
        else if (formatGuid == ImageFormat.Icon.Guid)
        {
            return ImageExtensions.ico;
        }
        else if (formatGuid == ImageFormat.Tiff.Guid)
        {
            return ImageExtensions.tiff;
        }
        else if (formatGuid == ImageFormat.Wmf.Guid)
        {
            return ImageExtensions.wmf;
        }
        else if (formatGuid == ImageFormat.Emf.Guid)
        {
            return ImageExtensions.emf;
        }
        else if (formatGuid == ImageFormat.Exif.Guid)
        {
            return ImageExtensions.exif;
        }
        else if (formatGuid == ImageFormat.MemoryBmp.Guid)
        {
            return ImageExtensions.bmp;
        }
        else
        {
            throw new NotImplementedException($"Unsupported image format: {formatGuid}");
        }
        return null;
    }

    /// <summary>
    /// Creates a bordered thumbnail of the given image and writes it to finalPath.
    /// </summary>
    public static void CreateThumbnail(string toFolderTempSlash, string fileNameWithoutExtension, string extension, string finalPath, Image image, Color borderColor, int thumbnailHeight, int thumbnailWidth)
    {
        Size targetSize = image.Size;
        Point targetPoint = new Point();
        if (!(image.Width > thumbnailWidth || image.Height > thumbnailHeight))
        {
            // Either height or width was smaller than the minimum - center the image
            while (targetSize.Height > thumbnailHeight && targetSize.Width > thumbnailWidth)
            {
                targetSize = new Size(Multiple(targetSize.Width), Multiple(targetSize.Height));
            }

            targetPoint = new Point(Divide(thumbnailWidth - targetSize.Width), Divide(thumbnailHeight - targetSize.Height));
        }

        string tempPath = toFolderTempSlash + fileNameWithoutExtension + "_tn" + extension;
        PathShim.CreateUpfoldersUnlessThere(tempPath);
        TransformImage(image, targetSize.Width, targetSize.Height, tempPath);
        using (Image resizedImage = Bitmap.FromFile(tempPath))
        {
            using (Bitmap canvas = new Bitmap(thumbnailWidth, thumbnailHeight))
            {
                using (Graphics graphics = Graphics.FromImage(canvas))
                {
                    graphics.Clear(borderColor);
                    Rectangle rectangle = new Rectangle(targetPoint, targetSize);
                    graphics.DrawImage(resizedImage, rectangle);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        canvas.Save(memoryStream, ImageFormat.Jpeg);
                        byte[] bytes = memoryStream.ToArray();
                        File.WriteAllBytes(finalPath, bytes);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Resizes the image to the given width/height (with scaling matrix) and saves it as jpeg to path.
    /// </summary>
    public static void TransformImage(Image image, int width, int height, string path)
    {
        float scale = (float)width / (float)image.Width;
        using (Bitmap thumbnail = new Bitmap(width, height))
        {
            using (Graphics graphics = Graphics.FromImage(thumbnail))
            {
                using (System.Drawing.Drawing2D.Matrix transform = new System.Drawing.Drawing2D.Matrix())
                {
                    transform.Scale(scale, scale, MatrixOrder.Append);
                    graphics.SetClip(new Rectangle(0, 0, width, height));
                    graphics.Transform = transform;
                    graphics.DrawImage(image, 0, 0, image.Width, image.Height);

                    ImageCodecInfo codecInfo = GetEncoderInfo("image/jpeg");
                    EncoderParameters encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 66L);
                    SaveImage(path, thumbnail, codecInfo, encoderParameters);
                }
            }
        }
    }

    /// <summary>
    /// Saves the image using the given codec/parameters. Caller must dispose thumbnail.
    /// </summary>
    private static void SaveImage(string path, Image thumbnail, ImageCodecInfo codecInfo, EncoderParameters encoderParameters)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            thumbnail.Save(memoryStream, codecInfo, encoderParameters);
            File.WriteAllBytes(path, memoryStream.ToArray());
        }
    }

    private static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        for (int i = 0; i < codecs.Length; i++)
        {
            if (codecs[i].MimeType == mimeType)
            {
                return codecs[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Creates a proportionally centered thumbnail (no border fill) and writes it to finalPath.
    /// </summary>
    public static void CreateThumbnailOptimal(string toFolderTempSlash, string fileNameWithoutExtension, string extension, string finalPath, Image image, int thumbnailWidth, int thumbnailHeight)
    {
        Size targetSize = image.Size;
        if (!(image.Width > thumbnailWidth || image.Height > thumbnailHeight))
        {
            while (targetSize.Height > thumbnailHeight && targetSize.Width > thumbnailWidth)
            {
                targetSize = new Size(Multiple(targetSize.Width), Multiple(targetSize.Height));
            }
        }

        string tempPath = toFolderTempSlash + fileNameWithoutExtension + "_tn" + extension;
        PathShim.CreateUpfoldersUnlessThere(tempPath);
        TransformImage(image, targetSize.Width, targetSize.Height, finalPath);
    }

    /// <summary>
    /// Double-precision overload of TransformImage.
    /// </summary>
    public static void TransformImage(Image image, double width, double height, string path)
    {
        TransformImage(image, (int)width, (int)height, path);
    }

    /// <summary>
    /// Scales down a dimension by 10% (used when iteratively shrinking a thumbnail to fit).
    /// </summary>
    public static int Multiple(int value)
    {
        return (int)((float)value * 0.9f);
    }

    /// <summary>
    /// Halves a dimension (used to center an image within its thumbnail border).
    /// </summary>
    public static int Divide(int value)
    {
        return (int)((float)value / 2f);
    }

    /// <summary>
    /// Saves the image using an encoder resolved from the given mime type. Caller must dispose thumbnail.
    /// </summary>
    public static void SaveImage(string path, Image thumbnail, string mimeType, EncoderParameters encoderParameters)
    {
        SaveImage(path, thumbnail, GetEncoderInfo(mimeType), encoderParameters);
    }

    /// <summary>
    /// Saves the image as a jpeg with the given quality, swallowing any exception.
    /// </summary>
    public static void SaveJpeg(string path, Image image, long quality)
    {
        path = PathShim.ChangeExtension(path, ImageExtensions.jpg);
        try
        {
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            if (jpegCodec == null)
            {
                return;
            }

            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = qualityParam;
            image.Save(path, jpegCodec, encoderParameters);
        }
        catch
        {
        }
    }
}
