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

                    // Introduce some random anomalies with unitialized binding;
                    if (i == 100 || i == 110 || i == 75)
                    {
                    }
                    else
                    {
                        device.InitializeCurrentDeviceBinding();
                    }
                }

                _context.SaveChanges();
            }

            if (!_context.Patients.Any())
            {
                for (int i = 0; i < 150; i++)
                {
                    DateTimeOffset randomTime =
                        DateTimeOffset.Now - TimeSpan.FromDays(rnd.Next(minValue: 1, maxValue: 100));

                    DateTime randomDate = DateTime.Now - TimeSpan.FromDays(rnd.Next(minValue: 3500, maxValue: 3800));

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
                for (int i = 0; i < 25; i++)
                {
                    DateTimeOffset randomTime =
                        DateTimeOffset.Now - TimeSpan.FromDays(rnd.Next(minValue: 1, maxValue: 100));
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
        }

        protected String GetSaltString()
        {
            String SALTCHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder salt = new StringBuilder();
            Random rnd = new Random();
            while (salt.Length < 10)
            {
                // length of the random string.
                int index = (int) (rnd.NextDouble() * SALTCHARS.Length);
                salt.Append(SALTCHARS[index]);
            }

            String saltStr = salt.ToString();
            return saltStr;
        }
    }
}