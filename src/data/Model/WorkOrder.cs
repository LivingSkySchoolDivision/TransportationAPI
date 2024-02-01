public class WorkOrder
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string Status { get; set; }
    public string WorkRequested { get; set; }
    public string VehicleNumber { get; set; }    
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Priority { get; set ;}
}