using System.Diagnostics;
using System.Text;
using AutomapperVsMapster.SimpleTypeMapping;
using Fakker.DTO;

namespace AutoMApperVsMapsterOldSchool_Net6
{
    internal class Benchmark
    {
        public bool Launch(StringBuilder sb)
        {
            int nbIteration = 10;
            Stopwatch stopWatch = new Stopwatch();

            var data = SimpleTypeMappingDataGenerator.GetSources(nbIteration);
            List<UserDto> dummyOldSchool = new List<UserDto> { };
            List<UserDto> dummyAutomapper = new List<UserDto> { };
            List<UserDto> dummyMapster = new List<UserDto> { };

            //START OLDSCHOOL
            stopWatch.Start();
            for (var i = 0; i < nbIteration; i++)
            {
                var dto = OldSchoolSimpleTypeMapping.Map(data[i]);
                dummyOldSchool.Add(dto);
            }
            stopWatch.Stop();
            //STOP
            var oldSchoolReport = $"Pour OldSchool :{dummyOldSchool.Count} données converties en {stopWatch.Elapsed.TotalNanoseconds} ns";
            sb.AppendLine(oldSchoolReport);
            Console.WriteLine(oldSchoolReport);


            //START AUTOMAPPER
            stopWatch.Restart();
            for (var i = 0; i < nbIteration; i++)
            {
                var dto = AutoMapperSimpleTypeMapping.Map(data[i]);
                dummyAutomapper.Add(dto);
            }
            stopWatch.Stop();
            //STOP
            var autoMapperReport = $"Pour AutoMapper :{dummyAutomapper.Count} données converties en {stopWatch.Elapsed.TotalNanoseconds} ns";
            sb.AppendLine(autoMapperReport);
            Console.WriteLine(autoMapperReport);

            //START MAPSTER
            stopWatch.Restart();
            for (var i = 0; i < nbIteration; i++)
            {
                var dto = MapsterSimpleTypeMapping.Map(data[i]);
                dummyMapster.Add(dto);
            }
            stopWatch.Stop();
            //STOP
            var mapsterReport = $"Pour Mapster :{dummyMapster.Count} données converties en {stopWatch.Elapsed.TotalNanoseconds} ns";
            sb.AppendLine(mapsterReport);
            Console.WriteLine(mapsterReport);

            return true;
        }
    }
}
