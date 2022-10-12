using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS8600
#pragma warning disable CS8601
#pragma warning disable CS8602
namespace PointDataComputation
{
    class Program
    {
        static void Main()
        {
            new CalculateGroupPoint().Calculate();
        }
    }

    public class CalculateGroupPoint
    {
        private string strFilename = "";
        private Dictionary<int, List<PointData>> GroupPointData = new Dictionary<int, List<PointData>>();
        private Dictionary<int, List<int>> GroupPath = new Dictionary<int, List<int>>();
        TimeSpan dGroupTime, dPathPlanTime, dOutputFile;
        private bool isDebug = false;
        public void Calculate()
        {
            Group();
            PointPath();
            // PrintGroupData();
            // CalculateMemory();
            OutputData();
        }

        private void Group()
        {
            Console.WriteLine("Input file name:");
            strFilename = isDebug ? "PointData_1000" : Console.ReadLine();
            var filePath = $"{strFilename}.csv";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
                return;
            }
            if (IsFileLocked(filePath))
            {
                Console.WriteLine("Failed to open file.");
                return;
            }

            using (var sr = new StreamReader(filePath))
            {
                var timer = DateTime.Now;
                Console.WriteLine("Load and group data start.");
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var lines = line.Split(',');
                    var title = !int.TryParse(lines[0], out var id);
                    if (title) continue; // Title will continue
                    var x = double.Parse(lines[1]);
                    var y = double.Parse(lines[2]);
                    var diameter = double.Parse(lines[3]);
                    var weight = double.Parse(lines[4]);
                    var point = new PointData(id, x, y, diameter, weight);
                    var group = (int)y / 5;
                    if (!GroupPointData.ContainsKey(group))
                    {
                        GroupPointData.Add(group, new List<PointData>());
                    }

                    GroupPointData[group].Add(point);
                }
                dGroupTime = DateTime.Now - timer;
                Console.WriteLine($"Load and group data finish.\nTotal time consume = {Math.Round(dGroupTime.TotalSeconds, 3)}");
                Console.WriteLine($"Total group size= {GroupPointData.Count}");
            }
        }

        private void PointPath()
        {
            Console.WriteLine("Plan point path start.");
            var timer = DateTime.Now;
            List<PointData> list;
            foreach (var group in GroupPointData.Keys)
            {
                //Console.WriteLine($"Plan no. {group} group path.");
                list = new List<PointData>(GroupPointData[group]);

                if (!GroupPath.ContainsKey(group))
                {
                    GroupPath.Add(group, new List<int>());
                }

                int index = 0;
                while (list.Count > 1)
                {
                    var PD = list[index];
                    GroupPath[group].Add(int.Parse(list[index].PointID.ToString()));
                    list.RemoveAt(index);

                    double distance = 0.0;
                    var tempDist = 99999.99999;
                    for (var i = 0; i < list.Count; i++)
                    {
                        var point = list[i];
                        distance = CalcPointDist(PD.X, PD.Y, point.X, point.Y);
                        if (tempDist > distance)
                        {
                            index = i;
                            tempDist = distance;
                        }
                    }
                }
                GroupPath[group].Add(int.Parse(list[0].PointID.ToString()));
            }
            dPathPlanTime = DateTime.Now - timer;
            Console.WriteLine($"Plan point path finish.\nTotal time consume = {Math.Round(dPathPlanTime.TotalSeconds, 3)}");
        }

        private double CalcPointDist(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2)));
        }

        private void PrintGroupData()
        {
            foreach (var key in GroupPointData.Keys)
            {
                Console.WriteLine("");
                var vp = GroupPointData[key];
                var path = GroupPath[key];
                Console.WriteLine($"Group {key} Data:(point size = {vp.Count})");
                for (var i = 1; i < vp.Count; i++)
                {
                    Console.WriteLine($"id={vp[i].PointID}    X={vp[i].X}    Y={vp[i].Y}    Diameter={vp[i].Diameter}    Weight={vp[i].Weight}");
                }
                Console.WriteLine($"Group {key} Data:(point size = {vp.Count})");
                Console.Write($"{path[0]}");
                for (var i = 1; i < path.Count; i++)
                {
                    Console.Write($" ---> {path[i]}");
                }
                Console.WriteLine("");
            }
        }

        private void CalculateMemory()
        {
            var data = new PointData(1, 1, 1, 1, 1);
            var memoryPointId = Marshal.SizeOf(data.PointID);
            var memoryX = Marshal.SizeOf(data.X);
            var memoryY = Marshal.SizeOf(data.Y);
            var memoryDiameter = Marshal.SizeOf(data.Diameter);
            var memoryWeight = Marshal.SizeOf(data.Weight);
            Console.WriteLine($"Memory Calculate: PointID:{memoryPointId} X:{memoryX} Y:{memoryY} Diameter:{memoryDiameter} Weight:{memoryWeight}");
        }

        private void OutputData()
        {
            bool isOutput = false;

            while (!isOutput)
            {
                Console.WriteLine("Do you want output processed data?(y/n)");
                var c = isDebug ? "Y" : Console.ReadLine();
                if (c.ToUpper() == "N") return;
                if (c.ToUpper() == "Y") isOutput = true;
            }

            var filePath = $"{strFilename}_Path.txt";
            if (IsFileLocked(filePath))
            {
                Console.WriteLine("Failed to open file.");
                return;
            }

            using (var sw = new StreamWriter(filePath))
            {
                Console.WriteLine("Output data to file start.");
                var timer = DateTime.Now;
                sw.WriteLine($"Total group size = {GroupPointData.Count}");
                sw.WriteLine("Time consuming");
                sw.WriteLine($"1.Group: {Math.Round(dGroupTime.TotalSeconds, 3)} sec");
                sw.WriteLine($"2.Path: {Math.Round(dPathPlanTime.TotalSeconds, 3)} sec");

                for(int key = -60; key <= 60; key++)
                {
                    if(!GroupPointData.ContainsKey(key)) continue;
                    
                    sw.WriteLine();
                    var vp = GroupPointData[key];
                    var path = GroupPath[key];
                    sw.WriteLine($"Group {key} Data:(point size = {vp.Count})");
                    for (var i = 1; i < vp.Count; i++)
                    {
                        sw.WriteLine($"id={vp[i].PointID}    X={vp[i].X}    Y={vp[i].Y}    Diameter={vp[i].Diameter}    Weight={vp[i].Weight}");
                    }

                    sw.WriteLine($"Group {key} Data:(point size = {path.Count})");
                    sw.Write(path[0]);
                    for (var i = 1; i < path.Count; i++)
                    {
                        sw.Write($" ---> {path[i]}");
                    }
                    sw.WriteLine();
                }
                dOutputFile = DateTime.Now - timer;
                Console.WriteLine($"Output data to file finish.\nTotal time consume = {Math.Round(dOutputFile.TotalSeconds, 3)}");
            }
        }

        private bool IsFileLocked(string file)
        {
            try
            {
                using (File.Open(file, FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    return false;
                }
            }
            catch (IOException exception)
            {
                var errorCode = Marshal.GetHRForException(exception) & 65535;
                return errorCode == 32 || errorCode == 33;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class PointData
    {
        public double PointID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Diameter { get; set; }
        public double Weight { get; set; }

        public PointData(int id, double x, double y, double diameter, double weight)
        {
            PointID = id;
            X = x;
            Y = y;
            Diameter = diameter;
            Weight = weight;
        }
    }
}