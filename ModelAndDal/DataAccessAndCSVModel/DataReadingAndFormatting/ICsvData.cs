using System;

namespace ModelAndDal
{
    public interface ICsvData
    {
        void Format(char seperator, string line);
    }
}