using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// csvファイルを読み取る TODO:Excelでマクロ組む
/// </summary>
public class csv_Reader {
    /// <summary>
    /// csvファイル読み取り関数
    /// </summary>
    /// <param name="file_path">読み取るファイルのパス</param>
    /// <param name="skip_line_number">読み飛ばす行数(上から)</param>
    /// <returns>行ごとに読み取ったList</returns>
    public List<string[]> Load_csv(string file_path, int skip_line_number = 0) {
        TextAsset csv = Resources.Load(file_path) as TextAsset;
        StringReader reader = new StringReader(csv.text);

        // 読み取った物を行ごとに格納する
        var tmp = new List<string[]>();

        int line_count = 1;
        // 使用可能な文字を返す
        while (reader.Peek() > -1) {
            string line;
            // 行を読み取る
            line = reader.ReadLine();

            if (skip_line_number < line_count) {
                // ','で区切って配列に格納する
                string[] values = line.Split(',');
                tmp.Add(values);
            }
            ++line_count;
        }
        return tmp;
    }
}
