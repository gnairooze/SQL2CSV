using SQL2CSV.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace SQL2CSV.Business
{
    public class Manager
    {
        #region attributes
        private readonly Interfaces.Models.Settings _Settings;
        private readonly IFileManager _FileManager;
        private readonly IEmailManager _EmailManager;
        #endregion

        public Manager(Interfaces.Models.Settings settings, IFileManager fileManager, IEmailManager emailManager)
        {
            _Settings = settings;
            _FileManager = fileManager;
            _EmailManager = emailManager;
        }

        public void ReadData()
        {
            DateTime startTime = DateTime.Now.ToUniversalTime();

            Console.WriteLine("Operation Started at {0} UTC.", startTime.ToString());

            using (var con = new SqlConnection(this._Settings.DataSetting.ConnectionString))
            {
                using SqlCommand? cmd = new(_Settings.DataSetting.SQL, con);
                cmd.CommandTimeout = this._Settings.DataSetting.Timeout;
                con.Open();
                using IDataReader rdr = cmd.ExecuteReader();
                int counter = 0;


                _FileManager.DeleteFile();

                StringBuilder b = new();
                do
                {
                    int rowCounter = 0;
                    int bufferCounter = 0;
                    int rowBufferCounter = 0;
                    int batchCount = 1;
                    counter++;

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("ResultSet {0}", counter.ToString());

                    List<string> columnNames = new();
                    object[] values = new object[rdr.FieldCount];
                    string[] valueTexts;

                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        columnNames.Add(rdr.GetName(i));
                    }

                    _FileManager.CreateFile(batchCount, columnNames);

                    while (rdr.Read())
                    {
                        if (this._Settings.DataSetting.MaxRecords > 0 && rowCounter == this._Settings.DataSetting.MaxRecords)
                        {
                            bufferCounter++;

                            _FileManager.AppendData(b.ToString());

                            b.Remove(0, b.Length);
                            rowBufferCounter = 0;

                            Console.Write("\rRow {0} reached.", rowCounter.ToString());

                            _FileManager.CompressWithBuffer();
                            //create new file
                            rowCounter = 0;
                            batchCount++;
                            _FileManager.CreateFile(batchCount, columnNames);
                        }
                        rowCounter++;
                        rowBufferCounter++;
                        rdr.GetValues(values);

                        valueTexts = ConvertObjectsToStrings(values);
                        _ = b.AppendLine(string.Join(this._Settings.CSVSetting.Separator, valueTexts));

                        if (rowBufferCounter == this._Settings.FileSetting.BufferWrite)
                        {
                            bufferCounter++;

                            _FileManager.AppendData(b.ToString());
                            b.Remove(0, b.Length);
                            rowBufferCounter = 0;

                            Console.Write("\rRow {0} reached.", rowCounter.ToString());
                        }

                    }

                    if (rowBufferCounter > 0)
                    {
                        bufferCounter++;

                        _FileManager.AppendData(b.ToString());
                        b.Remove(0, b.Length);
                        rowBufferCounter = 0;

                        Console.Write("\rRow {0} reached.", rowCounter.ToString());
                    }

                    Console.WriteLine();

                    string compressedFilename = _FileManager.CompressWithBuffer();

                    _EmailManager.SendEmail(_FileManager.FileName, compressedFilename);

                } while (rdr.NextResult());
            }

            Console.WriteLine();
            Console.WriteLine();
            DateTime stopTime = DateTime.Now.ToUniversalTime();
            Console.WriteLine("Operation Stopped at {0} UTC.", stopTime.ToString());
            Console.WriteLine("Operation Duration is {0} minutes.", (stopTime - startTime).TotalMinutes.ToString());
        }

        private string[] ConvertObjectsToStrings(object[] arr)
        {
            if (arr == null)
            {
                return Array.Empty<string>();
            }

            int len = arr.Length;
            List<string> results = new();

            for (int i = 0; i < len; i++)
            {
                string? arrayValue = arr[i].ToString();
                if (arrayValue == null)
                {
                    continue;
                }

                string v = TextManagement.Sanitize(arrayValue, this._Settings.CSVSetting);
                
                if (this._Settings.CSVSetting.Quote && v != "NULL")
                {
                    results.Add("\"" + v + "\"");
                }
                else
                {
                    results.Add(v);
                }
            }

            return results.ToArray();
        }
    }
}
