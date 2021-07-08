using System;

namespace ModelAndDal
{
    public class CsvUserData : ICsvData
    {
        private int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public decimal Balance { get; set; }


        public static explicit operator User(CsvUserData userCsvData)
        {
            return new User(userCsvData.Id, userCsvData.FirstName, userCsvData.LastName, userCsvData.Username, userCsvData.Email, userCsvData.Balance);
        }
        public void Format(char seperator, string csvDataLine)
        {
            string[] data = csvDataLine.Split(seperator);
            Id = int.Parse(data[0]);
            FirstName = data[1];
            LastName = data[2];
            Username = data[3];
            Balance = decimal.Parse(data[4]);
            Email = data[5];
        }

    }
}