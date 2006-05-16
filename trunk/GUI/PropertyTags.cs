using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;

namespace Photo
{
    static class PropertyTags
    {
        public const int GpsVer = 0;
        public const int GpsLatitudeRef = 1;
        public const int GpsLatitude = 2;
        public const int GpsLongitudeRef = 3;
        public const int GpsLongitude = 4;
        public const int GpsAltitudeRef = 5;
        public const int GpsAltitude = 6;
        public const int GpsGpsTime = 7;
        public const int GpsGpsSatellites = 8;
        public const int GpsGpsStatus = 9;
        public const int GpsGpsMeasureMode = 10;
        public const int GpsGpsDop = 11;
        public const int GpsSpeedRef = 12;
        public const int GpsSpeed = 13;
        public const int GpsTrackRef = 14;
        public const int GpsTrack = 15;
        public const int GpsImgDirRef = 16;
        public const int GpsImgDir = 17;
        public const int GpsMapDatum = 18;
        public const int GpsDestLatRef = 19;
        public const int GpsDestLat = 20;
        public const int GpsDestLongRef = 21;
        public const int GpsDestLong = 22;
        public const int GpsDestBearRef = 23;
        public const int GpsDestBear = 24;
        public const int GpsDestDistRef = 25;
        public const int GpsDestDist = 26;
        public const int NewSubfileType = 254;
        public const int SubfileType = 255;
        public const int ImageWidth = 256;
        public const int ImageHeight = 257;
        public const int BitsPerSample = 258;
        public const int Compression = 259;
        public const int PhotometricInterp = 262;
        public const int ThreshHolding = 263;
        public const int CellWidth = 264;
        public const int CellHeight = 265;
        public const int FillOrder = 266;
        public const int DocumentName = 269;
        public const int ImageDescription = 270;
        public const int EquipMake = 271;
        public const int EquipModel = 272;
        public const int StripOffsets = 273;
        public const int Orientation = 274;
        public const int SamplesPerPixel = 277;
        public const int RowsPerStrip = 278;
        public const int StripBytesCount = 279;
        public const int MinSampleValue = 280;
        public const int MaxSampleValue = 281;
        public const int XResolution = 282;
        public const int YResolution = 283;
        public const int PlanarConfig = 284;
        public const int PageName = 285;
        public const int XPosition = 286;
        public const int YPosition = 287;
        public const int FreeOffset = 288;
        public const int FreeByteCounts = 289;
        public const int GrayResponseUnit = 290;
        public const int GrayResponseCurve = 291;
        public const int T4Option = 292;
        public const int T6Option = 293;
        public const int ResolutionUnit = 296;
        public const int PageNumber = 297;
        public const int TransferFunction = 301;
        public const int SoftwareUsed = 305;
        public const int DateTime = 306;
        public const int Artist = 315;
        public const int HostComputer = 316;
        public const int Predictor = 317;
        public const int WhitePoint = 318;
        public const int PrimaryChromaticities = 319;
        public const int ColorMap = 320;
        public const int HalftoneHints = 321;
        public const int TileWidth = 322;
        public const int TileLength = 323;
        public const int TileOffset = 324;
        public const int TileByteCounts = 325;
        public const int InkSet = 332;
        public const int InkNames = 333;
        public const int NumberOfInks = 334;
        public const int DotRange = 336;
        public const int TargetPrinter = 337;
        public const int ExtraSamples = 338;
        public const int SampleFormat = 339;
        public const int SMinSampleValue = 340;
        public const int SMaxSampleValue = 341;
        public const int TransferRange = 342;
        public const int JPEGProc = 512;
        public const int JPEGInterFormat = 513;
        public const int JPEGInterLength = 514;
        public const int JPEGRestartInterval = 515;
        public const int JPEGLosslessPredictors = 517;
        public const int JPEGPointTransforms = 518;
        public const int JPEGQTables = 519;
        public const int JPEGDCTables = 520;
        public const int JPEGACTables = 521;
        public const int YCbCrCoefficients = 529;
        public const int YCbCrSubsampling = 530;
        public const int YCbCrPositioning = 531;
        public const int REFBlackWhite = 532;
        public const int Gamma = 769;
        public const int ICCProfileDescriptor = 770;
        public const int SRGBRenderingIntent = 771;
        public const int ImageTitle = 800;
        public const int ResolutionXUnit = 20481;
        public const int ResolutionYUnit = 20482;
        public const int ResolutionXLengthUnit = 20483;
        public const int ResolutionYLengthUnit = 20484;
        public const int PrintFlags = 20485;
        public const int PrintFlagsVersion = 20486;
        public const int PrintFlagsCrop = 20487;
        public const int PrintFlagsBleedWidth = 20488;
        public const int PrintFlagsBleedWidthScale = 20489;
        public const int HalftoneLPI = 20490;
        public const int HalftoneLPIUnit = 20491;
        public const int HalftoneDegree = 20492;
        public const int HalftoneShape = 20493;
        public const int HalftoneMisc = 20494;
        public const int HalftoneScreen = 20495;
        public const int JPEGQuality = 20496;
        public const int GridSize = 20497;
        public const int ThumbnailFormat = 20498;
        public const int ThumbnailWidth = 20499;
        public const int ThumbnailHeight = 20500;
        public const int ThumbnailColorDepth = 20501;
        public const int ThumbnailPlanes = 20502;
        public const int ThumbnailRawBytes = 20503;
        public const int ThumbnailSize = 20504;
        public const int ThumbnailCompressedSize = 20505;
        public const int ColorTransferFunction = 20506;
        public const int ThumbnailData = 20507;
        public const int ThumbnailImageWidth = 20512;
        public const int ThumbnailImageHeight = 20513;
        public const int ThumbnailBitsPerSample = 20514;
        public const int ThumbnailCompression = 20515;
        public const int ThumbnailPhotometricInterp = 20516;
        public const int ThumbnailImageDescription = 20517;
        public const int ThumbnailEquipMake = 20518;
        public const int ThumbnailEquipModel = 20519;
        public const int ThumbnailStripOffsets = 20520;
        public const int ThumbnailOrientation = 20521;
        public const int ThumbnailSamplesPerPixel = 20522;
        public const int ThumbnailRowsPerStrip = 20523;
        public const int ThumbnailStripBytesCount = 20524;
        public const int ThumbnailResolutionX = 20525;
        public const int ThumbnailResolutionY = 20526;
        public const int ThumbnailPlanarConfig = 20527;
        public const int ThumbnailResolutionUnit = 20528;
        public const int ThumbnailTransferFunction = 20529;
        public const int ThumbnailSoftwareUsed = 20530;
        public const int ThumbnailDateTime = 20531;
        public const int ThumbnailArtist = 20532;
        public const int ThumbnailWhitePoint = 20533;
        public const int ThumbnailPrimaryChromaticities = 20534;
        public const int ThumbnailYCbCrCoefficients = 20535;
        public const int ThumbnailYCbCrSubsampling = 20536;
        public const int ThumbnailYCbCrPositioning = 20537;
        public const int ThumbnailRefBlackWhite = 20538;
        public const int ThumbnailCopyRight = 20539;
        public const int LuminanceTable = 20624;
        public const int ChrominanceTable = 20625;
        public const int FrameDelay = 20736;
        public const int LoopCount = 20737;
        public const int GlobalPalette = 20738;
        public const int IndexBackground = 20739;
        public const int IndexTransparent = 20740;
        public const int PixelUnit = 20752;
        public const int PixelPerUnitX = 20753;
        public const int PixelPerUnitY = 20754;
        public const int PaletteHistogram = 20755;
        public const int Copyright = 33432;
        public const int ExifExposureTime = 33434;
        public const int ExifFNumber = 33437;
        public const int ExifIFD = 34665;
        public const int ICCProfile = 34675;
        public const int ExifExposureProg = 34850;
        public const int ExifSpectralSense = 34852;
        public const int GpsIFD = 34853;
        public const int ExifISOSpeed = 34855;
        public const int ExifOECF = 34856;
        public const int ExifVer = 36864;
        public const int ExifDTOrig = 36867;
        public const int ExifDTDigitized = 36868;
        public const int ExifCompConfig = 37121;
        public const int ExifCompBPP = 37122;
        public const int ExifShutterSpeed = 37377;
        public const int ExifAperture = 37378;
        public const int ExifBrightness = 37379;
        public const int ExifExposureBias = 37380;
        public const int ExifMaxAperture = 37381;
        public const int ExifSubjectDist = 37382;
        public const int ExifMeteringMode = 37383;
        public const int ExifLightSource = 37384;
        public const int ExifFlash = 37385;
        public const int ExifFocalLength = 37386;
        public const int ExifMakerNote = 37500;
        public const int ExifUserComment = 37510;
        public const int ExifDTSubsec = 37520;
        public const int ExifDTOrigSS = 37521;
        public const int ExifDTDigSS = 37522;
        public const int Title = 40091;
        public const int Comments = 40092;
        public const int Author = 40093;
        public const int Keywords = 40094;
        public const int Subject = 40095;
        public const int IIPhotoTag = 40096;
        public const int ExifFPXVer = 40960;
        public const int ExifColorSpace = 40961;
        public const int ExifPixXDim = 40962;
        public const int ExifPixYDim = 40963;
        public const int ExifRelatedWav = 40964;
        public const int ExifInterop = 40965;
        public const int ExifFlashEnergy = 41483;
        public const int ExifSpatialFR = 41484;
        public const int ExifFocalXRes = 41486;
        public const int ExifFocalYRes = 41487;
        public const int ExifFocalResUnit = 41488;
        public const int ExifSubjectLoc = 41492;
        public const int ExifExposureIndex = 41493;
        public const int ExifSensingMethod = 41495;
        public const int ExifFileSource = 41728;
        public const int ExifSceneType = 41729;
        public const int ExifCfaPattern = 41730;

