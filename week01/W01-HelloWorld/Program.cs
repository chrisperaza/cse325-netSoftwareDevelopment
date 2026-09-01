Console.WriteLine("Hello, World!");
Console.WriteLine("The current time is " + DateTime.Now);

DateTime today = DateTime.Today;
DateTime christmas = new DateTime(today.Year, 12, 25);
int daysLeftForChristmas = (christmas - today).Days;

Console.WriteLine("There are " + daysLeftForChristmas +  " days until the next Christmas");
