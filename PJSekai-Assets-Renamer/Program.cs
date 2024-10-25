using MessagePack;
using PJSekai_Assets_Renamer;
using Sekai;

Console.WriteLine("PJSekai Assets Renamer");
if (args.Length < 2 || args[0] == "--help")
{
    Console.WriteLine("Usage: PJSekai-Assets-Renamer.exe [Asset Data Folder Path] [Output Folder Path]");
    Environment.Exit(0);
}

string dataFolderPath = args[0].Trim('"');

// Load AssetBundleInfo
var assetBundleInfo = MessagePackSerializer.Deserialize<AssetBundleInfo>( File.ReadAllBytes( Path.Combine(dataFolderPath, "AssetBundleInfo") ) );

string outputPath = args[1].Trim('"');

foreach(var pair in assetBundleInfo.bundles)
{
    var element = pair.Value;
    try
    {
        string sourceDataPath = element.cacheDirectoryName == "" ? Path.Combine(dataFolderPath, element.cacheFileName) : Path.Combine(dataFolderPath, element.cacheDirectoryName, element.cacheFileName);
        if (!File.Exists(sourceDataPath))
        {
            Console.WriteLine($"File {element.cacheFileName} does not exist");
            continue;
        }
        byte[] sourceDataBytes = File.ReadAllBytes(sourceDataPath);

        string outputFilePath = Path.Combine(outputPath, Path.Combine(element.bundleName.Split('/')));
        Directory.CreateDirectory(Path.Combine(outputFilePath.Split(Path.DirectorySeparatorChar).SkipLast(1).ToArray()));
        var fileStream = File.OpenWrite(outputFilePath);
        var decryptedBytes = Decryptor.DecryptAsset(sourceDataBytes);
        fileStream.Write(decryptedBytes, 4, decryptedBytes.Length - 4);
        fileStream.Close();

        Console.WriteLine(element.bundleName);
    }
    catch (Exception ex)
    {
        Console.WriteLine("An exception occoured");
        Console.WriteLine("Bundle Name: " + element.bundleName);
        Console.WriteLine("Exception Message: " + Environment.NewLine + ex.Message);
        Console.WriteLine();
    }
}
