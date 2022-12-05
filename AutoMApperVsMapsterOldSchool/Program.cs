using AutoMApperVsMapsterOldSchool_Net6;
using System.Text;

//Yes, it's hard/fast coded, as all old school performance tests :-) 

Benchmark bench = new();
StringBuilder sb = new StringBuilder();

for(int i =0; i < 10; i++)
{
    bench.Launch(sb);
    var separator = "---------------------------------------------------";
    sb.AppendLine(separator);
    Console.WriteLine(separator);
}

var fileName = $"../../../../AutomapperVsMapster.Benchmark/BenchmarkDotNet.Artifacts/results/OLDSCHOOL_report_{Guid.NewGuid().ToString()}.txt";
var stream = File.CreateText(fileName);
stream.Write(sb.ToString().ToArray());
stream.Close();
    


Console.ReadKey();
