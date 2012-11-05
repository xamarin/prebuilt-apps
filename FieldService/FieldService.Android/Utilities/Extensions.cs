//
//  Copyright 2012  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Android.Graphics;

namespace FieldService.Android.Utilities {
    public static class Extensions {

        /// <summary>
        /// Converts a floating point longitude/latitude to what Google Maps requires
        /// </summary>
        public static int ToIntE6 (this float value)
        {
            return (int)(value * 1000000);
        }

        /// <summary>
        /// Helper method to safely convert a string to a int
        /// </summary>
        public static int ToInt (this string text)
        {
            int value = 0;
            int.TryParse (text, out value);
            return value;
        }

        /// <summary>
        /// Helper method to safely convert a string to a decimal
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static decimal ToDecimal (this string text)
        {
            decimal value = 0;
            decimal.TryParse (text, out value);
            return value;
        }

        /// <summary>
        /// Helper method to get the sample size of the image for resampling.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int ToSampleSize (this byte [] bytes)
        {
            var sampleSize = 0;
            BitmapFactory.Options options = new BitmapFactory.Options ();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeByteArray (bytes, 0, bytes.Length, options);
            sampleSize = (int)Math.Ceiling ((double)Math.Max (options.OutWidth / Constants.MaxWidth, options.OutHeight / Constants.MaxHeight));
            return sampleSize;
        }

        public static byte [] ToByteArray (this Bitmap bmp)
        {
            byte [] bytes = null;
            if (bmp != null) {
                using (MemoryStream stream = new MemoryStream ()) {
                    if (bmp.Compress (Bitmap.CompressFormat.Jpeg, 80, stream)) {
                        bytes = stream.ToArray ();
                    }
                }
            }
            return bytes;
        }

        public static Bitmap ResizeBitmap (Bitmap input, int destWidth, int destHeight)
        {
            int srcWidth = input.Width,
                srcHeight = input.Height;
            bool needsResize = false;
            float p;
            if (srcWidth > destWidth || srcHeight > destHeight) {
                needsResize = true;
                if (srcWidth > srcHeight && srcWidth > destWidth) {
                    p = (float)destWidth / (float)srcWidth;
                    destHeight = (int)(srcHeight * p);
                } else {
                    p = (float)destHeight / (float)srcHeight;
                    destWidth = (int)(srcWidth * p);
                }
            } else {
                destWidth = srcWidth;
                destHeight = srcHeight;
            }
            if (needsResize) {
                return Bitmap.CreateScaledBitmap (input, destWidth, destHeight, true);
            }
            return input;
        }
    }
}