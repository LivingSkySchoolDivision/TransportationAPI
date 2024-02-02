using System.Data.Common;
using System.Data.SqlClient;

public class BusInspectionRepository
{
    private string DBConnectionString = string.Empty;

    private static string SQLQuery_Open = @"SELECT
                                                Employees.LastName,
                                                Employees.FirstName,
                                                EmployeeTraining.DateCompleted,
                                                EmployeeTraining.ExpirationDate,
                                                CertificationType.Description,
                                                Vehicles.vehicle
                                            FROM
                                                EmployeeTraining
                                                LEFT OUTER JOIN CertificationType ON EmployeeTraining.CertificationTypeID=CertificationType.RecordID
                                                LEFT OUTER JOIN Employees ON EmployeeTraining.EmployeeID=Employees.RecordID
                                                LEFT OUTER JOIN VehicleDriver ON Employees.RecordID=VehicleDriver.EmployeeID
                                                LEFT OUTER JOIN Vehicles ON VehicleDriver.VehicleID=Vehicles.RecordID
                                            WHERE
                                                CertificationType.Description = 'bus inspection'
                                                AND Vehicles.vehicle IS NOT NULL
                                            ORDER BY EmployeeTraining.ExpirationDate ASC
                            ".Replace(Environment.NewLine, "").Trim();

    public BusInspectionRepository(string DBConnectionString)
    {
        this.DBConnectionString = DBConnectionString;
    }

    


    private BusInspection dataReaderToBusInspection(SqlDataReader reader)
    {
        return new BusInspection()
        {
            LastCompleted = Parsers.ParseDate(Parsers.ParseSQLReaderValue(reader, "DateCompleted")),
            Expiration = Parsers.ParseDate(Parsers.ParseSQLReaderValue(reader, "ExpirationDate")),
            VehicleNumber = Parsers.ParseSQLReaderValue(reader, "vehicle"),
            DriverFirstName = Parsers.ParseSQLReaderValue(reader, "FirstName"),
            DriverLastName = Parsers.ParseSQLReaderValue(reader, "LastName"),
        };
    }

    public IEnumerable<BusInspection> GetAllBusInspections()
    {
        List<BusInspection> returnMe = new List<BusInspection>();

        using (SqlConnection connection = new SqlConnection(this.DBConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand
            {
                Connection = connection,
                CommandType = System.Data.CommandType.Text,
                CommandText = SQLQuery_Open
            };
            sqlCommand.Connection.Open();
            SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

            if (dbDataReader.HasRows)
            {
                while (dbDataReader.Read())
                {
                    BusInspection inspection = dataReaderToBusInspection(dbDataReader);
                    if (inspection != null)
                    {
                        if (!returnMe.Contains(inspection))
                        {
                            returnMe.Add(inspection);
                        }
                    }
                }
            }

            sqlCommand.Connection.Close();
        }

        return returnMe;

    }

}