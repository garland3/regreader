
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;

class Program
{
    static void Main(string[] args)
    {
        string searchTerm = "Sld";
        string csvFilePath = "output.csv";

        // Get the root key for HKEY_CLASSES_ROOT
        RegistryKey rootKey = Registry.ClassesRoot;

        // Create a list to store the results
        List<KeyValuePair<string, object>> results = new List<KeyValuePair<string, object>>();

        // Recursively search for keys and subkeys that contain the search term
        SearchRegistryKeys(rootKey, searchTerm, results);

        // Write the results to a CSV file
        WriteResultsToCsv(results, csvFilePath);

        Console.WriteLine("Search complete");
        Console.WriteLine($"Results written to {csvFilePath}");
    }

    static void SearchRegistryKeys(RegistryKey key, string searchTerm, List<KeyValuePair<string, object>> results)
    {
        // Search the current key for the search term
        if (key.Name.Contains(searchTerm))
        {
            // Add the key and value to the results list
            results.Add(new KeyValuePair<string, object>(key.Name, key.GetValue(null)));
        }

        // Recursively search all subkeys
        foreach (string subKeyName in key.GetSubKeyNames())
        {
            using (RegistryKey subKey = key.OpenSubKey(subKeyName))
            {
                if (subKey != null)
                {
                    SearchRegistryKeys(subKey, searchTerm, results);
                }
            }
        }
    }

    static void WriteResultsToCsv(List<KeyValuePair<string, object>> results, string csvFilePath)
    {
        // Write the results to a CSV file
        using (StreamWriter sw = new StreamWriter(csvFilePath))
        {
            sw.WriteLine("Key,Value");

            foreach (KeyValuePair<string, object> result in results)
            {
                sw.WriteLine($"{result.Key},{result.Value}");
            }
        }
    }
}

// using Microsoft.Win32;
// // include console
// using System;

// class Program
// {
//     static void Main(string[] args)
//     {
//         // Open the SolidWorks key in the registry
//         // RegistryKey solidWorksKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\SolidWorks");
//         RegistryKey solidWorksKey = Registry.CurrentUser.OpenSubKey("Software\\SolidWorks");


//         if (solidWorksKey != null)
//         {
//             foreach (string subKeyName in solidWorksKey.GetSubKeyNames())
//             {
//                 using (RegistryKey subKey = solidWorksKey.OpenSubKey(subKeyName))
//                 {
//                     string versionString = subKey?.GetValue("Version")?.ToString();
//                     if (!string.IsNullOrEmpty(versionString))
//                     {
//                         Version version = new Version(versionString);
//                         Console.WriteLine($"SolidWorks version found in subkey {subKeyName}: {version}");
//                     }
//                 }
//             }
//         }
//         else
//         {
//             // Output an error message to the console
//             Console.WriteLine("SolidWorks is not installed");
//         }

//         // Wait for the user to press a key before exiting
//         Console.WriteLine("Press any key to exit...");
//         Console.ReadKey();
//     }
// }
