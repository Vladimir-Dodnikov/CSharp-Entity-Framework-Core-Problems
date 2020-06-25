using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Text;

namespace _04._Add_Minion
{
    public class StartUp
    {
        private const string connectionString = @"Data Source=DESKTOP-MQ67QHE;Database=MinionsDB;Integrated Security=True";
        static void Main()
        {
            using SqlConnection sqlConnection = new SqlConnection(connectionString);

            var minionInput = Console.ReadLine().Split(": ").ToArray();
            var minionInfo = minionInput[1].Split(" ").ToArray();

            var villainInput = Console.ReadLine().Split(": ").ToArray();
            var villainName = villainInput[1];

            string result = AddMinionToDatabase(sqlConnection, minionInfo, villainName);

            Console.WriteLine(result);
        }

        private static string AddMinionToDatabase(SqlConnection sqlConnection, string[] minionInfo, string villainName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sqlConnection.Open();

                string minionName = minionInfo[0];
                string minionAge = minionInfo[1];
                string minionTownName = minionInfo[2];

                string townId = CheckTownName(sqlConnection, minionTownName, sb);
                string villainId = CheckVillainName(sqlConnection, villainName, sb);

                string insertMinionQueryText = "INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";
                using SqlCommand addMinionCommand = new SqlCommand(insertMinionQueryText, sqlConnection);
                addMinionCommand.Parameters.AddRange(new[]
                {
                    new SqlParameter("@name", minionName),
                    new SqlParameter("@age", minionAge),
                    new SqlParameter("@townId", townId)
                });

                addMinionCommand.ExecuteNonQuery();

                string getMinionQueryText = "SELECT Id FROM Minions WHERE Name = @Name";
                using SqlCommand getMinionCommand = new SqlCommand(getMinionQueryText, sqlConnection);
                getMinionCommand.Parameters.AddWithValue("@Name", minionName);

                string minionId = getMinionCommand.ExecuteScalar().ToString();

                string servantQueryText = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";
                using SqlCommand addServantCommand = new SqlCommand(servantQueryText, sqlConnection);
                addServantCommand.Parameters.AddRange(new[]
                {
                    new SqlParameter("@villainId", villainId),
                    new SqlParameter("@minionId", minionId)
                });

                addServantCommand.ExecuteNonQuery();

                sb.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");

                return sb.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return "Error Generated. Details: " + ex.ToString();
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

        private static string CheckVillainName(SqlConnection sqlConnection, string villainName, StringBuilder sb)
        {
            string getVillainIdQueryText = "SELECT Id FROM Villains WHERE Name = @Name";

            using SqlCommand getVillainIdCommand = new SqlCommand(getVillainIdQueryText, sqlConnection);
            getVillainIdCommand.Parameters.AddWithValue("@Name", villainName);

            string villainId = getVillainIdCommand.ExecuteScalar()?.ToString();

            if (villainId == null)
            {
                string addNewVillainQueryText = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
                using SqlCommand addNewVillainCommand = new SqlCommand(addNewVillainQueryText, sqlConnection);
                addNewVillainCommand.Parameters.AddWithValue("@villainName", villainName);

                addNewVillainCommand.ExecuteNonQuery();

                villainId = addNewVillainCommand.ExecuteScalar().ToString();

                sb.AppendLine($"Villain {villainName} was added to the database.");
                return villainId;
            }
            else
            {
                return villainId;
            }
        }

        private static string CheckTownName(SqlConnection sqlConnection, string minionTownName, StringBuilder sb)
        {
            string getTownIdQueryText = "SELECT Id FROM Towns WHERE Name = @townName";
            
            using SqlCommand getTownIdCommand = new SqlCommand(getTownIdQueryText, sqlConnection);
            getTownIdCommand.Parameters.AddWithValue("@townName", minionTownName);

            string townId = getTownIdCommand.ExecuteScalar()?.ToString();

            if (townId == null)
            {
                string addNewTownQueryText = "INSERT INTO Towns (Name) VALUES (@townName)";
                using SqlCommand addNewTownCommand = new SqlCommand(addNewTownQueryText, sqlConnection);
                addNewTownCommand.Parameters.AddWithValue("@townName", minionTownName);

                addNewTownCommand.ExecuteNonQuery();

                townId = getTownIdCommand.ExecuteScalar().ToString();

                sb.AppendLine($"Town {minionTownName} was added to the database.");
                return townId;
            }
            else
            {
                return townId;
            }
        }
    }
}
