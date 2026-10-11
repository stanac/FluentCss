using BenchmarkDotNet.Running;

namespace FluentCss.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ClassRegistryBenchmarks>();
        }
    }
}