        private static Dictionary<int, string> defaultExifIdsDictionary;
        private static Dictionary<int, string> defaultExifDoBazyDictionary;

        static PropertyTags()
        {
            defaultExifIdsDictionary = new Dictionary<int, string>();
            defaultExifDoBazyDictionary = new Dictionary<int, string>();

            // Standardowe wartosci EXIF do wyswietlenia
            // Najpierw String ktory zostanie wyswietlony, a nastepnie 
            // Tag ktory zostanie pobrany ze zdjecia
            defaultExifIdsDictionary.Add(EquipMake, "Make");
            defaultExifIdsDictionary.Add(EquipModel, "Model");
            defaultExifIdsDictionary.Add(Orientation, "Orientation");
            defaultExifIdsDictionary.Add(ExifExposureTime, "Exposure Time");
            defaultExifIdsDictionary.Add(ExifFlash, "Flash");
            defaultExifIdsDictionary.Add(ExifShutterSpeed, "Shutter Speed");
            defaultExifIdsDictionary.Add(ExifBrightness, "Brightness");
            defaultExifIdsDictionary.Add(ExifISOSpeed, "ISO");
            defaultExifIdsDictionary.Add(ExifAperture, "Aperture");
            defaultExifIdsDictionary.Add(ExifFocalLength, "Focal Length");
            defaultExifIdsDictionary.Add(ExifUserComment, "Comment");
            defaultExifIdsDictionary.Add(ExifColorSpace, "Color Space");
            defaultExifIdsDictionary.Add(Compression, "Compression");
            defaultExifIdsDictionary.Add(DateTime, "Date & Time");
            defaultExifIdsDictionary.Add(IIPhotoTag, "IIPhotoTag");

            defaultExifDoBazyDictionary.Add(DateTime, "data_wykonania");
            defaultExifDoBazyDictionary.Add(ExifUserComment, "komentarz");
            defaultExifDoBazyDictionary.Add(Orientation, "orientacja");
            defaultExifDoBazyDictionary.Add(Author, "autor");
        }

