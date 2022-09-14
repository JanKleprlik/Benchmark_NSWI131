# Benchmark of FFT implementations specialized on audio data in .NET

## Motivation

For the purpose of my thesis I needed fast DFT implementation in .NET. As I found, there is a lot of them, but the implementation speed differs. In a benchmark published online [link](https://www.codeproject.com/Articles/1095473/Comparison-of-FFT-Implementations-for-NET) I found several implementations compared to each other.

The article suggests more implementations that are tested in this benchmark. Since the thesis required the implementation to be multiplatform and rely only on .net standard I ommited some of them here. Note that NAudio is not available in .net standard but the implementation can be extracted and used in project targeting .net standard.

The article also targets general FFT but the thesis required only real FFT, therefore the results of the benchmarks might differ due to specific input data.

## Used implementations

For the benchmark itself I used known benchmark framework benchmarkdotnet. This benchmark is widely accepted as the framework used for creating reliable benchmarks in the dotnet community. It is also used in the official dotnet runetimes and libraries and many other projects by Microsoft itself. This benchmark provides us great API for testing, provides warmup, outlier detection, basic statistic comptutation and it even turns on power mode of the machine. All those things we would have to implement manually, which is tedious.

Using benchmarking framework also brings its disadvantages such as if we are not carefull enough we might lose control over how is the benchmark performed. Because of that I limited myself to use only specified number of iterations and not to rely on the frameworks heuristics of determining such contstants. I have used the recommended medium size of iterations.

### Lomont
 - Used as a reference in benchmarks.  
 - Used in the thesis.
 - in-place
 - specialized for audio - real value inputs
 - [link](https://www.lomont.org/software/misc/fft/LomontFFT.cs)

### for-loop
 - manual naive implementation
 - in-place

### Accord 
 - in-place
 - general
 - [link](http://accord-framework.net/)

### Math.NET
 - in-place
 - general
 - [link](https://numerics.mathdotnet.com/)

### NAudio
 - in-place
 - general
 - [link](https://github.com/naudio/NAudio)
	
### NWaves 
 - in-place
   - NWaves64 (uses 64 bit value types)
   - NWaves
 - out of place (data allocation is not measured)
   - NWaves64Real (uses 64 bit value types)
   - NWavesReal 
 - both general and real value specialized
 - [link](https://github.com/ar1st0crat/NWaves)

## Benchmarks

Benchmarks were run on machine with following stats:
 - OS=Windows 10 (10.0.19042.1889/20H2/October2020Update)
 - Intel Core i5-10210U CPU 1.60GHz, 1 CPU, 8 logical and 4 physical cores
 - .NET SDK = 6.0.201
 - L1 cache = 256KB
 - L2 cache = 1MB
 - L3 cache = 6MB

### Legend
 | Legend                  | Meaning                       |
 |-------------------------|-------------------------------|
 | Size                    | Value of the 'Size' parameter |
 | Mean                    | Arithmetic mean of all measurements |
 | Error                   | Half of 99.9% confidence interval |
 | StdDev                  | Standard deviation of all measurements |
 | Median                  | Value separating the higher half of all measurements (50th percentile)   |
 | Ratio                   | Mean of the ratio distribution ([Current]/[Baseline])  |
 | RatioSD                 | Standard deviation of the ratio distribution ([Current]/[Baseline]) |
 | BranchInstructions/Op   | Hardware counter 'BranchInstructions' per single operation |
 | LLCReference/Op         | Hardware counter 'LLCReference' per single operation |
 | BranchMispredictions/Op | Hardware counter 'BranchMispredictions' per single operation |
 | Gen0                    | GC Generation 0 collects per 1000 operations |
 | Gen1                    | GC Generation 1 collects per 1000 operations |
 | Gen2                    | GC Generation 2 collects per 1000 operations |
 | Allocated               | Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)   |
 | 1 us                    | 1 Microsecond (0.000001 sec)|
 |RatioBranchMispredictions| BranchInstructions/BranchMispredictions|

 All benchamrks have the same input with the exception that some benchmarks use doubles and some use the `Complex` struct. In that case doubles are converted before the benchmark in such a way that the amount of data is the same and the values are the same as well. All benchmarks were performed two times in 15 iterations with 10 warmup iterations.

 It must be said that the machine was struggling heavily with thermal throttling. The surface was almost unbearably hot which might have affected the results a little bit.


### With all the counters
In the following benchmark we measured the time, allocated memory, GC activity and selected hardware instructions - branch instructions, branch mispredictions and LLC references. The benchmark runs on three different input sizes.

> Because the for-loop implementation was too slow - order of tens of minutes, I have ommited that implementation from this benchmark as it does not provide any useful information.

|       Method |     Size |            Mean |           Error |          StdDev |          Median | Ratio | RatioSD | BranchInstructions/Op | LLCReference/Op | BranchMispredictions/Op |      Gen0 |      Gen1 |      Gen2 |   Allocated |   RatioBranchMispredictions |
|------------- |--------- |----------------:|----------------:|----------------:|----------------:|------:|--------:|----------------------:|----------------:|------------------------:|----------:|----------:|----------:|------------:|----------------------------:|
|       Accord |    16384 |        295.0 us |         8.82 us |        12.65 us |        294.1 us |  0.95 |    0.10 |               412,123 |          15,853 |                     617 |   20.5078 |         - |         - |     65584 B |                      0.0014 |
|       Lomont |    16384 |        312.8 us |        25.35 us |        37.94 us |        287.7 us |  1.00 |    0.00 |               312,169 |           6,925 |                     552 |         - |         - |         - |           - |                      0.0018 |
|      MathNet |    16384 |        454.2 us |        19.71 us |        28.89 us |        464.1 us |  1.48 |    0.25 |               613,186 |         182,315 |                   4,769 |    7.8125 |         - |         - |     25024 B |                      0.0078 |
|       NAudio |    16384 |        237.7 us |        12.14 us |        17.79 us |        226.5 us |  0.76 |    0.05 |               227,486 |           1,595 |                     871 |         - |         - |         - |           - |                      0.0038 |
|     NWaves64 |    16384 |        348.7 us |        23.79 us |        34.87 us |        355.0 us |  1.13 |    0.21 |               335,443 |           5,521 |                     924 |         - |         - |         - |           - |                      0.0027 |
| NWaves64Real |    16384 |        194.8 us |        16.18 us |        22.68 us |        213.5 us |  0.63 |    0.14 |               350,224 |          11,678 |                     431 |         - |         - |         - |           - |                      0.0012 |
|       NWaves |    16384 |        657.9 us |        25.52 us |        38.19 us |        653.2 us |  2.13 |    0.21 |               710,981 |          12,053 |                   1,567 |         - |         - |         - |           - |                      0.0022 |
|   NWavesReal |    16384 |        172.2 us |         4.53 us |         6.63 us |        174.1 us |  0.56 |    0.07 |               349,817 |           3,337 |                     440 |         - |         - |         - |           - |                      0.0013 |
|              |          |                 |                 |                 |                 |       |         |                       |                 |                         |           |           |           |             |                             |
|       Accord |    65536 |      1,526.1 us |        81.74 us |       122.34 us |      1,500.5 us |  0.83 |    0.10 |             1,835,556 |         602,223 |                   2,909 |   70.3125 |   70.3125 |   70.3125 |    262213 B |                      0.0016 |
|       Lomont |    65536 |      1,850.0 us |       117.99 us |       176.60 us |      1,940.3 us |  1.00 |    0.00 |             1,428,958 |       1,765,171 |                   3,186 |         - |         - |         - |         2 B |                      0.0022 |
|      MathNet |    65536 |      1,997.9 us |        76.13 us |       111.59 us |      1,976.6 us |  1.09 |    0.13 |             2,231,484 |       1,580,271 |                  13,996 |   11.7188 |         - |         - |     41640 B |                      0.0063 |
|       NAudio |    65536 |      1,246.3 us |        49.15 us |        72.04 us |      1,217.5 us |  0.68 |    0.09 |             1,011,840 |          99,268 |                   2,643 |         - |         - |         - |         1 B |                      0.0026 |
|     NWaves64 |    65536 |      2,562.9 us |       196.88 us |       282.36 us |      2,391.7 us |  1.41 |    0.27 |             1,524,804 |       2,343,087 |                   3,752 |         - |         - |         - |         2 B |                      0.0025 |
| NWaves64Real |    65536 |      1,046.3 us |        26.44 us |        37.92 us |      1,024.2 us |  0.58 |    0.07 |             1,551,166 |         208,333 |                   1,876 |         - |         - |         - |         1 B |                      0.0012 |
|       NWaves |    65536 |      4,459.1 us |       131.53 us |       184.39 us |      4,573.2 us |  2.45 |    0.21 |             3,209,088 |       4,062,182 |                   6,231 |         - |         - |         - |         3 B |                      0.0019 |
|   NWavesReal |    65536 |      1,029.8 us |        29.92 us |        42.91 us |      1,007.0 us |  0.57 |    0.07 |             1,550,229 |          42,509 |                   1,860 |         - |         - |         - |           - |                      0.0012 |
|              |          |                 |                 |                 |                 |       |         |                       |                 |                         |           |           |           |             |                             |
|       Accord | 33554432 |  4,872,385.7 us |   358,860.53 us |   503,072.35 us |  5,075,896.4 us |  0.79 |    0.07 |         1,370,082,509 |   1,143,786,701 |               2,124,732 | 1000.0000 | 1000.0000 | 1000.0000 | 134220184 B |                      0.0016 |
|       Lomont | 33554432 |  6,211,433.0 us |   114,084.14 us |   170,755.77 us |  6,204,747.7 us |  1.00 |    0.00 |         1,162,206,686 |   1,421,877,794 |               2,081,860 |         - |         - |         - |      2072 B |                      0.0018 |
|      MathNet | 33554432 |  4,228,668.0 us |    88,080.41 us |   123,476.43 us |  4,260,529.8 us |  0.68 |    0.01 |         1,366,399,386 |   1,253,590,084 |               4,030,191 | 2000.0000 |         - |         - |   6363784 B |                      0.0029 |
|       NAudio | 33554432 |  5,241,807.1 us |    72,228.49 us |    96,422.97 us |  5,182,234.6 us |  0.85 |    0.03 |           792,199,168 |   1,216,645,257 |               1,930,581 |         - |         - |         - |      2208 B |                      0.0024 |
|     NWaves64 | 33554432 |  9,327,703.9 us |   431,499.37 us |   618,843.50 us |  8,849,794.2 us |  1.50 |    0.06 |         1,245,236,429 |   2,722,736,811 |               3,240,482 |         - |         - |         - |      3704 B |                      0.0026 |
| NWaves64Real | 33554432 |  4,047,460.0 us |    92,780.44 us |   138,869.39 us |  4,054,896.1 us |  0.65 |    0.01 |         1,173,596,843 |     983,079,322 |               1,641,677 |         - |         - |         - |      2072 B |                      0.0014 |
|       NWaves | 33554432 | 14,964,174.4 us | 1,599,899.33 us | 2,242,835.41 us | 17,041,241.7 us |  2.41 |    0.31 |         2,545,649,801 |   5,115,574,135 |               5,868,203 |         - |         - |         - |      3704 B |                      0.0023 |
|   NWavesReal | 33554432 |  3,471,235.9 us |    43,718.20 us |    65,435.35 us |  3,482,753.2 us |  0.56 |    0.01 |         1,167,239,851 |     906,943,966 |               1,523,985 |         - |         - |         - |      2072 B |                      0.0013 |



At first sight, we can point out several observations. 
 - We can see that Lomont implementation was not the fastest at all therefore not the best choice for the thesis.
 - We can also see that with the exception of *Math.NET* the input size did not matter that much and the relative speeds remain the same. 
 - We can also see that the fastest implementation - *NWavesReal* - is also the most stable (with the exception of the medium sized input where NWaves64Real is negligibly close).
 - With larger input size the relative speed comparison becomes more adequate since the relative comparison holds even if we take error into an account.
 - If we take a look at the hardware counters we can see a that the slowest implementation - *NWaves* - has consistently the highest number of branch instructions and with the exception of the smalles input size it has the highest LLCReference counter. Thah might be expected since the *NWave* library offers three more implementations which are more specialized either for real numbers or for 64 bit nubmers therefore the implementation for the most generic case might have to solve the most edgecases and therefore not be optimized that well.
 - As to why the NWavesReal might be the fastest is inconclusive from this data. There is no single metric that would show why it might be the fastest.
 - From the memory allocation point of view we can see that it has little to none impact on the performance. Even the library *Accord* which allocates the same amount of memory as it is needed to save the real values of the input is faster than the Lomont implementation which does not allocate any memory at all.
 - We can observe unusuall behaviour that the *Accord* implementation invokes all three generations of GC. By what I understand the reason behind this might be that the data are passed immediatelly between generations and then cleaned together (in another words data are registered by every generation and cleaned together). 
 - The reason for high number of invocations of Gen0 GC at *MathNet* might be due to different approach to handling the data in the implementation - it suggets that it uses a lot of short lived data.
  > Note that we can be sure that the memory allocation and GC activity does not happen only for the first benchmark because the benchmark was run with mixed up order of individual benchmarks and the results were the same. 

We might now talk why is it that *NWavesReal* is the fastest. The *NWaves* library offers the most specialized implementations of FFT - the version that uses only real value inputs (comlex values are implicitly zero). Even though we are running on 64-bit processor the version using 32 bits is faster. Since .NET optimizes on its own for vector instructions the 32-bit version might be faster because it can use SIMD instructions. Sadly we can not verify that because we do not have an adequate hardware counter for that.

### Without additional counters
In next benchmark I have turned off memory analyzer, HW counters and removal of the outliers.

|       Method |     Size |            Mean |         Error |        StdDev | Ratio | RatioSD |
|------------- |--------- |----------------:|--------------:|--------------:|------:|--------:|
|       Accord |    16384 |        344.1 us |      28.81 us |      43.13 us |  1.29 |    0.16 |
|       Lomont |    16384 |        265.8 us |       1.15 us |       1.73 us |  1.00 |    0.00 |
|      MathNet |    16384 |        383.1 us |       4.08 us |       6.11 us |  1.44 |    0.02 |
|       NAudio |    16384 |        215.7 us |       6.83 us |      10.23 us |  0.81 |    0.04 |
|     NWaves64 |    16384 |        304.8 us |       1.49 us |       2.22 us |  1.15 |    0.01 |
| NWaves64Real |    16384 |        168.0 us |       1.08 us |       1.61 us |  0.63 |    0.01 |
|       NWaves |    16384 |        635.0 us |       1.85 us |       2.77 us |  2.39 |    0.02 |
|   NWavesReal |    16384 |        152.5 us |       1.25 us |       1.87 us |  0.57 |    0.01 |
|              |          |                 |               |               |       |         |
|       Accord |    65536 |      1,431.0 us |      28.31 us |      42.37 us |  0.88 |    0.03 |
|       Lomont |    65536 |      1,632.0 us |       7.81 us |      11.70 us |  1.00 |    0.00 |
|      MathNet |    65536 |      1,772.9 us |      13.30 us |      19.91 us |  1.09 |    0.02 |
|       NAudio |    65536 |      1,169.7 us |       6.41 us |       9.59 us |  0.72 |    0.01 |
|     NWaves64 |    65536 |      2,112.8 us |       9.14 us |      13.68 us |  1.29 |    0.01 |
| NWaves64Real |    65536 |        916.5 us |       8.75 us |      13.09 us |  0.56 |    0.01 |
|       NWaves |    65536 |      3,726.0 us |      15.45 us |      23.13 us |  2.28 |    0.02 |
|   NWavesReal |    65536 |        929.3 us |       7.19 us |      10.77 us |  0.57 |    0.01 |
|              |          |                 |               |               |       |         |
|       Accord | 33554432 |  5,346,128.2 us | 131,226.21 us | 196,413.22 us |  0.83 |    0.03 |
|       Lomont | 33554432 |  6,441,349.1 us |  28,158.73 us |  42,146.66 us |  1.00 |    0.00 |
|      MathNet | 33554432 |  4,244,400.1 us |  28,551.41 us |  42,734.40 us |  0.66 |    0.01 |
|       NAudio | 33554432 |  5,771,988.7 us | 108,015.46 us | 161,672.46 us |  0.90 |    0.03 |
|     NWaves64 | 33554432 |  9,468,110.5 us | 346,384.77 us | 518,452.41 us |  1.47 |    0.09 |
| NWaves64Real | 33554432 |  4,094,550.0 us |  33,795.04 us |  50,582.82 us |  0.64 |    0.01 |
|       NWaves | 33554432 | 16,749,539.0 us | 541,033.86 us | 809,794.00 us |  2.60 |    0.14 |
|   NWavesReal | 33554432 |  3,527,683.8 us |  30,586.63 us |  45,780.63 us |  0.55 |    0.01 |

As we can see the relative results remained same with the exception of the first Accord benchmark which has unusually big error which might be the reason why its ratio is different. Overall the mean times have become smaller, which is expected. Also all of the benchmarks have become much more stable which is also expected and even more desirable. The second benchmark provides us much less information than the first one but the results are more reliable and more importantly correspond to the first benchmark.

## Conclusion
We can conclude that the NWavesReal implementation is the fastest as well as the most stable one.


## Other benchmarks
Note that since most of the implementations do not allocate any memory the change of GC should not have big impact on the results. Either way I have decieded to try the GCServer option on. The results are as follows:

|     Method |     Size |           Mean |        Error |        StdDev |         Median | Ratio | RatioSD |    Gen0 |    Gen1 |    Gen2 |   Allocated |
|----------- |--------- |---------------:|-------------:|--------------:|---------------:|------:|--------:|--------:|--------:|--------:|------------:|
|     Accord |    16384 |       364.9 us |     41.71 us |      62.43 us |       372.6 us |  1.38 |    0.23 |  2.4414 |       - |       - |     65584 B |
|     Lomont |    16384 |       264.9 us |      7.41 us |      11.09 us |       260.6 us |  1.00 |    0.00 |       - |       - |       - |           - |
|    MathNet |    16384 |       373.9 us |      4.41 us |       6.61 us |       373.3 us |  1.41 |    0.06 |  0.4883 |       - |       - |     25051 B |
| NWavesReal |    16384 |       194.1 us |     39.02 us |      58.41 us |       159.0 us |  0.74 |    0.23 |       - |       - |       - |           - |
|            |          |                |              |               |                |       |         |         |         |         |             |
|     Accord |    65536 |     1,475.2 us |     20.61 us |      30.84 us |     1,467.6 us |  0.93 |    0.02 | 13.6719 | 13.6719 | 13.6719 |    262197 B |
|     Lomont |    65536 |     1,580.9 us |     10.26 us |      15.35 us |     1,579.3 us |  1.00 |    0.00 |       - |       - |       - |         1 B |
|    MathNet |    65536 |     1,797.5 us |     12.22 us |      18.29 us |     1,798.7 us |  1.14 |    0.02 |       - |       - |       - |     41603 B |
| NWavesReal |    65536 |       955.5 us |    132.26 us |     197.96 us |       891.0 us |  0.60 |    0.13 |       - |       - |       - |           - |
|            |          |                |              |               |                |       |         |         |         |         |             |
|     Accord | 33554432 | 4,524,817.7 us | 83,710.79 us | 125,294.36 us | 4,475,665.5 us |  0.71 |    0.02 |       - |       - |       - | 134218696 B |
|     Lomont | 33554432 | 6,340,500.8 us | 30,278.45 us |  45,319.36 us | 6,328,156.8 us |  1.00 |    0.00 |       - |       - |       - |      1208 B |
|    MathNet | 33554432 | 4,215,916.9 us | 42,317.50 us |  63,338.85 us | 4,208,786.2 us |  0.66 |    0.01 |       - |       - |       - |   6365168 B |
| NWavesReal | 33554432 | 3,770,306.2 us | 35,646.45 us |  53,353.92 us | 3,774,766.5 us |  0.59 |    0.01 |       - |       - |       - |      1072 B |

We can see that the number of GC invokations is lower as expected since GC Server has bigger threshold before cleanup than the Workstation GC. The relative times remain the same though. Very weird is that there are no GC invocations on the biggest input size. One theory might be that the data were marked as Gen3 (longlived) and were not considered for garbage collections at all. 

Interesting point here is that the Error at *NWavesReal* has become signifiicantly bigger. Also Error at Accord in the first benchmark is high as it was in the previous benchmark run. I am not sure what is the exact reason behind that. It must be said that we should be catious to make any conclusions since the benchmarks are stable and provide us with the same results on every run with some implementations but behave differently on others. We can not determine wether that is due to the implementation or the benchmark itself (let me remind that benchmarks were ran in a different order as well).