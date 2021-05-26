using System;
using System.Collections.Generic;

namespace Mcc.HomecareProvider.Domain
{
    public class Patient
    {
        protected Patient()
        {
        }

        public Patient(
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            DateTimeOffset createdAt)
        {
            DeviceBindings = new List<DeviceBinding>();
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            CreatedAt = createdAt;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public List<DeviceBinding> DeviceBindings { get; set; }

        public DeviceBinding CurrentBinding { get; set; }
        public Guid CurrentBindingId { get; set; }

        public bool HasDevice()
        {
            EnsurePropertyLoaded(nameof(CurrentBinding), CurrentBinding);
            return CurrentBinding.Device != null;
        }

        private static void EnsurePropertyLoaded(string propertyName, object value)
        {
            if (value is null)
                throw new ApplicationException(
                    $"{propertyName} is null. Did you forget to Include(x => x.{propertyName})?");
        }
    }
}