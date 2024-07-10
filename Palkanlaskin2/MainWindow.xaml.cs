using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPFCustomMessageBox;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using static System.Net.Mime.MediaTypeNames;

namespace Palkanlaskin2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Canvas CanvasManualBenefits { get ; set; }
        public ScrollViewer ScrollViewerBenefit { get; set; }
        public Canvas CanvasManualDeduction { get; set; }
        public ScrollViewer ScrollViewerDeduction { get; set; }

        public OvertimeBenefit OvertimeBenefit { get; set; }
        public List<ShiftInfo> AllShifts { get; set; }
        public ShiftInfo ShiftInfo { get; set; }
        public Customer Customer { get; set; }
        public WeeklyBenefit WeeklyBenefit { get; set; }
        public BenefitOnADate BenefitOnADate { get; set; }
        public ManualBenefit ManualBenefit { get; set; }
        public ManualDeduction ManualDeduction { get; set; }
        public List<TextBox> txtBoxit { get; set; }
        public List<PasswordBox> passwordBoxit { get; set; }

        public List<CheckBox> CheckboxesBenefit { get; set; }
        public List<CheckBox> CheckboxesDeduction { get; set; }
        public List<CheckBox> CheckboxesBenefit2 { get; set; }
        public List<CheckBox> CheckboxesDeduction2 { get; set; }


        public List<Canvas> Canvases { get; set; }
        public List<CheckBox> CheckBoxesWeekdays { get; set; }
        public Job Job { get; set; }
        public Repository Repo { get; set; }
        public int BenefitCounter { get; set; }

        public List<int> listStarts { get; set; }
        public List<int> ListEnds { get; set; }

       





        public MainWindow()
        {
            InitializeComponent();

            CanvasManualBenefits = new Canvas();
            ScrollViewerBenefit = new ScrollViewer();

            CanvasManualDeduction = new Canvas();
            ScrollViewerDeduction = new ScrollViewer();

            txtBoxit = new List<TextBox>();
            passwordBoxit = new List<PasswordBox>();
            Canvases = new List<Canvas>();

            CheckboxesBenefit = new List<CheckBox>();
            CheckboxesDeduction = new List<CheckBox>();

            CheckboxesBenefit2 = new List<CheckBox>();
            CheckboxesDeduction2 = new List<CheckBox>();

            OvertimeBenefit = new OvertimeBenefit();
            AllShifts = new List<ShiftInfo>();
            ShiftInfo = new ShiftInfo();
            Customer = new Customer();
            Repo = new Repository();
            WeeklyBenefit = new WeeklyBenefit();
            BenefitOnADate = new BenefitOnADate();
            ManualDeduction = new ManualDeduction();
            ManualBenefit = new ManualBenefit();
            Job = new Job();

            BenefitCounter = 0;
            listStarts = new List<int>();
            ListEnds = new List<int>();
            CheckBoxesWeekdays = new List<CheckBox>();

            txtBoxitCanvasiin();
            SetCanvasesInAList();
            SetAllCanvasesInTheMiddle();

            AlustaComboboxes();

            AlustaKäyttöliittymäCreateOrLogin();
          



        }



        //CHANGE_ELELMENTS_VISIBILITY_AND_RANDOM_METHODS




        private void SetCanvasesInAList()
        {
            Canvases.Add(canvasHowToStart);
            Canvases.Add(canvasloginAndCreateAccount);
            Canvases.Add(canvasTypeInYourShifts);
        }
        private void SetAllCanvasesInTheMiddle()
        {
            foreach (var item in Canvases)
            {
                Canvas.SetTop(item, 83);
                Canvas.SetLeft(item, 0);
            }
        }
        private void txtBoxitCanvasiin()
        {
            txtBoxit.Clear();
            passwordBoxit.Clear();

            txtBoxit.Add((TextBox)txtC_A_Username);
            passwordBoxit.Add((PasswordBox)txtC_A_Password);
            passwordBoxit.Add((PasswordBox)txtC_A_RepeatPassword);

            txtBoxit.Add((TextBox)txtLoginsername);
            passwordBoxit.Add((PasswordBox)txtLoginPassword);

        }
        private void AlustaKäyttöliittymä()
        {
            canvasloginAndCreateAccount.Visibility = Visibility.Hidden;
            canvasHowToStart.Visibility = Visibility.Hidden;
            canvasTypeInYourShifts.Visibility = Visibility.Hidden;




        }
        private void AlustaKäyttöliittymäCreateOrLogin()
        {
            AlustaKäyttöliittymä();

            groupCreateAccount.Visibility = Visibility.Hidden;
            groupStart.Visibility = Visibility.Visible;
            groupLogin.Visibility = Visibility.Hidden;
            btnReturn.Visibility = Visibility.Hidden;
            btnCreate_Or_Login.Visibility = Visibility.Hidden;

            canvasloginAndCreateAccount.Visibility = Visibility.Visible;

            canvasWelcome.Visibility = Visibility.Hidden;

            Canvas.SetLeft(lblCYS, 179);
            Canvas.SetTop(lblCYS, 112);


        }
        private void AlustaAddBenefitsAndSalary()
        {
            AlustaKäyttöliittymä();

            canvasHowToStart.Visibility = Visibility.Hidden;
            canvasSalaryAndAddBenefits.Visibility = Visibility.Visible;
            groupBenefit.Visibility = Visibility.Hidden;
            groupboxDeduction.Visibility = Visibility.Hidden;
            canvasTimeDate.Visibility = Visibility.Hidden;
        }
        private void AlustaHowTostart()
        {
            canvasTypeInYourShifts.Visibility = Visibility.Hidden;
            canvasloginAndCreateAccount.Visibility = Visibility.Hidden;

            canvasHowToStart.Visibility = Visibility.Visible;
            canvasSalaryAndAddBenefits.Visibility = Visibility.Visible;

            canvasTimeDate.Visibility = Visibility.Hidden;
            canvascalendar.Visibility = Visibility.Hidden;
            groupBenefit.Visibility = Visibility.Hidden;
            groupboxDeduction.Visibility = Visibility.Hidden;
            groupOvertime.Visibility = Visibility.Hidden;


            comDailyBenefit.ItemsSource = Customer.Job.BenefitsOnADate;
            comWeeklyBenefit.ItemsSource = Customer.Job.WeeklyBenefits;
            comManualBenefits.ItemsSource = Customer.Job.MBenefits;
            comManualDeductions.ItemsSource = Customer.Job.MDeductions;
            comOvertimeBenefit.ItemsSource = Customer.Job.Overtimebenefits;

            if (Customer.Job.SalaryPerHour != 0)
            {
                txtSalaryPerHour.Text = Customer.Job.SalaryPerHour.ToString();

            }

            Canvas.SetLeft(lblCYS, 50);
            Canvas.SetTop(lblCYS, 20);



            Debug.WriteLine(Customer);

        }
        private void AlustaComboboxes()
        {
            CheckBoxesWeekdays.Add(monday);
            CheckBoxesWeekdays.Add(tuesday);
            CheckBoxesWeekdays.Add(wensday);
            CheckBoxesWeekdays.Add(thursday);
            CheckBoxesWeekdays.Add(friday);
            CheckBoxesWeekdays.Add(saturday);
            CheckBoxesWeekdays.Add(sunday);
        }
       
       
        private void ChangeDotIfNeeded(TextBox textBox)
        {
            
            var sana = textBox.Text;
            decimal num;


            bool pilkku = textBox.Text.Contains(',');
            bool piste = textBox.Text.Contains('.');
            if (!pilkku && !piste)
            {
                return;
            }
            if (pilkku)
            {
                bool ok = decimal.TryParse(textBox.Text, out num);
                if (!ok)
                {
                    return;
                }
                if (num.ToString() != sana)
                {
                    
                    textBox.Text = sana.Replace(',', '.');

                    return;


                }

            }
            else if (piste)
            {

                bool ok = decimal.TryParse(textBox.Text, out num);
                if (!ok)
                {
                    return;
                }
                if (num.ToString() != sana)
                {

                    textBox.Text = sana.Replace('.', ',');
                    return;


                }

            }
            return;








        }


        // boolean methods
        private bool CheckIfPasswordsMatch(string string1, string string2)
        {
            bool ok = false;
            if (string1 == string2)
            {
                ok = true;
            }
            else
            {
                txtC_A_Password.Clear();
                txtC_A_RepeatPassword.Clear();
                txtC_A_Password.Focus();

                MessageBox.Show("Passwords don't match", "Error");
            }
            return ok;
        }
        private bool CheckUsernameLength(string username)
        {
            bool pass = false;
            if (username.Length < 5)
            {
                MessageBox.Show("Username has to be more than 4 letters", "Error");

            }
            else
            {
                pass = true;

            }
            return pass;

        }
        private bool CheckPasswordLength(string password)
        {
            bool pass = false;
            if (password.Length < 7)
            {
                MessageBox.Show("Password has to be more than 6 letters", "Error");

            }
            else
            {
                pass = true;

            }
            return pass;

        }

        

        //CHANGE_ELELMENTS_VISIBILITY_AND_RANDOM_METHODS





















        //LOGOUT
        private void btnLogOut_click(object sender, RoutedEventArgs e)
        {
            Customer = new Customer();
            WeeklyBenefit = new WeeklyBenefit();
            BenefitOnADate = new BenefitOnADate();


            //lblCheckYSalary.Visibility = Visibility.Hidden;
            //lblCheckYSalary = new Label();
            //lblCheckYSalary.Content = "Check Your Salary";
            //Canvas.SetLeft()

           

            if (groupLogin.Visibility == Visibility.Visible)
            {
                var currentValue = Canvas.GetTop(btnCreate_Or_Login);
                var newValue = currentValue + 5;
                Canvas.SetTop(btnCreate_Or_Login, newValue);


            }

            Canvas.SetLeft(lblCYS, 179);
            Canvas.SetTop(lblCYS, 112);

            canvasRightUpperCornerComponents.Visibility = Visibility.Visible;
            canvasSalaryAndAddBenefits.Visibility = Visibility.Visible;
            groupStart.Visibility = Visibility.Visible;
            groupLogin.Visibility = Visibility.Hidden;
            groupCreateAccount.Visibility = Visibility.Hidden;
            btnReturn.Visibility = Visibility.Hidden;
            btnCreate_Or_Login.Visibility = Visibility.Hidden;

            txtSalaryInfo.Text = string.Empty;
            AllShifts.Clear();




            foreach (var item in txtBoxit)
            {
                item.Text = string.Empty;

            }
            foreach (var item in passwordBoxit)
            {
                item.Clear();

            }

            canvasManualBenefitManualDeduction.Children.Clear();


            lblWelcome.Content = "Welcome ";

            AlustaAddBenefitsAndSalary();
            canvasTypeInYourShifts.Visibility = Visibility.Hidden;
            canvasHowToStart.Visibility = Visibility.Hidden;
            canvasloginAndCreateAccount.Visibility = Visibility.Visible;
            canvasWelcome.Visibility = Visibility.Hidden;

        }
        //LOGOUT






















        //CANVAS_LOGIN_AND_CREATE_ACCOUNT
        private void click_btnCreateUser_Or_Login(object sender, RoutedEventArgs e)
        {
            if (groupLogin.Visibility == Visibility.Visible)
            {
                Customer.Username = txtLoginsername.Text;
                Customer.Password = txtLoginPassword.Password.ToString();

                bool UsernameExists = Repo.CheckIfAccountExists(Customer.Username);
                if (!UsernameExists)
                {
                    txtLoginsername.Clear();
                    txtLoginPassword.Clear();
                    txtLoginsername.Focus();
                    MessageBox.Show($"Error", "Error");
                    return;

                }
                var matches = Repo.CheckIfPasswordMatchesUsername(Customer.Username, Customer.Password);
                if (!matches)
                {
                    txtLoginsername.Clear();
                    txtLoginPassword.Clear();
                    txtLoginPassword.Focus();

                    MessageBox.Show("Password is wrong", "Error");
                    return;
                }

                Customer = Repo.HaeAsiakas(Customer.Username, Customer.Password);
                Customer.Id = Repo.HaeAsiakkaanID(Customer);
                Customer.Job = Repo.GetCustomersTyösopimus(Customer.Id);


                AlustaKäyttöliittymä();
                if (Customer.Job.SalaryPerHour != 0)
                {
                    InitialiseCanvasTypeInYourShifts();

                }
                else
                {
                    AlustaHowTostart();


                }

            }
            else if (groupCreateAccount.Visibility == Visibility.Visible)
            {
                bool IsUsernameLongEnough = CheckUsernameLength(txtC_A_Username.Text);
                if (!IsUsernameLongEnough)
                {
                    return;
                }
                bool contains = txtC_A_Username.Text.Contains(' ');
                if (contains)
                {
                    MessageBox.Show("Username can not have a space in it", "Error");
                    return;
                }
                bool IsPasswordLongEnough = CheckPasswordLength(txtC_A_Password.Password.ToString());
                if (!IsPasswordLongEnough)
                {
                    return;
                }
                Customer.Username = txtC_A_Username.Text;
                Customer.Password = txtC_A_Password.Password.ToString();
                var passwordRepeat = txtC_A_RepeatPassword.Password.ToString();

                bool ok = CheckIfPasswordsMatch(Customer.Password, passwordRepeat);
                if (!ok)
                {
                    return;
                }

                bool UsernameExists = Repo.CheckIfAccountExists(Customer.Username);

                if (UsernameExists)
                {
                    MessageBox.Show("Pick another username.\n" +
                   $"Username: '{Customer.Username}' already exists", "Error");
                    txtC_A_Username.Clear();
                    txtC_A_Password.Clear();
                    txtC_A_RepeatPassword.Clear();
                    txtC_A_Username.Focus();
                    return;

                }

                Repo.CreateAccount(Customer.Username, Customer.Password);
                Customer = Repo.GetCustomerID(Customer); //lisää luokkaan vain id tiedon
                Job = new Job();
                Repo.CreateTyösopimusCustomerIDContractID(Customer.Id);
                Customer.Job = Repo.GetCustomersTyösopimus(Customer.Id);


                AlustaHowTostart();

                txtSalaryPerHour.Text = string.Empty;
                txtSalaryPerHour.Focus();


                Debug.WriteLine(Customer);

            }

        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            groupStart.Visibility = Visibility.Hidden;
            groupLogin.Visibility = Visibility.Visible;
            btnReturn.Visibility = Visibility.Visible;
            btnCreate_Or_Login.Content = "Login";
            btnCreate_Or_Login.Visibility = Visibility.Visible;

            txtLoginsername.Focus();

            var currentValue = Canvas.GetTop(btnCreate_Or_Login);
            var newValue = currentValue - 5;
            Canvas.SetTop(btnCreate_Or_Login, newValue);

        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            groupStart.Visibility = Visibility.Hidden;
            groupCreateAccount.Visibility = Visibility.Visible;
            btnReturn.Visibility = Visibility.Visible;
            btnCreate_Or_Login.Content = "Create";
            btnCreate_Or_Login.Visibility = Visibility.Visible;
            txtC_A_Username.Focus();

        }
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (groupLogin.Visibility == Visibility.Visible)
            {
                var currentValue = Canvas.GetTop(btnCreate_Or_Login);
                var newValue = currentValue + 5;
                Canvas.SetTop(btnCreate_Or_Login, newValue);


            }

            groupStart.Visibility = Visibility.Visible;
            groupLogin.Visibility = Visibility.Hidden;
            groupCreateAccount.Visibility = Visibility.Hidden;
            btnReturn.Visibility = Visibility.Hidden;
            btnCreate_Or_Login.Visibility = Visibility.Hidden;

            foreach (var item in txtBoxit)
            {
                item.Text = string.Empty;

            }
            foreach (var item in passwordBoxit)
            {
                item.Clear();

            }




        }
        private void btnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && groupLogin.Visibility == Visibility.Visible)
            {
                txtLoginsername.Focus();
            }
            else if (e.Key == Key.Down && groupLogin.Visibility == Visibility.Hidden)
            {
                txtC_A_Username.Focus();

            }

        }
        private void txtLoginsername_KDown(object sender, KeyEventArgs e)
        {
            if (txtLoginsername.Text != string.Empty && e.Key == Key.Enter || e.Key == Key.Down)
            {
                txtLoginPassword.Focus();

            }
            else if (e.Key == Key.Up)
            {
                btnReturn.Focus();

            }
            else if (e.Key == Key.OemQuestion)
            {
                e.Handled = true;

            }

        }
        private void txtLoginPassword_KDown(object sender, KeyEventArgs e)
        {

            if (txtLoginPassword.Password != string.Empty && e.Key == Key.Enter || e.Key == Key.Down)
            {
                btnCreate_Or_Login.Focus();

            }
            else if (e.Key == Key.Up)
            {
                txtLoginPassword.Focus();

            }
            else if (e.Key == Key.OemQuestion)
            {
                e.Handled = true;

            }
        }
        private void txtC_A_Username_KDown(object sender, KeyEventArgs e)
        {
            if (txtC_A_Username.Text != string.Empty && e.Key == Key.Enter || e.Key == Key.Down)
            {
                txtC_A_Password.Focus();
            }
            else if (e.Key == Key.Up)
            {
                btnReturn.Focus();

            }
            else if (e.Key == Key.OemQuestion)
            {
                e.Handled = true;

            }

        }
        private void txtC_A_Password_KDown(object sender, KeyEventArgs e)
        {

            if (txtC_A_Password.Password != string.Empty && e.Key == Key.Enter || e.Key == Key.Down)
            {
                txtC_A_RepeatPassword.Focus();

            }

            if (e.Key == Key.Up)
            {
                txtC_A_Username.Focus();

            }
            else if (e.Key == Key.OemQuestion)
            {
                e.Handled = true;

            }
        }
        private void txtC_A_RepeatPassword_KDown(object sender, KeyEventArgs e)
        {
            if (txtC_A_RepeatPassword.Password != string.Empty && e.Key == Key.Enter || e.Key == Key.Down)
            {
                btnCreate_Or_Login.Focus();

            }
            if (e.Key == Key.Up)
            {
                txtC_A_Password.Focus();

            }
            else if (e.Key == Key.OemQuestion)
            {
                e.Handled = true;

            }

        }
        private void btnCreate_Or_Login_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Up && groupLogin.Visibility == Visibility.Visible)
            {
                txtLoginPassword.Focus();
            }
            else if (e.Key == Key.Up && groupLogin.Visibility == Visibility.Hidden)
            {
                txtC_A_RepeatPassword.Focus();


            }
        }
        //CANVAS_LOGIN_AND_CREATE_ACCOUNT






















        //CANVAS HOW TO START

        // - groupAutomaticallyOccurringBenefits

        // - - btnSaveAllBenefits
        private void btnSaveAllBenefits_MDown(object sender, RoutedEventArgs e)
        {
            decimal salary = 0;
            if (txtSalaryPerHour.Text == string.Empty)
            {
                MessageBox.Show("Please type in your salary", "Error");

                return;


            }
            
            if (txtSalaryPerHour.Text != string.Empty)
            {
                ChangeDotIfNeeded(txtSalaryPerHour);


                bool ok = decimal.TryParse(txtSalaryPerHour.Text, out salary);
                if (!ok)
                {
                    MessageBox.Show("Error in your salary", "Error");
                    txtSalaryPerHour.Focus();
                    return;

                }
                Customer.Job.SalaryPerHour = salary;

                Customer.Job.SalaryPerHour = Math.Round(salary, 2);

                Repo.AddSalarypPerHour(Customer.Job.SalaryPerHour, Customer.Job.ContractID);

            }

            canvasHowToStart.Visibility = Visibility.Hidden;
            canvasTypeInYourShifts.Visibility = Visibility.Visible;


            InitialiseCanvasTypeInYourShifts();
            calendarShift.Focus();
        }


        // - - groupHourlySalary
        private void txtSalaryPerHour_KDown(object sender, KeyEventArgs e)
        {
            if (txtSalaryPerHour.Text != string.Empty && e.Key == Key.Enter)
            {
                ChangeDotIfNeeded(txtSalaryPerHour);

                float amount;
                bool ok = float.TryParse(txtSalaryPerHour.Text, out amount);
                if (!ok)
                {
                    return;
                }


            }
        }
       
        // - - groupHourlySalary


        // - - groupOvertimeBenefit
        private void btnAddOvertime_click(object sender, RoutedEventArgs e)
        {
            radioShift.IsChecked = false;
            radioWeek.IsChecked = false;
            radioMonth.IsChecked = false;
            
            txtOvertimeAmount.Text = string.Empty;

            for (int i = 0; i < 340; i++)
            {
                comhours.Items.Add(i);


            }
            for (int i = 0; i < 60; i++)
            {
                comminutes.Items.Add(i);
            }

            canvasSalaryAndAddBenefits.Visibility = Visibility.Hidden;
            groupOvertime.Visibility = Visibility.Visible;
        }
        private void btnRemoveOvertimeBenefit_click(object sender, RoutedEventArgs e)
        {
            if (comOvertimeBenefit.SelectedIndex > -1)
            {
                OvertimeBenefit = (OvertimeBenefit)comOvertimeBenefit.SelectedItem;
                Customer.Job.Overtimebenefits.Remove(OvertimeBenefit);

                Repo.RemoveOvertimeBenefit(Customer.Job.ContractID, OvertimeBenefit.ID);

                comOvertimeBenefit.ItemsSource = Customer.Job.Overtimebenefits;

            }
            else
            {
                MessageBox.Show("Please select a benefit you would like to remove", "Error");
            }

        }


        // - - groupOvertimeBenefit


        // - - groupSpecificDayBenefit
        private void btnRemoveSepecificDayBenefit_click(object sender, RoutedEventArgs e)
        {

            if (comDailyBenefit.SelectedIndex > -1)
            {
                BenefitOnADate = (BenefitOnADate)comDailyBenefit.SelectedItem;
                Customer.Job.BenefitsOnADate.Remove(BenefitOnADate);

                Repo.RemoveBenefitOnADate(Customer.Job.ContractID, BenefitOnADate.ID);

                comDailyBenefit.ItemsSource = Customer.Job.BenefitsOnADate;

            }
            else
            {
                MessageBox.Show("Please select a benefit you would like to remove", "Error");
            }


        }
        private void btnAddSpecificDayBenefit_click(object sender, RoutedEventArgs e)
        {
            //Alustetaan
            calendar.SelectedDate = DateTime.Now;
            txtShiftStarts1.Text = "- - : - -";
            txtShiftEnds1.Text = "- - : - -";
            txtAmountti.Text = string.Empty;

            listStarts.Clear();
            ListEnds.Clear();

            canvasSalaryAndAddBenefits.Visibility = Visibility.Hidden;
            canvasTimeDate.Visibility = Visibility.Hidden;
            canvascalendar.Visibility = Visibility.Hidden;
            canvasWeekly.Visibility = Visibility.Hidden;

            canvasTimeDate.Visibility = Visibility.Visible;
            canvasWeekly.Visibility = Visibility.Hidden;
            canvascalendar.Visibility = Visibility.Visible;
            calendar.Focusable = true;
            calendar.Focus();
        }
        // - - groupSpecificDayBenefit


        // - - groupWeeklyBenefit
        private void btnRemoveWeeklyBenefit_click(object sender, RoutedEventArgs e)
        {
            if (comWeeklyBenefit.SelectedIndex > -1)
            {
                WeeklyBenefit = (WeeklyBenefit)comWeeklyBenefit.SelectedItem;
                Customer.Job.WeeklyBenefits.Remove(WeeklyBenefit);

                Repo.RemoveWeeklyBenefit(Customer.Job.ContractID, WeeklyBenefit.ID);

                comWeeklyBenefit.ItemsSource = Customer.Job.WeeklyBenefits;

            }
            else
            {
                MessageBox.Show("Please select a benefit you would like to remove", "Error");
            }

        }
        private void btnWeekly_click(object sender, RoutedEventArgs e)
        {
            //Alustetaan
            calendar.SelectedDate = DateTime.Now;
            txtShiftStarts1.Text = "- - : - -";
            txtShiftEnds1.Text = "- - : - -";
            txtAmountti.Text = string.Empty;

            listStarts.Clear();
            ListEnds.Clear();

            canvasSalaryAndAddBenefits.Visibility = Visibility.Hidden;
            canvasTimeDate.Visibility = Visibility.Hidden;
            canvascalendar.Visibility = Visibility.Hidden;
            canvasWeekly.Visibility = Visibility.Hidden;


            canvasTimeDate.Visibility = Visibility.Visible;
            canvascalendar.Visibility = Visibility.Hidden;
            canvasWeekly.Visibility = Visibility.Visible;
            canvasWeekly.Focusable = true;
            canvasWeekly.Focus();

        }
        // - - groupWeeklyBenefit

        // - groupAutomaticallyOccurringBenefits







        // - groupManuallyActivatedBenefitsAndDeduction

        // - - groupManualBenefits
        private void btnRemoveManualBenefits_click(object sender, RoutedEventArgs e)
        {
            
            if (comManualBenefits.SelectedIndex > -1)
            {
                ManualBenefit = new ManualBenefit();
                ManualBenefit = (ManualBenefit)comManualBenefits.SelectedItem;
                Customer.Job.MBenefits.Remove(ManualBenefit);

                Repo.RemoveManualBenefit(Customer.Job.ContractID, ManualBenefit.ID);

                comManualBenefits.ItemsSource = Customer.Job.MBenefits;
               
            }
            else
            {
                MessageBox.Show("Please select a benefit you would like to remove", "Error");
            }


           

        }
        private void btnAddManualBenefit_click(object sender, RoutedEventArgs e)
        {
            

            txtNameOfBenefit.Text = "Name of benefit";
            txtgroupBenefitAmount.Text = "Amount";

            txtNameOfDeduction.Text = "Name of deduction";
            txtgroupDeductionAmount.Text = "Amount";

            canvasSalaryAndAddBenefits.Visibility = Visibility.Hidden;
            groupboxDeduction.Visibility = Visibility.Hidden;
            groupBenefit.Visibility = Visibility.Visible;


        }
        // - - groupManualBenefits


        // - - groupManualDeduction
        private void btnRemoveManualDeduction_click(object sender, RoutedEventArgs e)
        {
            
            if (comManualDeductions.SelectedIndex > -1)
            {
                ManualDeduction = new ManualDeduction();
                ManualDeduction = (ManualDeduction)comManualDeductions.SelectedItem;
                Customer.Job.MDeductions.Remove(ManualDeduction);

                Repo.RemoveManualDeduction(Customer.Job.ContractID, ManualDeduction.ID);

                comManualDeductions.ItemsSource = Customer.Job.MDeductions;

            }
            else
            {
                MessageBox.Show("Please select a deduction you would like to remove", "Error");
            }


        }
        private void btnAddManualDeduction_click(object sender, RoutedEventArgs e)
        {
            
            txtNameOfBenefit.Text = "Name of benefit";
            txtgroupBenefitAmount.Text = "Amount";

            txtNameOfDeduction.Text = "Name of deduction";
            txtgroupDeductionAmount.Text = "Amount";

            radioDeductedOnce.IsChecked = true;

            canvasSalaryAndAddBenefits.Visibility = Visibility.Hidden;
            groupboxDeduction.Visibility = Visibility.Visible;
            groupBenefit.Visibility = Visibility.Hidden;

        }
        // - - groupManualDeduction

        // - groupManuallyActivatedBenefitsAndDeduction






        // - canvasTimeDate

        // - - calendar

        //          ei metodeja vielä
        // - - calendar


        // - - canvasweekly

        //          ei metodeja vielä

        // - - canvasweekly


        // - - btnStart00_End24
        private void btnAllday_click(object sender, RoutedEventArgs e)
        {
            txtShiftStarts1.Text = "00:00";
            txtShiftEnds1.Text = "24:00";
            if (canvascalendar.Visibility == Visibility.Visible)
            {
                if (calendar.SelectedDate == null)
                {
                    calendar.Focus();

                }
                else
                {
                    btnSaveDateTimeOrWeekDay.Focus();
                }

            }

           
            txtAmountti.BorderBrush = Brushes.Gray;

        }


        // - txtShiftStarts1
        private void txtShiftStartsGotFocus1(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("txtShiftStarts_GotFocus");
            txtAmountti.BorderBrush = Brushes.White;
            txtShiftStarts1.BorderBrush = Brushes.Black;
            txtShiftEnds1.BorderBrush = Brushes.White;

            txtShiftStarts1.Text = string.Empty;
            listStarts.Clear();
        }
        private void txtShiftStarts_P_KDown1(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key.ToString());
            if (e.Key == Key.Right)
            {
                var dateshift = DateOnly.FromDateTime((DateTime)calendar.SelectedDate);
                dateshift = dateshift.AddDays(1);
                calendar.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Left)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendar.SelectedDate);
                dateshift = dateshift.AddDays(-1);
                calendar.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Down)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendar.SelectedDate);
                dateshift = dateshift.AddDays(7);
                calendar.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Up)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendar.SelectedDate);
                dateshift = dateshift.AddDays(-7);
                calendar.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Back)
            {
                listStarts.Clear();
                txtShiftStarts1.Text = string.Empty;
                return;
            }
            if (e.Key.ToString().Length == 1)
            {
                e.Handled = true; return;

            }

            else if (e.Key.ToString().Length == 2)
            {
                string twoChars = e.Key.ToString();
                char c = twoChars[1];
                bool ok = int.TryParse(c.ToString(), out int value);

                if (ok)
                {
                    if (listStarts.Count == 4)
                    {
                        txtShiftEnds1.Focus();
                        e.Handled = true;
                        return;

                    }

                    if (listStarts.Count == 0)
                    {
                        if (value == 0 || value == 1 || value == 2)
                        {
                            listStarts.Add(value);
                            Debug.WriteLine(listStarts.Count.ToString());

                            return;
                        }
                        else
                        {


                        }

                    }
                    if (listStarts.Count == 1)
                    {
                        if (value < 10)
                        {
                            listStarts.Add(value);
                            Debug.WriteLine(listStarts.Count.ToString());
                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }

                    }
                    if (listStarts.Count == 2)
                    {
                        AddDoubleDot(txtShiftStarts1);
                        if (value < 6)
                        {


                            listStarts.Add(value);
                            Debug.WriteLine(listStarts.Count.ToString());


                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }


                    }
                    if (listStarts.Count == 3)
                    {

                        if (value < 10)
                        {
                            listStarts.Add(value);
                            Debug.WriteLine(listStarts.Count.ToString());
                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }

                    }

                }
                else
                {
                    e.Handled = true;
                    return;
                }

            }

        }      
        private void txtShiftStarts_PKup1(object sender, KeyEventArgs e)
        {

            if (e.Key.ToString().Length == 2)
            {

                string twoChars = e.Key.ToString();
                char c = twoChars[1];
                bool ok = int.TryParse(c.ToString(), out int value);

                if (ok)
                {
                    if (listStarts.Count == 2)
                    {
                        AddDoubleDot(txtShiftStarts1);
                        e.Handled = true;
                        return;

                    }
                    else if (listStarts.Count == 4)
                    {
                        txtShiftEnds1.Focus();
                        e.Handled = true;
                        return;

                    }
                    else
                    {
                        e.Handled = true;
                        return;

                    }

                }
            }

        }
        // - txtShiftStarts1


        // - txtShiftEnds1
        private void txtShiftEnds_GotFocus1(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("txtShiftEnds_GotFocus");
            txtAmountti.BorderBrush = Brushes.White;
            txtShiftEnds1.BorderBrush = Brushes.Black;
            txtShiftStarts1.BorderBrush = Brushes.White;
            txtShiftEnds1.Text = string.Empty;
            ListEnds.Clear();

        }
        private void txtShiftEnds_P_KDown1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendar.SelectedDate);
                dateshift = dateshift.AddDays(1);
                calendar.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Left)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendar.SelectedDate);
                dateshift = dateshift.AddDays(-1);
                calendar.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Down)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendar.SelectedDate);
                dateshift = dateshift.AddDays(7);
                calendar.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Up)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendar.SelectedDate);
                dateshift = dateshift.AddDays(-7);
                calendar.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (ListEnds.Count == 4 && e.Key == Key.Enter)
            {
                //btnSaveShift_click(sender, e);
                return;

            }
            if (e.Key == Key.Back)
            {
                if (ListEnds.Count == 0)
                {
                    txtShiftEnds1.Text = "- - : - -";
                    txtShiftStarts1.Focus();

                }
                else
                {
                    ListEnds.Clear();
                    txtShiftEnds1.Text = string.Empty;
                }

            }
            if (e.Key.ToString().Length == 1)
            {
                e.Handled = true; return;

            }
            else if (e.Key.ToString().Length == 2)
            {

                string twoChars = e.Key.ToString();
                char c = twoChars[1];
                bool ok = int.TryParse(c.ToString(), out int value);

                if (ok)
                {
                    if (ListEnds.Count == 4)
                    {
                        e.Handled = true;

                        return;
                    }

                    if (ListEnds.Count == 0)
                    {
                        if (value == 0 || value == 1 || value == 2)
                        {
                            ListEnds.Add(value);
                            Debug.WriteLine(ListEnds.Count.ToString());

                            return;
                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }

                    }
                    if (ListEnds.Count == 1)
                    {
                        if (value < 10)
                        {
                            ListEnds.Add(value);
                            Debug.WriteLine(ListEnds.Count.ToString());


                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }

                    }
                    if (ListEnds.Count == 2)
                    {
                        if (txtShiftEnds1.Text[txtShiftEnds1.Text.Length - 1] != ':')
                        {
                            AddDoubleDot(txtShiftEnds1);
                        }

                        if (value < 6)
                        {
                            ListEnds.Add(value);
                            Debug.WriteLine(ListEnds.Count.ToString());

                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }


                    }
                    if (ListEnds.Count == 3)
                    {

                        if (value < 10)
                        {
                            ListEnds.Add(value);
                            Debug.WriteLine(ListEnds.Count.ToString());
                            

                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }



                    }





                }
                else
                {
                    e.Handled = true;
                    return;
                }

            }
        }
        private void txtShiftEnds1_P_Kup1(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().Length == 2)
            {

                string twoChars = e.Key.ToString();
                char c = twoChars[1];
                bool ok = int.TryParse(c.ToString(), out int value);

                if (ok)
                {
                    if (ListEnds.Count == 2)
                    {
                        if (value < 6)
                        {

                            AddDoubleDot(txtShiftEnds1);
                            e.Handled = true;
                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }


                    }
                    else if (ListEnds.Count == 4)
                    {
                        txtShiftEnds1.BorderBrush = Brushes.Gray;
                        btnSaveDateTimeOrWeekDay.Focus();
                        e.Handled = true;
                        return;

                    }
                }
            }

        }
    



        // - txtShiftEnds1

        public void AddBenefit()
        {


            DateOnly S = new DateOnly();
            

            ChangeDotIfNeeded(txtAmountti);

            double amount;
            bool ok = double.TryParse(txtAmountti.Text, out amount);
            if (!ok)
            {
                txtAmountti.Focus();
                return;
            }
            decimal decimalAmount = Math.Round((decimal)amount, 3);






            if (txtShiftStarts1.Text == "- - : - -" || txtShiftStarts1.Text == string.Empty || txtShiftStarts1.Text.Length != 5)
            {
                MessageBox.Show("Incorrect start time", "Error");
                return;
            }
            if (!txtShiftStarts1.Text.Contains(':'))
            {
                MessageBox.Show("Please add ':' in start time.", "Error");
                return;

            }

            var stringEnd = txtShiftEnds1.Text;
            if (txtShiftEnds1.Text == "- - : - -" || txtShiftEnds1.Text == string.Empty || txtShiftEnds1.Text.Length != 5)
            {
                MessageBox.Show("Incorrect end time", "Error");
                return;
            }
            if (!txtShiftEnds1.Text.Contains(':'))
            {
                MessageBox.Show("Please add ':' in end time.", "Error");
                return;

            }

            if (txtShiftEnds1.Text == "00:00")
            {
                txtShiftEnds1.Text = "24:00";

            }

            var start = GetTime(txtShiftStarts1);
            var end = GetTime(txtShiftEnds1);
            TimeSpan midnight = new TimeSpan(24, 0, 0);
            if (start > end || end > midnight) 
            {
                MessageBox.Show("Benefit can not extend to the next day.\nAdd two separete benefits if the benefits end time extends to the next day.", "Error");
                txtShiftEnds1.Focus();


                return;
            }
            string StartTime = txtShiftStarts1.Text;
            string EndTime = txtShiftEnds1.Text;


            // tähän mennessä on todennettu että vuoron aloitus ja lopetus on syötetty oikein.






            if (canvascalendar.Visibility == Visibility.Visible)
            {
                if (calendar.SelectedDate == null)
                {
                    MessageBox.Show("Please select the day from calendar");
                    return;

                }
                if (calendar.SelectedDates.Count > 1)
                {
                    for (int i = 0; i < calendar.SelectedDates.Count; i++)
                    {

                        S = DateOnly.FromDateTime((DateTime)calendar.SelectedDates[i]);

                        Repo.CreateBenefitOnADate(decimalAmount, S, StartTime, EndTime);
                        BenefitOnADate = Repo.GetBenefitOnADate(decimalAmount, S, StartTime, EndTime);
                        Customer.Job.BenefitsOnADate.Add(BenefitOnADate);

                        Repo.AddToTableJobIdAndContractIDBenefitOnADate(Customer.Job.ContractID, BenefitOnADate.ID);

                    }
                    Debug.WriteLine(Customer);

                    Customer.Job = Repo.GetCustomersTyösopimus(Customer.Id);
                    comDailyBenefit.ItemsSource = Customer.Job.BenefitsOnADate;

                }
                else
                {
                    S = DateOnly.FromDateTime((DateTime)calendar.SelectedDate);
                    Repo.CreateBenefitOnADate(decimalAmount, S, StartTime, EndTime);
                    BenefitOnADate = Repo.GetBenefitOnADate(decimalAmount, S, StartTime, EndTime);
                    Customer.Job.BenefitsOnADate.Add(BenefitOnADate);

                    Repo.AddToTableJobIdAndContractIDBenefitOnADate(Customer.Job.ContractID, BenefitOnADate.ID);


                    Customer.Job = Repo.GetCustomersTyösopimus(Customer.Id);
                    comDailyBenefit.ItemsSource = Customer.Job.BenefitsOnADate;

                    Debug.WriteLine(Customer);






                }


                //Alustetaan
                calendar.SelectedDate = DateTime.Now;

                txtAmountti.Text = string.Empty;

                listStarts.Clear();
                ListEnds.Clear();

                AlustaAddBenefitsAndSalary();
                canvasHowToStart.Visibility = Visibility.Visible;
                canvasSalaryAndAddBenefits.Visibility = Visibility.Visible;
                canvasTimeDate.Visibility = Visibility.Hidden;
                canvasWeekly.Visibility = Visibility.Hidden;

                MessageBox.Show("Benefit saved");


            }

            if (canvasWeekly.Visibility == Visibility.Visible)
            {
                int day = 0;
                for (int i = 0; i < CheckBoxesWeekdays.Count; i++)
                {
                    if (CheckBoxesWeekdays[i].IsChecked.HasValue)
                    {
                        if (CheckBoxesWeekdays[i].IsChecked.Value == true)
                        {
                            day = i + 1;
                            if (day == 7)
                            {
                                day = 0;

                            }
                            Repo.CreateWeeklyBenefit(day, StartTime, EndTime, decimalAmount);
                            WeeklyBenefit = Repo.GetTheWeeklyBenefit(day, StartTime, EndTime, decimalAmount);
                            //Selvitä miksi WeeklybenefitIDtä ei pystytä luomaan. 
                            Customer.Job.WeeklyBenefits.Add(WeeklyBenefit);
                            Repo.AddToTableJobIDAndWeeklyBenefit(Customer.Job.ContractID, WeeklyBenefit.ID);
                        }
                    }
                }

                //Alustetaan
                calendar.SelectedDate = DateTime.Now;
                txtShiftStarts1.Text = "- - : - -";
                txtShiftEnds1.Text = "- - : - -";
                txtAmountti.Text = string.Empty;

                listStarts.Clear();
                ListEnds.Clear();


                AlustaAddBenefitsAndSalary();
                canvasHowToStart.Visibility = Visibility.Visible;
                canvasSalaryAndAddBenefits.Visibility = Visibility.Visible;
                canvasTimeDate.Visibility = Visibility.Hidden;
                canvascalendar.Visibility = Visibility.Hidden;

                Debug.WriteLine(Customer);
                MessageBox.Show("Benefit saved.");

                Customer.Job = Repo.GetCustomersTyösopimus(Customer.Id);
                comWeeklyBenefit.ItemsSource = Customer.Job.WeeklyBenefits;

                foreach (var item in CheckBoxesWeekdays)
                {
                    item.IsChecked = false;

                }


            }

        }


        // - calendar
        private void calendar_PKDown1(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("calendarKeyDown");

            if (e.Key == Key.Back)
            {
                txtAmountti.Focus();
                txtAmount_KeyDown(txtAmountti, e);
                e.Handled = true;
                return;

            }


            if (e.Key.ToString().Length == 1)
            {
                e.Handled = true; return;

            }

            else if (e.Key.ToString().Length == 2)
            {

                string twoChars = e.Key.ToString();
                char c = twoChars[1];
                bool ok = int.TryParse(c.ToString(), out int value);

                if (ok)
                {
                    txtAmountti.Focus();
                    txtAmount_KeyDown(txtAmountti, e);

                }
                else
                {
                    e.Handled = true; return;
                }

            }

        }

        private void calendar_MouseDown1(object sender, MouseButtonEventArgs e)
        {
            txtShiftStarts1.Text = "- - : - -";
            txtShiftEnds1.Text = "- - : - -";
            listStarts.Clear();
            ListEnds.Clear();
            txtAmountti.BorderBrush = Brushes.Black;

        }
        //- calendar 




        // - - btnExitTimeDate
        private void btnExitTimeDate_click(object sender, RoutedEventArgs e)
        {
            calendar.SelectedDate = DateTime.Now;
            txtShiftStarts1.Text = "- - : - -";
            txtShiftEnds1.Text = "- - : - -";
            txtAmountti.Text = string.Empty;

            listStarts.Clear();
            ListEnds.Clear();

            AlustaHowTostart();
            foreach (var item in CheckBoxesWeekdays)
            {
                item.IsChecked = false;

            }

        }

        // - - btnSaveDateTimeOrWeekDay
        private void btnSaveDateTimeOrWeekDay_click(object sender, RoutedEventArgs e)
        {
            AddBenefit();


        }
        private void btnSaveDateTimeOrWeekDay_KDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                txtShiftEnds1.Focus();
                txtShiftEnds1.BorderBrush = Brushes.Black;
            }
            if (e.Key == Key.Right)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendar.SelectedDate);
                dateshift = dateshift.AddDays(1);
                calendar.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Left)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendar.SelectedDate);
                dateshift = dateshift.AddDays(-1);
                calendar.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Down)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendar.SelectedDate);
                dateshift = dateshift.AddDays(7);
                calendar.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Up)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendar.SelectedDate);
                dateshift = dateshift.AddDays(-7);
                calendar.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Back)
            {
                txtShiftEnds1.Focus();
                txtShiftEnds1.BorderBrush = Brushes.Black;

            }
        }

        private void btnSaveDateTimeOrWeekDay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (listStarts.Count < 4)
                {
                    listStarts.Remove(listStarts.Count - 1);
                    Debug.WriteLine(listStarts.Count + "  " + ListEnds.Count);

                    txtShiftStarts1.BorderBrush = Brushes.Black;
                    txtShiftEnds1.BorderBrush = Brushes.Gray;
                    txtShiftStarts1.Focus();

                }
                else
                {
                    ListEnds.Remove(ListEnds.Count - 1);
                    Debug.WriteLine(listStarts.Count + "  " + ListEnds.Count);

                    txtShiftEnds1.BorderBrush = Brushes.Black;
                    txtShiftStarts1.BorderBrush = Brushes.Gray;
                    txtShiftEnds1.Focus();
                }

                return;

            }

        }
        // - - btnSaveDateTimeOrWeekDay


        // - - txtAmountti
        private void txtAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {

                if (txtAmountti.Text != string.Empty)
                {
                    if (listStarts.Count < 4)
                    {
                        txtShiftStarts1.BorderBrush = Brushes.Black;
                        txtShiftEnds1.BorderBrush = Brushes.Gray;
                        txtShiftStarts1.Focus();
                    }
                    else if (listStarts.Count == 4 && ListEnds.Count < 4)
                    {
                        txtShiftStarts1.BorderBrush = Brushes.Gray;
                        txtShiftEnds1.BorderBrush = Brushes.Black;
                        txtShiftEnds1.Focus();
                    }
                    else
                    {
                        txtAmountti.BorderBrush = Brushes.Gray;
                        txtShiftStarts1.BorderBrush = Brushes.Gray;
                        txtShiftEnds1.BorderBrush = Brushes.Gray;
                        btnSaveDateTimeOrWeekDay.Focus();

                    }

                }

            }

        }

        // - canvasTimeDate






        // - groupboxDeduction

        // - - btnclosegroupDeduction
        private void btnclosegroupDeduction_click(object sender, RoutedEventArgs e)
        {
            txtNameOfDeduction.Text = "Name of deduction";
            txtgroupDeductionAmount.Text = "Amount";

            AlustaAddBenefitsAndSalary();
            canvasHowToStart.Visibility = Visibility.Visible;
            canvasSalaryAndAddBenefits.Visibility = Visibility.Visible;
            groupboxDeduction.Visibility = Visibility.Hidden;


        }


        // - - btnSaveDeduction
        private void btnSaveDeduction_klick(object sender, RoutedEventArgs e)
        {
            
            ManualDeduction = new ManualDeduction();

            if (txtNameOfDeduction.Text == "Name of deduction" || txtNameOfDeduction.Text == string.Empty)
            {
                MessageBox.Show("Please give deduction a name");
                return;
            }


            if (txtgroupDeductionAmount.Text == string.Empty)
            {
                MessageBox.Show("Deduction amount typed wrong", "Error");
                txtgroupDeductionAmount.Text = "Amount";
                txtgroupDeductionAmount.Focus();
                return;

            }

            ChangeDotIfNeeded(txtgroupDeductionAmount);

            decimal amount;

            bool ok = decimal.TryParse(txtgroupDeductionAmount.Text, out amount);
            if (!ok)
            {
                return;

            }
            
            if (radioDeductedHourly.IsChecked == true)
            {
                ManualDeduction.OurlyOrOnce = 0;


            }
            else if (radioDeductedOnce.IsChecked == true)
            {
                ManualDeduction.OurlyOrOnce = 1;

            }
            else
            {
                MessageBox.Show("Select weather the deduction is hourly or deducted once", "Error");
                return;

            }
            

            ManualDeduction.Amount = amount;
            ManualDeduction.Amount = Math.Round((decimal)amount, 3);

            ManualDeduction.Name = txtNameOfDeduction.Text;
            ManualDeduction = Repo.CreateAndGetManualDeduction(ManualDeduction.Name, ManualDeduction.Amount, ManualDeduction.OurlyOrOnce, Customer.Job.ContractID);

            Customer.Job.MDeductions.Add(ManualDeduction);
            comManualDeductions.ItemsSource = Customer.Job.MDeductions;


            AlustaAddBenefitsAndSalary();
            canvasHowToStart.Visibility = Visibility.Visible;
            canvasSalaryAndAddBenefits.Visibility = Visibility.Visible;
            groupboxDeduction.Visibility = Visibility.Hidden;

            MessageBox.Show("Deduction saved");


        }


        // - - txtNameOfDeduction
        private void txtNameOfDeduction_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtNameOfDeduction.Text == "Name of deduction")
            {
                txtNameOfDeduction.Text = string.Empty;

            }
            else if (txtNameOfDeduction.Text == string.Empty)
            {
                txtNameOfDeduction.Text = "Name of deduction";

            }
            if (txtgroupDeductionAmount.Text == string.Empty)
            {
                txtgroupBenefitAmount.Text = "Amount";

            }
        }
        private void txtNameOfDeduction_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                txtgroupDeductionAmount_GotFocus(sender, e);
            }
        }
        private void txtNameOfDeduction_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNameOfBenefit.Text == string.Empty)
            {
                txtNameOfBenefit.Text = "Name of deduction";

            }
        }

        // - - txtNameOfDeduction


        // - - txtgroupDeductionAmount
        private void txtgroupDeductionAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtgroupDeductionAmount.Text == "Amount")
            {
                txtgroupDeductionAmount.Text = string.Empty;

            }

            if (txtNameOfDeduction.Text == string.Empty)
            {
                txtNameOfDeduction.Text = "Name of deduction";

            }


        }

        private void txtgroupDeductionAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtgroupDeductionAmount.Text == string.Empty)
            {
                txtgroupDeductionAmount.Text = "Amount";
            }

        }

        private void txtgroupDeductionAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                btnSaveDeduction_klick(sender, e);
            }
        }
        // - - txtgroupDeductionAmount

        // - groupboxDeduction







        // - groupBenefit

        // - - txtNameOfBenefit
        private void txtNameOfBenefit_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtNameOfBenefit.Text == "Name of benefit")
            {
                txtNameOfBenefit.Text = string.Empty;

            }
            else if (txtNameOfBenefit.Text == string.Empty)
            {
                txtNameOfBenefit.Text = "Name of benefit";
            }

            if (txtgroupBenefitAmount.Text == string.Empty)
            {
                txtgroupBenefitAmount.Text = "Amount";
            }


        }
        private void txtNameOfBenefit_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNameOfBenefit.Text == string.Empty)
            {
                txtNameOfBenefit.Text = "Name of benefit";

            }

        }
        private void txtNameOfBenefit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                txtgroupBenefitAmount_GotFocus(sender, e);

            }

        }
        // - - txtNameOfBenefit


        // - - txtgroupBenefitAmount

        private void txtgroupBenefitAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtgroupBenefitAmount.Text == "Amount")
            {
                txtgroupBenefitAmount.Text = string.Empty;

            }
            if (txtNameOfBenefit.Text == string.Empty)
            {
                txtNameOfBenefit.Text = "Name of benefit";

            }
        }
        private void txtgroupBenefitAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtgroupBenefitAmount.Text == string.Empty)
            {
                txtgroupBenefitAmount.Text = "Amount";

            }

        }
        private void txtgroupBenefitAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                btnSaveBenefit_click(sender, e);
            }

        }
        // - - txtgroupBenefitAmount


        // - - btncloseGroupBenefit
        private void btncloseGroupBenefit_click(object sender, RoutedEventArgs e)
        {
            txtNameOfBenefit.Text = "Name of benefit";
            txtgroupBenefitAmount.Text = "Amount";

            AlustaAddBenefitsAndSalary();
            canvasHowToStart.Visibility = Visibility.Visible;
            canvasSalaryAndAddBenefits.Visibility = Visibility.Visible;
            groupBenefit.Visibility = Visibility.Hidden;

        }


        // - - btnSaveBenefit
        private void btnSaveBenefit_click(object sender, RoutedEventArgs e)
        {
            ManualBenefit = new ManualBenefit();

            if (txtNameOfBenefit.Text == "Name of amount" || txtNameOfBenefit.Text == string.Empty)
            {
                MessageBox.Show("Please give benefit a name");
                return;
            }

            ChangeDotIfNeeded(txtgroupBenefitAmount);

            decimal amount;
            bool ok = decimal.TryParse(txtgroupBenefitAmount.Text, out amount);
            if (!ok)
            {
                txtgroupBenefitAmount.Focus();
                return;
            }

           

            if (radioPaidHourly.IsChecked == true)
            {
                ManualBenefit.OurlyOrOnce = 0;


            }
            else if (radioPaidOnce.IsChecked == true)
            {
                ManualBenefit.OurlyOrOnce = 1;

            }
            else
            {
                MessageBox.Show("Select weather the benefit is paid hourly or once", "Error");
                return;
            }

            ManualBenefit.Amount = Math.Round(amount, 3);

            ManualBenefit.Name = txtNameOfBenefit.Text;

            ManualBenefit.Name = txtNameOfBenefit.Text;
            ManualBenefit = Repo.CreateAndGetManualBenefit(ManualBenefit.Name, ManualBenefit.Amount, ManualBenefit.OurlyOrOnce, Customer.Job.ContractID);

            Customer.Job.MBenefits.Add(ManualBenefit);
            comManualBenefits.ItemsSource = Customer.Job.MBenefits;


            AlustaAddBenefitsAndSalary();
            canvasHowToStart.Visibility = Visibility.Visible;
            canvasSalaryAndAddBenefits.Visibility = Visibility.Visible;
            groupBenefit.Visibility = Visibility.Hidden;

            MessageBox.Show("Benefit saved");

        }


        // - groupBenefit







        // - groupOvertime

        // - - txtOvertimeAmount
        //                   ei metodeja vielä



        // - - comHours
        private void comhours_dropDownClosed(object sender, EventArgs e)
        {
            comminutes.SelectedIndex = 0;

        }



        // - - comMinutes



        // - - radioShift
        //                 ei metodeja vielä 



        // - - radioweek
        //                 ei metodeja vielä 



        // - - radioMonth
        //                 ei metodeja vielä 



        //  - - btnOvertimeSave
        private void btnOvertimeSave_Click(object sender, RoutedEventArgs e)
        {
            OvertimeBenefit = new OvertimeBenefit();

            if (txtOvertimeAmount.Text == string.Empty)
            {
                MessageBox.Show("Please type in the amount of overtime benefit (per hour) you get paid.", "Error");
                txtOvertimeAmount.Focus();
                return;

            }
            decimal overtimeAmount;
            bool ok = decimal.TryParse(txtOvertimeAmount.Text, out overtimeAmount);
            if (!ok)
            {
                MessageBox.Show("Please type in the amount of overtime benefit (per hour) you get paid correctyly.", "Error");
                txtOvertimeAmount.Text = string.Empty;
                txtOvertimeAmount.Focus();
                return;


            }
            else
            {
                OvertimeBenefit.BenefitAmount = Math.Round(overtimeAmount, 3);
            }
            if (comhours.SelectedIndex == -1)
            {
                MessageBox.Show("Please select the hours and minutes of work when overtime benefit is set to start", "Error");
                comhours.Focus();
                return;
            }
            if (comminutes.SelectedIndex == -1)
            {
                MessageBox.Show("Please select the minutes of work when overtime benefit is set to start", "Error");
                comminutes.Focus();
                return;
            }

           
            var hours = "";
            var minutes = "";

            if (comhours.SelectedItem.ToString().Length == 1)
            {
                hours = "0";
                hours += comhours.SelectedItem.ToString();


            }
            else
            {
                hours = comhours.SelectedItem.ToString();
            }
            if (comminutes.SelectedItem.ToString().Length == 1)
            {
                minutes = "0";
                minutes += comminutes.SelectedItem.ToString();

            }
            if (radioShift.IsChecked == false && radioWeek.IsChecked == false && radioMonth.IsChecked == false)
            {
                MessageBox.Show("Please select timeperiod", "Error");
                var shift = "during one shift"; var week = "in a week"; var month = "in a month";

                MessageBox.Show($"Please select wether the benefit starts after {hours}:{minutes} of work {shift} / {week} / {month}", "Error");

                return;

            }


            
            hours = comhours.SelectedItem.ToString();
            minutes = comminutes.SelectedItem.ToString();
            int intHours;
            int intMinutes;
            ok = int.TryParse(hours, out intHours);
            if (!ok)
            {
                MessageBox.Show("Error", "Error");
                return;

            }
            ok = int.TryParse(minutes, out intMinutes);
            if (!ok)
            {
                MessageBox.Show("Error", "Error");
                return;

            }
            TimeSpan time = new TimeSpan(intHours, intMinutes, 0);

            OvertimeBenefit.Time = time;


            if (radioShift.IsChecked == true)
            {
                OvertimeBenefit.TimePeriod = 0;

            }
            else if (radioWeek.IsChecked == true)
            {
                OvertimeBenefit.TimePeriod = 1;

            }
            else if (radioMonth.IsChecked == true)
            {
                OvertimeBenefit.TimePeriod = 2;

            }
            bool löytyy = Repo.CheckIfOvertimeBenefitTimeSpanExists(Customer.Job.ContractID, OvertimeBenefit.TimePeriod);
            if (löytyy)
            {
                var word = "";
                if (OvertimeBenefit.TimePeriod == 0)
                {
                    word = "shift";

                }
                else if (OvertimeBenefit.TimePeriod == 1)
                {
                    word = "week";

                }
                else if (OvertimeBenefit.TimePeriod == 2)
                {
                    word = "month";

                }
                MessageBox.Show($"You can only have one {word}-overtime benefit", "Error");
                return;
            }

            Repo.CreateOvertimeBenefit(OvertimeBenefit.TimePeriod, OvertimeBenefit.Time, OvertimeBenefit.BenefitAmount, Customer.Job.ContractID);

            OvertimeBenefit = Repo.GetOvertimeBenefitExists(OvertimeBenefit.TimePeriod, OvertimeBenefit.Time, OvertimeBenefit.BenefitAmount);

            Debug.WriteLine(OvertimeBenefit.OvertimeInfo + ",  " + "ID:" + OvertimeBenefit.ID);

            Customer.Job.Overtimebenefits.Add(OvertimeBenefit);

            Repo.AddToTableJobIdAndContractIDOvertimeBenefi(Customer.Job.ContractID, OvertimeBenefit.ID);

            comOvertimeBenefit.ItemsSource = Customer.Job.Overtimebenefits;

            groupOvertime.Visibility = Visibility.Hidden;
            canvasSalaryAndAddBenefits.Visibility = Visibility.Visible;

            MessageBox.Show("Benefit saved");

        }


        // - - btnReturnFromOvertime
        private void btnReturnFromOvertime_click(object sender, RoutedEventArgs e)
        {
            canvasSalaryAndAddBenefits.Visibility = Visibility.Visible;
            groupOvertime.Visibility = Visibility.Hidden;

        }


        // - groupOvertime


        //CANVAS HOW TO START




































        // CANVAS_TYPE_IN_YOUR_SHIFTS

        // - btnModifySalary
        private void btnModifySalary_click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(canvasCalendarShiftAndAndShiftStartAndEnd, 30);

            canvasManualBenefitManualDeduction.Children.Clear();

            Canvas.SetLeft(canvasCalendarShiftAndAndShiftStartAndEnd, 204);

            AlustaHowTostart();

        }


        // - calendarShift
        private void calendarKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("calendarKeyDown");

            if (e.Key == Key.Back)
            {
                txtShiftStarts.Focus();
                txtShiftStarts_P_KDown(txtShiftStarts, e);
                e.Handled = true;
                return;

            }


            if (e.Key.ToString().Length == 1)
            {
                e.Handled = true; return;

            }

            else if (e.Key.ToString().Length == 2)
            {

                string twoChars = e.Key.ToString();
                char c = twoChars[1];
                bool ok = int.TryParse(c.ToString(), out int value);

                if (ok)
                {
                    txtShiftStarts.Focus();
                    txtShiftStarts_P_KDown(txtShiftStarts, e);

                }
                else
                {
                    e.Handled = true; return;
                }

            }

        }
        private void calendarShift_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            txtShiftStarts.Text = "- - : - -";
            txtShiftEnds.Text = "- - : - -";
            listStarts.Clear();
            ListEnds.Clear();
            txtShiftStarts.BorderBrush = Brushes.Black;

        }

        // - calendarShift


        // - txtShiftStarts
        private void txtShiftStartsGotFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("txtShiftStarts_GotFocus");
            txtShiftStarts.BorderBrush = Brushes.Black;
            txtShiftEnds.BorderBrush = Brushes.White;

            txtShiftStarts.Text = string.Empty;
            listStarts.Clear();
        }
        private void txtShiftStarts_P_KDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key.ToString());
            if (e.Key == Key.Right)
            {
                var dateshift = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDate);
                dateshift = dateshift.AddDays(1);
                calendarShift.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Left)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDate);
                dateshift = dateshift.AddDays(-1);
                calendarShift.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Down)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDate);
                dateshift = dateshift.AddDays(7);
                calendarShift.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Up)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDate);
                dateshift = dateshift.AddDays(-7);
                calendarShift.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Back)
            {
                listStarts.Clear();
                txtShiftStarts.Text = string.Empty;
                return;
            }
            if (e.Key.ToString().Length == 1)
            {
                e.Handled = true; return;

            }

            else if (e.Key.ToString().Length == 2)
            {             
                string twoChars = e.Key.ToString();
                char c = twoChars[1];
                bool ok = int.TryParse(c.ToString(), out int value);

                if (ok)
                {
                    if (listStarts.Count == 4)
                    {
                        txtShiftEnds.Focus();
                        e.Handled = true;
                        return;

                    }
                    
                    if (listStarts.Count == 0)
                    {
                        if (value == 0 || value == 1 || value == 2)
                        {
                            listStarts.Add(value);
                            Debug.WriteLine(listStarts.Count.ToString());

                            return;
                        }
                        else
                        {
                            

                        }

                    }
                    if (listStarts.Count == 1)
                    {
                        if (value < 10)
                        {
                            listStarts.Add(value);
                            Debug.WriteLine(listStarts.Count.ToString());
                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }

                    }
                    if (listStarts.Count == 2)
                    {
                        AddDoubleDot(txtShiftStarts);
                        if (value < 6)
                        {

                     
                            listStarts.Add(value);
                            Debug.WriteLine(listStarts.Count.ToString());


                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }


                    }
                    if (listStarts.Count == 3)
                    {

                        if (value < 10)
                        {
                            listStarts.Add(value);
                            Debug.WriteLine(listStarts.Count.ToString());
                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }

                    }

                }
                else
                {
                    e.Handled = true;
                    return;
                }

            }

        }
        private void tztShiftStarts_PKup(object sender, KeyEventArgs e)
        {
           

            if (e.Key.ToString().Length == 2)
            {

                string twoChars = e.Key.ToString();
                char c = twoChars[1];
                bool ok = int.TryParse(c.ToString(), out int value);

                if (ok)
                {
                    if (listStarts.Count == 2)
                    {
                        AddDoubleDot(txtShiftStarts);
                        e.Handled = true;
                        return;             

                    }
                    else if (listStarts.Count == 4)
                    {
                        txtShiftEnds.Focus();
                        e.Handled = true;
                        return;

                    }
                    else
                    {
                        e.Handled = true;
                        return;

                    }

                }
            }
            


        }

        // - txtShiftStarts


        // - txtShiftEnds
        private void txtShiftEnds_GotFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("txtShiftEnds_GotFocus");
            txtShiftEnds.BorderBrush = Brushes.Black;
            txtShiftStarts.BorderBrush = Brushes.White;
            txtShiftEnds.Text = string.Empty;
            ListEnds.Clear();

        }
        private void txtShiftEnds_P_KDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDate);
                dateshift = dateshift.AddDays(1);
                calendarShift.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Left)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDate);
                dateshift = dateshift.AddDays(-1);
                calendarShift.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Down)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDate);
                dateshift = dateshift.AddDays(7);
                calendarShift.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Up)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDate);
                dateshift = dateshift.AddDays(-7);
                calendarShift.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (ListEnds.Count == 4 && e.Key == Key.Enter)
            {
                //btnSaveShift_click(sender, e);
                return;

            }
            if (e.Key == Key.Back)
            {
                if (ListEnds.Count == 0)
                {
                    txtShiftEnds.Text = "- - : - -";
                    txtShiftStarts.Focus();

                }
                else
                {
                    ListEnds.Clear();
                    txtShiftEnds.Text = string.Empty;
                }

            }
            if (e.Key.ToString().Length == 1)
            {
                e.Handled = true; return;

            }
            else if (e.Key.ToString().Length == 2)
            {

                string twoChars = e.Key.ToString();
                char c = twoChars[1];
                bool ok = int.TryParse(c.ToString(), out int value);

                if (ok)
                {
                    if (ListEnds.Count == 4)
                    {
                        e.Handled = true;
                        return;
                    }

                    if (ListEnds.Count == 0)
                    {
                        if (value == 0 || value == 1 || value == 2)
                        {
                            ListEnds.Add(value);
                            Debug.WriteLine(ListEnds.Count.ToString());

                            return;
                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }

                    }
                    if (ListEnds.Count == 1)
                    {
                        if (value < 10)
                        {
                            ListEnds.Add(value);
                            Debug.WriteLine(ListEnds.Count.ToString());


                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }

                    }
                    if (ListEnds.Count == 2)
                    {
                        if (txtShiftEnds.Text[txtShiftEnds.Text.Length - 1] != ':')
                        {
                            AddDoubleDot(txtShiftEnds);
                        }

                        if (value < 6)
                        {
                            ListEnds.Add(value);
                            Debug.WriteLine(ListEnds.Count.ToString());
                           
                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }


                    }
                    if (ListEnds.Count == 3)
                    {
                      
                        if (value < 10)
                        {
                            ListEnds.Add(value);
                            Debug.WriteLine(ListEnds.Count.ToString());
                            
                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }



                    }





                }
                else
                {
                    e.Handled = true;
                    return;
                }

            }
        }
        private void txtShiftsEnds_P_KUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().Length == 2)
            {

                string twoChars = e.Key.ToString();
                char c = twoChars[1];
                bool ok = int.TryParse(c.ToString(), out int value);

                if (ok)
                {
                    if (ListEnds.Count == 2)
                    {
                        if (value < 6)
                        {

                            AddDoubleDot(txtShiftEnds);
                            e.Handled = true;
                            return;

                        }
                        else
                        {
                            e.Handled = true;
                            return;

                        }


                    }
                }
            }
        }

        // - txtShiftEnds


        // - btnSaveShift
        private void btnSaveShift_KDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                txtShiftEnds.Focus();
               
            }
            if (e.Key == Key.Right)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDate);
                dateshift = dateshift.AddDays(1);
                calendarShift.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Left)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDate);
                dateshift = dateshift.AddDays(-1);
                calendarShift.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Down)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDate);
                dateshift = dateshift.AddDays(7);
                calendarShift.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Up)
            {

                var dateshift = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDate);
                dateshift = dateshift.AddDays(-7);
                calendarShift.SelectedDate = new DateTime(dateshift.Year, dateshift.Month, dateshift.Day);
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Back)
            {
                txtShiftEnds.Focus();
                txtShiftEnds.BorderBrush = Brushes.Black;

            }
        }
        private void btnSaveShift_click(object sender, RoutedEventArgs e)
        {
           
            TimeSpan timezero = new TimeSpan(0, 0, 0);


            if (txtShiftStarts.Text == "- - : - -" || txtShiftStarts.Text == string.Empty || txtShiftStarts.Text.Length != 5)
            {
                MessageBox.Show("Incorrect start time", "Error");
                return;
            }

            var stringEnd = txtShiftEnds.Text;
            if (txtShiftEnds.Text == "- - : - -" || txtShiftEnds.Text == string.Empty || txtShiftEnds.Text.Length != 5)
            {
                MessageBox.Show("Incorrect end time", "Error");
                return;
            }

            var start = GetTime(txtShiftStarts);
            var end = GetTime(txtShiftEnds);
            TimeSpan midnight = new TimeSpan(24, 0, 0);
            if (end > midnight)
            {
                MessageBox.Show("Incorrect end time", "Error");
                return;

            }
            TimeSpan checkDifference = OvertimeBenefit.GetDifference(start, end);
            TimeSpan twelvehours = new TimeSpan(16, 0, 0);
            if (checkDifference>twelvehours)
            {
                var result = MessageBox.Show("Are you sure your shift is over 16 hours?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {

                    return;
                }
               

            }

            ShiftInfo.Start = start;
            ShiftInfo.End = end;


            if (ShiftInfo.End == timezero)
            {

                ShiftInfo.End = midnight;

            }

            // tähän mennessä on todennettu että vuoron aloitus ja lopetus on syötetty oikein.

            if (calendarShift.SelectedDate == null)
            {
                MessageBox.Show("Please select the day from calendar");
                return;

            }
            if (calendarShift.SelectedDates.Count > 1)
            {
                for (int i = 0; i < calendarShift.SelectedDates.Count; i++)
                {
                    ShiftInfo.Date = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDates[i]);
                    AllShifts.Add(ShiftInfo);
                }

            }
            else
            {
                ShiftInfo.Date = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDate);
                AllShifts.Add(ShiftInfo);

            }

            // tähän mennessä on saatu ShiftInfo ilmentymän ominaisuuksille arvot. 

            decimal value = 0;
            decimal benefitValue = 0;
            decimal deductionValue = 0;
            decimal salaryValue = 0;



            for (int i = 0; i < AllShifts.Count; i++)
            {

                if (i == (AllShifts.Count - 1))
                {

                    Debug.WriteLine(Customer.Job.SalaryPerHour);
                    AllShifts[i].Salary = AllShifts[i].Difference * Customer.Job.SalaryPerHour;

                    AllShifts[i].Day = (int)AllShifts[i].Date.DayOfWeek;

                    foreach (var item in CheckboxesBenefit)
                    {
                        if (item.IsChecked == true)
                        {
                            ManualBenefit = new ManualBenefit();
                            ManualBenefit = (ManualBenefit)item.Tag;

                            AllShifts[i].manualBenefits.Add(ManualBenefit);

                            if (ManualBenefit.OurlyOrOnce == 0)
                            {
                                AllShifts[i].ManualBenefitAmount += ManualBenefit.Amount * AllShifts[i].Difference;

                            }
                            else if (ManualBenefit.OurlyOrOnce == 1)
                            {
                                AllShifts[i].ManualBenefitAmount += ManualBenefit.Amount;

                            }
                            else
                            {
                                MessageBox.Show("Error - 2990");
                            }




                        }


                    }
                    foreach (var item in CheckboxesDeduction)
                    {
                        if (item.IsChecked == true)
                        {
                            ManualDeduction = new ManualDeduction();
                            ManualDeduction = (ManualDeduction)item.Tag;

                            AllShifts[i].manualDeductions.Add(ManualDeduction);

                            if (ManualDeduction.OurlyOrOnce == 0)
                            {
                                AllShifts[i].ManualDeductionAmount += ManualDeduction.Amount * AllShifts[i].Difference;

                            }
                            else if (ManualDeduction.OurlyOrOnce == 1)
                            {
                                AllShifts[i].ManualDeductionAmount += ManualDeduction.Amount;

                            }
                            else
                            {
                                MessageBox.Show("Error - 2990");
                            }


                        }
                    }

                    TimeSpan benefitStart = new TimeSpan(0, 0, 0);

                    TimeSpan benefitEnd = new TimeSpan(0, 0, 0);

                    foreach (var item in Customer.Job.WeeklyBenefits)
                    {
                        WeeklyBenefit = new WeeklyBenefit();

                        if (item.dayOfWeek == AllShifts[i].Day)
                        {
                            Debug.WriteLine(item.ID);


                            if (AllShifts[i].Start <= item.Start && AllShifts[i].End > item.Start)
                            {

                                benefitStart = item.Start;
                                Debug.WriteLine(benefitStart);
                                if (AllShifts[i].End >= item.End)
                                {
                                    benefitEnd = item.End;
                                    Debug.WriteLine(benefitEnd);
                                }
                                else if (AllShifts[i].End < item.End)
                                {
                                    benefitEnd = AllShifts[i].End;
                                    Debug.WriteLine(benefitEnd);
                                }
                            }
                            if (AllShifts[i].Start > item.Start && AllShifts[i].Start < item.End)
                            {
                                benefitStart = AllShifts[i].Start;
                                Debug.WriteLine(benefitStart);
                                if (AllShifts[i].End <= item.End)
                                {
                                    benefitEnd = AllShifts[i].End;
                                    Debug.WriteLine(benefitEnd);

                                }
                                else if (AllShifts[i].End > item.End)
                                {
                                    benefitEnd = item.End;
                                    Debug.WriteLine(benefitEnd);
                                }
                            }


                            if (AllShifts[i].End < AllShifts[i].Start)
                            {
                                benefitEnd = item.End;
                            }

                            if (benefitEnd != timezero)
                            {

                                WeeklyBenefit.dayOfWeek = item.dayOfWeek; WeeklyBenefit.Amount = item.Amount; WeeklyBenefit.Start = benefitStart; WeeklyBenefit.End = benefitEnd; WeeklyBenefit.ID = item.ID;
                                AllShifts[i].WeeklyBenefitList.Add(WeeklyBenefit);

                            }
                        }


                    }

                    foreach (var item in Customer.Job.BenefitsOnADate)
                    {
                        Debug.WriteLine(item);

                        if (AllShifts[i].Date == item.Date)
                        {
                            BenefitOnADate = new BenefitOnADate();
                            if (AllShifts[i].Start <= item.Start && AllShifts[i].End > item.Start)
                            {
                                benefitStart = item.Start;
                                Debug.WriteLine(benefitStart);
                                if (AllShifts[i].End >= item.End)
                                {
                                    benefitEnd = item.End;
                                    Debug.WriteLine(benefitEnd);
                                }
                                else if (AllShifts[i].End < item.End)
                                {
                                    benefitEnd = AllShifts[i].End;
                                    Debug.WriteLine(benefitEnd);
                                }
                            }
                            if (AllShifts[i].Start > item.Start && AllShifts[i].Start < item.End)
                            {
                                benefitStart = AllShifts[i].Start;
                                Debug.WriteLine(benefitStart);
                                if (AllShifts[i].End <= item.End)
                                {
                                    benefitEnd = AllShifts[i].End;
                                    Debug.WriteLine(benefitEnd);

                                }
                                else if (AllShifts[i].End > item.End)
                                {
                                    benefitEnd = item.End;
                                    Debug.WriteLine(benefitEnd);
                                }
                            }
                            if (AllShifts[i].End < AllShifts[i].Start)
                            {
                                benefitEnd = item.End;
                            }

                            if (benefitEnd != timezero)
                            {
                                BenefitOnADate.Date = item.Date; BenefitOnADate.ID = item.ID; BenefitOnADate.Amount = item.Amount; BenefitOnADate.Start = benefitStart; BenefitOnADate.End = benefitEnd;
                                AllShifts[i].BenefitOnADateList.Add(BenefitOnADate);

                            }



                        }

                    }

                    foreach (var item in AllShifts[i].WeeklyBenefitList)
                    {
                        AllShifts[i].WeeklyBenefitAmount += AllShifts[i].GetWeeklyBenefitAmount(item.Start, item.End, item.Amount);
                    }


                    foreach (var item in AllShifts[i].BenefitOnADateList)
                    {
                        Debug.WriteLine(item);
                        AllShifts[i].BenefitOnADateAmount += AllShifts[i].GetBenefitOnADateAmount(item.Start, item.End, item.Amount);
                    }

                    foreach (var item in Customer.Job.Overtimebenefits)
                    {
                        if (item.TimePeriod == 0)
                        {
                            OvertimeBenefit = OvertimeBenefit.returnPossibleShiftsOvertimeBenefits(item, AllShifts[i].Start, AllShifts[i].End);
                            AllShifts[i].Overtimebenefits.Add(OvertimeBenefit);
                            Debug.WriteLine("Overtime shift benefit:  " + OvertimeBenefit.ResultShiftAmount);

                        }
                        else if (item.TimePeriod == 1)
                        {
                            OvertimeBenefit = OvertimeBenefit.AddnewWeek(item, AllShifts[i].Date, AllShifts[i].Start, AllShifts[i].End);
                            AllShifts[i].Overtimebenefits.Add(OvertimeBenefit);
                            AllShifts[i].WeekinfoTime = OvertimeBenefit.returnWeekInfoTime(OvertimeBenefit, AllShifts[i].Date, AllShifts[i].Start, AllShifts[i].End);
                            Debug.WriteLine("Time: " + AllShifts[i].WeekinfoTime);
                        }
                        else if (item.TimePeriod == 2)
                        {
                            OvertimeBenefit = OvertimeBenefit.AddnewMonth(item, AllShifts[i].Date, AllShifts[i].Start, AllShifts[i].End);
                            AllShifts[i].Overtimebenefits.Add(OvertimeBenefit);
                            AllShifts[i].MonthinfoTime = OvertimeBenefit.returnMonthInfoTime(OvertimeBenefit, AllShifts[i].Date, AllShifts[i].Start, AllShifts[i].End);
                            Debug.WriteLine("Time: " + AllShifts[i].MonthinfoTime);

                        }
                    }
                    foreach (var item in AllShifts[i].Overtimebenefits)
                    {
                        if (item.TimePeriod == 0)
                        {
                            AllShifts[i].OvertimeShiftAmount += item.ResultShiftAmount;

                        }
                        else if (item.TimePeriod == 1)
                        {
                            AllShifts[i].OvertimeWeekAmount += item.BenefitAmount * AllShifts[i].WeekinfoTime;
                        }
                        else if (item.TimePeriod == 2)
                        {
                            AllShifts[i].OvertimeMonthAmount += item.BenefitAmount * AllShifts[i].MonthinfoTime;

                        }

                    }

                }
                  
              

            }

            txtSalaryInfo.Text = string.Empty;

            for (int i = 0; i < AllShifts.Count; i++)
            {
                benefitValue += AllShifts[i].OvertimeShiftAmount + AllShifts[i].OvertimeWeekAmount + AllShifts[i].OvertimeMonthAmount + AllShifts[i].WeeklyBenefitAmount + AllShifts[i].BenefitOnADateAmount + AllShifts[i].ManualBenefitAmount;
                deductionValue += AllShifts[i].ManualDeductionAmount;
                salaryValue += AllShifts[i].Salary;

                txtSalaryInfo.Text += AllShifts[i].ToString();

            }



            var decimalBenefitValue = Math.Round(benefitValue, 2);
            var decimaldeductionValue = Math.Round(deductionValue, 2);
            var decimalSalaryValue = Math.Round(salaryValue, 2);

            value = salaryValue + benefitValue - deductionValue;
            var decimalMath = Math.Round((decimal)value, 2);

            txtSalaryInfo.Text += "\n\n -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -\n";


            if (decimaldeductionValue > 0 && decimalBenefitValue > 0)
            {
                txtSalaryInfo.Text += $"\n(Hourly salary){decimalSalaryValue} + (Benefits){decimalBenefitValue} - (Deductions){decimaldeductionValue}\nSalary: {decimalMath} ";

            }
            else if (decimaldeductionValue > 0 && decimalBenefitValue == 0)
            {
                txtSalaryInfo.Text += $"\n(Hourly salary){decimalSalaryValue} - (Deductions){decimaldeductionValue}\nSalary:  {decimalMath}";

            }
            else if (decimalBenefitValue > 0 && decimaldeductionValue == 0)
            {
                txtSalaryInfo.Text += $"\n(Hourly salary){decimalSalaryValue} + (Benefits){decimalBenefitValue}\nSalary:  {decimalMath}";

            }
            else if (decimalBenefitValue == 0 && decimaldeductionValue == 0)
            {
                txtSalaryInfo.Text += $"\nSalary:  {decimalMath}";


            }
            txtSalaryInfo.Text += "\n";


            txtSalaryInfo.ScrollToEnd();


            ShiftInfo = new ShiftInfo();
       

            txtSalaryInfo.ScrollToEnd();

            ShiftInfo = new ShiftInfo();

            UncheckCheckboxes();

            DateOnly date = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDates[calendarShift.SelectedDates.Count - 1]);
            DateOnly datePlus1 = date.AddDays(1);
            DateTime dateTime = new DateTime(datePlus1.Year, datePlus1.Month, datePlus1.Day);
            calendarShift.SelectedDate = dateTime;

            ResetCalendar();


            



        }
        private void btnRemoveLatestShift_click(object sender, RoutedEventArgs e)
        {
            bool löytyy = false;
            foreach (var shift in AllShifts)
            {
                if (shift.Salary > 0)
                {
                    löytyy = true;
                    break;
                }

            }
            if (löytyy == false)
            {
                return;
            }

            if (AllShifts.Count > 0)
            {

                decimal value = 0;
                decimal benefitValue = 0;
                decimal deductionValue = 0;
                decimal salaryValue = 0;

                txtSalaryInfo.Text = string.Empty;


                for (int a = AllShifts[AllShifts.Count - 1].Overtimebenefits.Count - 1; a > -1; a--)
                {
                    if (AllShifts[AllShifts.Count - 1].Overtimebenefits[a].TimePeriod == 1)
                    {
                        AllShifts[AllShifts.Count - 1].Overtimebenefits[a].WeekList = OvertimeBenefit.RemoveWeek(AllShifts[AllShifts.Count - 1].Overtimebenefits[a].WeekList, AllShifts[AllShifts.Count - 1].Date, AllShifts[AllShifts.Count - 1].Start, AllShifts[AllShifts.Count - 1].End);

                    }
                    else if (AllShifts[AllShifts.Count - 1].Overtimebenefits[a].TimePeriod == 2)
                    {
                        AllShifts[AllShifts.Count - 1].Overtimebenefits[a].MonthsList = OvertimeBenefit.RemoveMonth(AllShifts[AllShifts.Count - 1].Overtimebenefits[a].MonthsList, AllShifts[AllShifts.Count - 1].Date, AllShifts[AllShifts.Count - 1].Start, AllShifts[AllShifts.Count - 1].End);


                    }


                }

                foreach (var item in AllShifts[AllShifts.Count - 1].Overtimebenefits)
                {
                    if (item.TimePeriod == 0)
                    {
                        AllShifts[AllShifts.Count - 1].OvertimeShiftAmount += item.ResultShiftAmount;

                    }
                    else if (item.TimePeriod == 1)
                    {
                        AllShifts[AllShifts.Count - 1].OvertimeWeekAmount += item.BenefitAmount * AllShifts[AllShifts.Count - 1].WeekinfoTime;
                    }
                    else if (item.TimePeriod == 2)
                    {
                        AllShifts[AllShifts.Count - 1].OvertimeMonthAmount += item.BenefitAmount * AllShifts[AllShifts.Count - 1].MonthinfoTime;

                    }
                }

                AllShifts.Remove(AllShifts[AllShifts.Count - 1]);


                for (int i = 0; i < AllShifts.Count; i++)
                {

                    benefitValue += AllShifts[i].OvertimeShiftAmount + AllShifts[i].OvertimeWeekAmount + AllShifts[i].OvertimeMonthAmount + AllShifts[i].WeeklyBenefitAmount + AllShifts[i].BenefitOnADateAmount + AllShifts[i].ManualBenefitAmount;
                    deductionValue += AllShifts[i].ManualDeductionAmount;
                    salaryValue += AllShifts[i].Salary;

                    txtSalaryInfo.Text += AllShifts[i].ToString();


                }


                var decimalBenefitValue = Math.Round((decimal)benefitValue, 2);
                var decimaldeductionValue = Math.Round((decimal)deductionValue, 2);
                var decimalSalaryValue = Math.Round((decimal)salaryValue, 2);

                value = salaryValue + benefitValue - deductionValue;
                var decimalMath = Math.Round((decimal)value, 2);

                txtSalaryInfo.Text += "\n\n -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -\n";
                
                
                if (decimaldeductionValue > 0 && decimalBenefitValue > 0)
                {
                    txtSalaryInfo.Text += $"\n(Hourly salary){decimalSalaryValue} + (Benefits){decimalBenefitValue} - (Deductions){decimaldeductionValue}\nSalary: {decimalMath} ";

                }
                else if (decimaldeductionValue > 0 && decimalBenefitValue == 0)
                {
                    txtSalaryInfo.Text += $"\n(Hourly salary){decimalSalaryValue} - (Deductions){decimaldeductionValue}\nSalary:  {decimalMath}";

                }
                else if (decimalBenefitValue > 0 && decimaldeductionValue == 0)
                {
                    txtSalaryInfo.Text += $"\n(Hourly salary){decimalSalaryValue} + (Benefits){decimalBenefitValue}\nSalary:  {decimalMath}";

                }
                else if (decimalBenefitValue == 0 && decimaldeductionValue == 0)
                {
                    txtSalaryInfo.Text += $"\nSalary:  {decimalMath}";


                }
                txtSalaryInfo.Text += "\n";


                txtSalaryInfo.ScrollToEnd();


                ShiftInfo = new ShiftInfo();
            }





            DateOnly date = DateOnly.FromDateTime((DateTime)calendarShift.SelectedDates[calendarShift.SelectedDates.Count - 1]);
            DateOnly datePlus1 = date.AddDays(-1);
            DateTime dateTime = new DateTime(datePlus1.Year, datePlus1.Month, datePlus1.Day);
            calendarShift.SelectedDate = dateTime;

            ResetCalendar();


            
            // methods for the canvas


        }
        public void InitialiseCanvasTypeInYourShifts()
        {
            Canvas.SetLeft(lblCYS, 50);
            Canvas.SetTop(lblCYS, 20);
            canvasManualBenefitManualDeduction.Children.Clear();

            Canvas.SetLeft(canvasCalendarShiftAndAndShiftStartAndEnd, 42);
            Canvas.SetLeft(canvasManualBenefitManualDeduction, 22);
            Canvas.SetLeft(canvasSalaryTextblock, 282);

            calendarShift.SelectedDate = DateTime.Now;

            Label lbl_T_Y_S_deduction = new Label();
            lbl_T_Y_S_deduction.Content = "Deduction";
            canvasManualBenefitManualDeduction.Children.Add(lbl_T_Y_S_deduction);


            Label lbl_T_Y_S_Keepthedeductionon = new Label();
            lbl_T_Y_S_Keepthedeductionon.Content = "Keep it on";
            canvasManualBenefitManualDeduction.Children.Add(lbl_T_Y_S_Keepthedeductionon);
            Canvas.SetLeft(lbl_T_Y_S_Keepthedeductionon, 150);

            Label lbl_T_Y_S_Benefits = new Label();
            lbl_T_Y_S_Benefits.Content = "Benefit";
            canvasManualBenefitManualDeduction.Children.Add(lbl_T_Y_S_Benefits);
            Canvas.SetTop(lbl_T_Y_S_Benefits, 135);

            Label lbl_T_Y_S_KeepTheBenefitOn = new Label();
            lbl_T_Y_S_KeepTheBenefitOn.Content = "Keep it on";
            canvasManualBenefitManualDeduction.Children.Add(lbl_T_Y_S_KeepTheBenefitOn);
            Canvas.SetTop(lbl_T_Y_S_KeepTheBenefitOn, 135); Canvas.SetLeft(lbl_T_Y_S_KeepTheBenefitOn, 150);


            lbl_T_Y_S_Benefits.Visibility = Visibility.Hidden;
            lbl_T_Y_S_deduction.Visibility = Visibility.Hidden;
            lbl_T_Y_S_KeepTheBenefitOn.Visibility = Visibility.Hidden;
            lbl_T_Y_S_Keepthedeductionon.Visibility = Visibility.Hidden;

            canvasTypeInYourShifts.Visibility = Visibility.Visible;
            calendarShift.Focus();
            bool manualbenefitLöytyy = Repo.CheckIfManualBenefitExists(Customer.Job.ContractID);
            bool manualdeductionLöytyy = Repo.CheckIfManualDeductionExists(Customer.Job.ContractID);
            if (!manualbenefitLöytyy && !manualdeductionLöytyy)
            {
                Canvas.SetTop(canvasCalendarShiftAndAndShiftStartAndEnd, 84);
                canvasManualBenefitManualDeduction.Visibility = Visibility.Hidden;
            }


            CheckboxesBenefit.Clear();
            CheckboxesBenefit2.Clear();
            CheckboxesDeduction2.Clear();
            CheckboxesDeduction.Clear();

            if (manualbenefitLöytyy)
            {
                lbl_T_Y_S_Benefits.Visibility = Visibility.Visible;
                lbl_T_Y_S_KeepTheBenefitOn.Visibility = Visibility.Visible;
                Customer.Job.MBenefits = Repo.GetCustomersManualBenefits(Customer.Job.ContractID);

                CanvasManualBenefits = new Canvas();
                ScrollViewerBenefit = new ScrollViewer();

                CanvasManualBenefits.Width = 50;
                CanvasManualBenefits.Height = 80;
                ScrollViewerBenefit.Width = 200;
                ScrollViewerBenefit.Height = 80;



                Grid grid = new Grid();
                var col = new ColumnDefinition();
                col.Width = new GridLength(160);
                grid.ColumnDefinitions.Add(col);
                grid.ColumnDefinitions.Add(new ColumnDefinition());

                StackPanel stackPanel0 = new StackPanel();
                StackPanel stackPanel1 = new StackPanel();

                int benefitlaskuri = 0; int KTBP_laskuri = 0;

                foreach (var item in Customer.Job.MBenefits)
                {
                    CheckBox checkBenefit = new CheckBox();
                    checkBenefit.Margin = new Thickness(0, 0, 2, 2); checkBenefit.FontSize = 10;
                    checkBenefit.Click += Checkbox_clicked;
                    checkBenefit.Tag = (ManualBenefit)item;   

                    CheckBox checkKeepTheButtonPressed = new CheckBox();
                    checkKeepTheButtonPressed.Margin = new Thickness(0, 0, 2, 2); checkKeepTheButtonPressed.FontSize = 10;
                    checkKeepTheButtonPressed.Click += Checkbox_clicked;
                    decimal amount = Math.Round(item.Amount, 2);
                    checkBenefit.Content = item.Name + " + " + amount + "";
                    if (item.OurlyOrOnce == 0)
                    {
                        checkBenefit.Content += " (hourly)";
                    }
                    checkBenefit.VerticalContentAlignment = VerticalAlignment.Center;

                    CheckboxesBenefit.Add(checkBenefit);
                    CheckboxesBenefit2.Add(checkKeepTheButtonPressed);

                    stackPanel0.Children.Add(checkBenefit);
                    stackPanel1.Children.Add(checkKeepTheButtonPressed);


                    benefitlaskuri++;
                    KTBP_laskuri++;

                }


                grid.Children.Add(stackPanel0);
                grid.Children.Add(stackPanel1);
                Grid.SetColumn(stackPanel1, 1);



                if (Customer.Job.MBenefits.Count > 5)
                {


                    ScrollViewerBenefit.Content = grid;

                    canvasManualBenefitManualDeduction.Children.Add(ScrollViewerBenefit);

                    if (manualdeductionLöytyy)
                    {
                        Canvas.SetLeft(ScrollViewerBenefit, 4);
                        Canvas.SetTop(ScrollViewerBenefit, 165);
                    }
                    else
                    {
                        Canvas.SetLeft(ScrollViewerBenefit, 4);
                        Canvas.SetTop(ScrollViewerBenefit, 100);

                        Canvas.SetTop(lbl_T_Y_S_KeepTheBenefitOn, 80); Canvas.SetLeft(lbl_T_Y_S_KeepTheBenefitOn, 150); Canvas.SetTop(lbl_T_Y_S_Benefits, 80);

                    }

                }
                else if (Customer.Job.MBenefits.Count == 1)
                {
                    CanvasManualBenefits.Children.Add(grid);
                    canvasManualBenefitManualDeduction.Children.Add(CanvasManualBenefits);
                    if (manualdeductionLöytyy)
                    {
                        Canvas.SetLeft(CanvasManualBenefits, 4);
                        Canvas.SetTop(CanvasManualBenefits, 165);
                    }
                    else
                    {
                        Canvas.SetLeft(CanvasManualBenefits, 4);
                        Canvas.SetTop(CanvasManualBenefits, 120);

                        Canvas.SetTop(lbl_T_Y_S_KeepTheBenefitOn, 90); Canvas.SetLeft(lbl_T_Y_S_KeepTheBenefitOn, 150); Canvas.SetTop(lbl_T_Y_S_Benefits, 90);

                    }
                }
                else
                {
                    CanvasManualBenefits.Children.Add(grid);
                    canvasManualBenefitManualDeduction.Children.Add(CanvasManualBenefits);
                    if (manualdeductionLöytyy)
                    {
                        Canvas.SetLeft(CanvasManualBenefits, 4);
                        Canvas.SetTop(CanvasManualBenefits, 167);
                    }
                    else
                    {
                        Canvas.SetLeft(CanvasManualBenefits, 4);
                        Canvas.SetTop(CanvasManualBenefits, 100);

                        Canvas.SetTop(lbl_T_Y_S_KeepTheBenefitOn, 70); Canvas.SetLeft(lbl_T_Y_S_KeepTheBenefitOn, 150); Canvas.SetTop(lbl_T_Y_S_Benefits, 70);

                    }

                }
            }
            if (manualdeductionLöytyy)
            {
                lbl_T_Y_S_deduction.Visibility = Visibility.Visible;
                lbl_T_Y_S_Keepthedeductionon.Visibility = Visibility.Visible;
                Customer.Job.MDeductions = Repo.GetCustomersManualDeduction(Customer.Job.ContractID);

                CanvasManualDeduction = new Canvas();
                ScrollViewerDeduction = new ScrollViewer();

                CanvasManualDeduction.Width = 50;
                CanvasManualDeduction.Height = 80;
                ScrollViewerDeduction.Width = 200;
                ScrollViewerDeduction.Height = 80;



                Grid grid = new Grid();
                var col = new ColumnDefinition();
                col.Width = new GridLength(160);
                grid.ColumnDefinitions.Add(col);
                grid.ColumnDefinitions.Add(new ColumnDefinition());


                StackPanel stackPanel0 = new StackPanel();
                StackPanel stackPanel1 = new StackPanel();

                int deductionLaskuri = 0; int KTBP_laskuri = 0;

                foreach (var item in Customer.Job.MDeductions)
                {
                    CheckBox checkDeduction = new CheckBox();
                    checkDeduction.Margin = new Thickness(0, 0, 2, 2); checkDeduction.FontSize = 10;
                    checkDeduction.Tag = (ManualDeduction)item;
                    checkDeduction.Click += Checkbox_clicked;

                    CheckBox checkKeepTheButtonPressed = new CheckBox();
                    checkKeepTheButtonPressed.Margin = new Thickness(0, 0, 2, 2); checkKeepTheButtonPressed.FontSize = 10;

                    checkKeepTheButtonPressed.Click += Checkbox_clicked;
                    decimal amount = Math.Round(item.Amount, 2);
                    checkDeduction.Content = item.Name + " - " + amount + "";
                    if (item.OurlyOrOnce == 0)
                    {
                        checkDeduction.Content += " (hourly)";
                    }
                    checkDeduction.VerticalContentAlignment = VerticalAlignment.Center; 

                    stackPanel0.Children.Add(checkDeduction);
                    stackPanel1.Children.Add(checkKeepTheButtonPressed);

                    CheckboxesDeduction.Add(checkDeduction);
                    CheckboxesDeduction2.Add(checkKeepTheButtonPressed);

                    deductionLaskuri++;
                    KTBP_laskuri++;

                }


                grid.Children.Add(stackPanel0);
                grid.Children.Add(stackPanel1);
                Grid.SetColumn(stackPanel1, 1);


                if (Customer.Job.MDeductions.Count > 5)
                {
                    ScrollViewerDeduction.Content = grid;
                    canvasManualBenefitManualDeduction.Children.Add(ScrollViewerDeduction);

                    if (manualbenefitLöytyy)
                    {
                        Canvas.SetLeft(ScrollViewerDeduction, 4);
                        Canvas.SetTop(ScrollViewerDeduction, 40);
                    }

                    else
                    {
                        Canvas.SetLeft(ScrollViewerDeduction, 4);
                        Canvas.SetTop(ScrollViewerDeduction, 100);

                        Canvas.SetTop(lbl_T_Y_S_Keepthedeductionon, 90); Canvas.SetLeft(lbl_T_Y_S_Keepthedeductionon, 150); Canvas.SetTop(lbl_T_Y_S_deduction, 90);
                    }


                }
                else if (Customer.Job.MDeductions.Count == 1)
                {
                    CanvasManualDeduction.Children.Add(grid);
                    canvasManualBenefitManualDeduction.Children.Add(CanvasManualDeduction);
                    if (manualbenefitLöytyy)
                    {
                        Canvas.SetLeft(CanvasManualDeduction, 4);
                        Canvas.SetTop(CanvasManualDeduction, 100);

                        Canvas.SetTop(lbl_T_Y_S_Keepthedeductionon, 70); Canvas.SetLeft(lbl_T_Y_S_Keepthedeductionon, 150); Canvas.SetTop(lbl_T_Y_S_deduction, 70);

                    }
                    else
                    {
                        Canvas.SetLeft(CanvasManualDeduction, 4);
                        Canvas.SetTop(CanvasManualDeduction, 120);

                        Canvas.SetTop(lbl_T_Y_S_Keepthedeductionon, 90); Canvas.SetLeft(lbl_T_Y_S_Keepthedeductionon, 150); Canvas.SetTop(lbl_T_Y_S_deduction, 90);

                    }

                }

                else if (Customer.Job.MDeductions.Count == 2 || Customer.Job.MDeductions.Count == 3)
                {
                    CanvasManualDeduction.Children.Add(grid);
                    canvasManualBenefitManualDeduction.Children.Add(CanvasManualDeduction);
                    if (manualbenefitLöytyy)
                    {
                        Canvas.SetLeft(CanvasManualDeduction, 4);
                        Canvas.SetTop(CanvasManualDeduction, 80);

                        Canvas.SetTop(lbl_T_Y_S_Keepthedeductionon, 50); Canvas.SetLeft(lbl_T_Y_S_Keepthedeductionon, 150); Canvas.SetTop(lbl_T_Y_S_deduction, 50);

                    }
                    else
                    {
                        Canvas.SetLeft(CanvasManualDeduction, 4);
                        Canvas.SetTop(CanvasManualDeduction, 120);

                        Canvas.SetTop(lbl_T_Y_S_Keepthedeductionon, 90); Canvas.SetLeft(lbl_T_Y_S_Keepthedeductionon, 150); Canvas.SetTop(lbl_T_Y_S_deduction, 90);

                    }

                }

                else if (Customer.Job.MDeductions.Count == 4 || Customer.Job.MDeductions.Count == 5)
                {
                    CanvasManualDeduction.Children.Add(grid);
                    canvasManualBenefitManualDeduction.Children.Add(CanvasManualDeduction);
                    if (manualbenefitLöytyy)
                    {
                        Canvas.SetLeft(CanvasManualDeduction, 4);
                        Canvas.SetTop(CanvasManualDeduction, 50);

                        Canvas.SetTop(lbl_T_Y_S_Keepthedeductionon, 20); Canvas.SetLeft(lbl_T_Y_S_Keepthedeductionon, 150); Canvas.SetTop(lbl_T_Y_S_deduction, 20);

                    }
                    else
                    {
                        Canvas.SetLeft(CanvasManualDeduction, 4);
                        Canvas.SetTop(CanvasManualDeduction, 120);

                        Canvas.SetTop(lbl_T_Y_S_Keepthedeductionon, 90); Canvas.SetLeft(lbl_T_Y_S_Keepthedeductionon, 150); Canvas.SetTop(lbl_T_Y_S_deduction, 90);

                    }

                }

                else
                {

                    CanvasManualDeduction.Children.Add(grid);
                    canvasManualBenefitManualDeduction.Children.Add(CanvasManualDeduction);
                    if (manualbenefitLöytyy)
                    {
                        Canvas.SetLeft(CanvasManualDeduction, 4);
                        Canvas.SetTop(CanvasManualDeduction, 30);
                    }
                    else
                    {
                        Canvas.SetLeft(CanvasManualDeduction, 4);
                        Canvas.SetTop(CanvasManualDeduction, 120);

                        Canvas.SetTop(lbl_T_Y_S_Keepthedeductionon, 90); Canvas.SetLeft(lbl_T_Y_S_Keepthedeductionon, 50); Canvas.SetTop(lbl_T_Y_S_deduction, 90);

                    }



                }
                
                
            }


            var laskuri = Customer.Job.MDeductions.Count + Customer.Job.MBenefits.Count;
            if (laskuri <= 1)
            {
                Canvas.SetTop(canvasManualBenefitManualDeduction, 160);

            }
            else if (laskuri == 2)
            {
                Canvas.SetTop(canvasManualBenefitManualDeduction, 180);

            }
            else if (laskuri == 3)
            {
                Canvas.SetTop(canvasManualBenefitManualDeduction, 200);


            }
            else if (laskuri == 4)
            {
                Canvas.SetTop(canvasManualBenefitManualDeduction, 220);

            }
            else if (laskuri == 5)
            {
                Canvas.SetTop(canvasManualBenefitManualDeduction, 240);

            }
            else if (laskuri == 6)
            {
                Canvas.SetTop(canvasManualBenefitManualDeduction, 260);

            }
            else if (laskuri == 7)
            {
                Canvas.SetTop(canvasManualBenefitManualDeduction, 280);

            }
            
            else
            {
                Canvas.SetTop(canvasManualBenefitManualDeduction, 300);


            }




        }
        public void UncheckCheckboxes()
        {
            for (int i = 0; i < CheckboxesBenefit.Count; i++)
            {
                if (CheckboxesBenefit[i].IsChecked == true)
                {
                    if (CheckboxesBenefit2[i].IsChecked == false)
                    {
                        CheckboxesBenefit[i].IsChecked = false;

                    }

                }
            }

            for (int i = 0; i < CheckboxesDeduction.Count; i++)
            {
                if (CheckboxesDeduction[i].IsChecked == true)
                {
                    if (CheckboxesDeduction2[i].IsChecked == false)
                    {
                        CheckboxesDeduction[i].IsChecked = false;

                    }

                }
            }
        }
        public TimeSpan GetTime(TextBox txtTime)
        {
            var text = txtTime.Text;
            TimeSpan startTime = new TimeSpan(0, 0, 0);
          
            var parts = text.Split(':');

            string stringStartHours = "";
            string stringStartMinutes = "";
            int startHours = 0;
            int startMinutes = 0;

            stringStartHours = parts[0].ToString();
            stringStartMinutes = parts[1].ToString();

            bool ok = int.TryParse(stringStartHours, out startHours);
            if (!ok)
            {
                MessageBox.Show("Incorrect start time", "Error");
                txtAmountti.Focus();
                return startTime;
            }

            ok = int.TryParse(stringStartMinutes, out startMinutes);
            if (!ok)
            {
                MessageBox.Show("Incorrect start time", "Error");
                txtAmountti.Focus();
                return startTime;
            }

            startTime = new TimeSpan(startHours, startMinutes, 0);

            return startTime;

        }
        private void ResetCalendar()
        {
            txtShiftStarts.Text = "- - : - -";
            txtShiftEnds.Text = "- - : - -";

            txtShiftEnds.BorderBrush = Brushes.White;
            txtShiftStarts.BorderBrush = Brushes.Black;
            calendarShift.Focus();
        }
        private void AddDoubleDot(TextBox textBox)
        {
            if (textBox.Text[textBox.Text.Length - 1] != ':')
            {
                var text = textBox.Text;
                text += ':';
                textBox.Text = text;
                textBox.SelectionStart = textBox.Text.Length;

            }
           
        }
        private void Checkbox_clicked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < CheckboxesBenefit2.Count; i++)
            {

                if (CheckboxesBenefit2[i].IsChecked == true)
                {
                    CheckboxesBenefit[i].IsChecked = true;

                }
            }
            for (int i = 0; i < CheckboxesDeduction2.Count; i++)
            {

                if (CheckboxesDeduction2[i].IsChecked == true)
                {
                    CheckboxesDeduction[i].IsChecked = true;

                }
            }

            calendarShift.Focus();


        }
        private void txtShiftEnds_Keyup(object sender, KeyEventArgs e)
        {
            if (txtShiftEnds.Text.Length == 5)
            {
                btnSaveShift.Focus();
                txtShiftEnds.BorderBrush = Brushes.White;


            }

        }
        private void canvasManualBenefitManualDeduction_click(object sender, MouseButtonEventArgs e)
        {
            calendarShift.Focus();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
          
            for (int i = 0; i < AllShifts.Count; i++)
            {
               
                for (int a = AllShifts[i].Overtimebenefits.Count - 1; a > -1; a--)
                {
                    AllShifts[i].Overtimebenefits[a].WeekList.Clear();
                    AllShifts[i].Overtimebenefits[a].MonthsList.Clear();
                }
  
            }
            for (int i = 0; i < AllShifts.Count; i++)
            {
                AllShifts.Remove(AllShifts[i]);


            }

            AllShifts.Clear();
            AllShifts = new List<ShiftInfo>();

            txtSalaryInfo.Text = string.Empty;
            txtShiftStarts.Text = "- - : - -";
            txtShiftEnds.Text = "- - : - -";

            txtShiftEnds.BorderBrush = Brushes.White;
            txtShiftStarts.BorderBrush = Brushes.Black;
            calendarShift.Focus();


        }


        // CANVAS_TYPE_IN_YOUR_SHIFTS




        private void btnAutoBenefitAndDeductionInfo_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("There are 3 different types of benefits that can be set to count automatically\n" +
                "based on a specific date, weekday or amount of ours worked in a day/week/month.", "Info");

        }

        private void btnBenefitsSpecificDateInfo(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add a benefit that will only apply to the specific date\n" +
                "you choose from the calendar. You can choose multiple dates by clicking\n" +
                "and dragging mouse over the calendar. " +
                "\n\nPress 'Add new' to add a new benefit.\n" +
                "Benefit info will appear in the drop-down list", "Info");
        }

        private void btnWeeklyOvertimeBenefitsInfo_click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Add a new benefit that will occur weekly." +              
                 "\n\nPress 'Add new' to add a new benefit.\n" +
                 "Benefit info will appear in the drop-down list", "Info");
        }


        private void btnOvertimeBenefitInfo_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add a benefit that can be set to occur after you have reached\n" +
                "a specific amounts of hours in a day, week or a month. \n" +
                "\n\nPress 'Add new' to add a new benefit.\n" +
                "Benefit info will appear in the drop-down list", "Info");
        }

        private void btnHourlySalaryInfo_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please input your hourly salary.\n" +
                "(If you get paid a set amount (per week/month), insert '0')", "Info");
        }
        
        private void btnManualBenefitsInfo_click(object sender, RoutedEventArgs e)
        {
            {
                MessageBox.Show("Manual benefit can be activated when you input your shift-information\n" +
                    "\n\nPress 'Add new' to add a new benefit.\n" +
                     "Benefit info will appear in the drop-down list", "Info");
            }
        }

        private void btnManualDeductionsInfo_click(object sender, RoutedEventArgs e)
        {
            {
                MessageBox.Show("Manual deductions can be activated when you input your shift-information" +
                    "\n\nPress 'Add new' to add a new deduction.\n" +
                    "Deductio info will appear in the drop-down list", "Info");
            }
        }

        private void btnManuallyActivatedBandDInfo_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Manual benefits and - deductions can be activated manually\n" +
                "when you are inputting your shift-information." +
                "", "Info");
        }

      
    }
}

