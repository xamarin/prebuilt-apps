using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Data {
    /// <summary>
    /// Static class for holding all test data within the app
    /// </summary>
    public static class TestData {
        /// <summary>
        /// This is just a test description, we just put it on all the assignments for now
        /// </summary>
        public const string Description = "This is the desription created for this specific assignment. It would include helpful information related to the assignment, and you would probably want to read it.  Important information would be here, like which tools to use and what needs to be done for the assignment.";

        public static readonly Assignment Assignment1 = new Assignment {
            Id = 1,
            Priority = 1,
            JobNumber = "2001",
            CompanyName = "Xamarin",
            Description = Description,
            ContactName = "Miguel de Icaza",
            ContactPhone = "1.232.234.2352",
            Address = "306 5th Street",
            City = "Adrian",
            State = "TX",
            Zip = "79001",
            Latitude = 35.273618f,
            Longitude = -102.669257f,
            Status = AssignmentStatus.Active,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddHours (2),
        };

        public static readonly Assignment Assignment2 = new Assignment {
            Id = 2,
            Priority = 2,
            JobNumber = "2002",
            CompanyName = "Shackles, Inc",
            Description = Description,
            ContactName = "Greg Shackles",
            ContactPhone = "1.232.234.2112",
            Address = "316 Avalon Cir",
            City = "Smithtown",
            State = "NY",
            Zip = "11787",
            Latitude = 40.851216f,
            Longitude = -73.169185f,
            Status = AssignmentStatus.Hold,
            StartDate = DateTime.Now.AddDays (1),
            EndDate = DateTime.Now.AddDays (1).AddHours (2),
        };

        public static readonly Assignment Assignment3 = new Assignment {
            Id = 3,
            Priority = 3,
            JobNumber = "3113",
            CompanyName = "Xamarin",
            Description = Description,
            ContactName = "Xamarin",
            ContactPhone = "1.855.926.2746",
            Address = "1796 18th Street",
            City = "San Fancisco",
            State = "CA",
            Zip = "94107",
            Latitude = 37.762658f,
            Longitude = -122.400239f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays (1),
            EndDate = DateTime.Now.AddDays (1).AddHours (2),
        };

        public static readonly Assignment Assignment4 = new Assignment {
            Id = 4,
            Priority = 4,
            JobNumber = "3114",
            CompanyName = "Hitcents, Inc",
            Description = Description,
            ContactName = "HERB",
            ContactPhone = "270.796.5063",
            Address = "2425 Nashville Road",
            City = "Bowling Green",
            State = "KY",
            Zip = "42101",
            Latitude = 36.961457f,
            Longitude = -86.468627f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays (1),
            EndDate = DateTime.Now.AddDays (1).AddHours (2),
        };

        public static readonly Assignment Assignment5 = new Assignment {
            Id = 5,
            Priority = 5,
            JobNumber = "4445",
            CompanyName = "Google",
            Description = Description,
            ContactName = "Google",
            ContactPhone = "650.253.0000",
            Address = "1600 Amphitheatre Parkway",
            City = "Mountain View",
            State = "CA",
            Zip = "94043",
            Latitude = 37.426616f,
            Longitude = -122.08388f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays (1),
            EndDate = DateTime.Now.AddDays (1).AddHours (2),
        };

        public static readonly Assignment Assignment6 = new Assignment {
            Id = 6,
            Priority = 6,
            JobNumber = "4446",
            CompanyName = "Willy Wonka's",
            Description = Description,
            ContactName = "Hershey",
            ContactPhone = "717.298.1298",
            Address = "200 Crystal A Drive",
            City = "Hershey",
            State = "PA",
            Zip = "17033",
            Latitude = 40.299715f,
            Longitude = -76.646753f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays (1),
            EndDate = DateTime.Now.AddDays (1).AddHours (2),
        };

        public static readonly Assignment Assignment7 = new Assignment {
            Id = 7,
            Priority = 7,
            JobNumber = "5677",
            CompanyName = "M$oft",
            Description = Description,
            ContactName = "Microsoft",
            ContactPhone = "425.882.8080",
            Address = "1 157th Avenue Northeast",
            City = "Redmond",
            Zip = "98052",
            State = "WA",
            Latitude = 47.710108f,
            Longitude = -122.130572f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays (1),
            EndDate = DateTime.Now.AddDays (1).AddHours (2),
        };

        public static readonly Assignment Assignment8 = new Assignment {
            Id = 8,
            Priority = 8,
            JobNumber = "5678",
            CompanyName = "Strikes Back",
            Description = Description,
            ContactName = "The Empire",
            ContactPhone = "212.736.3100",
            Address = "350 5th Avenue",
            City = "New York",
            Zip = "10118",
            State = "NY",
            Latitude = 40.749029f,
            Longitude = -73.984469f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays (1),
            EndDate = DateTime.Now.AddDays (1).AddHours (2),
        };

        public static readonly Assignment Assignment9 = new Assignment {
            Id = 9,
            Priority = 9,
            JobNumber = "7809",
            CompanyName = "Tolkein Enterprises",
            Description = Description,
            ContactName = "The Shire",
            ContactPhone = "360.748.3720",
            Address = "465 Northwest Chehalis Avenue",
            City = "Chehalis",
            Zip = "98532",
            State = "WA",
            Latitude = 46.66758f,
            Longitude = -122.971198f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays (1),
            EndDate = DateTime.Now.AddDays (1).AddHours (2),
        };

        public static readonly Assignment Assignment10 = new Assignment {
            Id = 10,
            Priority = 10,
            JobNumber = "7810",
            CompanyName= "NBCUniversal",
            Description = Description,
            ContactName = "Universal Studios",
            ContactPhone = "800.864.8377",
            Address = "100 Universal City Plaza",
            City = "Universal City",
            Zip = "91608",
            State = "CA",
            Latitude = 34.141646f,
            Longitude = -118.354194f,
            Status = AssignmentStatus.New,
            StartDate = DateTime.Now.AddDays (1),
            EndDate = DateTime.Now.AddDays (1).AddHours (2),
        };

        public static object [] All = new object [] {
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

            new AssignmentHistory
            {
                AssignmentId = Assignment1.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment1.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment1.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment2.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment2.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment2.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment3.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment3.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment3.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment4.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment4.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment4.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment5.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment5.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment5.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment6.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment6.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment6.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment7.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment7.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment7.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment8.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment8.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment8.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment9.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment9.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment9.Id,
                Date = DateTime.Today.AddDays(-90),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment10.Id,
                Date = DateTime.Today.AddDays(-30),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment10.Id,
                Type = AssignmentHistoryType.PhoneCall,
                CallLength = TimeSpan.FromHours(1.25),
                CallDescription = "Received call about a new issue.",
                Date = DateTime.Today.AddDays(-60),
            },
            new AssignmentHistory
            {
                AssignmentId = Assignment10.Id,
                Date = DateTime.Today.AddDays(-90),
            },
        };
    }
}