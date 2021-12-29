﻿using DeltaQ.SuffixSorting;
using DeltaQ.SuffixSorting.LibDivSufSort;
using DeltaQ.SuffixSorting.SAIS;
using Humanizer;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.IO;

namespace DeltaQ.CommandLine;
using static Defaults;

internal static partial class Commands
{
    public static Action<CommandLineApplication> DeltaCommand { get; } = command =>
    {
        command.Description = "Generate a delta (difference) between two files";
        command.HelpOption(HelpOptions);

        var oldFileArg = command.Argument("[oldfile]", "");
        var newFileArg = command.Argument("[newfile]", "");
        var deltaFileArg = command.Argument("[deltafile]", "");
        var algoArg = command.Option("-ss|--suffix-sort <LIB>", "Suffix sort library: [sais], [divsufsort]", CommandOptionType.SingleValue);

        command.OnExecute(() =>
        {
            var oldFile = oldFileArg.Value;
            var newFile = newFileArg.Value;
            var deltaFile = deltaFileArg.Value;
            ISuffixSort sort = algoArg.Value() switch
            {
                "sais" => new SAIS(),
                "divsufsort" => new LibDivSufSort(),
                _ => GetDefaultSort()
            };
            Console.WriteLine("Generating BsDiff delta between");
            Console.WriteLine($@"Old file: ""{oldFile}""");
            Console.WriteLine($@"New file: ""{newFile}""");
            Console.WriteLine($"with suffix sort {sort.GetType().Name}");
            Console.WriteLine();
            try
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                BsDiff.Diff.Create(File.ReadAllBytes(oldFile), File.ReadAllBytes(newFile), File.Create(deltaFile), sort);
                sw.Stop();
                
                Console.WriteLine($"Finished in {sw.Elapsed.Humanize()} [{sw.Elapsed}]");
                Console.WriteLine($@"Delta file: ""{deltaFile}""");
                var deltaFileInfo = new FileInfo(deltaFile);
                var deltaFileRatio = (double)deltaFileInfo.Length / (new FileInfo(oldFile).Length + new FileInfo(newFile).Length);
                Console.WriteLine($@"Delta size: {deltaFileInfo.Length.Bytes()} ({deltaFileRatio:0.00%})");
            }
            catch
            {
                Console.Error.WriteLine("Failed to create delta");
                throw;
            }
            return 0;
        });
    };
}