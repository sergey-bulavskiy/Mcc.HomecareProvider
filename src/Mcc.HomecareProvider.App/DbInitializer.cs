using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mcc.HomecareProvider.Domain;
using Mcc.HomecareProvider.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mcc.HomecareProvider.App
{
    public class DbInitializer
    {
        private readonly PostgresDbContext _context;

        public DbInitializer(PostgresDbContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Performs the DB migration and seeding with the initial data.
        /// </summary>
        public void Initialize()
        {
            // Run migration first.
            Migrate();
            Seed();
        }

        private void Migrate()
        {
            _context.Database.Migrate();
        }

        public void Seed()
        {
            var rnd = new Random(DateTimeOffset.Now.Millisecond);
            if (!_context.Devices.Any())
            {
                for (var i = 0; i < 150; i++)
                {
                    var randomTime =
                        DateTimeOffset.Now - TimeSpan.FromDays(rnd.Next(1, 100));
                    var serialNumberAsInt = rnd.Next(100000, 555555);
                    var device = new Device(serialNumberAsInt.ToString(), randomTime);
                    _context.Add(device);
                    _context.SaveChanges();

                    if (!device.SerialNumber.StartsWith("12"))
                        device.InitializeCurrentDeviceBinding();
                }

                // Artificial anomaly that some number of our devices are "bugged". 
                for (var i = 0; i < 5; i++)
                {
                    var randomTime =
                        DateTimeOffset.Now - TimeSpan.FromDays(rnd.Next(1, 100));
                    var serialNumberAsInt = rnd.Next(120000, 129999);
                    var device = new Device(serialNumberAsInt.ToString(), randomTime);
                    _context.Add(device);
                }

                _context.SaveChanges();
            }

            if (!_context.Patients.Any())
            {
                for (var i = 0; i < 150; i++)
                {
                    var randomTime =
                        DateTimeOffset.Now - TimeSpan.FromDays(rnd.Next(1, 10000)) -
                        TimeSpan.FromSeconds(rnd.Next());

                    var randomDate = DateTime.Now - TimeSpan.FromDays(rnd.Next(3500, 30800));

                    var email = GetSaltString() + "@gmail.com";

                    var names = new List<string> {"Alister", "John", "Alison", "Mathew", "Jack", "Alex", "Arthur"};

                    var lastNames = new List<string>
                        {"Wright", "Waters", "Gilmour", "Mason", "Barrett", "Close", "Bieber"};

                    var firstName = names[rnd.Next(0, 6)];
                    var lastName = lastNames[rnd.Next(0, 6)];

                    var patient = new Patient(email, firstName, lastName, randomDate, randomTime);
                    _context.Add(patient);
                }

                _context.SaveChanges();
            }

            if (!_context.DeviceBindings.Any(b => b.PatientId != null))
            {
                for (var i = 0; i < 75; i++)
                {
                    var randomTime =
                        DateTimeOffset.Now - TimeSpan.FromDays(rnd.Next(1, 100)) -
                        TimeSpan.FromSeconds(rnd.Next());

                    var device = _context.Devices
                        .Include(d => d.CurrentBinding)
                        .ThenInclude(d => d.Patient)
                        .ToList()[i];

                    // Since we have anomaly in devices, that some of them not initialized properly, 
                    // we need to make sure that we won't accidentally "fix" them by assigning.
                    if (device.CurrentBinding == null || device.CurrentBinding.HasPatient())
                        continue;

                    var patient = _context.Patients.Include(x => x.CurrentBinding).ToList()[i];
                    device.AssignToPatient(patient, randomTime);
                }

                _context.SaveChanges();
            }

            if (!FirstSqlTaskIsPrepared()) PrepareDataForFirstSqlTask();

            if (!SecondSqlTaskIsPrepared()) PrepareDataForSecondSqlTask();
        }


        private bool SecondSqlTaskIsPrepared()
        {
            return _context.Patients.Any(p => p.Email.StartsWith("jimi"));
        }

        /// <summary>
        ///     Find exactly which serial number given patient had on the specific date time.
        /// </summary>
        private void PrepareDataForSecondSqlTask()
        {
            var createdAt = DateTimeOffset.Now;

            var patient = new Patient(
                firstName: "Jimi",
                lastName: "Hendrix",
                email: "jimi.hendrix@gmail.com",
                createdAt: createdAt,
                dateOfBirth: new DateTime(1942, 11, 27));

            _context.Patients.Add(patient);
            _context.SaveChanges();

            var serialNumbers = new List<string> {"999443", "600423", "555662", "777332"};
            foreach (var sn in serialNumbers)
            {
                var device = new Device(sn, createdAt);
                _context.Devices.Add(device);
                _context.SaveChanges();

                device.InitializeCurrentDeviceBinding();
                _context.SaveChanges();
            }

            var datesOfAssignment = new List<DateTime>
            {
                new(2000, 12, 31),
                new(2005, 5, 6),
                new(2005, 5, 8),
                new(2010, 1, 1)
            };

            var dbDevices = _context.Devices
                .Include(d => d.CurrentBinding)
                .Include(d => d.DeviceBindings)
                .Where(d => serialNumbers.Contains(d.SerialNumber))
                .ToList();

            for (var index = 0; index < datesOfAssignment.Count; index++)
            {
                var device = dbDevices.Single(d => d.SerialNumber == serialNumbers[index]);
                device.AssignToPatient(patient, datesOfAssignment[index]);
            }

            _context.SaveChanges();
        }

        /// <summary>
        ///     First SQL Task for tester is something like:
        ///     Sort patients by number of devices they had for a whole period,
        ///     find First/Last names of the patients with more than 1 device, and find patient
        ///     with biggest number of devices historically.
        /// </summary>
        private void PrepareDataForFirstSqlTask()
        {
            var patients = _context.Patients
                .Include(p => p.CurrentBinding)
                .Include(p => p.DeviceBindings)
                .Where(x => x.CurrentBindingId == null)
                .ToList();

            var devices = _context.Devices
                .Include(d => d.CurrentBinding)
                .ThenInclude(b => b.Patient)
                .Where(d => d.CurrentBinding.PatientId == null && !d.SerialNumber.StartsWith("12"))
                .ToList();

            for (var i = 1; i < 6; i++)
            {
                var patient = patients[i];
                patient.Email = $"ShouldEndUpInOutput{i}@gmail.com";

                for (var k = 0; k <= i; k++)
                {
                    var device = devices[k];
                    device.AssignToPatient(patient, DateTimeOffset.Now);
                }
            }

            _context.SaveChanges();
        }

        private bool FirstSqlTaskIsPrepared()
        {
            var requiredPatients = _context.Patients
                .Where(x => x.Email.StartsWith("ShouldEndUpInOutput")).ToList();
            return requiredPatients.Count != 0;
        }

        /// <summary>
        ///     Stole it from: https://stackoverflow.com/a/45841798
        /// </summary>
        private string GetSaltString()
        {
            const string saltchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var salt = new StringBuilder();
            var rnd = new Random();
            while (salt.Length < 10)
            {
                // length of the random string.
                var index = (int) (rnd.NextDouble() * saltchars.Length);
                salt.Append(saltchars[index]);
            }

            var saltStr = salt.ToString();
            return saltStr;
        }
    }
}