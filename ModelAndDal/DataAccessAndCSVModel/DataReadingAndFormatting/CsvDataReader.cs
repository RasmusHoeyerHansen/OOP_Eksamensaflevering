using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ModelAndDal
{
    public class CsvDataReader<T> where T : ICsvData, new()
    {
        private readonly char _sepator;

        public CsvDataReader(char seperator)
        {
            _sepator = seperator;
        }

        /// <summary>
        /// Reads data from file, and returns array of specified ICsvData
        /// </summary>
        /// <param name="path">, path to file to read from.</param>
        /// <returns></returns>
        public IEnumerable<T> ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            string[] csvDataString = File.ReadAllLines(path);
            return File.ReadAllLines(path).Skip(1)
                .Select(dataString=>
                {
                    dataString = FormatString(dataString);
                    T data = new T();
                    data.Format(_sepator, dataString);
                    return data;
                });
        }


        private string FormatString(string dataString)
        { 
            return Regex.Replace(dataString, "((<[^>]*>)*(\")*)", string.Empty);
        }
    }
 }
        

