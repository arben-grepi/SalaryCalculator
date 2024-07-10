# SalaryCalculator

An application for calculating hourly wage workers' salaries along with weekly/monthly recurring benefits. Personalize your benefits and calculate your earnings quickly and easily.

The application is compatible with .NET Frameworks and utilizes MariaDB, an open-source relational database management system.

**Before Using the Application:**
1. Before running the application (e.g., in Visual Studio), configure your mariadb database with following information: 
@"Server=127.0.0.1; Port=3306; User ID=opiskelija; Pwd=opiskelija1; Database=Palkanlaskin;". If you would rather use your own information, modify them to the Repository.cs file.  
2. Copy the db-contents from `PalkanlaskinDatabase.txt` and use it to create a database.
3. Build and run the application.

**When Using the Application:**
1. Create an account. Do not use passwords that you do not want stored in your local MariaDB database.
2. Customize your salary, such as weekly or monthly recurring benefits. Save it to the database.
3. Type in your shifts and get calculations on your earnings.
   
