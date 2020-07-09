using P01_HospitalDatabase.Data;
using System;

namespace P01_HospitalDatabase
{
    class StartUp
    {
        static void Main()
        {
            var hospitalContext = new HospitalContext();
            hospitalContext.Database.EnsureDeleted();
            hospitalContext.Database.EnsureCreated();
        }
    }
}
