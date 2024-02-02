using System.Data.Common;
using System.Data.SqlClient;

public class WorkOrderRepository 
{
    private string DBConnectionString = string.Empty;

    private static string SQLQuery_Open = @"SELECT 
                                WorkOrders.RecordID as Id,
                                WorkOrders.WONumber as Number,
                                Lists_Status.Item as Status,
                                WorkOrders.WorkRequested,
                                Vehicles.vehicle as VehicleNumber,
                                WorkOrders.CreatedTime as Created,
                                WorkOrders.UpdatedTime as LastUpdated,
                                Lists_Priority.Item as Priority
                            FROM
                                WorkOrders
                                LEFT OUTER JOIN Lists AS Lists_Status ON WorkOrders.Status = Lists_Status.ItemId
                                LEFT OUTER JOIN Lists AS Lists_Priority ON WorkOrders.Priority = Lists_Priority.ItemId
                                LEFT OUTER JOIN Vehicles ON WorkOrders.VehKey=Vehicles.RecordID
                            WHERE
                                Lists_Status.Item != 'Completed'
                                AND WorkOrders.CreatedTime IS NOT NULL
                                AND WorkRequested IS NOT NULL
                            ".Replace(Environment.NewLine, "").Trim();

    public WorkOrderRepository(string DBConnectionString)
    {
        this.DBConnectionString = DBConnectionString;
    }

    private WorkOrder dataReaderToWorkOrder(SqlDataReader reader) 
    {
        return new WorkOrder()
        {
            Id = Parsers.ParseInt(Parsers.ParseSQLReaderValue(reader, "Id")),
            Number = Parsers.ParseSQLReaderValue(reader, "Number"),
            Status = Parsers.ParseSQLReaderValue(reader, "Status"),
            WorkRequested = Parsers.ParseSQLReaderValue(reader, "WorkRequested"),
            VehicleNumber = Parsers.ParseSQLReaderValue(reader, "VehicleNumber"),
            Created = Parsers.ParseDate(Parsers.ParseSQLReaderValue(reader, "Created")),
            LastUpdated = Parsers.ParseDate(Parsers.ParseSQLReaderValue(reader, "LastUpdated")),
            Priority = Parsers.ParseSQLReaderValue(reader, "Priority")
        };
    }

    public IEnumerable<WorkOrder> GetOpenWorkOrders()
    {
        List<WorkOrder> returnMe = new List<WorkOrder>();

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
                    WorkOrder workOrder = dataReaderToWorkOrder(dbDataReader);
                    if (workOrder != null)
                    {
                        returnMe.Add(workOrder);
                    }
                }
            }

            sqlCommand.Connection.Close();
        }

        return returnMe;

    }

}