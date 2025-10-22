using UnityEngine;
using TMPro;

namespace solsyssim {
    
    // Calculates and displays the current date in the simulation based on elapsed time.
    
    public class DateCalc : MonoBehaviour {
        private TMP_Text _dateLabel; // UI element for displaying the date.
        private int _baseYear = 2000; // The year simulation starts from.

        private void Start() {
            _dateLabel = GetComponent<TMP_Text>(); // Initialize the date label.
        }

        void Update() {
            if (!SpaceTime.Instance._timePause) {
                UpdateDateLabel(); // Update the date label if the game is not paused.
            }
        }

        
        // Updates the date label with the current date calculated from the elapsed simulation time.
        
        private void UpdateDateLabel() {
            double timePool = SpaceTime.Instance.ElapsedTime; // Total elapsed time in the simulation.

            int year = ComputeYear(ref timePool, _baseYear);
            int month = ComputeMonth(ref timePool, IsLeapYear(year));
            int day = Mathf.FloorToInt((float)timePool) + 1;

            // Calculate hours, minutes, and seconds from the remaining timePool fraction.
            int hours = Mathf.FloorToInt((float)(timePool % 1 * 24));
            int minutes = Mathf.FloorToInt((float)((timePool % 1 * 24 - hours) * 60));
            int seconds = Mathf.FloorToInt((float)((((timePool % 1 * 24 - hours) * 60) - minutes) * 60));

            // Format the date and time string.
            string dd = day < 10 ? "0" + day.ToString() : day.ToString();
            string mm = month < 10 ? "0" + month.ToString() : month.ToString();
            string yyyy = year.ToString();
            string timeString = $"{hours:D2}:{minutes:D2}:{seconds:D2}";

            _dateLabel.text = $"{dd}/{mm}/{yyyy} {timeString}"; // Update the UI element.
        }

        
        // Returns the current date string.
        
        public string GetDateString() {
            return _dateLabel != null ? _dateLabel.text : "DateLabel is not set.";
        }

        
        // Computes the current year based on elapsed time.
        
        private int ComputeYear(ref double pool, int baseYear) {
            int year;
            int dayCheck;
            int leapCheck = IsLeapYear(baseYear) ? 1 : 0;

            year = Mathf.FloorToInt((float)(pool / 365.2425f));
            dayCheck = Mathf.FloorToInt((float)(pool - (365 * year + (year - 1) / 4 - (year - 1) / 100 + (year - 1) / 400 + leapCheck)));

            if (dayCheck < 0) year -= 1;

            pool -= year > 0 ? (365 * year + (year - 1) / 4 - (year - 1) / 100 + (year - 1) / 400 + leapCheck) : 0;

            return year + baseYear;
        }

        
        // Determines if a given year is a leap year.
        
        private bool IsLeapYear(int year) {
            return year % 400 == 0 || (year % 100 != 0 && year % 4 == 0);
        }

        
        // Computes the current month based on elapsed time and whether it's a leap year.
        
        private int ComputeMonth(ref double pool, bool leap) {
            int[] daysToMonthEnd = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };
            int[] daysToMonthEndLeap = { 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };
            int month;

            for (month = 1; month <= 12; month++) {
                if ((!leap && pool < daysToMonthEnd[month]) || (leap && pool < daysToMonthEndLeap[month])) {
                    pool -= leap ? daysToMonthEndLeap[month - 1] : daysToMonthEnd[month - 1];
                    break;
                }
            }

            return month;
        }
    }
}