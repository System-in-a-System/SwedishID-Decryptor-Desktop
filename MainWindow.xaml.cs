using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;

namespace SwedishID_Decryptor_Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void send_Button_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user data from the textbox
            string socialSecurityNumber = PN_textbox.Text;


            //__Check user input for validity________________________________________________________________________________//

            // Initialize SSN patterns in four versions
            string pattern1 = @"^\d{6}-\d{4}$";
            string pattern2 = @"^\d{8}-\d{4}$";
            string pattern3 = @"^\d{10}$";
            string pattern4 = @"^\d{12}$";

            Regex regex1 = new Regex(pattern1);
            Regex regex2 = new Regex(pattern2);
            Regex regex3 = new Regex(pattern3);
            Regex regex4 = new Regex(pattern4);


            // Check the current input for validity
            bool validSocialSecurityNumber = regex1.IsMatch(socialSecurityNumber) || regex2.IsMatch(socialSecurityNumber) ||
                                             regex3.IsMatch(socialSecurityNumber) || regex4.IsMatch(socialSecurityNumber);

            // If the current input is NOT valid...
            if (!validSocialSecurityNumber)
            {
                // inform 
                output_Box.Text = "Invalid Personal Number...";

                // terminate the algorithm
                return;
            }


            // Unify SSN format: YYYYMMDD-XXXX:
            // If the short pattern of SSN was followed...
            if (regex1.IsMatch(socialSecurityNumber) || regex3.IsMatch(socialSecurityNumber))
            {
                // retrieve year information 
                int shortYearVersion = int.Parse(socialSecurityNumber.Substring(0, 2));

                // complement the SSN accordingly
                if (shortYearVersion > 20 && shortYearVersion <= 99)
                {
                    socialSecurityNumber = "19" + socialSecurityNumber;
                }
                else
                {
                    socialSecurityNumber = "20" + socialSecurityNumber;
                }
            }
            //_______________________________________________________________________________________________________________//



            // Information to decrypt from the input data: 
            string gender = "Unknown";
            int age = 0;
            string generation = "Unknown";
            string generationInformation = "Undefined";



            // --------------------------------------------- Define gender --------------------------------------------------//
            int genderNumber = int.Parse(socialSecurityNumber.Substring(socialSecurityNumber.Length - 2, 1));
            bool isFemale = genderNumber % 2 == 0;
            gender = isFemale ? "Female" : "Male";



            // ---------------------------------------------- Define age ----------------------------------------------------//
            // Assign the default value to birthDate
            DateTime birthDate = DateTime.Now;


            // Retrieve birthday date from personal number:

            try
            {
                // If we are dealing with the short version of personal number...
                if (socialSecurityNumber.Length >= 10 && socialSecurityNumber.Length <= 11)
                {
                    birthDate = DateTime.ParseExact(socialSecurityNumber.Substring(0, 6), "yyMMdd", CultureInfo.InvariantCulture);
                }
                // If we are dealing with the long version of personal number...
                else if (socialSecurityNumber.Length >= 12 && socialSecurityNumber.Length <= 13)
                {
                    birthDate = DateTime.ParseExact(socialSecurityNumber.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                // if the length of the personal number is not valid...
                else
                {
                    // inform about an invalid input
                    output_Box.Text = "Invalid Personal Number...";

                    // terminate the algorithm
                    return;
                }
            }
            catch (Exception)
            {
                // inform about an invalid input
                output_Box.Text = "Birthdate information could not be retrieved...";
                
                // terminate the algorithm
                return;
            }



            // Calculate age based on retrieved data
            age = DateTime.Now.Year - birthDate.Year;

            // Possible age correction depending on the day of the year
            if ((birthDate.Month > DateTime.Now.Month) || (birthDate.Month == DateTime.Now.Month && birthDate.Day > DateTime.Now.Day))
            {
                age--;
            }



            // ------------------------------------------ Define generation ------------------------------------------------- // 
            // source: http://socialmarketing.org/archives/generations-xy-z-and-the-others/

            int birthYear = birthDate.Year;

            if (birthYear >= 1995)
            {
                generation = "Generation Z";
                generationInformation = "smartphones, social media, never knowing a country not at war.";
            }
            else if (birthYear >= 1977)
            {
                generation = "Millennials";
                generationInformation = "the Great Recession, the technological explosion of the internet and social media, 9/11.";
            }
            else if (birthYear >= 1965)
            {
                generation = "Lost generation";
                generationInformation = "the end of the Cold war, the lowest voting participation rate, skepticism.";
            }
            else if (birthYear >= 1946)
            {
                generation = "Baby-Boomers generation";
                generationInformation = "post-WWII optimism, the cold war, and the hippie movement.";
            }
            else if (birthYear >= 1928)
            {
                generation = "Post-War generation";
                generationInformation = "post-war economic boom, Cold War tensions, the potential for nuclear war.";
            }
            else if (birthYear >= 1922)
            {
                generation = "War generation";
                generationInformation = "the Korean War, the Second World War, the Cold War.";
            }
            else if (birthYear >= 1912)
            {
                generation = "Depression generation";
                generationInformation = "the Great Depression, the Global Unrest.";
            }


            // Output the results
            output_Box.Text = $"Gender: {gender}" +
                              $"\nAge: {age}" +
                              $"\n\nGeneration: {generation}" +
                              $"\nShaping events: {generationInformation}";


        }
    }
}
