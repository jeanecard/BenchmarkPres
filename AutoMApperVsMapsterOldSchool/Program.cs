using AutoMApperVsMapsterOldSchool_Net6;
using System.Text;

//Yes, it's hard/fast coded, as all old school performance tests :-) 
//Welcome to the god class !

Benchmark bench = new();
//We are going to use a StringBuilder to update reporting in the benchmark process
StringBuilder sb = new StringBuilder();

//We launch the performance benchmark 10 times to see if the results are constants
//Si on ne lance qu'une fois les résultats vont être très différents d'une moyenne constatée.
for(int i =0; i < 10; i++)
{
    bench.Launch(sb);
    //Updating reporting
    var separator = "---------------------------------------------------";
    sb.AppendLine(separator);
    //Reporting in console
    Console.WriteLine(separator);
}
//Serialisation of the report (a true old goog god class ....)
var fileName = $"../../../../AutomapperVsMapster.Benchmark/BenchmarkDotNet.Artifacts/results/OLDSCHOOL_report_{Guid.NewGuid().ToString()}.txt";
var stream = File.CreateText(fileName);
stream.Write(sb.ToString().ToArray());
stream.Close();
  
Console.ReadKey();
