using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Data
{
    /// <summary>
    /// Static class for holding all test data within the app
    /// </summary>
    public static class TestData
    {
        /// <summary>
        /// This is just a test description, we just put it on all the assignments for now
        /// </summary>
        public const string Description = "This is the desription created for this specific assignment. It would include helpful information related to the assignment, and you would probably want to read it.  Important information would be here, like which tools to use and what needs to be done for the assignment.";

        #region Assignments

        public static readonly Assignment Assignment1 = new Assignment
        {
            Id = 1,
            Priority = 1,
            JobNumber = "2001",
            CompanyName = "GC Mechanical",
            Description = Description,
            ContactName = "Joseph Grimes",
            ContactPhone = "414-367-4348",
            Address = "2030 Judah St",
            City = "San Francisco",
            State = "CA",
            Zip = "94122",
            Latitude = 37.761402f,
            Longitude = -122.484182f,
            Status = AssignmentStatus.Active,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddHours(2),
        };

        public static readonly Assignment Assignment2 = new Assignment
        {
            Id = 2,
            Priority = 2,
            JobNumber = "2002",
            CompanyName = "Calcom Logistics",
            Description = Description,
            ContactName = "Monica Green",
            ContactPhone = "925-353-8029",
            Address = "231 3rd Ave",
            City = "San Francisco",
            State = "CA",
            Zip = "94118",
            Latitude = 37.783839f,
            Longitude = -122.461592f,
            Status = AssignmentStatus.Hold,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(1).AddHours(2),
        };

        public static readonly Assignment Assignment3 = new Assignment
        {
            Id = 3,
            Priority = 3,
            JobNumber = "3113",
            CompanyName = "St. Francis Hospitals",
            Description = Description,
            ContactName = "Christopher Joyner",
            ContactPhone = "979-943-0937",
            Address = "400 Grant Ave",
            City = "San Fancisco",
            State = "CA",
            Zip = "94108",
            Latitude = 37.790786f,
            Longitude = -122.405412f,
            Status = AssignmentStatus.New,
            StartDate = new DateTime(2012, 10, 4),
            EndDate = new DateTime(2012, 12, 6),
        };

        public static readonly Assignment Assignment4 = new Assignment
        {
            Id = 4,
            Priority = 4,
            JobNumber = "3114",
            CompanyName = "Bay Unified School District",
            Description = Description,
            ContactName = "Joan Marcum",
            ContactPhone = "914-870-7670",
            Address = "448 Grand Ave",
            City = "South San Francisco",
            State = "CA",
            Zip = "94080",
            Latitude = 37.656103f,
            Longitude = -122.414271f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(1).AddHours(2),
        };

        public static readonly Assignment Assignment5 = new Assignment
        {
            Id = 5,
            Priority = 5,
            JobNumber = "4445",
            CompanyName = "Redwood City Medical Group",
            Description = Description,
            ContactName = "Margaret Cargill",
            ContactPhone = "208-816-9793",
            Address = "1037 Middlefield Rd",
            City = "Redwood City",
            State = "CA",
            Zip = "94063",
            Latitude = 37.484513f,
            Longitude = -122.227647f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(1).AddHours(2),
        };

        public static readonly Assignment Assignment6 = new Assignment
        {
            Id = 6,
            Priority = 6,
            JobNumber = "4446",
            CompanyName = "JG Manufacturing",
            Description = Description,
            ContactName = "Benjamin Jones",
            ContactPhone = "505-562-3086",
            Address = "2901 Cowper St",
            City = "Palo Alto",
            State = "CA",
            Zip = "94306",
            Latitude = 37.429546f,
            Longitude = -122.129313f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(1).AddHours(2),
        };

        public static readonly Assignment Assignment7 = new Assignment
        {
            Id = 7,
            Priority = 7,
            JobNumber = "5677",
            CompanyName = "Pacific Cabinetry",
            Description = Description,
            ContactName = "Alvin Gray",
            ContactPhone = "720-344-7823",
            Address = "1773 Lincoln St",
            City = "Santa Clara",
            Zip = "95050",
            State = "CA",
            Latitude = 37.355546f,
            Longitude = -121.955441f,
            Status = AssignmentStatus.New,
            StartDate = new DateTime(2013, 7, 5),
            EndDate = new DateTime(2013, 8, 12),
        };

        public static readonly Assignment Assignment8 = new Assignment
        {
            Id = 8,
            Priority = 8,
            JobNumber = "5678",
            CompanyName = "Evergreen Mechanical",
            Description = Description,
            ContactName = "Michelle Wilson",
            ContactPhone = "917-245-7975",
            Address = "208 Jackson St",
            City = "San Jose",
            Zip = "95112",
            State = "CA",
            Latitude = 37.349005f,
            Longitude = -121.894038f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(1).AddHours(2),
        };

        public static readonly Assignment Assignment9 = new Assignment
        {
            Id = 9,
            Priority = 9,
            JobNumber = "7809",
            CompanyName = "Peninsula University",
            Description = Description,
            ContactName = "Jennifer Gillespie",
            ContactPhone = "831-427-6746",
            Address = "10002 N De Anza Blvd",
            City = "Cupertino",
            Zip = "95014",
            State = "CA",
            Latitude = 37.323387f,
            Longitude = -122.031769f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(1).AddHours(2),
        };

        public static readonly Assignment Assignment10 = new Assignment
        {
            Id = 10,
            Priority = 10,
            JobNumber = "7810",
            CompanyName = "Creative Automotive Group",
            Description = Description,
            ContactName = "Thomas White",
            ContactPhone = "214-865-0771",
            Address = "1181 Linda Mar Blvd",
            City = "Pacifica",
            Zip = "94044",
            State = "CA",
            Latitude = 38.585774f,
            Longitude = -122.488545f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(1).AddHours(2),
        };

        public static readonly Assignment Assignment11 = new Assignment
        {
            Id = 11,
            Priority = 11,
            JobNumber = "7531",
            CompanyName = "XYZ Robotics",
            Description = Description,
            ContactName = "Ivan Diaz",
            ContactPhone = "406-496-8774",
            Address = "1960 Mandela Pkwy",
            City = "Oakland",
            Zip = "94607",
            State = "CA",
            Latitude = 37.814795f,
            Longitude = -122.289854f,
            Status = AssignmentStatus.New,
            StartDate = new DateTime(2013, 8, 27),
            EndDate = new DateTime(2013, 8, 30),
        };

        public static readonly Assignment Assignment12 = new Assignment
        {
            Id = 12,
            Priority = 12,
            JobNumber = "1680",
            CompanyName = "MMSRI, Inc",
            Description = Description,
            ContactName = "Eric Grant",
            ContactPhone = "360-693-2388",
            Address = "2043 Martin Luther King Jr Way",
            City = "Berkeley",
            Zip = "94704",
            State = "CA",
            Latitude = 37.871118f,
            Longitude = -122.272942f,
            Status = AssignmentStatus.New,
            StartDate = new DateTime(2013, 9, 18),
            EndDate = new DateTime(2013, 9, 30),
        };

        public static readonly Assignment Assignment13 = new Assignment
        {
            Id = 13,
            Priority = 13,
            JobNumber = "3052",
            CompanyName = "Global Manufacturing",
            Description = Description,
            ContactName = "Stacey Valdovinos",
            ContactPhone = "440-243-7987",
            Address = "98 Udayakavi Ln",
            City = "Danville",
            Zip = "94526",
            State = "CA",
            Latitude = 37.842446f,
            Longitude = -122.005243f,
            Status = AssignmentStatus.New,
            StartDate = new DateTime(2013, 10, 24),
            EndDate = new DateTime(2013, 11, 15),
        };

        public static readonly Assignment Assignment14 = new Assignment
        {
            Id = 14,
            Priority = 14,
            JobNumber = "9610",
            CompanyName = "Pacific Marine Supply",
            Description = Description,
            ContactName = "Jesus Cardella ",
            ContactPhone = "410-745-5521",
            Address = "1008 Rachele Rd",
            City = "Walnut Creek",
            Zip = "94597",
            State = "CA",
            Latitude = 37.913775f,
            Longitude = -122.079070f,
            Status = AssignmentStatus.New,
            StartDate = new DateTime(2013, 12, 24),
            EndDate = new DateTime(2013, 12, 25),
        };

        public static readonly Assignment Assignment15 = new Assignment
        {
            Id = 15,
            Priority = 15,
            JobNumber = "4873",
            CompanyName = "Mission School District",
            Description = Description,
            ContactName = "Wilma Woolley",
            ContactPhone = "940-696-1852",
            Address = "7277 Moeser Ln",
            City = "El Cerrito",
            Zip = "94530",
            State = "CA",
            Latitude = 37.914458f,
            Longitude = -122.301514f,
            Status = AssignmentStatus.New,
            StartDate = new DateTime(2013, 4, 10),
            EndDate = new DateTime(2013, 4, 17),
        };

        #endregion

        #region Assignment History

        public static readonly Assignment Assignment16 = new Assignment
        {
            Id = 16,
            Priority = 1,
            JobNumber = "2016",
            CompanyName = "City of Redmond",
            Description = Description,
            ContactName = "Evan Armstead",
            ContactPhone = "415-336-2228",
            Address = "398 23rd St",
            City = "Richmond",
            State = "CA",
            Zip = "94804",
            Latitude = 37.936871f,
            Longitude = -122.347516f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-30),
            EndDate = DateTime.Now.AddDays(-30).AddHours(2),
        };

        public static readonly Assignment Assignment17 = new Assignment
        {
            Id = 17,
            Priority = 2,
            JobNumber = "2017",
            CompanyName = "BayTech Credit Union",
            Description = Description,
            ContactName = "Douglas Greenly",
            ContactPhone = "201-929-0094",
            Address = "2267 Alameda Ave",
            City = "Alameda",
            State = "CA",
            Zip = "94501",
            Latitude = 37.764955f,
            Longitude = -122.245887f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        public static readonly Assignment Assignment18 = new Assignment
        {
            Id = 18,
            Priority = 3,
            JobNumber = "3118",
            CompanyName = "East Bay Commercial Bank",
            Description = Description,
            ContactName = "James Jones",
            ContactPhone = "313-248-9644",
            Address = "4501 Pleasanton Ave",
            City = "Pleasanton",
            State = "CA",
            Zip = "94556",
            Latitude = 37.661690f,
            Longitude = -121.887367f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        public static readonly Assignment Assignment19 = new Assignment
        {
            Id = 19,
            Priority = 4,
            JobNumber = "3119",
            CompanyName = "Rockridge Hotel",
            Description = Description,
            ContactName = "Brent Mason",
            ContactPhone = "940-482-7759",
            Address = "1960 Mandela Pkwy",
            City = "Oakland",
            State = "CA",
            Zip = "94607",
            Latitude = 37.814778f,
            Longitude = -122.288597f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        public static readonly Assignment Assignment20 = new Assignment
        {
            Id = 20,
            Priority = 5,
            JobNumber = "4420",
            CompanyName = "Marin Luxury Senior Living",
            Description = Description,
            ContactName = "Richard Hogan",
            ContactPhone = "978-658-7545",
            Address = "674 Tiburon Blvd",
            City = "Belvedere Tiburon",
            State = "CA",
            Zip = "94920",
            Latitude = 37.890050f,
            Longitude = -122.478655f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        public static readonly Assignment Assignment21 = new Assignment
        {
            Id = 21,
            Priority = 6,
            JobNumber = "4421",
            CompanyName = "Cityview Consulting",
            Description = Description,
            ContactName = "Daniel Granville",
            ContactPhone = "330-616-7467",
            Address = "300 Spencer Ave",
            City = "Saulsalito",
            State = "CA",
            Zip = "94965",
            Latitude = 37.852171f,
            Longitude = -122.490258f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        public static readonly Assignment Assignment22 = new Assignment
        {
            Id = 22,
            Priority = 7,
            JobNumber = "5622",
            CompanyName = "Marin Cultural Center",
            Description = Description,
            ContactName = "Margaret Kidd",
            ContactPhone = "406-784-0602",
            Address = "106 Throckmorton Ave",
            City = "Mill Valley",
            Zip = "94941",
            State = "CA",
            Latitude = 37.906189f,
            Longitude = -122.548484f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        public static readonly Assignment Assignment23 = new Assignment
        {
            Id = 23,
            Priority = 8,
            JobNumber = "5623",
            CompanyName = "San Rafael Chambers of Commerce",
            Description = Description,
            ContactName = "Leo Parsons",
            ContactPhone = "773-991-5214",
            Address = "199 Clorinda Ave",
            City = "San Rafael",
            Zip = "94901",
            State = "CA",
            Latitude = 37.967382f,
            Longitude = -122.539084f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        public static readonly Assignment Assignment24 = new Assignment
        {
            Id = 24,
            Priority = 9,
            JobNumber = "7824",
            CompanyName = "Millenium Mortgage",
            Description = Description,
            ContactName = "Raymond Denson",
            ContactPhone = "253-594-5757",
            Address = "1612 Center Rd",
            City = "Novato",
            Zip = "94947",
            State = "CA",
            Latitude = 38.101042f,
            Longitude = -122.576288f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        public static readonly Assignment Assignment25 = new Assignment
        {
            Id = 25,
            Priority = 10,
            JobNumber = "7825",
            CompanyName = "Northern CA Agricultural Assoc",
            Description = Description,
            ContactName = "Mary Fraser",
            ContactPhone = "850-221-7352",
            Address = "1998 Caulfield Ln",
            City = "Petaluma",
            Zip = "94954",
            State = "CA",
            Latitude = 38.251013f,
            Longitude = -122.606438f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        public static readonly Assignment Assignment26 = new Assignment
        {
            Id = 26,
            Priority = 11,
            JobNumber = "7526",
            CompanyName = "Bay Area Casualty and Life",
            Description = Description,
            ContactName = "Michael Scott",
            ContactPhone = "484-597-9853",
            Address = "1098 Lombard St",
            City = "San Francisco",
            Zip = "94133",
            State = "CA",
            Latitude = 37.802019f,
            Longitude = -122.419669f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        public static readonly Assignment Assignment27 = new Assignment
        {
            Id = 27,
            Priority = 12,
            JobNumber = "1627",
            CompanyName = "Marsh Media Group",
            Description = Description,
            ContactName = "Gerald Becker",
            ContactPhone = "408-583-8482",
            Address = "821 Larchmont Dr",
            City = "Daly City",
            Zip = "94015",
            State = "CA",
            Latitude = 37.692275f,
            Longitude = -122.485992f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        public static readonly Assignment Assignment28 = new Assignment
        {
            Id = 28,
            Priority = 13,
            JobNumber = "3028",
            CompanyName = "Port of Oakland",
            Description = Description,
            ContactName = "Victor Allen",
            ContactPhone = "610-361-8981",
            Address = "10854 Edes Ave",
            City = "Oakland",
            Zip = "94603",
            State = "CA",
            Latitude = 37.731547f,
            Longitude = -122.176006f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        public static readonly Assignment Assignment29 = new Assignment
        {
            Id = 29,
            Priority = 14,
            JobNumber = "9629",
            CompanyName = "Westfield Enterprises",
            Description = Description,
            ContactName = "Alice Dominguez",
            ContactPhone = "540-510-6123",
            Address = "1720 Mt Diablo Blvd",
            City = "Walnut Creek",
            Zip = "94596",
            State = "CA",
            Latitude = 37.897384f,
            Longitude = -122.063812f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        public static readonly Assignment Assignment30 = new Assignment
        {
            Id = 30,
            Priority = 15,
            JobNumber = "4830",
            CompanyName = "Center of the Performing Arts",
            Description = Description,
            ContactName = "Sterling Conner",
            ContactPhone = "267-795-6664",
            Address = "1815 Lincoln Ave",
            City = "San Jose",
            Zip = "95125",
            State = "CA",
            Latitude = 37.296562f,
            Longitude = -122.892831f,
            Status = AssignmentStatus.Complete,
            StartDate = DateTime.Now.AddDays(-31),
            EndDate = DateTime.Now.AddDays(-31).AddHours(2),
        };

        #endregion

        public static object[] All = new object[] {
            //Some assignments
            Assignment1,
            Assignment2,
	    Assignment3,
            Assignment4,
            Assignment5,
            Assignment6,
            Assignment7,
            Assignment8,
            Assignment9,
            Assignment10,
            Assignment11,
            Assignment12,
            Assignment13,
            Assignment14,
            Assignment15,
            Assignment16,
            Assignment17,
            Assignment18,
            Assignment19,
            Assignment20,
            Assignment21,
            Assignment22,
            Assignment23,
            Assignment24,
            Assignment25,
            Assignment26,
            Assignment27,
            Assignment28,
            Assignment29,
            Assignment30,

            //Some items
            new Item
            {
                Id = 1,
                Name = "Sheet Metal Screws",
                Number = "1001",
            },
            new Item
            {
                Id = 2,
                Name = "PVC Pipe - Small",
                Number = "1002",
            },
            new Item
            {
                Id = 3,
                Name = "PVC Pipe - Medium",
                Number = "1003",
            },
            new Item
            {
                Id = 4,
                Name = "Diatium Power Cell",
                Number = "1004",
            },
            new Item
            {
                Id = 5,
                Name = "Flux Aperture",
                Number = "1005",
            },
            new Item
            {
                Id = 6,
                Name = "Ion Energy Cell",
                Number = "1006",
            },
            new Item
            {
                Id = 7,
                Name = "Telgorn Jolt Cell Mark I",
                Number = "1007",
            },
            new Item
            {
                Id = 8,
                Name = "Bifurcating Cyclical-Ignition Pulse",
                Number = "1008",
            },
            new Item
            {
                Id = 9,
                Name = "Flux Capacitor",
                Number = "1009",
            },
            new Item
            {
                Id = 10,
                Name = "Unobtainium",
                Number = "1010",
            },
            new Item
            {
                Id = 11,
                Name = "Adamantium",
                Number = "1011",
            },
            new Item
            {
                Id = 12,
                Name = "Tesseract",
                Number = "1012",
            },

            //Some items on assignments
            new AssignmentItem
            {
                AssignmentId = Assignment1.Id,
                ItemId = 1,
            },
            new AssignmentItem
            {
                AssignmentId = Assignment1.Id,
                ItemId = 2,
            },
            new AssignmentItem
            {
                AssignmentId = Assignment2.Id,
                ItemId = 2,
            },
            new AssignmentItem
            {
                AssignmentId = Assignment2.Id,
                ItemId = 3,
            },
	    new AssignmentItem
	    {
		AssignmentId = Assignment3.Id,
		ItemId = 1,
	    },
            new AssignmentItem
	    {
		AssignmentId = Assignment5.Id,
		ItemId = 1,
	    },
            new AssignmentItem
            {
                AssignmentId = Assignment16.Id,
                ItemId = 1,
            },
            new AssignmentItem
            {
                AssignmentId = Assignment17.Id,
                ItemId = 2,
            },
            new AssignmentItem
            {
                AssignmentId = Assignment18.Id,
                ItemId = 2,
            },
            new AssignmentItem
            {
                AssignmentId = Assignment19.Id,
                ItemId = 3,
            },
	    new AssignmentItem
	    {
		AssignmentId = Assignment20.Id,
		ItemId = 1,
	    },
            new AssignmentItem
	    {
		AssignmentId = Assignment21.Id,
		ItemId = 1,
	    },
            new AssignmentItem
            {
                AssignmentId = Assignment22.Id,
                ItemId = 1,
            },
            new AssignmentItem
            {
                AssignmentId = Assignment23.Id,
                ItemId = 2,
            },
            new AssignmentItem
            {
                AssignmentId = Assignment24.Id,
                ItemId = 2,
            },
            new AssignmentItem
            {
                AssignmentId = Assignment25.Id,
                ItemId = 3,
            },
	    new AssignmentItem
	    {
		AssignmentId = Assignment26.Id,
		ItemId = 1,
	    },
            new AssignmentItem
	    {
		AssignmentId = Assignment27.Id,
		ItemId = 1,
	    },
            new AssignmentItem
            {
                AssignmentId = Assignment28.Id,
                ItemId = 3,
            },
	    new AssignmentItem
	    {
		AssignmentId = Assignment29.Id,
		ItemId = 1,
	    },
            new AssignmentItem
	    {
		AssignmentId = Assignment30.Id,
		ItemId = 1,
	    },

            //Some labor for assignments
            new Labor
            {
                AssignmentId = Assignment1.Id,
                Description = "Sheet Metal Screw Sorting",
                Hours = TimeSpan.FromHours(10),
                Type = LaborType.HolidayTime,
            },
            new Labor
            {
                AssignmentId = Assignment1.Id,
                Description = "Pipe Fitting",
                Hours = TimeSpan.FromHours(4),
                Type = LaborType.Hourly,
            },
            new Labor
            {
                AssignmentId = Assignment2.Id,
                Description = "Attaching Sheet Metal To PVC Pipe",
                Hours = TimeSpan.FromHours(8),
                Type = LaborType.OverTime,
            },
            new Labor
            {
                AssignmentId = Assignment3.Id,
                Description = "Sheet Metal / Pipe Decoupling",
                Hours = TimeSpan.FromHours(4),
                Type = LaborType.Hourly,
            },
            new Labor
            {
                AssignmentId = Assignment16.Id,
                Description = "Sheet Metal Screw Sorting",
                Hours = TimeSpan.FromHours(10),
                Type = LaborType.HolidayTime,
            },
            new Labor
            {
                AssignmentId = Assignment16.Id,
                Description = "Pipe Fitting",
                Hours = TimeSpan.FromHours(4),
                Type = LaborType.Hourly,
            },
            new Labor
            {
                AssignmentId = Assignment17.Id,
                Description = "Attaching Sheet Metal To PVC Pipe",
                Hours = TimeSpan.FromHours(8),
                Type = LaborType.OverTime,
            },
            new Labor
            {
                AssignmentId = Assignment18.Id,
                Description = "Sheet Metal / Pipe Decoupling",
                Hours = TimeSpan.FromHours(4),
                Type = LaborType.Hourly,
            },
            new Labor
            {
                AssignmentId = Assignment19.Id,
                Description = "Sheet Metal Screw Sorting",
                Hours = TimeSpan.FromHours(10),
                Type = LaborType.HolidayTime,
            },
            new Labor
            {
                AssignmentId = Assignment20.Id,
                Description = "Pipe Fitting",
                Hours = TimeSpan.FromHours(4),
                Type = LaborType.Hourly,
            },
            new Labor
            {
                AssignmentId = Assignment21.Id,
                Description = "Attaching Sheet Metal To PVC Pipe",
                Hours = TimeSpan.FromHours(8),
                Type = LaborType.OverTime,
            },
            new Labor
            {
                AssignmentId = Assignment22.Id,
                Description = "Sheet Metal / Pipe Decoupling",
                Hours = TimeSpan.FromHours(4),
                Type = LaborType.Hourly,
            },
            new Labor
            {
                AssignmentId = Assignment23.Id,
                Description = "Sheet Metal Screw Sorting",
                Hours = TimeSpan.FromHours(10),
                Type = LaborType.HolidayTime,
            },
            new Labor
            {
                AssignmentId = Assignment24.Id,
                Description = "Pipe Fitting",
                Hours = TimeSpan.FromHours(4),
                Type = LaborType.Hourly,
            },
            new Labor
            {
                AssignmentId = Assignment25.Id,
                Description = "Attaching Sheet Metal To PVC Pipe",
                Hours = TimeSpan.FromHours(8),
                Type = LaborType.OverTime,
            },
            new Labor
            {
                AssignmentId = Assignment26.Id,
                Description = "Sheet Metal / Pipe Decoupling",
                Hours = TimeSpan.FromHours(4),
                Type = LaborType.Hourly,
            },
            new Labor
            {
                AssignmentId = Assignment27.Id,
                Description = "Sheet Metal Screw Sorting",
                Hours = TimeSpan.FromHours(10),
                Type = LaborType.HolidayTime,
            },
            new Labor
            {
                AssignmentId = Assignment28.Id,
                Description = "Pipe Fitting",
                Hours = TimeSpan.FromHours(4),
                Type = LaborType.Hourly,
            },
            new Labor
            {
                AssignmentId = Assignment29.Id,
                Description = "Attaching Sheet Metal To PVC Pipe",
                Hours = TimeSpan.FromHours(8),
                Type = LaborType.OverTime,
            },
            new Labor
            {
                AssignmentId = Assignment30.Id,
                Description = "Sheet Metal / Pipe Decoupling",
                Hours = TimeSpan.FromHours(4),
                Type = LaborType.Hourly,
            },

            //Some expenses for assignments
            new Expense
            {
                AssignmentId = Assignment1.Id,
                Description = "Filled up tank at Speedway",
                Category = ExpenseCategory.Gas,
                Cost = 40.5M,
            },
            new Expense
            {
                AssignmentId = Assignment1.Id,
                Description = "Hot Dog from Speedway",
                Category = ExpenseCategory.Food,
                Cost = 0.99M,
            },
            new Expense
            {
                AssignmentId = Assignment2.Id,
                Description = "Duct Tape",
                Category = ExpenseCategory.Supplies,
                Cost = 3.5M,
            },
            new Expense
            {
                AssignmentId = Assignment3.Id,
                Description = "Taquito from Speedway",
                Category = ExpenseCategory.Food,
                Cost = 0.99M,
            },
            new Expense
            {
                AssignmentId = Assignment5.Id,
                Description = "Toll Road",
                Category = ExpenseCategory.Other,
                Cost = 1,
            },
            new Expense
            {
                AssignmentId = Assignment6.Id,
                Description = "Chocolate",
                Category = ExpenseCategory.Food,
                Cost = 1.5M,
            },
            new Expense
            {
                AssignmentId = Assignment7.Id,
                Description = "Advertising",
                Category = ExpenseCategory.Other,
                Cost = 200M,
            },
            new Expense
            {
                AssignmentId = Assignment8.Id,
                Description = "Cleaning Supplies",
                Category = ExpenseCategory.Supplies,
                Cost = 1.99M,
            },
            new Expense
            {
                AssignmentId = Assignment9.Id,
                Description = "Ingredients",
                Category = ExpenseCategory.Food,
                Cost = 1.99M,
            },
            new Expense
            {
                AssignmentId = Assignment10.Id,
                Description = "Universal Stuff",
                Category = ExpenseCategory.Supplies,
                Cost = 99.99M,
            },
            new Expense
            {
                AssignmentId = Assignment16.Id,
                Description = "Filled up tank at Speedway",
                Category = ExpenseCategory.Gas,
                Cost = 40.5M,
            },
            new Expense
            {
                AssignmentId = Assignment17.Id,
                Description = "Hot Dog from Speedway",
                Category = ExpenseCategory.Food,
                Cost = 0.99M,
            },
            new Expense
            {
                AssignmentId = Assignment18.Id,
                Description = "Duct Tape",
                Category = ExpenseCategory.Supplies,
                Cost = 3.5M,
            },
            new Expense
            {
                AssignmentId = Assignment19.Id,
                Description = "Taquito from Speedway",
                Category = ExpenseCategory.Food,
                Cost = 0.99M,
            },
            new Expense
            {
                AssignmentId = Assignment20.Id,
                Description = "Toll Road",
                Category = ExpenseCategory.Other,
                Cost = 1,
            },
            new Expense
            {
                AssignmentId = Assignment21.Id,
                Description = "Chocolate",
                Category = ExpenseCategory.Food,
                Cost = 1.5M,
            },
            new Expense
            {
                AssignmentId = Assignment22.Id,
                Description = "Advertising",
                Category = ExpenseCategory.Other,
                Cost = 200M,
            },
            new Expense
            {
                AssignmentId = Assignment23.Id,
                Description = "Cleaning Supplies",
                Category = ExpenseCategory.Supplies,
                Cost = 1.99M,
            },
            new Expense
            {
                AssignmentId = Assignment24.Id,
                Description = "Ingredients",
                Category = ExpenseCategory.Food,
                Cost = 1.99M,
            },
            new Expense
            {
                AssignmentId = Assignment25.Id,
                Description = "Universal Stuff",
                Category = ExpenseCategory.Supplies,
                Cost = 99.99M,
            },

            new AssignmentHistory
            {
                AssignmentId = Assignment16.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment16.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment16.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment17.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment17.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment17.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment18.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment18.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment18.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment19.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment19.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment19.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment20.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment20.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment20.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment21.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment21.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment21.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment22.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment22.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment22.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment23.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment23.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment23.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment24.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment24.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment24.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment25.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment25.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment25.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment26.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment26.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment26.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment27.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment27.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment27.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment28.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment28.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment28.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment29.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment29.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment29.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment30.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment30.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment30.Id,
                Date = DateTime.Today.AddDays(-90),
            },
        };
    }
}