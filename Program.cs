using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace x264BatchEncoding
{
    class Program
    {
        static void Loops(string inputFilename)
        {
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
                            {
                                // add profile + format, preset and crf to FFmpeg comand, add encode number to output filename

                                string FFCmd = ff + profileStr + profileArr[profileInt] + " " + presetStr + presetArr[presetInt] + " " + CRFStr + CRFInt + " " + pix_fmtStr + pix_fmtArr[pix_fmtInt];

                                outputFilename = inputFilenameNoExt + "_" +profileInt+ profileArr[profileInt] + "_" + presetInt+presetArr[presetInt] + "_" + CRFInt + "_" + pix_fmtArr[pix_fmtInt] + "_" + encodeInt + ".mov";

                                writeStrList.Add(FFCmd + " "+outputFilename);
                            }

                        }


                    }

                    File.WriteAllLines("0-" + profileInt + profileArr[profileInt] + presetInt +"-"+ presetInt+ presetArr[presetInt] + ".bat", writeStrList);
                    writeStrList.Clear();
                }

            } // last loop done

            

        }
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



            Loops(inputFilename);

            
            

            




            

        }
    }
}
