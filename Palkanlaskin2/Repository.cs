using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;
using Xceed.Wpf.Toolkit;

namespace Palkanlaskin2
{
    public class Repository
    {
        private const string local = @"Server=127.0.0.1; Port=5432; User ID=opiskelija; Pwd=opiskelija1;";
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=opiskelija; Pwd=opiskelija1; Database=Palkanlaskin;";
        private bool Löytyy { get; set; }
        public Repository() 
        {
            Löytyy = false;
        }

        //LoginAndCreateAccount
        public bool CheckIfAccountExists(string username)
        
        {
            bool löytyy = false;

            var käyttäjä = new Customer();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var sql = $"Use palkanlaskin;" +
                    $"SELECT * FROM `customerinfo` WHERE `Käyttäjätunnus` = '{username}';";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    käyttäjä = new Customer
                    {
                        Username = dr.GetString("Käyttäjätunnus"),

                    };
                }
                if (käyttäjä.Username == username)
                {
                    löytyy = true;
                }
            }

            return löytyy;

        }
        public void CreateAccount(string username, string password)
        {
            using (var connn = new MySqlConnection(localWithDb))
            {
                connn.Open();

                MySqlCommand cmd = new MySqlCommand("USE palkanlaskin; INSERT INTO customerinfo (Käyttäjätunnus, Salasana)VALUES (@username, @password);", connn);

                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);

                cmd.ExecuteNonQuery();

            }
        }
        public Customer GetCustomerID(Customer customer)
        {
            var väliaikainencustomer = new Customer();
            using (var connn = new MySqlConnection(localWithDb))
            {
                connn.Open();
                var sql = "USE palkanlaskin; SELECT `KäyttäjäID` FROM `customerinfo` WHERE `Käyttäjätunnus` = @`Käyttäjätunnus` AND Salasana = @Salasana;";

                MySqlCommand cmd = new MySqlCommand(sql, connn);

                cmd.Parameters.AddWithValue("@`Käyttäjätunnus`", customer.Username);
                cmd.Parameters.AddWithValue("@Salasana", customer.Password);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    väliaikainencustomer = new Customer
                    {
                        Id = dr.GetInt32("KäyttäjäID")

                    };
                }

            }
            customer.Id = väliaikainencustomer.Id;
            return customer;
        }
        public Customer HaeAsiakas(string username, string password)
        {
            var customer = new Customer();
            using (var connn = new MySqlConnection(localWithDb))
            {
                connn.Open();
                var sql = "USE palkanlaskin; SELECT `Käyttäjätunnus`, Salasana FROM `customerinfo` WHERE `Käyttäjätunnus` = @Käyttäjätunnus AND Salasana = @Salasana;";

                MySqlCommand cmd = new MySqlCommand(sql, connn);

                cmd.Parameters.AddWithValue("@Käyttäjätunnus", username);
                cmd.Parameters.AddWithValue("@Salasana", password);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    customer = new Customer
                    {
                        Username = dr.GetString("Käyttäjätunnus"),
                        Password = dr.GetString("Salasana")

                    };
                }

            }
            return customer;

        }
        public int HaeAsiakkaanID(Customer customer)
        {
            Customer newcustomer = new Customer();
            using (var connn = new MySqlConnection(localWithDb))
            {
                connn.Open();
                var sql = "USE palkanlaskin; SELECT `KäyttäjäID` FROM customerinfo WHERE Käyttäjätunnus = @tunnus AND Salasana = @sala ;";

                MySqlCommand cmd = new MySqlCommand(sql, connn);

                cmd.Parameters.AddWithValue("@tunnus", customer.Username);
                cmd.Parameters.AddWithValue("@sala", customer.Password);


                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    newcustomer = new Customer
                    {
                        Id = dr.GetInt32("KäyttäjäID")

                    };
                }

            }
          
            return newcustomer.Id;
        }
        public bool CheckIfPasswordMatchesUsername(string username, string password)
        {

            var matches = false;

            var customer = new Customer();

            using (var connn = new MySqlConnection(localWithDb))
            {
                connn.Open();
                var sql = "USE palkanlaskin; SELECT `Käyttäjätunnus`, Salasana FROM `customerinfo` WHERE `Käyttäjätunnus` = @`Käyttäjätunnus` AND Salasana = @Salasana;";

                MySqlCommand cmd = new MySqlCommand(sql, connn);

                cmd.Parameters.AddWithValue("@`Käyttäjätunnus`", username);
                cmd.Parameters.AddWithValue("@Salasana", password);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    customer = new Customer
                    {
                        Username = dr.GetString("Käyttäjätunnus"),
                        Password = dr.GetString("Salasana")

                    };
                }

            }

            if (customer.Username == username && customer.Password == password)
            {
                matches = true;

            }
            return matches;
        }




        //Overtimebenefit
        public void CreateOvertimeBenefit(int timePeriod, TimeSpan time, decimal amount, int contractID)
        {
            var overtimeBenefit = new OvertimeBenefit();

            Löytyy = CheckIfOvertimeBenefitTimeSpanExists(contractID, timePeriod);

            if (!Löytyy)
            {
                using (var connn = new MySqlConnection(localWithDb))
                {
                    connn.Open();

                    var sql = $"USE palkanlaskin; " +
                        $"INSERT INTO `ylityölisä` (`Määrä`, `Työtunnit`, `Aikaväli`) " +
                        $"VALUES (@amount, @time, @timeperiod) ; ";

                    MySqlCommand cmd = new MySqlCommand(sql, connn);

                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@time", time);
                    cmd.Parameters.AddWithValue("@timeperiod", timePeriod);

                    cmd.ExecuteNonQuery();

                }
            }


        }
        public bool CheckIfOvertimeBenefitTimeSpanExists(int contractID, int timePeriod)
        {
            var benefits = new ObservableCollection<OvertimeBenefit>();
            var overtimeBenefit = new OvertimeBenefit();
            Löytyy = false;

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {

                conn.Open();

                var sql = $"USE palkanlaskin; SELECT `Aikaväli`, `LisäID` FROM `ylityölisä` WHERE `LisäID` IN (SELECT `LisäID` " +
                    $"FROM `työsopimusylityölisä` WHERE SopimusID = {contractID}) ;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                  
                    benefits.Add(overtimeBenefit = new OvertimeBenefit
                    {
                        ID = dr.GetInt32("LisäID"),
                        TimePeriod = dr.GetInt32("Aikaväli")
                    });
                }

                if (benefits.Count == 0)
                {
                    Löytyy = false;
                }
                else
                {
                    foreach (var item in benefits)
                    {
                        if (item.TimePeriod == timePeriod)
                        {
                            Löytyy = true; break;
                        }

                    }
                }
               


            }

            return Löytyy;

        }

        //private bool CheckIfOvertimeBenefitExists(int timePeriod, TimeSpan timeSpan, decimal amount)
        //{
        //    var overtimeBenefit = new OvertimeBenefit();

        //    using (MySqlConnection conn = new MySqlConnection(localWithDb))
        //    {
        //        conn.Open();

        //        var sql = $"USE palkanlaskin; SELECT `LisäID`, `Määrä`, `Työtunnit`, `Aikaväli` FROM" +
        //            $" `ylityölisä` WHERE `Määrä` = {amount} && `Työtunnit` = '{timeSpan}' && `Aikaväli` = {timePeriod} ; ";

        //        MySqlCommand cmd = new MySqlCommand(sql, conn);

        //        var dr = cmd.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            overtimeBenefit = new OvertimeBenefit
        //            {
        //                ID = dr.GetInt32("LisäID"),
        //                BenefitAmount = dr.GetDecimal("Määrä"),
        //                Time = dr.GetTimeSpan("Työtunnit"),
        //                TimePeriod = dr.GetInt32("Aikaväli")
        //            };
        //        }

        //        if (overtimeBenefit.BenefitAmount == amount && overtimeBenefit.TimePeriod == timePeriod && overtimeBenefit.Time == timeSpan)
        //        {
        //            Löytyy = true;
        //        }
        //        else
        //        {
        //            Löytyy = false;
        //        }


        //    }

        //    return Löytyy;

        //}   
        public OvertimeBenefit GetOvertimeBenefitExists(int timePeriod, TimeSpan time, decimal amount)
        {
            var overtimeBenefit = new OvertimeBenefit();
            string stringTime = overtimeBenefit.TurnTimeSpanintoString(time);

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Corrected SQL query: replaced && with AND, added spaces for readability
                var sql = $"SELECT `LisäID`, `Määrä`, `Työtunnit`, `Aikaväli` " +
                          $"FROM `ylityölisä` " +
                          $"WHERE `Määrä` = @amount AND `Työtunnit` = @time AND `Aikaväli` = @timePeriod;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                // Using parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@time", stringTime);
                cmd.Parameters.AddWithValue("@timePeriod", timePeriod);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        overtimeBenefit = new OvertimeBenefit
                        {
                            ID = dr.GetInt32("LisäID"),
                            BenefitAmount = dr.GetDecimal("Määrä"),
                            Time = dr.GetTimeSpan("Työtunnit"),
                            TimePeriod = dr.GetInt32("Aikaväli")
                        };
                    }
                }
            }

            return overtimeBenefit;
        }

        public void AddToTableJobIdAndContractIDOvertimeBenefi(int contractID, int benefitID)
        {
            if (!Löytyy)
            {
                // Check if the benefitID exists in the ylityölisä table
                using (var checkConn = new MySqlConnection(localWithDb))
                {
                    checkConn.Open();

                    var checkSql = "SELECT COUNT(*) FROM `ylityölisä` WHERE `LisäID` = @lisäid;";
                    using (var checkCmd = new MySqlCommand(checkSql, checkConn))
                    {
                        checkCmd.Parameters.AddWithValue("@lisäid", benefitID);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        // If the benefitID exists, proceed with the insertion
                        if (count > 0)
                        {
                            var sql = "INSERT INTO `työsopimusylityölisä` (LisäID, SopimusID) VALUES (@lisäid, @sopimusid);";

                            using (var connn = new MySqlConnection(localWithDb))
                            {
                                connn.Open();

                                MySqlCommand cmd = new MySqlCommand(sql, connn);
                                cmd.Parameters.AddWithValue("@lisäid", benefitID);
                                cmd.Parameters.AddWithValue("@sopimusid", contractID);

                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Handle the case where the benefitID does not exist
                            Debug.WriteLine($"BenefitID {benefitID} does not exist in ylityölisä.");
                        }
                    }
                }
            }
        }

        public void RemoveOvertimeBenefit(int contractID, int benefitID)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("USE palkanlaskin;  DELETE FROM `työsopimusylityölisä` WHERE `LisäID` = @lisäid AND SopimusID = @sopimusid ; DELETE FROM `ylityölisä` WHERE `LisäID`= @lisäid ;", conn);

                cmd.Parameters.AddWithValue("@lisäid", benefitID);
                cmd.Parameters.AddWithValue("@sopimusid", contractID);

                cmd.ExecuteNonQuery();

            }

        }






        //WeeklyBenefit
        public void CreateWeeklyBenefit(int day, string start, string end, decimal amount)
        {
            bool löytyy = CheckIfWeeklyBenefitExists(day, start, end, amount);

            if (!löytyy)
            {
                using (var connn = new MySqlConnection(localWithDb))
                {
                    connn.Open();

                    var sql = $"USE palkanlaskin; " +
                        $"INSERT INTO `viikottainenlisä`(`Määrä`, `Viikonpäivä`, Alkaa, Loppuu)" +
                        $"VALUES (@maara, @viikonpaiva, @alkaa, @loppuu);";
                  

                    MySqlCommand cmd = new MySqlCommand(sql, connn);

                    cmd.Parameters.AddWithValue("@maara", amount);
                    cmd.Parameters.AddWithValue("@viikonpaiva", day);
                    cmd.Parameters.AddWithValue("@alkaa", start);
                    cmd.Parameters.AddWithValue("@loppuu", end);

                    cmd.ExecuteNonQuery();

                }
            }
           
          
        }
        private bool CheckIfWeeklyBenefitExists(int day, string start, string end, decimal amount)
        {
            bool exists = false; // Define the boolean variable to track if the benefit exists

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Corrected SQL query: replaced && with AND, removed USE statement
                var sql = $"SELECT `LisäID`, `Määrä`, `Viikonpäivä`, Alkaa, Loppuu " +
                          $"FROM `viikottainenlisä` " +
                          $"WHERE `Määrä` = @amount AND `Viikonpäivä` = @day AND `Alkaa` = @start AND `Loppuu` = @end;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                // Using parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@day", day);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // Create a new WeeklyBenefit instance only if a record is found
                        var existingWeeklyBenefit = new WeeklyBenefit
                        {
                            dayOfWeek = dr.GetInt32("Viikonpäivä"),
                            Start = dr.GetTimeSpan("Alkaa"),
                            End = dr.GetTimeSpan("Loppuu"),
                            Amount = dr.GetDecimal("Määrä"),
                            ID = dr.GetInt32("LisäID")
                        };

                        // Check if the existing benefit matches the input
                        if (existingWeeklyBenefit.dayOfWeek == day &&
                            existingWeeklyBenefit.Amount == amount &&
                            existingWeeklyBenefit.Start == TimeSpan.Parse(start) &&
                            existingWeeklyBenefit.End == TimeSpan.Parse(end))
                        {
                            exists = true; // Set exists to true if a match is found
                            break; // Exit the loop early if a match is found
                        }
                    }
                }
            }

            return exists; // Return the boolean indicating existence
        }

        public WeeklyBenefit GetTheWeeklyBenefit(int day, string start, string end, decimal amount)
        {
            WeeklyBenefit temporaryWeeklyBenefit = null; // Initialize to null

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Corrected SQL query: replaced && with AND, removed USE statement
                var sql = "SELECT * FROM `viikottainenlisä` " +
                          "WHERE `Määrä` = @amount AND `Viikonpäivä` = @day AND Alkaa = @start AND Loppuu = @end;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                // Use parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@day", day);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // Create a new WeeklyBenefit instance if a record is found
                        temporaryWeeklyBenefit = new WeeklyBenefit
                        {
                            ID = dr.GetInt32("LisäID"),
                            Amount = dr.GetDecimal("Määrä"),
                            dayOfWeek = dr.GetInt32("Viikonpäivä"),
                            Start = dr.GetTimeSpan("Alkaa"),
                            End = dr.GetTimeSpan("Loppuu")
                        };

                        // Break after the first match, if you expect only one benefit per criteria
                        break;
                    }
                }
            }

            return temporaryWeeklyBenefit; // Return the found benefit or null if not found
        }

        public void AddToTableJobIDAndWeeklyBenefit(int contractID, int weeklyBenefitID)
        {
            if (!Löytyy)
            {
                var sql = "USE palkanlaskin; INSERT INTO `työsopimusviikottainenlisä`(SopimusID, `LisäID`)VALUES (@sopimusID, @lisäID)";

                using (var connn = new MySqlConnection(localWithDb))
                {
                    connn.Open();

                    MySqlCommand cmd = new MySqlCommand(sql, connn);

                    cmd.Parameters.AddWithValue("@sopimusID", contractID);
                    cmd.Parameters.AddWithValue("@lisäID", weeklyBenefitID);

                    cmd.ExecuteNonQuery();

                }

            }


        }

        public void RemoveWeeklyBenefit(int contractID, int benefitID)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("USE palkanlaskin; DELETE FROM `työsopimusviikottainenlisä` WHERE SopimusID = @contractid AND `LisäID` = @lisäid ; DELETE FROM `viikottainenlisä` WHERE `LisäID` = @lisäid ;", conn);

                cmd.Parameters.AddWithValue("@lisäid", benefitID);
                cmd.Parameters.AddWithValue("@contractid", contractID);


                cmd.ExecuteNonQuery();

            }

        }






        //Benefit On a Date
        public void CreateBenefitOnADate(decimal amount, DateOnly date, string start, string end)
        {
            var benefit = new BenefitOnADate();
            Löytyy = CheckIfBenefitOnADateExists(amount, date, start, end);

            if (!Löytyy)
            {
                using (var connn = new MySqlConnection(localWithDb))
                {
                    connn.Open();

                    var sql = $"USE palkanlaskin; " +
                        $"INSERT INTO `lisä`(`Määrä`, `pvm`, Alkaa, Loppuu)" +
                        $"VALUES (@maara, @pvm, @alkaa, @loppuu);";
                    Debug.WriteLine(benefit.Amount + " " + benefit.Date + " "
                        + benefit.stringStart + " " + benefit.stringEnd);

                    MySqlCommand cmd = new MySqlCommand(sql, connn);

                    cmd.Parameters.AddWithValue("@maara", amount);
                    cmd.Parameters.AddWithValue("@pvm", date);
                    cmd.Parameters.AddWithValue("@alkaa", start);
                    cmd.Parameters.AddWithValue("@loppuu", end);

                    cmd.ExecuteNonQuery();

                }
            }
            

        }
        private bool CheckIfBenefitOnADateExists(decimal amount, DateOnly date, string start, string end)
        {
            bool found = false; // Renamed variable to 'found' for clarity

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                // Format the date as yyyy-MM-dd
                var formattedDate = date.ToString("yyyy-MM-dd");
                conn.Open();

                // Fixed SQL query: replaced && with AND, used parameters
                var sql = "SELECT `LisäID`, `Määrä`, `pvm`, `Alkaa`, `Loppuu` " +
                          "FROM `lisä` " +
                          "WHERE `Määrä` = @amount AND `pvm` = @date AND `Alkaa` = @start AND `Loppuu` = @end;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                // Use parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@date", formattedDate);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        // Create benefit only if record exists
                        var existingBenefit = new BenefitOnADate
                        {
                            Date = dr.GetDateOnly("pvm"),
                            Start = dr.GetTimeSpan("Alkaa"),
                            End = dr.GetTimeSpan("Loppuu"),
                            Amount = dr.GetDecimal("Määrä"),
                            ID = dr.GetInt32("LisäID")
                        };

                        // Check if the existing benefit matches the input
                        found = existingBenefit.Date == date &&
                                existingBenefit.Amount == amount &&
                                existingBenefit.Start == TimeSpan.Parse(start) &&
                                existingBenefit.End == TimeSpan.Parse(end);
                    }
                }
            }

            return found;
        }


        public BenefitOnADate GetBenefitOnADate(decimal amount, DateOnly date, string start, string end)
        {
            // Simplified date formatting to "yyyy-MM-dd"
            var formattedDate = date.ToString("yyyy-MM-dd");
            BenefitOnADate benefit = null;

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Use parameters to prevent SQL injection
                var sql = "SELECT * FROM `lisä` WHERE `Määrä` = @amount AND `pvm` = @date AND `Alkaa` = @start AND `Loppuu` = @end;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@date", formattedDate);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        benefit = new BenefitOnADate
                        {
                            ID = dr.GetInt32("LisäID"),
                            Amount = dr.GetDecimal("Määrä"),
                            Date = dr.GetDateOnly("pvm"),
                            Start = dr.GetTimeSpan("Alkaa"),
                            End = dr.GetTimeSpan("Loppuu")
                        };
                    }
                }
            }

            return benefit;
        }


        public void AddToTableJobIdAndContractIDBenefitOnADate(int contractID, int benefitID)
        {
            if (!Löytyy)
            {
                var sql = "USE palkanlaskin; INSERT INTO `työsopimuslisä`(SopimusID, `LisäID`)VALUES (@sopimusID, @lisäID)";

                using (var connn = new MySqlConnection(localWithDb))
                {
                    connn.Open();

                    MySqlCommand cmd = new MySqlCommand(sql, connn);

                    cmd.Parameters.AddWithValue("@sopimusID", contractID);
                    cmd.Parameters.AddWithValue("@lisäID", benefitID);

                    cmd.ExecuteNonQuery();

                }
            }
           
        }
        public void RemoveBenefitOnADate(int contractID, int benefitID)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("USE palkanlaskin;  DELETE FROM `työsopimuslisä` WHERE SopimusID = @contractid AND `LisäID` = @lisäid ; DELETE FROM `lisä` WHERE `LisäID` = @lisäid ;", conn);

                cmd.Parameters.AddWithValue("@lisäid", benefitID);
                cmd.Parameters.AddWithValue("@contractid", contractID);

                cmd.ExecuteNonQuery();

            }

        }







        public void CreateTyösopimusCustomerIDContractID(int id)
        {
            using (var connn = new MySqlConnection(localWithDb))
            {
                connn.Open();

                MySqlCommand cmd = new MySqlCommand("USE palkanlaskin; INSERT INTO `työsopimus`(KäyttäjäID, Tuntipalkka )VALUES (@ID, 0)", connn);

                cmd.Parameters.AddWithValue("@ID", id);



                cmd.ExecuteNonQuery();

            }

        }
        public Job GetCustomersLatestContractIDandCustomerID(int custID)
        {


            var job = new Job();

            var jobID = GetCustomersLatestJobID(custID);

            using (var connn = new MySqlConnection(localWithDb))
            {
                connn.Open();

                var sql = "USE palkanlaskin; SELECT * FROM `työsopimus` WHERE `SopimusID` = @id";

                MySqlCommand cmd = new MySqlCommand(sql, connn);

                cmd.Parameters.AddWithValue("@id", jobID);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    job = new Job
                    {
                        ContractID = dr.GetInt32("SopimusID"),
                        CustomerID = dr.GetInt32("KäyttäjäID"),



                    };
                }

            }
            return job;

        }
        private int GetCustomersLatestJobID(int custID)
        {


            var job = new Job();
            using (var connn = new MySqlConnection(localWithDb))
            {
                connn.Open();
                var sql = "USE palkanlaskin; SELECT MAX(SopimusID)  FROM `työsopimus` WHERE `KäyttäjäID` = @id ;";

                MySqlCommand cmd = new MySqlCommand(sql, connn);

                cmd.Parameters.AddWithValue("@id", custID);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    job = new Job
                    {
                        ContractID = dr.GetInt32("MAX(SopimusID)")

                    };
                }
                Debug.WriteLine(job.ContractID);

            }
            return job.ContractID;


        }

        public Job GetCustomersTyösopimus(int custID)
        {
            var job = new Job();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var sql = "USE palkanlaskin; SELECT * FROM `työsopimus` WHERE `KäyttäjäID` = " + custID ;

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                var dr = cmd.ExecuteReader();
                
                while (dr.Read())
                {

                    job = new Job
                    {
                        CustomerID = dr.GetInt32("KäyttäjäID"),
                        ContractID = dr.GetInt32("SopimusID"),
                        
                    };
                    
                }
            }

            job.SalaryPerHour = GetContractsSalary(job.ContractID);     

            job.BenefitsOnADate = GetCustomersBenefitsOnADate(job.ContractID);

            job.WeeklyBenefits = GetCustomersWeeklyBenefits(job.ContractID);

            job.MBenefits = GetContractsManualBenefits(job.ContractID);

            job.MDeductions = GetContractsManualDeductions(job.ContractID);

            job.Overtimebenefits = GetCustomersOvertimeBenefit(job.ContractID);


            return job;
        }

        private ObservableCollection<ManualBenefit> GetContractsManualBenefits(int contractID)
        {
            var väliaikainenManualBenefits = new ObservableCollection<ManualBenefit>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var sql = "SELECT r.`LisäID`, `Määrä`, Nimi FROM randombenefit r, `työsopimusrandombenefit` rl WHERE r.`LisäID` = rl.`LisäID` AND rl.SopimusID = @contractID ;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@contractID", contractID);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    väliaikainenManualBenefits.Add(new ManualBenefit
                    {
                        ID = dr.GetInt32("LisäID"),                
                       
                    });
                }

            }

            var uusimanualbenefitS = new ObservableCollection<ManualBenefit>();
            var manualbenefit = new ManualBenefit();

            foreach (ManualBenefit item in väliaikainenManualBenefits)
            {
                manualbenefit = GetTheManualBenefit(item.ID);
                uusimanualbenefitS.Add(manualbenefit);
            }
            return uusimanualbenefitS;
        }

        private ObservableCollection<ManualDeduction> GetContractsManualDeductions(int contractID)
        {
            var väliaikainenManualDeductions = new ObservableCollection<ManualDeduction>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var sql = "SELECT r.`LisäID`, `Määrä`, Nimi FROM randomdeduction r, työsopimusrandomdeduction rl WHERE r.`LisäID` = rl.`LisäID` AND rl.SopimusID = @contractID ;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@contractID", contractID);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    väliaikainenManualDeductions.Add(new ManualDeduction
                    {
                        ID = dr.GetInt32("LisäID"),

                    });
                }

            }
            var uusimanualbenefitS = new ObservableCollection<ManualDeduction>();
            var manualdeduction = new ManualDeduction();

            foreach (ManualDeduction item in väliaikainenManualDeductions)
            {
                manualdeduction = GetTheManualDeduction(item.ID);
                uusimanualbenefitS.Add(manualdeduction);
            }
            return uusimanualbenefitS;
        }
        private decimal GetContractsSalary(int contractID)
        {
            var job = new Job();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var sql = "SELECT Tuntipalkka FROM `työsopimus` WHERE SopimusID = @contractID ;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@contractID", contractID);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    job = new Job
                    {
                        SalaryPerHour = dr.GetDecimal("Tuntipalkka"),
                       
                    };
                }

            }
            return job.SalaryPerHour;

        }
         

        private ObservableCollection<OvertimeBenefit> GetCustomersOvertimeBenefit(int contractID) 
        {
            var väliaikainenOvertimeBenefit = new ObservableCollection<OvertimeBenefit>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var sql = "SELECT y.`LisäID`, `Määrä`, Työtunnit, Aikaväli FROM ylityölisä y, työsopimusylityölisä tl WHERE y.`LisäID` = tl.`LisäID` AND tl.SopimusID = @contractID ;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@contractID", contractID);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    väliaikainenOvertimeBenefit.Add(new OvertimeBenefit
                    {
                        //jos ei toimi niin voi johtua siitä että pitää laittaa muotoon l´'LisäID'
                        ID = dr.GetInt32("LisäID"),
                        BenefitAmount = dr.GetDecimal("Määrä"),
                        Time = dr.GetTimeSpan("Työtunnit"),
                        TimePeriod = dr.GetInt32("Aikaväli")

                    });
                }

            }
            return väliaikainenOvertimeBenefit;


        }
        private ObservableCollection<BenefitOnADate> GetCustomersBenefitsOnADate(int contractID)
        {
            var väliaikainenBenefit = new ObservableCollection<BenefitOnADate>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var sql = "SELECT l.`LisäID`, `Määrä`, pvm, Alkaa, Loppuu FROM lisä l, työsopimuslisä tl WHERE l.`LisäID` = tl.`LisäID` AND tl.SopimusID = @contractID ;";
               
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@contractID", contractID);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    väliaikainenBenefit.Add(new BenefitOnADate
                    {
                        //jos ei toimi niin voi johtua siitä että pitää laittaa muotoon l´'LisäID'
                        ID = dr.GetInt32("LisäID"),
                        Amount = dr.GetDecimal("Määrä"),
                        Date = dr.GetDateOnly("pvm"),
                        Start = dr.GetTimeSpan("Alkaa"),
                        End = dr.GetTimeSpan("Loppuu")
                    });
                }
              
            }
            return väliaikainenBenefit;


        }

        private ObservableCollection<WeeklyBenefit> GetCustomersWeeklyBenefits(int contractID)
        {
            var weeklyBenefitS = new ObservableCollection<WeeklyBenefit>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var sql = "SELECT vl.`LisäID`, `Määrä`, viikonpäivä, Alkaa, Loppuu FROM `viikottainenlisä` vl, `työsopimusviikottainenlisä` tvl WHERE vl.`LisäID` = tvl.`LisäID` AND tvl.SopimusID = @contractID ;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@contractID", contractID);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    weeklyBenefitS.Add(new WeeklyBenefit
                    {         
                        ID = dr.GetInt32("LisäID"),
                        Amount = dr.GetDecimal("Määrä"),
                        dayOfWeek = dr.GetInt32("Viikonpäivä"),
                        Start = dr.GetTimeSpan("Alkaa"),
                        End = dr.GetTimeSpan("Loppuu")
                    });
                }

            }
            return weeklyBenefitS;
        }

        public void AddMonthlySalary(decimal palkka, int contractID)
        {

            var sql = "USE palkanlaskin; UPDATE `työsopimus` SET Kuukausipalkka = @kpalkka , SET Tuntipalkka = 0 WHERE SopimusID = @contractID ;";

            using (var connn = new MySqlConnection(localWithDb))
            {
                connn.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connn);

                cmd.Parameters.AddWithValue("@kpalkka", palkka);
                cmd.Parameters.AddWithValue("@contractID", contractID);

                cmd.ExecuteNonQuery();

            }

        }
        public void AddSalarypPerHour(decimal palkka, int contractID)
        {
            var sql = "USE palkanlaskin; UPDATE `työsopimus` SET Tuntipalkka= @tpalkka WHERE SopimusID = @contractID ; ";

            using (var connn = new MySqlConnection(localWithDb))
            {
                connn.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connn);

                cmd.Parameters.AddWithValue("@tpalkka", palkka);
                cmd.Parameters.AddWithValue("@contractID", contractID);


                cmd.ExecuteNonQuery();

            }
        }






        //Manual Deduduction

        public ManualDeduction CreateAndGetManualDeduction(string name, decimal amount, int ourly_or_once, int contractID)
        {
            bool löytyy = CheckIfManualDeductionExists(name, amount, ourly_or_once);
            ManualDeduction manualDeduction = new ManualDeduction();


            if (!löytyy)
            {
                using (var connn = new MySqlConnection(localWithDb))
                {
                    connn.Open();

                    var sql = $"USE palkanlaskin; INSERT INTO randomdeduction (`Määrä`, Nimi, Tunnittain_Tai_Kerta) VALUES (@määrä, @nimi, @Tunnittain_Tai_Kerta) ;";


                    MySqlCommand cmd = new MySqlCommand(sql, connn);

                    cmd.Parameters.AddWithValue("@määrä", amount);
                    cmd.Parameters.AddWithValue("@nimi", name);
                    cmd.Parameters.AddWithValue("@Tunnittain_Tai_Kerta", ourly_or_once);


                    cmd.ExecuteNonQuery();

                }

                manualDeduction = GetTheRamdomDeduction(name, amount, ourly_or_once);

                AddContractIDAndManualDeductionIdToTable(contractID, manualDeduction.ID);



            }
            else
            {
                manualDeduction = GetTheRamdomDeduction(name, amount, ourly_or_once);

            }

            return manualDeduction;
        }
        private bool CheckIfManualDeductionExists(string name, decimal amount, int ourly_or_once)
        {
            var manualDeduction = new ManualDeduction();
            bool löytyykö = false;  // Declare 'Löytyy'

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Use parameterized query
                var sql = "SELECT `Määrä`, `Nimi`, `Tunnittain_Tai_Kerta` FROM randomdeduction WHERE `Määrä` = @amount AND Nimi = @name AND Tunnittain_Tai_Kerta = @ourly_or_once;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@ourly_or_once", ourly_or_once);

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        manualDeduction = new ManualDeduction
                        {
                            Name = dr.GetString("Nimi"),
                            Amount = dr.GetDecimal("Määrä"),
                            OurlyOrOnce = dr.GetInt32("Tunnittain_Tai_Kerta")
                        };
                        löytyykö = true;  // Found a matching record
                    }
                }

                Debug.WriteLine(manualDeduction.Amount);
            }

            return löytyykö;
        }

        public bool CheckIfManualDeductionExists(int contractID)
        {
            ObservableCollection<MDeduction_SopimusID_LisäID> manualDeduction = new ObservableCollection<MDeduction_SopimusID_LisäID>();
            bool löytyykö = false;  // Declare 'Löytyy'

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Use parameterized query
                var sql = "SELECT * FROM työsopimusrandomdeduction WHERE `sopimusID` = @contractID;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@contractID", contractID);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        manualDeduction.Add(new MDeduction_SopimusID_LisäID
                        {
                            sopimusid = dr.GetInt32("SopimusID"),
                            lisäid = dr.GetInt32("LisäID")
                        });
                    }
                }
            }

            // Check if any record exists
            if (manualDeduction.Count > 0)
            {
                löytyykö = true;
            }

            return löytyykö;
        }



        private ManualDeduction GetTheRamdomDeduction(string name, decimal amount, int ourly_or_once)
        {
            var manualDeduction = new ManualDeduction();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Use parameterized query
                var sql = "SELECT * FROM randomdeduction WHERE `Määrä` = @amount AND Nimi = @name AND Tunnittain_Tai_Kerta = @ourly_or_once;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@ourly_or_once", ourly_or_once);

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        manualDeduction = new ManualDeduction
                        {
                            Name = dr.GetString("Nimi"),
                            Amount = dr.GetDecimal("Määrä"),
                            ID = dr.GetInt32("LisäID"),
                            OurlyOrOnce = dr.GetInt32("Tunnittain_Tai_Kerta")
                        };
                    }
                }

                Debug.WriteLine(manualDeduction.ID);
                Debug.WriteLine(manualDeduction.OurlyOrOnce);

                return manualDeduction;
            }
        }

        private void AddContractIDAndManualDeductionIdToTable(int contractID, int benefitID)
        {
            using (var connn = new MySqlConnection(localWithDb))
            {
                connn.Open();

                var sql = $"INSERT INTO `työsopimusrandomdeduction` (SopimusID, `LisäID`) VALUES (@sopimusid, @benefitID) ;";


                MySqlCommand cmd = new MySqlCommand(sql, connn);

                cmd.Parameters.AddWithValue("@sopimusid", contractID);
                cmd.Parameters.AddWithValue("@benefitID", benefitID);

                cmd.ExecuteNonQuery();

            }
        }
        private ManualDeduction GetTheManualDeduction(int id)
        {
            var manualDeduction = new ManualDeduction();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();


                var sql = $"USE palkanlaskin; SELECT * FROM randomdeduction WHERE LisäID = {id} ;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    manualDeduction = new ManualDeduction
                    {
                        Name = dr.GetString("Nimi"),
                        Amount = dr.GetDecimal("Määrä"),
                        ID = dr.GetInt32("LisäID"),
                        OurlyOrOnce = dr.GetInt32("Tunnittain_Tai_Kerta")

                    };
                }
                Debug.WriteLine(manualDeduction.ID);
                Debug.WriteLine(manualDeduction.Amount);

                return manualDeduction;
            }
        }
        public ObservableCollection<ManualDeduction> GetCustomersManualDeduction(int contractID)
        {
            ObservableCollection<MDeduction_SopimusID_LisäID> mDeduction_ID = new ObservableCollection<MDeduction_SopimusID_LisäID>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var sql = $"USE palkanlaskin; SELECT * FROM työsopimusrandomdeduction WHERE `SopimusID` = {contractID} ;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    mDeduction_ID.Add(new MDeduction_SopimusID_LisäID
                    {
                        lisäid = dr.GetInt32("LisäID")
                    });
                }
            }

            ObservableCollection<ManualDeduction> manualDeduction= new ObservableCollection<ManualDeduction>();

            foreach (var item in mDeduction_ID)
            {
                using (MySqlConnection conn = new MySqlConnection(localWithDb))
                {
                    conn.Open();


                    var sql = $"USE palkanlaskin; SELECT * FROM randomdeduction WHERE `LisäID` = {item.lisäid};";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        manualDeduction.Add(new ManualDeduction
                        {
                            Name = dr.GetString("Nimi"),
                            Amount = dr.GetDecimal("Määrä"),
                            ID = dr.GetInt32("LisäID"),
                            OurlyOrOnce = dr.GetInt32("Tunnittain_Tai_Kerta")

                        });


                    }

                      

                }

            }
            return manualDeduction;




        }
        public void RemoveManualDeduction(int contractID, int benefitID)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("USE palkanlaskin; DELETE FROM `työsopimusrandomdeduction` WHERE SopimusID = @contractid AND `LisäID` = @lisäid ; DELETE FROM randomdeduction WHERE `LisäID` = @lisäid; ", conn);

                cmd.Parameters.AddWithValue("@lisäid", benefitID);
                cmd.Parameters.AddWithValue("@contractid", contractID);


                cmd.ExecuteNonQuery();

            }

        }




        //Add Manual Benefit

        public ManualBenefit CreateAndGetManualBenefit(string name, decimal amount, int ourly_or_once,  int contractID)
        {
            bool löytyy = CheckIfManualBenefitExists(name, amount, ourly_or_once);
            ManualBenefit manualBenefit = new ManualBenefit();

            if (!löytyy)
            {
                using (var connn = new MySqlConnection(localWithDb))
                {
                    connn.Open();

                    var sql = $"USE palkanlaskin; INSERT INTO randombenefit (`Määrä`, Nimi, Tunnittain_Tai_Kerta) VALUES (@määrä, @nimi, @Tunnittain_Tai_Kerta) ;";


                    MySqlCommand cmd = new MySqlCommand(sql, connn);

                    cmd.Parameters.AddWithValue("@määrä", amount);
                    cmd.Parameters.AddWithValue("@nimi", name);
                    cmd.Parameters.AddWithValue("@Tunnittain_Tai_Kerta", ourly_or_once);


                    cmd.ExecuteNonQuery();

                }

                manualBenefit = GetTheManualBenefit(name, amount, ourly_or_once);
                AddContractIDAndManualBenefitIdToTable(contractID, manualBenefit.ID);

            }
            else
            {
                manualBenefit = GetTheManualBenefit(name, amount, ourly_or_once);
            }
            return manualBenefit;

        }
        private bool CheckIfManualBenefitExists(string name, decimal amount, int ourly_or_once)
        {
            var manualBenefit = new ManualBenefit();
            bool löytyykö = false;  // Declare the 'Löytyy' variable

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Remove the USE statement, and use a parameterized query to avoid SQL injection
                var sql = "SELECT `Määrä`, Nimi, Tunnittain_Tai_Kerta FROM randombenefit WHERE `Määrä` = @amount AND Nimi = @name AND Tunnittain_Tai_Kerta = @ourly_or_once;";

                // Create command and add parameters to avoid SQL injection
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@ourly_or_once", ourly_or_once);

                using (var dr = cmd.ExecuteReader())
                {
                    // Read through the results
                    while (dr.Read())
                    {
                        manualBenefit = new ManualBenefit
                        {
                            Name = dr.GetString("Nimi"),
                            Amount = dr.GetDecimal("Määrä"),
                            OurlyOrOnce = dr.GetInt32("Tunnittain_Tai_Kerta")
                        };
                    }
                }


                // Check if the benefit was found
                if (manualBenefit.Name == name)
                {
                    löytyykö = true;
                }
            }

            return löytyykö;
        }


        public bool CheckIfManualBenefitExists(int contractID)
        {
            ObservableCollection<MBenefit_SopimusID_LisäID> manualBenefit = new ObservableCollection<MBenefit_SopimusID_LisäID>();
            bool löytyykö = false;  // Declare 'Löytyy'

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Remove the USE statement and use a parameterized query
                var sql = "SELECT * FROM työsopimusrandombenefit WHERE `sopimusID` = @contractID;";

                // Create command and add parameter
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@contractID", contractID);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        manualBenefit.Add(new MBenefit_SopimusID_LisäID
                        {
                            sopimusid = dr.GetInt32("SopimusID"),
                            lisäid = dr.GetInt32("LisäID")
                        });
                    }
                }
            }

            // Check if any record with the matching contractID exists in the result
            if (manualBenefit.Count > 0)
            {
                löytyykö = true;
            }

            return löytyykö;
        }


        private ManualBenefit GetTheManualBenefit(string name, decimal amount, int ourly_or_once)
        {
            var manualBenefit = new ManualBenefit();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                // Remove the USE statement and use a parameterized query
                var sql = "SELECT * FROM randombenefit WHERE `Määrä` = @amount AND Nimi = @name AND Tunnittain_Tai_Kerta = @ourly_or_once;";

                // Create command and add parameters
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@ourly_or_once", ourly_or_once);

                using (var dr = cmd.ExecuteReader())
                {
                    // If a result is found, populate the ManualBenefit object
                    if (dr.Read())
                    {
                        manualBenefit = new ManualBenefit
                        {
                            Name = dr.GetString("Nimi"),
                            Amount = dr.GetDecimal("Määrä"),
                            ID = dr.GetInt32("LisäID"),
                            OurlyOrOnce = dr.GetInt32("Tunnittain_Tai_Kerta")
                        };
                    }
                }

                // Debugging to verify the ID
                Debug.WriteLine(manualBenefit.ID);

                return manualBenefit;
            }
        }

        private void AddContractIDAndManualBenefitIdToTable(int contractID, int benefitID)
        {
            using (var connn = new MySqlConnection(localWithDb))
            {
                connn.Open();

                var sql = $"INSERT INTO `työsopimusrandombenefit` (SopimusID, `LisäID`) VALUES (@sopimusid, @benefitID) ;";


                MySqlCommand cmd = new MySqlCommand(sql, connn);

                cmd.Parameters.AddWithValue("@sopimusid", contractID);
                cmd.Parameters.AddWithValue("@benefitID", benefitID);

                cmd.ExecuteNonQuery();

            }
        }

        private ManualBenefit GetTheManualBenefit(int id)
        {
            var manualBenefit = new ManualBenefit();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();


                var sql = $"USE palkanlaskin; SELECT * FROM randombenefit WHERE LisäID = {id} ;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    manualBenefit = new ManualBenefit
                    {
                        Name = dr.GetString("Nimi"),
                        Amount = dr.GetDecimal("Määrä"),
                        ID = dr.GetInt32("LisäID"),
                        OurlyOrOnce = dr.GetInt32("Tunnittain_Tai_Kerta")

                    };


                }
                Debug.WriteLine(manualBenefit.ID);

                return manualBenefit;


            }




        }

        public ObservableCollection<ManualBenefit> GetCustomersManualBenefits(int contractID)
        {


            ObservableCollection<MBenefit_SopimusID_LisäID> mBenefitsContractIDLisäID = new ObservableCollection<MBenefit_SopimusID_LisäID>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                var sql = $"USE palkanlaskin; SELECT * FROM työsopimusrandombenefit WHERE `SopimusID` = {contractID} ;";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    mBenefitsContractIDLisäID.Add(new MBenefit_SopimusID_LisäID
                    {
                        lisäid = dr.GetInt32("LisäID")
                    });
                }
            }

            ObservableCollection<ManualBenefit> manualBenefits = new ObservableCollection<ManualBenefit>();

            foreach (var item in mBenefitsContractIDLisäID)
            {
                using (MySqlConnection conn = new MySqlConnection(localWithDb))
                {
                    conn.Open();


                    var sql = $"USE palkanlaskin; SELECT * FROM randombenefit WHERE `LisäID` = {item.lisäid};";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        manualBenefits.Add(new ManualBenefit
                        {
                            Name = dr.GetString("Nimi"),
                            Amount = dr.GetDecimal("Määrä"),
                            ID = dr.GetInt32("LisäID"),
                            OurlyOrOnce = dr.GetInt32("Tunnittain_Tai_Kerta")

                        });


                    }



                }

            }
            return manualBenefits;  
            

        }

        public void RemoveManualBenefit(int contractID, int benefitID)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("USE palkanlaskin; DELETE FROM `työsopimusrandombenefit` WHERE SopimusID = @contractid AND `LisäID` = @lisäid ; DELETE FROM randombenefit WHERE `LisäID` = @lisäid;", conn);

                cmd.Parameters.AddWithValue("@lisäid", benefitID);
                cmd.Parameters.AddWithValue("@contractid", contractID);


                cmd.ExecuteNonQuery();

            }

        }







      
    }

}
