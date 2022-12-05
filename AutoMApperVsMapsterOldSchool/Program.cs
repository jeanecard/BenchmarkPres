using AutoMApperVsMapsterOldSchool_Net6;
using System.Text;

Benchmark bench = new();
StringBuilder sb = new StringBuilder();

for(int i =0; i < 10; i++)
{
    bench.Launch(sb);
    var separator = "---------------------------------------------------";
    sb.AppendLine(separator);
    Console.WriteLine(separator);
}
var stream = File.CreateText($"./report_{Guid.NewGuid().ToString()}.txt");
stream.Write(sb.ToString().ToArray());
stream.Close();
    


Console.ReadKey();

//TODO tester le automapper et mapster avec cette même méthode.