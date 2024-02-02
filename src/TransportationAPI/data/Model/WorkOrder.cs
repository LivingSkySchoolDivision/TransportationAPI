public class WorkOrder
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string WorkRequested { get; set; } = string.Empty;
    public string VehicleNumber { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Priority { get; set ;} = string.Empty;
}