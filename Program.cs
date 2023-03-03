
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
        // RegistryKey rootKey = Registry.ClassesRoot;

        // Get the root key for the local machine registry hive
        RegistryKey rootKey = Registry.LocalMachine;

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
            try
            {
                using (RegistryKey subKey = key.OpenSubKey(subKeyName))
                {
                    if (subKey != null)
                    {
                        SearchRegistryKeys(subKey, searchTerm, results);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message+ "For key: " + subKeyName);
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
