public class BusInspection : IEquatable<BusInspection>
{
    public DateTime LastCompleted { get; set; }
    public DateTime Expiration { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
    public string DriverFirstName { get; set; } = string.Empty;
    public string DriverLastName { get; set; } = string.Empty;
    
    public override string ToString()
    {
        return $"{{ LastCompleted: {this.LastCompleted}, Expiration: {this.Expiration}, VehicleNumber: {this.VehicleNumber} }}";
    }

    public bool Equals(BusInspection? other)
    {
        if (other == null) {
            return false;
        } else {
            bool equality_test = true;

            if (!this.VehicleNumber.Equals(other.VehicleNumber)) 
            {
                equality_test = false;
            }

            if (!this.LastCompleted.Equals(other.LastCompleted)) 
            {
                equality_test = false;
            }

            if (!this.Expiration.Equals(other.Expiration)) 
            {
                equality_test = false;
            }

            return equality_test;
        }
    }
}