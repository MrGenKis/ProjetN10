using PatientService.Models;

namespace PatientService.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        var testPatients = new List<Patient>
        {
            new Patient
            {
                FirstName = "Test",
                LastName = "TestNone",
                DateOfBirth = new DateTime(1966, 12, 31),
                Gender = "F",
                Address = "1 Brookside St",
                PhoneNumber = "100-222-3333"
            },

            new Patient
            {
                FirstName = "Test",
                LastName = "TestBorderline",
                DateOfBirth = new DateTime(1945, 6, 24),
                Gender = "M",
                Address = "2 High St",
                PhoneNumber = "200-333-4444"
            },

            new Patient
            {
                FirstName = "Test",
                LastName = "TestInDanger",
                DateOfBirth = new DateTime(2004, 6, 18),
                Gender = "M",
                Address = "3 Club Road",
                PhoneNumber = "300-444-5555"
            },

            new Patient
            {
                FirstName = "Test",
                LastName = "TestEarlyOnset",
                DateOfBirth = new DateTime(2002, 6, 28),
                Gender = "F",
                Address = "4 Valley Dr",
                PhoneNumber = "400-555-6666"
            }
        };

        foreach (var patient in testPatients)
        {
            var exists = context.Patients.Any(p =>
                p.FirstName == patient.FirstName &&
                p.LastName == patient.LastName);

            if (!exists)
            {
                context.Patients.Add(patient);
            }
        }

        context.SaveChanges();
    }
}