        public static Dictionary<int, string> defaultExifIds
        {
            get
            {
                return defaultExifIdsDictionary;
            }
        }

        public static Dictionary<int, string> defaultExifDoBazy
        {
            get 
            {
                return defaultExifDoBazyDictionary;
            }
        }

        internal static string ParseProp(PropertyItem propItem/* int propID, string val*/)
        {
            string val;
            switch (propItem.Type)
            {
                case 1: val = Encoding.Unicode.GetString(propItem.Value); break;
                case 2: val = Encoding.ASCII.GetString(propItem.Value); break;
                case 3: val = BitConverter.ToUInt16(propItem.Value, 0).ToString(); break;
                default: val = "Value not supported"; break;
            }
            switch (propItem.Id)
            {
                case ExifFlash: if (val.Equals("1"))
                    {
                        val = "Fired";
                    }
                    else
                    {
                        val = "Not Fired";
                    }
                    break;
                case Orientation:
                    switch (val)
                    {
                        case "1":
                            val = "Normal";
                            break;
                        case "2":
                            val = "Flip Horizontal";
                            break;
                        case "3":
                            val = "Clockwise 180°";
                            break;
                        case "4":
                            val = "Flip Vertical";
                            break;
                        case "5":
                            val = "Flip Horizontal & Clockwise 90°";
                            break;
                        case "6":
                            val = "Clockwise 90°";
                            break;
                        case "7":
                            val = "Flip Horizontal & Counter Clockwise 90°";
                            break;
                        case "8":
                            val = "Counter Clockwise 90°";
                            break;
                        default:
                            val = "Error";
                            break;
                    }
                    break;
            }
            return val;
        }
    }
}
