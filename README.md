# BenchmarkPres

Here is a dotnet solution design to introduce benchmarkdotnet
All source code is based on the wonderful work of code-maze available at : 
https://code-maze.com/automapper-vs-mapster-dotnet/



# AutomapperVsMapster project
This project is reponsible of Mapster and Automapper configuration. 

It is used by both BenchmarkDnotnet and "old school benchmark".

# Fakker project
This project is responsible of 
- Definition of DAO entities and correpsonding DTO's
- Generation of fake data (thanks to Bogus library)

It is used by both BenchmarkDnotnet and "Old school benchmark".

# Perfomance measurment : The "Old school" way

**AutoMapperVsMapsterOldSchool.csproj** : this .net project is responsible of :
- Measure performance of three ways to map simple objects from DAO => DTO in .net 7. **(System.Diagnostics.StopWatch)**
- Report in console **(Console.WriteLine())**
- Serialize report **(System.IO.File)**

All is specific and can not be easily reused.

Pros : 
- Easy to create / run

Cons : 
- Hard to maintain
- Results are hard to interpret (all iteration return a different result especially the first one)
- Does not measure memory usage
- Reporting is very "raw"

Sample of reporting 
```
Pour OldSchool :10 données converties en 3978300 ns
Pour AutoMapper :10 données converties en 165635000 ns
Pour Mapster :10 données converties en 585700800 ns
---------------------------------------------------
Pour OldSchool :10 données converties en 8700 ns
Pour AutoMapper :10 données converties en 22100 ns
Pour Mapster :10 données converties en 10700 ns
---------------------------------------------------
Pour OldSchool :10 données converties en 4700 ns
Pour AutoMapper :10 données converties en 20400 ns
....
```




# Performance measurment : The Benchmarkdotnet way

**AutomapperVsMapster.Benchmark.csproj** : this .net project is responsible of :
- Measure performance of three ways to map simple objects from DAO => DTO in .net 7 and .net 6.0 **native**
- Report in console **native**
- Serialize report **native (html, csv etc..)**

Configuration used :
Benchmark on different OS 
```
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net60, baseline:true)]
```

Follow memory usage
```
[MemoryDiagnoser(true)]
```

Nothing is specific and can be easily maintained.

Pros : 
- Results are easier to interpret as BenchmarkDotnet warm up the CLR before running numerous benchmarks.
- Each instance of benchmark is run in a specific thread. This is this very thread wich is analysed (performance, memory etc..). The source code is not modified.
- Easy reporting

Cons : 
- Performance analyse results are not that easy to interpret

