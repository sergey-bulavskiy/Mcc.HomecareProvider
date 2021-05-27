using System;
using System.Collections.Generic;

namespace Mcc.HomecareProvider.Domain
{
    public class Device
    {
        protected Device()
        {
        }

        public Device(string serialNumber, DateTimeOffset createdAt)
        {
            SerialNumber = serialNumber;
            CreatedAt = createdAt;
            DeviceBindings = new List<DeviceBinding>();
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string SerialNumber { get; }
        public DateTimeOffset CreatedAt { get; }
        public List<DeviceBinding> DeviceBindings { get; }
        public DeviceBinding CurrentBinding { get; private set; }

        public bool IsAssignedToPatient => CurrentBinding.HasPatient();
        public Guid? CurrentBindingId { get; private set; }

        public DeviceBinding AssignToPatient(Patient patient, DateTimeOffset currentTime)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));

            if (patient == CurrentBinding.Patient) return null;

            if (patient.HasDevice())
                throw new ArgumentException(
                    $"Patient with Id: {patient.Id} already assigned to a device");

            EnsurePropertyLoaded(nameof(CurrentBinding), CurrentBinding);
            if (!IsAssignedToPatient) CurrentBinding.InitializeWithPatient(patient, currentTime);

            return CurrentBinding;
        }

        public void InitializeCurrentDeviceBinding()
        {
            if (CurrentBinding != null)
                throw new InvalidOperationException($"Device {Id} was already initialized.");

            CurrentBinding = new DeviceBinding(this, null, CreatedAt);
            DeviceBindings.Add(CurrentBinding);
        }

        private static void EnsurePropertyLoaded(string propertyName, object value)
        {
            if (value is null)
                throw new ApplicationException(
                    $"{propertyName} is null. Did you forget to Include(x => x.{propertyName})?");
        }
    }
}