using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

string projectName = new DirectoryInfo(Directory.GetCurrentDirectory()).Name;
string destinationDir = @"n:\code\backup\" + projectName;
string currentDir = Directory.GetCurrentDirectory();
string tempDir = @"d:\temp\backup";
string time = DateTimeOffset.UtcNow.ToString("yyyyMMdd-HHmmss");

string tempBackupDir = Path.Combine(tempDir, $"{projectName}-{time}");

if (!Directory.Exists(destinationDir))
{
    Directory.CreateDirectory(destinationDir);
}

if (!Directory.Exists(tempDir))
{
    Directory.CreateDirectory(tempDir);
}

var excludeDirs = new string[] { "obj", "bin", ".vs", "pack-result", "nugets" };

// Copy all files that are not in the excluded directories to tempBackupDir
Console.WriteLine("Copying files to temporary directory...");
CopyDirectory(currentDir, tempBackupDir, excludeDirs);
Console.WriteLine("File copy complete.");

string branchName = GetCurrentGitBranch(currentDir);
string zipFileName = $"{projectName}-backup-{time}-{branchName}.zip";
string zipFilePath = Path.Combine(tempDir, zipFileName);

Console.WriteLine("Zipping temporary directory...");
ZipFile.CreateFromDirectory(tempBackupDir, zipFilePath);
Console.WriteLine("Zipping complete.");

// Copy zip to destinationDir
string destinationZipPath = Path.Combine(destinationDir, zipFileName);
long fileLenght = new FileInfo(zipFilePath).Length / 1024 / 1024;
Console.WriteLine($"Copying zip file ({fileLenght} MB) to {destinationZipPath}");
File.Copy(zipFilePath, destinationZipPath);
Console.WriteLine("Backup process completed successfully!");

try
{
    Console.WriteLine($"Deleting file: {zipFilePath}");
    File.Delete(zipFilePath);
    Console.WriteLine($"File deleted: {zipFilePath}");

    Console.WriteLine($"Deleting dir: {tempBackupDir}");
    StripReadOnly(tempBackupDir);
    Directory.Delete(tempBackupDir, true);
    Console.WriteLine("Temporary files cleaned up.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error cleaning up temporary files: {ex.Message}");
    Console.WriteLine("Waiting 120 seconds");
    Thread.Sleep(120_000);
    
    try
    {
        Console.WriteLine("Second try");

        if (File.Exists(zipFilePath))
        {
            Console.WriteLine($"Deleting file: {zipFilePath}");
            File.Delete(zipFilePath);
        }
        else
        {
            Console.WriteLine("Skipping file delete, already deleted");
        }

        Console.WriteLine($"Deleting dir: {tempBackupDir}");
        StripReadOnly(tempBackupDir);
        Directory.Delete(tempBackupDir, true);
        Console.WriteLine("Temporary files cleaned up.");
    }
    catch 
    {
        Console.WriteLine($"Error cleaning up temporary files: {ex.Message}");

        Console.WriteLine("Waiting 30 seconds");
        Thread.Sleep(30_000);

        try
        {
            Console.WriteLine("Second try");

            if (File.Exists(zipFilePath))
            {
                Console.WriteLine($"Deleting file: {zipFilePath}");
                File.Delete(zipFilePath);
            }
            else
            {
                Console.WriteLine("Skipping file delete, already deleted");
            }

            Console.WriteLine($"Deleting dir: {tempBackupDir}");
            StripReadOnly(tempBackupDir);
            Directory.Delete(tempBackupDir, true);
            Console.WriteLine("Temporary files cleaned up.");
        }
        catch
        {
            Console.WriteLine($"Error cleaning up temporary files: {ex.Message}");
        }
    }
}

static void CopyDirectory(string sourceDir, string destinationDir, string[] excludeDirs)
{
    var dir = new DirectoryInfo(sourceDir);

    if (!dir.Exists)
    {
        throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");
    }

    Directory.CreateDirectory(destinationDir);

    // Copy all files.
    foreach (FileInfo file in dir.GetFiles())
    {
        string targetFilePath = Path.Combine(destinationDir, file.Name);
        file.CopyTo(targetFilePath);
    }

    // Recursively copy subdirectories.
    foreach (DirectoryInfo subDir in dir.GetDirectories())
    {
		if (subDir.Name.Contains(".ncrunch_", StringComparison.OrdinalIgnoreCase))
		{
			continue;
		}
		
        if (!excludeDirs.Contains(subDir.Name))
        {
            string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
            CopyDirectory(subDir.FullName, newDestinationDir, excludeDirs);
        }
    }
}

static void StripReadOnly(string directoryPath)
{
    if (!Directory.Exists(directoryPath))
    {
        return;
    }

    // Strip ReadOnly from all files and directories recursively
    // Directories must be processed too — Git creates read-only dirs (e.g., .git/objects/...)
    foreach (var path in Directory.EnumerateFileSystemEntries(directoryPath, "*", SearchOption.AllDirectories))
    {
        var attrs = File.GetAttributes(path);
        if ((attrs & FileAttributes.ReadOnly) != 0)
            File.SetAttributes(path, attrs & ~FileAttributes.ReadOnly);
    }

    // Also strip the root directory itself
    var rootAttrs = File.GetAttributes(directoryPath);
    if ((rootAttrs & FileAttributes.ReadOnly) != 0)
        File.SetAttributes(directoryPath, rootAttrs & ~FileAttributes.ReadOnly);
}

static string GetCurrentGitBranch(string workingDirectory)
{
    try
    {
        var startInfo = new ProcessStartInfo("git")
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            Arguments = "rev-parse --abbrev-ref HEAD",
            WorkingDirectory = workingDirectory
        };

        using (var process = Process.Start(startInfo))
        {
            if (process == null)
            {
                return "no-git-process";
            }
            process.WaitForExit();
            string branchName = process.StandardOutput.ReadToEnd().Trim();

            // Sanitize branch name for use in a file name by replacing directory separators
            return string.IsNullOrEmpty(branchName) ? "unknown-branch" : branchName.Replace('/', '-').Replace('\\', '-');
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Could not determine Git branch. Is Git installed and are you in a Git repository? Error: {ex.Message}");
        return "no-git";
    }
}