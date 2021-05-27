using System;

namespace Mcc.HomecareProvider.Domain
{
    /// <summary>
    ///     Represents the fact that at a specific period of time, a <see cref="Device" />
    ///     belonged to an organisation and was potentially assigned to a <see cref="Patient" />
    /// </summary>
    public class DeviceBinding
    {
        protected DeviceBinding()
        {
        }

        internal DeviceBinding(Device device, Patient patient, DateTimeOffset createdAt)
        {
            Id = Guid.NewGuid();
            Device = device;
            Patient = patient;
            CreatedAt = createdAt;
        }

        public Guid Id { set; get; }

        public Guid? PatientId { get; private set; }
        public Guid DeviceId { get; private set; }
        public Device Device { get; }
        public Patient Patient { get; private set; }
        public DateTimeOffset CreatedAt { get; }

        public DateTimeOffset? AssignedToPatientAt { get; set; }

        public bool HasPatient()
        {
            return PatientId.HasValue;
        }

        internal void InitializeWithPatient(Patient patient, DateTimeOffset initializedAt)
        {
            if (PatientId.HasValue)
                throw new ArgumentException(
                    "Could not initialize DeviceBinding with Patient. Already initialized.");

            PatientId = patient.Id;
            Patient = patient;
            AssignedToPatientAt = initializedAt;
        }
    }
}