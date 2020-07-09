using System;
using System.Collections.Generic;
using System.Text;

namespace P03_SalesDatabase.Data.IOManagement.Contracts
{
    public interface IWriter
    {
        void Write(string text);
        void WriteLine(string text);
    }
}
