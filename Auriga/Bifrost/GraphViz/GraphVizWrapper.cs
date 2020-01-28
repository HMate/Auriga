using Bifrost.Dot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Bifrost.GraphViz
{
    public class GraphVizWrapper
    {
        public static DotGraph LayoutNodes(DotGraph dg)
        {
            // call graphviz dot to layout the nodes
            string dotFile = DotFileGenerator.Serialize(dg);
            string resultDot = "";
            string errorOutput = "";
            using (Process proc = new Process())
            {
                // TODO: We will need to give a full path to the dot.exe, for now it should be in the path
                proc.StartInfo.FileName = "dot.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;

                proc.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    resultDot += e.Data;
                });
                proc.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    errorOutput += e.Data;
                });

                proc.Start();

                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();

                proc.StandardInput.Write(dotFile + "\n");

                while (!resultDot.TrimEnd().EndsWith("}"))
                {
                    System.Threading.Thread.Sleep(10);
                }
            }
            if (errorOutput != "")
            {
                Debug.WriteLine(errorOutput);
            }
            return DotLoader.Load(resultDot);
        }
    }
}
