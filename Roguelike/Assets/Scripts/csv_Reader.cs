using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class csv_Reader : MonoBehaviour {
    void Start() {

    }

    public static List<string[]> Load_csv(string file_path, int skip_line_number = 0) {
        TextAsset csv = Resources.Load(file_path) as TextAsset;
        StringReader reader = new StringReader(csv.text);

        List<string[]> tmp = new List<string[]>();

        int line_count = 1;
        while (reader.Peek() > -1) {
            string line = reader.ReadLine();

            if (skip_line_number < line_count) {
                string[] values = line.Split(',');
                tmp.Add(values);
            }

            ++line_count;
        }

        return tmp;
    }
}