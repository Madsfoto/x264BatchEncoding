﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace x264BatchEncoding
{
    class Program
    {
     
        static void Main(string[] args)
        {
            string inputFilename = "";
            if(args.Length==0)
            {
                inputFilename = "ColorCube.mov";
                Console.WriteLine("Defaulting to ColorCube.mov, use 1st argument for other name");
            }
            else
            {
                inputFilename = args[1];
            }

            string[] profileArr = { "baseline", "main", "high", "high10", "high422", "high444" };
            string[] presetArr = { "ultrafast", "superfast", "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow", "placebo" };
            string[] pix_fmtArr = { "yuv420p", "yuv422p", "yuv444p", "yuv420p10le", "yuv422p10le", "yuv444p10le" };

            string inputFilenameNoExt = inputFilename.Substring(0, (inputFilename.Length - inputFilename.LastIndexOf(".") + 1));

            string ff = "ffmpeg -i " + inputFilename + " -c:v libx264 ";
            string profileStr = "-profile:v ";
            string presetStr = "-preset ";
            string CRFStr = "-crf ";
            string pix_fmtStr = "-pix_fmt ";

            List<string> writeStrList = new List<string>();
            String outputFilename = "";

            for (int profileInt = 0; profileInt < profileArr.Length; profileInt++)
            {
                for (int presetInt = 0; presetInt < presetArr.Length; presetInt++)
                {
                    for (int CRFInt = 0; CRFInt < 64; CRFInt++)
                    {
                        for (int pix_fmtInt = 0; pix_fmtInt < pix_fmtArr.Length; pix_fmtInt++)
                        {
                            for (int encodeInt = 0; encodeInt < 51; encodeInt++)
                            {// add profile + format, preset and crf to FFmpeg comand, add encode number to output filename
                             // x264 encode support chart: https://docs.google.com/spreadsheets/d/1sq7D_HHv0QEouiFY-RXSFn6KbG7D8rI4A-nE6U8Pr7k/edit?usp=sharing
                                #region Explicit not supported
                                // In baseline, main and high only yuv420p is supported, no 10 bit format or 422 or 444 subsampling or CRF 0 is supported
                                if ((profileInt == 0 || profileInt == 1 || profileInt == 2) && (pix_fmtInt != 0 || CRFInt == 0))
                                {
                                    break;
                                }
                                // in high10 only up to 420p10le is supported
                                if (profileInt == 3 && (pix_fmtInt == 1 || pix_fmtInt == 2 || pix_fmtInt == 4 || pix_fmtInt == 5))
                                {
                                    break;

                                }

                                // high422 (should be named high10422, as it's high10 + 422 subsampling support
                                if (profileInt == 4 && (pix_fmtInt == 2 || pix_fmtInt == 5))
                                {
                                    break;
                                }
                                // high444, again high10444, supports everything
                                #endregion

                                
                                // if profileint4 -> only do CRF when format == 10 bit

                                if (profileInt == 3 && CRFInt == 0 && pix_fmtInt == 0)
                                {
                                    break;
                                }
                                if (profileInt == 4 && CRFInt == 0 && (pix_fmtInt == 0 || pix_fmtInt==1 || pix_fmtInt==2) ) // pix_fmtInt == 2 is already tested above,
                                                                                                                            // but it is also tested here for completeness
                                {
                                    break;
                                }


                                string CRFIntStr = CRFInt.ToString("D2");
                                string encodeIntStr = encodeInt.ToString("D2");

                                

                                string FFCmd = ff + profileStr + profileArr[profileInt] + " " + presetStr + presetArr[presetInt] + " " + CRFStr + CRFInt + " " + pix_fmtStr + pix_fmtArr[pix_fmtInt];

                                outputFilename = inputFilenameNoExt + "_" + profileInt + profileArr[profileInt] + "_" + presetInt + presetArr[presetInt] + "_" + CRFIntStr + "_" + pix_fmtArr[pix_fmtInt] + "_" + encodeIntStr + ".mov";

                                writeStrList.Add(FFCmd + " " + outputFilename);
                            }

                        }


                    }

                    File.WriteAllLines("0-" + profileInt + profileArr[profileInt] + "-" + presetInt + presetArr[presetInt] + ".bat", writeStrList);
                    writeStrList.Clear();
                }

            } // last loop done


            
            
            

            




            

        }
    }
}
