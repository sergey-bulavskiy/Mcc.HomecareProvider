using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// Performs the DB migration and seeding with the initial data.
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
                for (int i = 0; i < 150; i++)
                {
                    DateTimeOffset randomTime =
                        DateTimeOffset.Now - TimeSpan.FromDays(rnd.Next(minValue: 1, maxValue: 100));
                    int serialNumberAsInt = rnd.Next(minValue: 100000, maxValue: 999999);
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
                        DateTimeOffset.Now - TimeSpan.FromDays(rnd.Next(minValue: 1, maxValue: 100));
                    var serialNumberAsInt = rnd.Next(minValue: 120000, maxValue: 129999);
                    var device = new Device(serialNumberAsInt.ToString(), randomTime);
                    _context.Add(device);
                }

                _context.SaveChanges();
            }

            if (!_context.Patients.Any())
            {
                for (int i = 0; i < 150; i++)
                {
                    DateTimeOffset randomTime =
                        DateTimeOffset.Now - TimeSpan.FromDays(rnd.Next(minValue: 1, maxValue: 10000)) -
                        TimeSpan.FromSeconds(rnd.Next());

                    DateTime randomDate = DateTime.Now - TimeSpan.FromDays(rnd.Next(minValue: 3500, maxValue: 30800));

                    string email = GetSaltString() + "@gmail.com";

                    List<string> names = new List<string>()
                        {"Alister", "John", "Alison", "Mathew", "Jack", "Alex", "Arthur"};

                    var lastNames = new List<string>()
                        {"Wright", "Waters", "Gilmour", "Mason", "Barrett", "Close", "Bieber"};

                    string firstName = names[rnd.Next(minValue: 0, maxValue: 6)];
                    string lastName = lastNames[rnd.Next(minValue: 0, maxValue: 6)];

                    var patient = new Patient(email, firstName, lastName, randomDate, randomTime);
                    _context.Add(patient);
                }

                _context.SaveChanges();
            }

            if (!_context.DeviceBindings.Any(b => b.PatientId != null))
            {
                for (int i = 0; i < 75; i++)
                {
                    DateTimeOffset randomTime =
                        DateTimeOffset.Now - TimeSpan.FromDays(rnd.Next(minValue: 1, maxValue: 100)) -
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

            if (!SqlTaskIsPrepared())
            {
                PrepareDataForSqlTask();
            }
        }

        private void PrepareDataForSqlTask()
        {
            var patients = _context.Patients
                .Include(p => p.CurrentBinding)
                .Where(x => x.CurrentBindingId == null)
                .ToList();

            var devices = _context.Devices
                .Include(d => d.CurrentBinding)
                .ThenInclude(b => b.Patient)
                .Where(d => d.CurrentBinding.PatientId == null && !d.SerialNumber.StartsWith("12"))
                .ToList();

            for (var i = 1; i < 6; i++)
            {
                Patient patient = patients[i];
                patient.Email = $"ShouldEndUpInOutput{i}@gmail.com";

                for (var j = 0; j < i; j++)
                {
                    var device = devices[i];
                    device.AssignToPatient(patient, DateTimeOffset.Now);
                }
            }

            _context.SaveChanges();
        }

        private bool SqlTaskIsPrepared()
        {
            var requiredPatients = _context.Patients
                .Where(x => x.Email.StartsWith("ShouldEndUpInOutput")).ToList();
            return requiredPatients.Count != 0;
        }

        /// <summary>
        ///  Stole it from: https://stackoverflow.com/a/45841798
        /// </summary>
        private String GetSaltString()
        {
            const string saltchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder salt = new StringBuilder();
            Random rnd = new Random();
            while (salt.Length < 10)
            {
                // length of the random string.
                int index = (int) (rnd.NextDouble() * saltchars.Length);
                salt.Append(saltchars[index]);
            }

            String saltStr = salt.ToString();
            return saltStr;
        }
    }
}