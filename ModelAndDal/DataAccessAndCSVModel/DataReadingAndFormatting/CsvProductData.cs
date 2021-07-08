using System;

namespace ModelAndDal
{
    public class CsvProductData : ICsvData
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; } = 0m;
        public bool IsActive { get; set; } = false;

        private string _textLine = "";
        private char _seperator = ';';


        public void Execute(Action<char, string> functionToExecute)
        {
            functionToExecute?.Invoke(_seperator, _textLine);
        }

        public void Format(char seperator, string line)
        {
            string[] csvData = line.Split(seperator);

            Id = int.Parse(csvData[0]);
            Name = csvData[1];
            Price = decimal.Parse(csvData[2]);
            try
            {
                IsActive = bool.Parse(csvData[3]);
            }
            catch (Exception e)
            {
                if (csvData[3].Equals("0"))
                {
                    IsActive = false;
                }
                else if (csvData[3].Equals("1"))
                {
                    IsActive = true;
                }
                else
                {
                    throw new ArithmeticException("Could not cast from string to bool" + nameof(IsActive), e);
                }
            }
        }

        public static explicit operator Product(CsvProductData productCsvData)
        {
            return new Product(productCsvData.Id, productCsvData.Name, productCsvData.Price, false, productCsvData.IsActive);

        }
    }
}