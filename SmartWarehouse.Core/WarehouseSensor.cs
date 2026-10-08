namespace SmartWarehouse.Core;

public class WarehouseSensor
{
    // Properties for the warehouse sensor object
    public string SensorId {get; private set;}
    public string LocationTag {get; private set;}
    private double CurrentTemperature;
    public bool IsActive {get; private set;}
    public bool IsAlertTriggered {get; private set;}
    private double CriticalThresholdCelcius;

    // Constructor for the warehouse sensor object
    public WarehouseSensor(string SensorId, string LocationTag, double CriticalThresholdCelcius = 4.0)
    {
        this.SensorId = SensorId;
        this.LocationTag = LocationTag;
        this.CriticalThresholdCelcius = CriticalThresholdCelcius;
        if (string.IsNullOrEmpty(SensorId) || string.IsNullOrEmpty(LocationTag) || string.IsNullOrWhiteSpace(SensorId) || string.IsNullOrWhiteSpace(LocationTag))
        {
            throw new ArgumentException("SensorId and LocationTag cannot be null or empty.");
        }
        this.CurrentTemperature = 0.0;
        this.IsActive = false;
        this.IsAlertTriggered = false;
    }
    // Activates the sensor
    public void Activate()
    {
        IsActive = true;
    }
    // Deactivates the sensor
    public void Deactivate()
    {
        IsActive = false;
        IsAlertTriggered = false;
    }
    // Records a new temperature reading for the sensor
    public void RecordReading(double newTemperature)
    {
        this.CurrentTemperature = newTemperature;
        if (!IsActive)
        {
            throw new InvalidOperationException("Cannot record reading: Sensor is not active.");
        }
        if (newTemperature < -50 || newTemperature > 80)
        {
            throw new ArgumentOutOfRangeException("Temperature reading must be between -50 and 80 degrees Celsius.");
        }
        if (newTemperature >= CriticalThresholdCelcius)
        {
            this.IsAlertTriggered = true;
        }
        else
        {
            this.IsAlertTriggered = false;
        }
    }

    // Updates the critical temperature threshold for the sensor
    public void UpdateThreshold(double newThreshold)
    {
        this.CriticalThresholdCelcius = newThreshold;
        if (newThreshold < -30 || newThreshold > 50)
        {
            throw new ArgumentOutOfRangeException("Threshold must be between -30 and 50 degrees Celsius.");
        }
        if (this.CurrentTemperature >= this.CriticalThresholdCelcius)
        {
            this.IsAlertTriggered = true;
        }
        else
        {
            this.IsAlertTriggered = false;
        }
    }

}
