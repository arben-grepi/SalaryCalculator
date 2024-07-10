using Palkanlaskin2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WPFCustomMessageBox;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Palkanlaskin2
{

    public class ShiftInfo
    {
        public DateOnly Date { get; set; }
        public int Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public Decimal WeekinfoTime { get; set; }
        public Decimal MonthinfoTime { get; set; }
        public string stringStart
        {
            get
            {
                var start = "";

                var time = Start.ToString();
                var parts = time.Split(':');
                start = parts[0] + ":" + parts[1];

                return start;



            }
        }
        public string stringEnd
        {
            get
            {
                var end = "";

                var time = End.ToString();

                if (time == "1.00:00:00")
                {
                    end = "24:00";
                    return end;
                }
                else
                {
                    var parts = time.Split(':');

                    end = parts[0] + ":" + parts[1];

                    return end;

                }





            }
        }
        public decimal OvertimeShiftAmount { get; set; }
        public decimal OvertimeWeekAmount { get; set; }
        public decimal OvertimeMonthAmount { get; set; }
        public decimal WeeklyBenefitAmount { get; set; }
        public decimal BenefitOnADateAmount { get; set; }
        public decimal ManualBenefitAmount { get; set; }
        public decimal ManualDeductionAmount { get; set; }
        public decimal Salary { get; set; }
        public ObservableCollection<OvertimeBenefit> Overtimebenefits { get; set; }
        public ObservableCollection<WeeklyBenefit> WeeklyBenefitList { get; set; }
        public ObservableCollection<BenefitOnADate> BenefitOnADateList { get; set; }
        public ObservableCollection<ManualDeduction> manualDeductions { get; set; }
        public ObservableCollection<ManualBenefit> manualBenefits { get; set; }

        public OvertimeBenefit OBenefitRepo { get; set; }


        public Decimal Difference 
        {
            get
            {
                decimal value = 0;
                TimeSpan d = new TimeSpan(0, 0, 0);
                TimeSpan midnight = new TimeSpan(24, 0, 0);

                if (End < Start)
                {
                    d = (midnight - Start) + End;
                }
                else
                {
                    d = End - Start;
                }
                decimal hours = 0;
                bool ok = decimal.TryParse(d.Hours.ToString(), out hours);
                if (!ok)
                {
                    return 0;
                }
                value = hours;

                decimal minutes = 0;
                ok = decimal.TryParse(d.Minutes.ToString(), out minutes);
                if (!ok)
                {
                    return 0;
                }
                value += minutes / 60;
                return value;

            }
        }
        public ShiftInfo()
        {
            Date = new DateOnly();
            Day = 0;
            Start = new TimeSpan(0, 0, 0);
            End = new TimeSpan(0, 0, 0);
            Overtimebenefits = new ObservableCollection<OvertimeBenefit>();
            WeeklyBenefitList = new ObservableCollection<WeeklyBenefit>();
            BenefitOnADateList = new ObservableCollection<BenefitOnADate>();
            manualBenefits = new ObservableCollection<ManualBenefit>();
            manualDeductions = new ObservableCollection<ManualDeduction>();
            OBenefitRepo = new OvertimeBenefit();
            WeekinfoTime = 0;
            Salary = 0;

            MonthinfoTime = 0;


            OvertimeShiftAmount = 0;
            OvertimeWeekAmount = 0;
            OvertimeMonthAmount = 0;

            OvertimeWeekAmount = 0;

            ManualBenefitAmount = 0;
            ManualDeductionAmount = 0;

            WeeklyBenefitAmount = 0;
            BenefitOnADateAmount = 0;
        }



        public decimal GetWeeklyBenefitAmount(TimeSpan start, TimeSpan end, decimal amount)
        {
            var weeklyBenefit = new WeeklyBenefit();

            decimal difference = weeklyBenefit.GetDifference(start, end);

            decimal value = difference * amount;
            return value;

        }
        public decimal GetBenefitOnADateAmount(TimeSpan start, TimeSpan end, decimal amount)
        {
            var benefitAmount = new BenefitOnADate();

            decimal difference = benefitAmount.GetDifference(start, end);

            decimal value = difference * amount;
            return value;

        }

        public override string ToString()
        {
          
            decimal benefitValue = 0;
            decimal deductionValue = 0;

            var message = $"\n{Date.DayOfWeek} {Date},  From: {stringStart} / {stringEnd}";

            if (WeeklyBenefitAmount > 0)
            {

                foreach (var item in WeeklyBenefitList)
                {
                    Debug.WriteLine(item.ID);

                    decimal decimalbenefitperHourAmount = Math.Round(item.Amount, 2);
                    decimal decimalDifference = Math.Round((decimal)item.Difference, 2);

                    var doubleResult = GetWeeklyBenefitAmount(item.Start, item.End, item.Amount);
                    decimal decimalResult = Math.Round(doubleResult, 2);

                    var W_benefit = $"\n{item.stringDay} benefit";
                    if (WeeklyBenefitList.Count > 1)
                    {
                        W_benefit += $"(ID:{item.ID})";
                    }
                    W_benefit += $" + {decimalResult} ({decimalDifference}h * {decimalbenefitperHourAmount}. From: {item.stringStart}-{item.stringEnd}) ";

                    message += W_benefit;


                }


            }
            if (BenefitOnADateAmount > 0)
            {
                
                foreach (var item in BenefitOnADateList)
                {
                    var W_benefit = "";

                    Debug.WriteLine($"{item}");
                    decimal decimalbenefitperHourAmount = Math.Round(item.Amount, 2);

                    decimal decimalDifference = Math.Round((decimal)item.Difference, 2);

                    var doubleResult = GetBenefitOnADateAmount(item.Start, item.End, item.Amount);
                    decimal decimalResult = Math.Round(doubleResult, 2);

                    W_benefit += $"\nBenfit on the date";
                    if (BenefitOnADateList.Count > 1)
                    {
                        W_benefit += $"(ID:{item.ID})";

                    }
                    W_benefit += $" + {decimalResult} ({decimalDifference}h * {decimalbenefitperHourAmount}. From {item.stringStart}-{item.stringEnd}). ";
                    message += W_benefit;
                }
              


            }
            int shiftlaskuri = 0;
            int weeklaskuri = 0;
            int monthlaskuri = 0;

            foreach (var item in Overtimebenefits)
            {
                if (OvertimeShiftAmount > 0 && item.TimePeriod == 0)
                {
                   
                    if (item.ResultShiftAmount > 0)
                    {
                        shiftlaskuri++;

                    }

                }
                else if (OvertimeWeekAmount > 0 && item.TimePeriod == 1)
                {
                    if (item.ResultWeekAmount > 0)
                    {
                        weeklaskuri++;


                    }
                }
                else if (OvertimeMonthAmount > 0 && item.TimePeriod == 2)
                {
                    if (item.ResultMonthAmount > 0)
                    {
                        monthlaskuri++;

                    }

                }

            } // lasketaan kuinka monta on kutakin 
            if (OvertimeShiftAmount > 0)
            {
                foreach (var item in Overtimebenefits)
                {

                    if (item.TimePeriod == 0)
                    {
                        var result = item.ResultShiftAmount;
                        var resultAmount = Math.Round(result, 2);

                        var difference  = result / item.BenefitAmount;
                        var decimalDifference = Math.Round(difference, 2);

                        var decimalbenefitperHourAmount = Math.Round(item.BenefitAmount, 2);

                        var overtimestring = $"\nOvertime for the day";
                        if (shiftlaskuri > 1)
                        {
                            overtimestring += $"ID({item.ID})";
                        }
                        overtimestring += $" + {resultAmount} ({decimalDifference}h * {decimalbenefitperHourAmount}) ";

                        message += overtimestring;

                    }




                }

            
                  

            }
            if (OvertimeWeekAmount > 0)
            {
                foreach (var item in Overtimebenefits)
                {
                    if (item.TimePeriod == 1)
                    {

                        decimal resultAmount = item.BenefitAmount * WeekinfoTime;

                        var decimalDifference = Math.Round(WeekinfoTime, 2); 

                        var decimalbenefitperHourAmount = Math.Round(item.BenefitAmount, 2);

                        var overtimestring = $"\nOvertime for the week";
                        if (shiftlaskuri > 1)
                        {
                            overtimestring += $"ID({item.ID})";
                        }
                        overtimestring += $" + {resultAmount} ({decimalDifference}h * {decimalbenefitperHourAmount}) ";

                        message += overtimestring;


                    }


                }
            }
            if (OvertimeMonthAmount > 0)
            {
                foreach (var item in Overtimebenefits)
                {
                    if (item.TimePeriod == 2)
                    {
                        decimal resultAmount = item.BenefitAmount * MonthinfoTime;

                        var decimalDifference = Math.Round(MonthinfoTime, 2);

                        var decimalbenefitperHourAmount = Math.Round(item.BenefitAmount, 2);

                        var overtimestring = $"\nOvertime for the month";
                        if (shiftlaskuri > 1)
                        {
                            overtimestring += $"ID({item.ID})";
                        }
                        overtimestring += $" + {resultAmount} ({decimalDifference}h * {decimalbenefitperHourAmount}) ";

                        message += overtimestring;

                    }

                }
            }

            if (ManualBenefitAmount > 0)
            {
                foreach (var item in manualBenefits)
                {

                    var O_benefit = $"\n{item.Name}";

                    if (item.OurlyOrOnce == 0)
                    {
                        decimal doubleValue = Difference * item.Amount;
                        decimal decimalvalue = Math.Round(doubleValue, 2);

                        decimal decimalDifferenced = Math.Round((decimal)Difference, 2);


                        decimal decimalAmount = Math.Round((decimal)item.Amount, 2);


                        O_benefit += $" + {decimalvalue} ({decimalDifferenced}h * {decimalAmount}) ";

                    }
                    else if (item.OurlyOrOnce == 1)
                    {
                        decimal value = Math.Round(item.Amount, 2);

                        O_benefit += $" + {value}";
                    }

                    message += O_benefit;

                }

                


            }
            if (ManualDeductionAmount > 0)
            {
                foreach (var item in manualDeductions)
                {

                    var O_benefit = $"\n{item.Name}";

                    if (item.OurlyOrOnce == 0)
                    {
                        decimal doubleValue = Difference * item.Amount;

                        decimal decimalvalue = Math.Round(doubleValue, 2);

                        decimal decimalDifferenced = Math.Round(Difference, 2);

                        decimal decimalAmount = Math.Round(item.Amount, 2);

                        O_benefit += $" - {decimalvalue} ({decimalDifferenced}h * {decimalAmount}) ";

                    }
                    else if (item.OurlyOrOnce == 1)
                    {
                        decimal value = Math.Round(item.Amount, 2);

                        O_benefit += $" - {value}";
                    }

                    message += O_benefit;
                }
            }

            if (WeeklyBenefitAmount == 0 && BenefitOnADateAmount == 0 && OvertimeShiftAmount == 0 && OvertimeWeekAmount == 0 && OvertimeMonthAmount == 0 && ManualBenefitAmount == 0 && ManualDeductionAmount == 0 )
            {
                message += "";
            }
            else
            {
                message += "\n";
            }


            benefitValue = OvertimeShiftAmount + OvertimeWeekAmount + OvertimeMonthAmount + WeeklyBenefitAmount + BenefitOnADateAmount + ManualBenefitAmount;
            deductionValue = ManualDeductionAmount;


            var decimalBenefitValue = Math.Round(benefitValue, 2);
            var decimaldeductionValue = Math.Round(deductionValue, 2);
            var decimalSalaryValue = Math.Round(Salary, 2);

            decimal allTogether = Salary + benefitValue - deductionValue;
            decimal decimalAllTogehther = Math.Round(allTogether, 2);

            if (decimaldeductionValue > 0 && decimalBenefitValue > 0)
            {
                message += $"Salary: (Hourly salary){decimalSalaryValue} + (Benefits){decimalBenefitValue} - (Deductions){decimaldeductionValue} = {decimalAllTogehther}";

            }
            else if (decimaldeductionValue > 0 && decimalBenefitValue == 0)
            {
                message += $"Salary: (Hourly salary){decimalSalaryValue} - (Deductions){decimaldeductionValue} = {decimalAllTogehther}";

            }
            else if (decimalBenefitValue > 0 && decimaldeductionValue == 0)
            {
                message += $"Salary: (Hourly salary){decimalSalaryValue} + (Benefits){decimalBenefitValue} = {decimalAllTogehther}";

            }
            else if (decimalBenefitValue == 0 && decimaldeductionValue == 0)
            {
                message += $"\nSalary: {decimalAllTogehther}";


            }
            message += "\n";

            return message;


        }

    }
    public class Month
    {
        public TimeSpan Difference { get; set; }
        public Month()
        {
            Difference = new TimeSpan(0, 0, 0);
        }
    }
    public class Week
    {
        public TimeSpan Difference { get; set; }
        public Week()
        { Difference = new TimeSpan(0, 0, 0); }
    }
    public class MonthInfo
    {
        public int Year { get; set; }
        public int IntMonth { get; set; }

        public List<Month> Months { get; set; }
        public TimeSpan Time
        {
            get
            {
                TimeSpan timeSpan = new TimeSpan();

                foreach (Month month in Months)
                {
                    timeSpan += month.Difference;

                }
                return timeSpan;
            }
        }
        public MonthInfo() { Months = new List<Month>(); Year = 0; IntMonth = 0; }



    }
    public class WeekInfo
    {
        public int Year { get; set; }
        public int IntMonth { get; set; }
        public int DayOfYear { get; set; }
        public List<Week> Weeks { get; set; }
        public TimeSpan Time
        {
            get
            {
                TimeSpan timeSpan = new TimeSpan();

                foreach (Week week in Weeks)
                {
                    timeSpan += week.Difference;

                }
                return timeSpan;

            }
        }
        public WeekInfo() { Weeks = new List<Week>(); Year = 0; IntMonth = 0; DayOfYear = 0; }
    }
    public class OvertimeBenefit
    {
        public List<MonthInfo> MonthsList { get; set; }
        public List<WeekInfo> WeekList { get; set; }

      
        public int ID { get; set; }
        public int TimePeriod { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan TimeSurpassed { get; set; }
        public string StringTime
        {
            get
            {
                var stringTime = TurnTimeSpanintoString(Time);

                var parts = stringTime.Split(':');
                stringTime = parts[0] + ":" + parts[1];

                return stringTime;
            }
        }
        public decimal BenefitAmount { get; set; }
        public string OvertimeInfo
        {
            get
            {
                decimal amount = 0;
                if (BenefitAmount > 0)
                {
                    amount = Math.Round(BenefitAmount, 2);


                }
                else
                {
                    amount = Math.Round((decimal)BenefitAmount, 2);

                }
                var aikaväli = "";

                if (TimePeriod == 0)
                {
                    aikaväli = "shift.";

                }
                else if (TimePeriod == 1)
                {
                    aikaväli = "week.";
                }
                else if (TimePeriod == 2)
                {
                    aikaväli = "month.";
                }



                return $"Overtimebenefit {amount} per hour, after {StringTime} hours of work in a {aikaväli} ";

            }
        }

        public decimal ResultWeekAmount { get; set; }
        public decimal ResultMonthAmount { get; set; }
        public decimal ResultShiftAmount { get ; set; }

        public OvertimeBenefit()
        {

            MonthsList = new List<MonthInfo>();
            WeekList = new List<WeekInfo>();

            ResultWeekAmount = 0;
            ResultMonthAmount = 0;
            ResultShiftAmount = 0;

            ID = 0;
            BenefitAmount = 0;
            TimePeriod = -1;
            Time = TimeSpan.Zero;
            TimeSurpassed = TimeSpan.Zero;

        }

        public Decimal returnWeekInfoTime(OvertimeBenefit overtimeBenefit, DateOnly originalDate, TimeSpan start, TimeSpan end)
        {

            DateOnly shiftMonday = new DateOnly();

            if (originalDate.DayOfWeek == DayOfWeek.Monday)
            {
                shiftMonday = originalDate;

            }
            else
            {

                while (originalDate.DayOfWeek != DayOfWeek.Monday)
                {
                    originalDate = originalDate.AddDays(-1);
                }

                shiftMonday = originalDate;

            }

            var dif = GetDifference(start, end);
            var shiftDifference = TurnTimeSpanIntoDouble(dif);

            decimal value = 0;

            foreach (var weekInfo in overtimeBenefit.WeekList) // tässä katsotaan mihin MonthListan MonthInfoon viimeksi lisätty vuoro kuuluu. Katsotaan kuinka paljon ne vuorot on ajallisesti yhteensä                                           // sekä verrataan sitä Overtime.Timeen, jolloin tiedetään onko se mennyt yli vai ei. 
            {

                if (weekInfo.Year == shiftMonday.Year && weekInfo.IntMonth == shiftMonday.Month && weekInfo.DayOfYear == shiftMonday.DayOfYear)
                {
                    var weekInfoTime = TurnTimeSpanIntoDouble(weekInfo.Time);
                    var overtimeBenefitTime = TurnTimeSpanIntoDouble(overtimeBenefit.Time);
                    if (weekInfoTime > overtimeBenefitTime)
                    {
                        var time = weekInfoTime - overtimeBenefitTime;

                        if (time > shiftDifference)
                        {

                           return shiftDifference;
                        }
                        else
                        {
                            return time;
                        }
                    }

                }

            }

            return 0;
        }
        public Decimal returnMonthInfoTime(OvertimeBenefit overtimeBenefit, DateOnly date, TimeSpan start, TimeSpan end)
        {
          
            var month = new Month();
            month.Difference = GetDifference(start, end);

            var dif = GetDifference(start, end);
            var shiftDifference = TurnTimeSpanIntoDouble(dif);

            foreach (var monthInfo in overtimeBenefit.MonthsList)
            {
                if (monthInfo.Year == date.Year && monthInfo.IntMonth == date.Month)
                {
                    var monthInfoTime = TurnTimeSpanIntoDouble(monthInfo.Time);
                    var overtimeBenefitTime = TurnTimeSpanIntoDouble(overtimeBenefit.Time);
                    if (monthInfoTime > overtimeBenefitTime)
                    {
                        var time = monthInfoTime - overtimeBenefitTime;

                        if (time > shiftDifference)
                        {

                            return shiftDifference;
                        }
                        else
                        {
                            return time;
                        }
                    }

                }

                

            }



            // tähän saakka  on pitänyt lisätä listaan uusi Month
            return 0;

        }
        public OvertimeBenefit AddnewWeek(OvertimeBenefit overtimeB, DateOnly originalDate, TimeSpan start, TimeSpan end)
        {

            var week = new Week();
            week.Difference = GetDifference(start, end);
            var weekInfo = new WeekInfo();



            DateOnly shiftMonday = new DateOnly();

            if (originalDate.DayOfWeek == DayOfWeek.Monday)
            {
                shiftMonday = originalDate;

            }
            else
            {

                while (originalDate.DayOfWeek != DayOfWeek.Monday)
                {
                    originalDate = originalDate.AddDays(-1);
                }

                shiftMonday = originalDate;

            }



            if (overtimeB.WeekList.Count == 0)
            {
                weekInfo.Weeks.Add(week); weekInfo.Year = shiftMonday.Year; weekInfo.IntMonth = shiftMonday.Month; weekInfo.DayOfYear = shiftMonday.DayOfYear;
                overtimeB.WeekList.Add(weekInfo);

            }
            else
            {
                bool löytyi = false;
                foreach (var winfo in overtimeB.WeekList)
                {
                    if (winfo.Year == shiftMonday.Year && winfo.IntMonth == shiftMonday.Month && winfo.DayOfYear == shiftMonday.DayOfYear)
                    {
                        winfo.Weeks.Add(week);
                        löytyi = true; break;
                    }

                }
                if (löytyi == false)
                {
                    weekInfo.Weeks.Add(week); weekInfo.Year = originalDate.Year; weekInfo.IntMonth = originalDate.Month; weekInfo.DayOfYear = shiftMonday.DayOfYear;
                    overtimeB.WeekList.Add(weekInfo);

                }

            }

            // tähän saakka  on pitänyt lisätä listaan uusi Month

          
            return overtimeB;


        }
        public List<WeekInfo> RemoveWeek(List<WeekInfo> weeklist, DateOnly originalDate, TimeSpan start, TimeSpan end)
        {

            var week = new Week();
            var Difference = GetDifference(start, end);


            DateOnly shiftMonday = new DateOnly();

            if (originalDate.DayOfWeek == DayOfWeek.Monday)
            {
                shiftMonday = originalDate;

            }
            else
            {

                while (originalDate.DayOfWeek != DayOfWeek.Monday)
                {
                    originalDate = originalDate.AddDays(-1);
                }

                shiftMonday = originalDate;

            }


            bool löytyi = false;
            for (int i = 0; i < weeklist.Count; i++)
            {
                if (weeklist[i].Year == shiftMonday.Year && weeklist[i].IntMonth == shiftMonday.Month && weeklist[i].DayOfYear == shiftMonday.DayOfYear)
                {
                    for (int a = 0; a < weeklist[i].Weeks.Count; a++)
                    {
                        if (weeklist[i].Weeks[a].Difference == Difference)
                        {
                            weeklist[i].Weeks.Remove(weeklist[i].Weeks[a]);
                            löytyi = true;
                            break;
                        }
                    }
                    if (löytyi)
                    {
                        break;
                    }

                }
                if (löytyi)
                {
                    break;
                }
            }

          



            // tähän saakka  on pitänyt lisätä listaan uusi Month


            return weeklist;


        }
        public List<MonthInfo> RemoveMonth(List<MonthInfo> monthInfos, DateOnly date, TimeSpan start, TimeSpan end)
        {


            var month = new Month();
            month.Difference = GetDifference(start, end);

            bool löytyi = false;

            for (int i = 0; i < monthInfos.Count; i++)
            {
                if (monthInfos[i].Year == date.Year && monthInfos[i].IntMonth == date.Month)
                {
                    for (int a = 0; a < monthInfos[i].Months.Count; a++)
                    {
                        if (monthInfos[i].Months[a].Difference == month.Difference)
                        {
                            monthInfos[i].Months.Remove(monthInfos[i].Months[a]);
                            löytyi = true;
                            break;
                        }
                    }
                    if (löytyi)
                    {
                        break;
                    }

                }
                if (löytyi)
                {
                    break;

                }
            }

            return monthInfos;


        }
        public OvertimeBenefit AddnewMonth(OvertimeBenefit overtimeBenefit, DateOnly date, TimeSpan start, TimeSpan end)
        {
            var month = new Month();
            month.Difference = GetDifference(start, end);



            if (MonthsList.Count == 0)
            {
                var monthInfo = new MonthInfo();
                monthInfo.Months.Add(month); monthInfo.Year = date.Year; monthInfo.IntMonth = date.Month;
                overtimeBenefit.MonthsList.Add(monthInfo);

            }
            else
            {
                bool löytyi = false;
                foreach (var monthInfo in overtimeBenefit.MonthsList)
                {
                    if (monthInfo.Year == date.Year && monthInfo.IntMonth == date.Month)
                    {
                        monthInfo.Months.Add(month);
                        löytyi = true; break;
                    }

                }
                if (löytyi == false)
                {
                    var monthInfo = new MonthInfo();
                    monthInfo.Months.Add(month); monthInfo.Year = date.Year; monthInfo.IntMonth = date.Month;
                    overtimeBenefit.MonthsList.Add(monthInfo);



                }


            }

            
            // tähän saakka  on pitänyt lisätä listaan uusi Month
            return overtimeBenefit;













        }
        public decimal ReturnPossibleMonthOvertimeBenefit(OvertimeBenefit overtimeBenefit, DateOnly date, TimeSpan start, TimeSpan end)
        {
            decimal value = 0;

            var month = new Month();
            month.Difference = GetDifference(start, end);

            // tähän saakka  on pitänyt lisätä listaan uusi Month

            foreach (var monthInfo in overtimeBenefit.MonthsList) // tässä katsotaan mihin MonthListan MonthInfoon viimeksi lisätty vuoro kuuluu. Katsotaan kuinka paljon ne vuorot on ajallisesti yhteensä
                // sekä verrataan sitä Overtime.Timeen, jolloin tiedetään onko se mennyt yli vai ei. 
            {
                if (monthInfo.Year == date.Year && monthInfo.IntMonth == date.Month)
                {
                    if (monthInfo.Time > Time)
                    {
                        // niin tällöin ollaan ylityötunneilla
                        TimeSpan difference = monthInfo.Time.Subtract(Time);
                        if (difference < month.Difference)
                        {
                            decimal time = TurnTimeSpanIntoDouble(difference);
                            value = time * BenefitAmount;
                            return value;

                        }
                        else
                        {

                            decimal time = TurnTimeSpanIntoDouble(month.Difference);
                            value = time * BenefitAmount;
                            return value;

                        }
                    }
                    else
                    {
                        return value;
                    }
                }

            }

            return value;
        }
        public OvertimeBenefit returnPossibleShiftsOvertimeBenefits(OvertimeBenefit overtimeB, TimeSpan start, TimeSpan end)
        {

            decimal doubleDifference = 0;


            TimeSpan shiftdifference = GetDifference(start, end);
            if (shiftdifference > overtimeB.Time)
            {
                decimal doubleTimeSpanifference = 0;
                decimal doubleTime = 0;                      


                doubleTimeSpanifference = TurnTimeSpanIntoDouble(shiftdifference);
                doubleTime = TurnTimeSpanIntoDouble(overtimeB.Time);
                doubleDifference = doubleTimeSpanifference - doubleTime;

            }
            overtimeB.ResultShiftAmount = doubleDifference * overtimeB.BenefitAmount;
            return overtimeB;





        }
        public TimeSpan GetDifference(TimeSpan start, TimeSpan end)
        {
            TimeSpan difference = new TimeSpan(0, 0, 0);
            if (start == difference)
            {
                difference = end;
                return difference;
            }

            TimeSpan midnight = new TimeSpan(24, 0, 0);

            if (end < start)
            {
                difference = (midnight - start) + end;
            }
            else
            {
                difference = end - start;
            }
            return difference;
        }
        public decimal TurnTimeSpanIntoDouble(TimeSpan timeSpan)
        {
            decimal value = 0;

            if (timeSpan.ToString().Contains('.'))
            {

                var parts = timeSpan.ToString().Split('.');
                var days = parts[0];
                
                decimal decimalDays = 0;
                bool ok = decimal.TryParse(days, out decimalDays);
                if (!ok)
                {
                    return 0;
                }
                value += 24 * decimalDays;

                var parts2 = parts[1].Split(":");
                var hours = parts2[0];
                var minutes = parts2[1];


                decimal decimalHours = 0;
                ok = decimal.TryParse(hours, out decimalHours);
                if (!ok)
                {
                    return 0;
                }
                value += decimalHours;


                decimal decimalMinutes = 0;
                ok = decimal.TryParse(minutes, out decimalMinutes);
                if (!ok)
                {
                    return 0;
                }
                value += decimalMinutes / 60;

                return value;



            }
            else
            {
                var parts = timeSpan.ToString().Split(':');
                var hours = parts[0];
                var minutes = parts[1];


                decimal decimalHours = 0;
                bool ok = decimal.TryParse(hours, out decimalHours);
                if (!ok)
                {
                    return 0;
                }
                value += decimalHours;


                decimal decimalMinutes = 0;
                ok = decimal.TryParse(minutes, out decimalMinutes);
                if (!ok)
                {
                    return 0;
                }
                value += decimalMinutes / 60;

                return value;

            }
           



        }
        public string TurnTimeSpanintoString(TimeSpan timeSpan)
        {
            string time = "";

            double doubleHours = 0;
            string hours = "";

            double doubleDay = 0;
            string day = "";


            Debug.WriteLine("\n" + timeSpan);
            double value = 0;
            string stringTime = timeSpan.ToString();
            var parts = stringTime.Split(':');

            string min = parts[2];



            if (parts[0].Contains('.'))
            {
                var dayHours = parts[0].Split('.');
                day = dayHours[0];
                hours = dayHours[1];

                bool ok1 = double.TryParse(hours, out doubleHours);
                bool ok = double.TryParse(day, out doubleDay);
                if (ok && ok1)
                {
                    for (int i = 0; i < doubleDay; i++)
                    {
                        doubleHours += 24;
                    }

                }

                time = doubleHours.ToString() + ":" + min + ":00";
                Debug.WriteLine("\n" + time);

            }
            else
            {
                hours = parts[0];


                time = hours + ":" + min + ":00";
                Debug.WriteLine("\n" + time);

            }

            return time;

        }

       
    }
    public class MBenefit_SopimusID_LisäID
    {
        public int lisäid { get; set; }
        public int sopimusid { get; set; }
        public MBenefit_SopimusID_LisäID()
        {
            lisäid = 0;
            sopimusid = 0;

        }
    }
    public class MDeduction_SopimusID_LisäID
    {
        public int lisäid { get; set; }
        public int sopimusid { get; set; }
        public MDeduction_SopimusID_LisäID()
        {
            lisäid = 0;
            sopimusid = 0;

        }
    }
    public class Customer
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Job Job { get; set; }
        public Customer()
        {
            Id = 0;
            Username = string.Empty;
            Password = string.Empty;
            Job = new Job();
        }

        public override string ToString()
        {
            var info = $"\n\n\nCustomer info = ID:{Id}, Username: {Username}, Password: {Password}";

            info += "\nJOB: " + Job.ToString();
            info += "\nBenefit: ";

            for (int i = 0; i < Job.BenefitsOnADate.Count; i++)
            {
                info += Job.BenefitsOnADate[i].ToString();

            }
            info += "\nWeeklyBenefits: ";

            for (int i = 0; i < Job.WeeklyBenefits.Count; i++)
            {
                info += Job.WeeklyBenefits[i].ToString();

            }

            info += "\n\n\n";
            return info;

        }

    }
    public class Job
    {
        public int ContractID { get; set; }
        public int CustomerID { get; set; }
        public string? Name { get; set; }
        public decimal SalaryPerHour { get; set; }
        public ObservableCollection<OvertimeBenefit> Overtimebenefits { get; set; }
        public ObservableCollection<BenefitOnADate> BenefitsOnADate { get; set; }
        public ObservableCollection<WeeklyBenefit> WeeklyBenefits { get; set; }
        public ObservableCollection<ManualDeduction> MDeductions { get; set; }
        public ObservableCollection<ManualBenefit> MBenefits { get; set; }
        public Job()
        {
            ContractID = 0;
            CustomerID = 0;
            Name = string.Empty;
            Overtimebenefits = new ObservableCollection<OvertimeBenefit>();
            BenefitsOnADate = new ObservableCollection<BenefitOnADate>();
            WeeklyBenefits = new ObservableCollection<WeeklyBenefit>();
            MDeductions = new ObservableCollection<ManualDeduction>();
            MBenefits = new ObservableCollection<ManualBenefit>();

            SalaryPerHour = 0;
        }
        public override string ToString()
        {
            var message = $"ContractID:{ContractID}, CustomerID:{CustomerID} Name:{Name}\nSalary: p.hour {SalaryPerHour}";
            return message;
        }
    }
    public class ManualDeduction
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int OurlyOrOnce { get; set; }
        public string Info
        {
            get
            {
                decimal amount = 0;
                amount = Math.Round(Amount, 2);
                var message = $"Name: {Name}, Amount: {amount}";
                if (OurlyOrOnce == 0)
                {
                    message += " Deducted hourly";

                }
                else if (OurlyOrOnce == 1)
                {
                    message += " Deducted once";

                }
                return message;

            }
        }

        public ManualDeduction()
        {
            Amount = 0;
            Name = string.Empty;
            ID = 0;
            OurlyOrOnce = -1;
        }
    }
    public class ManualBenefit
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public int OurlyOrOnce { get; set; }
        public string Info
        {
            get
            {
                decimal amount = 0;
                amount = Math.Round(Amount, 2);

                var message = $"Name: {Name}, Amount: {amount}";
                if (OurlyOrOnce == 0)
                {
                    message += " Paid hourly";

                }
                else if (OurlyOrOnce == 1)
                {
                    message += " Paid once";

                }
                return message;

            }
        }
        public ManualBenefit()
        {
            Amount = 0;
            Name = string.Empty;
            ID = 0;
            OurlyOrOnce = -1;
        }


    }
    public class BenefitOnADate
    {
        public int ID { get; set; }
        public DateOnly Date { get; set; }

        public decimal Difference 
        { 
            get 
            {
                decimal value = GetDifference(Start, End);
                return value;
            }
        }

        public string stringStart
        {
            get
            {
                var start = "";

                var time = Start.ToString();
                var parts = time.Split(':');
                start = parts[0] + ":" + parts[1];

                return start;



            }
        }
        public string stringEnd
        {
            get
            {
                var end = "";

                var time = End.ToString();

                if (time == "1.00:00:00")
                {
                    end = "24:00";
                    return end;
                }
                else
                {
                    var parts = time.Split(':');

                    end = parts[0] + ":" + parts[1];

                    return end;

                }





            }
        }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public decimal Amount { get; set; }
        public string BenefitInfo
        {
            get
            {
                decimal amount = 0;
                amount = Math.Round(Amount, 2);

               

                return $"Date: {Date}, Amount: {amount} Time: {stringStart} - {stringEnd}";

            }
        }
        public BenefitOnADate()
        {
            ID = 0;
            Date = new DateOnly();
            Start = TimeSpan.Zero;
            End = TimeSpan.Zero;
            Amount = 0;

        }


        public override string ToString()
        {
            return $"\nDate:{Date}/Amount:{Amount}/S-Start:{stringStart}/S-End:{stringEnd}/Start{Start}/End:{End}";
        }

        public string GetTheBenefitOnADate(TimeSpan benefitStart, TimeSpan benefitEnd)
        {
            var StringStart = "";
            var StringEnd = "";

            decimal difference = GetDifference(benefitStart, benefitEnd);

            decimal amount = difference * Amount;

            decimal decimalAmount = Math.Round(amount, 2);


            var time = benefitStart.ToString();
            Debug.WriteLine(time);
            var parts = time.Split(':');
            StringStart = parts[0] + ":" + parts[1];


            time = benefitEnd.ToString();
            if (time == "1.00:00:00")
            {
                StringEnd = "24:00";
            }
            else
            {
                parts = time.Split(':');

                StringEnd = parts[0] + ":" + parts[1];

            }


            return $"Amount: {decimalAmount} (Paid from: {StringStart}-{StringEnd})";


        }
        public decimal GetDifference(TimeSpan timeStart, TimeSpan timeEnd)
        {
            decimal value = 0;
            TimeSpan difference = End.Subtract(Start);
            TimeSpan midnight = new TimeSpan(24, 0, 0);

            if (timeEnd < timeStart)
            {
                difference = (midnight - timeStart) + timeEnd;
            }
            else
            {
                difference = timeEnd - timeStart;
            }
            decimal hours = 0;
            bool ok = decimal.TryParse(difference.Hours.ToString(), out hours);
            if (!ok)
            {
                return 0;
            }  
            value = hours;

            decimal minutes = 0;
            ok = decimal.TryParse(difference.Minutes.ToString(), out minutes);
            if (!ok)
            {
                return 0;
            }
            value += minutes / 60;
            return value;
        }
    }
    public class WeeklyBenefit
    {
        public int ID { get; set; }
        public int dayOfWeek { get; set; }
        public decimal Difference
        {
            get
            {
                decimal value = GetDifference(Start, End);
                return value;
            }
        }
        public string stringDay
        {
            get
            {
                var day = "";


                if (dayOfWeek == 1)
                {
                    day = "Monday";
                }
                else if (dayOfWeek == 2)
                {
                    day = "Tuesday";

                }
                else if (dayOfWeek == 3)
                {
                    day = "Wendsday";

                }
                else if (dayOfWeek == 4)
                {
                    day = "Thursday";

                }
                else if (dayOfWeek == 5)
                {
                    day = "Friday";
                }
                else if (dayOfWeek == 6)
                {
                    day = "Saturday";

                }
                else if (dayOfWeek == 0)
                {
                    day = "Sunday";

                }

                return day;
            }

        }
        public string stringStart
        {
            get
            {
                var start = "";

                var time = Start.ToString();
                var parts = time.Split(':');
                start = parts[0] + ":" + parts[1];

                return start;



            }
        }
        public string stringEnd
        {
            get
            {
                var end = "";

                var time = End.ToString();

                if (time == "1.00:00:00")
                {
                    end = "24:00";
                    return end;
                }
                else
                {
                    var parts = time.Split(':');

                    end = parts[0] + ":" + parts[1];

                    return end;

                }





            }
        }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public decimal Amount { get; set; }
        public string WeeklyBenefitInfo
        {
            get
            {
                var day = "";
                if (dayOfWeek == 1)
                {
                    day = "Mondays";
                }
                else if (dayOfWeek == 2)
                {
                    day = "Tuesdays";

                }
                else if (dayOfWeek == 3)
                {
                    day = "Wendsdays";

                }
                else if (dayOfWeek == 4)
                {
                    day = "Thursdays";

                }
                else if (dayOfWeek == 5)
                {
                    day = "Fridays";
                }
                else if (dayOfWeek == 6)
                {
                    day = "Saturdays";

                }
                else if (dayOfWeek == 0)
                {
                    day = "Sundays";

                }

                decimal amount = 0;
                amount = Math.Round(Amount, 2);

                return $"Day: {day}, Amount: {amount} Time: {stringStart}-{stringEnd}";

            }
        }
        public string weeklybenefitInfo { get; set; }

        public WeeklyBenefit()
        {
            ID = 0;
            weeklybenefitInfo = string.Empty;

            Start = TimeSpan.Zero;
            End = TimeSpan.Zero;
            Amount = 0;
        }

        public string GetTheWeeklyBenefit(TimeSpan benefitStart, TimeSpan benefitEnd)
        {
            var StringStart = "";
            var StringEnd = "";


            decimal difference = GetDifference(benefitStart, benefitEnd);

            decimal amount = difference * Amount;

            decimal decimalAmount = Math.Round(amount, 2);


            var time = benefitStart.ToString();
            Debug.WriteLine(time);
            var parts = time.Split(':');
            StringStart = parts[0] + ":" + parts[1];


            time = benefitEnd.ToString();
            Debug.WriteLine(time);
            if (time == "1.00:00:00")
            {
                StringEnd = "24:00";
            }
            else
            {
                var sparts = time.Split(':');
                Debug.WriteLine(sparts[0] + " " + sparts[1]);

                StringEnd = sparts[0] + ":" + sparts[1];

            }

            return $"Amount: {decimalAmount} (Paid from: {StringStart}-{StringEnd})";

        }

        public decimal GetDifference(TimeSpan timeStart, TimeSpan timeEnd)
        {
            decimal value = 0;
            TimeSpan difference = new TimeSpan(0, 0, 0);
            TimeSpan midnight = new TimeSpan(24, 0, 0);

            if (timeEnd < timeStart)
            {
                difference = (midnight - timeStart) + timeEnd;
            }
            else
            {
                difference = timeEnd - timeStart;
            }
            decimal hours = 0;
            bool ok = decimal.TryParse(difference.Hours.ToString(), out hours);
            if (!ok)
            {
                return 0;
            }
            value = hours;

            decimal minutes = 0;
            ok = decimal.TryParse(difference.Minutes.ToString(), out minutes);
            if (!ok)
            {
                return 0;
            }
            value += minutes / 60;
            return value;
        }



        public override string ToString()
        {
            return $"\nDayofWeek:{dayOfWeek}/Amount:{Amount}/S-Start{stringStart}/S-End{stringEnd}/Start:{Start}/End:{End}";
        }

    }

}