Sample of reporting
```
// * Summary *

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19045.2251)
11th Gen Intel Core i7-1185G7 3.00GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 6.0.11 (6.0.1122.52304), X64 RyuJIT AVX2
  .NET 6.0 : .NET 6.0.11 (6.0.1122.52304), X64 RyuJIT AVX2
  .NET 7.0 : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


|                               Method |      Job |  Runtime | SizeParam |     Mean |     Error |    StdDev |   Median | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|------------------------------------- |--------- |--------- |---------- |---------:|----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
|        AutoMapper_SimpleMapping_User | .NET 6.0 | .NET 6.0 |        10 | 3.075 μs | 0.1336 μs | 0.3659 μs | 2.946 μs |  1.00 |    0.00 | 0.2289 |   1.41 KB |        1.00 |
|        AutoMapper_SimpleMapping_User | .NET 7.0 | .NET 7.0 |        10 | 3.719 μs | 0.2200 μs | 0.6133 μs | 3.569 μs |  1.23 |    0.24 | 0.2289 |   1.41 KB |        1.00 |
|                                      |          |          |           |          |           |           |          |       |         |        |           |             |
|           Mapster_SimpleMapping_User | .NET 6.0 | .NET 6.0 |        10 | 2.380 μs | 0.1028 μs | 0.2900 μs | 2.308 μs |  1.00 |    0.00 | 0.2289 |   1.41 KB |        1.00 |
|           Mapster_SimpleMapping_User | .NET 7.0 | .NET 7.0 |        10 | 2.203 μs | 0.1075 μs | 0.2978 μs | 2.175 μs |  0.93 |    0.15 | 0.2289 |   1.41 KB |        1.00 |
|                                      |          |          |           |          |           |           |          |       |         |        |           |             |
| Mapster_SimpleMapping_User_Duplicate | .NET 6.0 | .NET 6.0 |        10 | 2.757 μs | 0.0547 μs | 0.1430 μs | 2.714 μs |  1.00 |    0.00 | 0.2289 |   1.41 KB |        1.00 |
| Mapster_SimpleMapping_User_Duplicate | .NET 7.0 | .NET 7.0 |        10 | 2.462 μs | 0.0492 μs | 0.1188 μs | 2.425 μs |  0.89 |    0.06 | 0.2289 |   1.41 KB |        1.00 |
|                                      |          |          |           |          |           |           |          |       |         |        |           |             |
|         OldSchool_SimpleMapping_User | .NET 6.0 | .NET 6.0 |        10 | 1.865 μs | 0.0369 μs | 0.0795 μs | 1.848 μs |  1.00 |    0.00 | 0.2289 |   1.41 KB |        1.00 |
|         OldSchool_SimpleMapping_User | .NET 7.0 | .NET 7.0 |        10 | 1.505 μs | 0.0299 μs | 0.0576 μs | 1.496 μs |  0.81 |    0.05 | 0.2308 |   1.41 KB |        1.00 |

// * Warnings *
MultimodalDistribution
  AutomapperVsMapsterBenchmark.Mapster_SimpleMapping_User: .NET 6.0 -> It seems that the distribution can have several modes (mValue = 3.09)

// * Hints *
Outliers
  AutomapperVsMapsterBenchmark.AutoMapper_SimpleMapping_User: .NET 6.0        -> 13 outliers were removed (5.24 μs..8.27 μs)
  AutomapperVsMapsterBenchmark.AutoMapper_SimpleMapping_User: .NET 7.0        -> 10 outliers were removed (5.76 μs..8.11 μs)
  AutomapperVsMapsterBenchmark.Mapster_SimpleMapping_User: .NET 6.0           -> 8 outliers were removed (3.85 μs..5.18 μs)
  AutomapperVsMapsterBenchmark.Mapster_SimpleMapping_User: .NET 7.0           -> 11 outliers were removed (3.12 μs..3.89 μs)
  AutomapperVsMapsterBenchmark.Mapster_SimpleMapping_User_Duplicate: .NET 6.0 -> 6 outliers were removed (3.33 μs..3.91 μs)
  AutomapperVsMapsterBenchmark.Mapster_SimpleMapping_User_Duplicate: .NET 7.0 -> 7 outliers were removed (2.84 μs..3.46 μs)
  AutomapperVsMapsterBenchmark.OldSchool_SimpleMapping_User: .NET 6.0         -> 7 outliers were removed (2.26 μs..2.67 μs)
  AutomapperVsMapsterBenchmark.OldSchool_SimpleMapping_User: .NET 7.0         -> 5 outliers were removed (1.69 μs..2.06 μs)

// * Legends *
  SizeParam   : Value of the 'SizeParam' parameter
  Mean        : Arithmetic mean of all measurements
  Error       : Half of 99.9% confidence interval
  StdDev      : Standard deviation of all measurements
  Median      : Value separating the higher half of all measurements (50th percentile)
  Ratio       : Mean of the ratio distribution ([Current]/[Baseline])
  RatioSD     : Standard deviation of the ratio distribution ([Current]/[Baseline])
  Gen0        : GC Generation 0 collects per 1000 operations
  Allocated   : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  Alloc Ratio : Allocated memory ratio distribution ([Current]/[Baseline])
  1 μs        : 1 Microsecond (0.000001 sec)

// * Diagnostic Output - MemoryDiagnoser *


// ***** BenchmarkRunner: End *****
```

# References


+ GitHub BenchmarkDotNet https://github.com/dotnet/BenchmarkDotNet
+ Best practices https://benchmarkdotnet.org/articles/guides/good-practices.html
+ Run command under AutomapperVsMapster.Benchmark.csproj ```dotnet run -c Release -f net6.0 net7.0 --runtimes net6.0 net7.0```

