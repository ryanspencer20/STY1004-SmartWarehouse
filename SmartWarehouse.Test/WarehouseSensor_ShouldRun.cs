using SmartWarehouse.Core;
namespace SmartWarehouse.Test;

[TestClass]
public sealed class WarehouseSensor_ShouldRun
{    
    [DataTestMethod]
    [DataRow("", "ConveyorBelt001")]
    [DataRow("Sensor001", "")]
    [DataRow("", "")]
    [DataRow("   ", "ConveyorBelt001")]
    [DataRow("Sensor001", "   ")]
    [DataRow("   ", "   ")]
    [DataRow(null, "   ")]
    [DataRow("   ", null)]
    [DataRow(null, "")]
    [DataRow("", null)]
    [DataRow(null, "ConveyorBelt001")]
    [DataRow("Sensor001", null)]
    [DataRow(null, null)]
    public void SensorInitialState_MissingInitialParameters_ThrowsException(string sensorID, string locationTag)
    {
        // Arrange & Act & Assert
        Assert.ThrowsException<ArgumentException>(() => new WarehouseSensor(sensorID, locationTag));
    }
    [TestMethod]
    public void SensorInitialState_IsInactive_ReturnsFalse()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act & Assert
        Assert.IsFalse(sensor001.IsActive);
    }
    [TestMethod]
    public void SensorInitialState_IsAlertTriggered_ReturnsFalse()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act & Assert
        Assert.IsFalse(sensor001.IsAlertTriggered);
    }
    [TestMethod]
    public void SensorActive_CallActivate_ReturnsTrue()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act
        sensor001.Activate();
        // Assert
        Assert.IsTrue(sensor001.IsActive);
    }
    [TestMethod]
    public void SensorInactive_CallDeactivate_ReturnsFalse()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act
        sensor001.Activate();
        sensor001.Deactivate();
        // Assert
        Assert.IsFalse(sensor001.IsActive);
    }
    [TestMethod]
    public void SensorDeactivatedRecordingReading_CallRecordReading_ThrowsException()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act & Assert 
        Assert.ThrowsException<InvalidOperationException>(() => sensor001.RecordReading(10.0)); 
    }
    [TestMethod]
    public void SensorGetsDeactivatedRecordingReading_CallRecordReading_ThrowsException()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        sensor001.Activate();
        sensor001.Deactivate();
        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => sensor001.RecordReading(10.0)); 
    }
    [DataTestMethod]
    [DataRow(4.0)]
    [DataRow(10.0)]
    [DataRow(15.5)]
    [DataRow(20.0)]
    [DataRow(25.3)]
    [DataRow(30.0)]
    [DataRow(35.7)]
    [DataRow(40.0)]
    public void RecordReading_InputTempX_ReturnsTrue(double temp)
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act
        sensor001.Activate();
        sensor001.RecordReading(temp);
        // Assert
        Assert.IsTrue(sensor001.IsAlertTriggered);
    }
    [DataTestMethod]
    [DataRow(-46.6)]
    [DataRow(-40.0)]
    [DataRow(-35.1)]
    [DataRow(-30.0)]
    [DataRow(-24.2)]
    [DataRow(-10.0)]
    [DataRow(0.0)]
    [DataRow(3.9)]
    public void RecordReading_InputTempX_ReturnsFalse(double temp)
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        sensor001.Activate();
        // Act
        sensor001.RecordReading(temp);
        // Assert
        Assert.IsFalse(sensor001.IsAlertTriggered);
    }
    [TestMethod]
    public void NoActivation_CallRecordReading_ThrowsInvalidOperationException()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => sensor001.RecordReading(10.0));
    }
    [TestMethod]
    public void RecordReading_InputUnderThreshold_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        sensor001.Activate();
        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => sensor001.RecordReading(-50.1));
    }
    [TestMethod]
    public void RecordReading_ThresholdInputExceeded_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        sensor001.Activate();
        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => sensor001.RecordReading(80.1));
    }
    [TestMethod]
    public void RecordReading_InputMinimumThreshold_ReturnsFalse()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act
        sensor001.Activate();
        sensor001.RecordReading(-50);
        // Assert
        Assert.IsFalse(sensor001.IsAlertTriggered);
    }
    [TestMethod]
    public void RecordReading_InputMaximumThreshold_ReturnsTrue()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act
        sensor001.Activate();
        sensor001.RecordReading(80);   
        // Assert
        Assert.IsTrue(sensor001.IsAlertTriggered);
    }
    [TestMethod]
    public void UpdateThreshold_ThresholdExceeded_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        sensor001.Activate();
        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => sensor001.UpdateThreshold(50.1));
    }
    [TestMethod]
    public void UpdateThreshold_ThresholdNotMet_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        sensor001.Activate();
        // Act & Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => sensor001.UpdateThreshold(-30.1));
    }
    [TestMethod]
    public void UpdateThreshold_ThresholdAtMaximum_ReturnsFalse()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act
        sensor001.Activate();
        sensor001.UpdateThreshold(50);
        // Assert
        Assert.IsFalse(sensor001.IsAlertTriggered);
    }
    [TestMethod]
    public void UpdateThreshold_ThresholdAtMinimum_ReturnsTrue()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act
        sensor001.Activate();
        sensor001.UpdateThreshold(-30);
        // Assert
        Assert.IsTrue(sensor001.IsAlertTriggered);
    }
    [DataTestMethod]
    [DataRow(4.0)]
    [DataRow(10.0)]
    [DataRow(20.0)]
    [DataRow(30.0)]
    public void UpdateThreshold_InputThresholdX_ReturnsFalse(double threshold)
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act
        sensor001.Activate();
        sensor001.RecordReading(0.0);
        sensor001.UpdateThreshold(threshold);
        // Assert
        Assert.IsFalse(sensor001.IsAlertTriggered);
    }
    [DataTestMethod]
    [DataRow(4.0)]
    [DataRow(10.0)]
    [DataRow(20.0)]
    [DataRow(25.2)]
    public void UpdateThreshold_InputThresholdX_ReturnsTrue(double threshold)
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act
        sensor001.Activate();
        sensor001.RecordReading(25.3);
        sensor001.UpdateThreshold(threshold);
        // Assert
        Assert.IsTrue(sensor001.IsAlertTriggered);
    }
    [TestMethod]
    public void UpdateThreshold_InputThresholdAboveCurrentReading_ReturnsTrue()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act
        sensor001.Activate();
        sensor001.UpdateThreshold(9.3);
        sensor001.RecordReading(10);
        sensor001.UpdateThreshold(25.4);
        sensor001.RecordReading(26.8);
        // Assert
        Assert.IsTrue(sensor001.IsAlertTriggered);
    }
    [TestMethod]
    public void UpdateThresholds_InputThresholdMultipleCurrentReading_ReturnsFalse()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act
        sensor001.Activate();
        sensor001.UpdateThreshold(9.3);
        sensor001.RecordReading(10);
        sensor001.UpdateThreshold(25.4);
        sensor001.RecordReading(26.8);
        sensor001.RecordReading(20.5);
        // Assert
        Assert.IsFalse(sensor001.IsAlertTriggered);
    }
    [TestMethod]
    public void UpdateThresholdsThenDeactivate_InputThresholdMultipleCurrentReading_ReturnsFalse()
    {
        // Arrange
        WarehouseSensor sensor001 = new WarehouseSensor("Sensor001", "ConveyorBelt001");
        // Act
        sensor001.Activate();
        sensor001.UpdateThreshold(9.3);
        sensor001.RecordReading(10);
        sensor001.UpdateThreshold(25.4);
        sensor001.RecordReading(26.8);
        sensor001.Deactivate();
        // Assert
        Assert.IsFalse(sensor001.IsAlertTriggered);
    }
}
