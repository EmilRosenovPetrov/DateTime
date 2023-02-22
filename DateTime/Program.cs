using System;
using System.Globalization;
using System.Text;

int DaysBetweenDates(DateTime date1, DateTime date2, string inputParams)
{
    string[] baseForPeriod = inputParams.Split('/', StringSplitOptions.RemoveEmptyEntries);

    int result = 0;

    int firstYear = date1.Year;
    int secondYear = date2.Year;
    int firstMonth = date1.Month;
    int secondMonth = date2.Month;
    int firstDay = date1.Day;
    int secondDay = date2.Day;

    bool IsEndOfMonth(int year, int month, int day)
    {
        int maxdtMonth = new DateTime(year, month, day).AddDays(1).Day;

        if (maxdtMonth == 1)
        {
            return true;
        }

        return false;
    };

    //Проверка дали двете дати са края на Февруари.
    bool BothFebruaryCheck(int firstMonth, int secondMonth, int firstDay, int secondDay)
    {
        int maxdtFirstMonth = new DateTime(firstYear, firstMonth, firstDay).AddDays(1).Day;
        int maxdtSecondMonth = new DateTime(secondYear, secondMonth, secondDay).AddDays(1).Day;

        if (firstMonth == 2 && secondMonth == 2 && maxdtFirstMonth == 1 && maxdtSecondMonth == 1)
        {
            return true;
        }

        return false;
    }

    //Изпълнява условието за 30 дневни месеци върху двете дати.
    void All30DaysMonths()
    {
        if (BothFebruaryCheck(firstMonth, secondMonth, firstDay, secondDay))
        {
            secondDay = 30;
        }

        if (firstMonth == 2 && IsEndOfMonth(firstYear, firstMonth, firstDay) && !BothFebruaryCheck(firstMonth, secondMonth, firstDay, secondDay))
        {
            firstDay = 30;
        }

        if (firstDay >= 30 && secondDay > 30)
        {
            secondDay = 30;
        }
        if (firstDay > 30)
        {
            firstDay = 30;
        }
    }


    if (baseForPeriod[0] == "30E")
    {
        All30DaysMonths();

        result = 360 * (secondYear - firstYear) + 30 * (secondMonth - firstMonth) + (secondDay - firstDay);
    };

    if (baseForPeriod[0] == "30")
    {
        if (secondDay < 31)
        {
            All30DaysMonths();
            result = 360 * (secondYear - firstYear) + 30 * (secondMonth - firstMonth) + (secondDay - firstDay);
        }

        if (secondDay == 31 && firstDay != 1)
        {

            result = 360 * (secondYear - firstYear) + 30 * (secondMonth - firstMonth) + (secondDay - firstDay) + 1;
        }
    }

    if (baseForPeriod[0] == baseForPeriod[1])
    {
        TimeSpan differense = date2 - date1;

        result = (int)differense.TotalDays;
    }

    return result;

}


string RateOfInterestCalculator(DateTime firstDate, DateTime secondDate, decimal anualPercentage, string baseForPeriod, decimal principalForInterest)
{
    //Задаваме база за формулата
    int yearLength = DateTime.IsLeapYear((int)DateTime.Now.Year) ? 366 : 365;

    string[] baseForPeriodParts = baseForPeriod.Split('/', StringSplitOptions.RemoveEmptyEntries);
    if (baseForPeriodParts[1] == "360")
    {
        yearLength = 360;
    }

    StringBuilder sb = new StringBuilder();

    

    int totalDaysInPeriod = DaysBetweenDates(firstDate, secondDate, baseForPeriod);

    decimal totalPointOfInterest = principalForInterest * (anualPercentage / 100) * totalDaysInPeriod / yearLength;

    sb.Append($"Your rate of interest for period: {firstDate.Day}.{firstDate.Month}.{firstDate.Year} - {secondDate.Day}.{secondDate.Month}.{secondDate.Year} is: {totalPointOfInterest:F4}.");
    sb.AppendLine();

    return sb.ToString();
};

//Тук се стартира програмата.

Console.WriteLine("Please, enter start of period, end of period, anualPercentage, baseForPeriod and principalForInterest in following format:");
Console.WriteLine("DD.MM.YYYY; DD.MM.YYYY; Percentage number; 30/360, 30E/360 or Act/Act; Principal of interest as a number.");
Console.WriteLine("Type End to stop the program.");
Console.WriteLine();

while (true)
{
    string input = Console.ReadLine();
    if (input == "End" || input == "end")
    {
        break;
    }

    string[] parameters = input.Split("; ", StringSplitOptions.RemoveEmptyEntries);

    if (parameters.Length < 5)
    {
        Console.WriteLine("Please, enter all required parameters!");
        Console.WriteLine();
        continue;
    }

    string firstDateString = parameters[0].TrimEnd();
    string secondDateString = parameters[1].TrimEnd();
    string anualPercentageString = parameters[2].TrimEnd();
    string baseForPeriod = parameters[3].TrimEnd();
    string principalForInterestString = parameters[4].TrimEnd();

    int anualPercentage = 0;
    decimal principalForInterest = 0;
    DateTime firstDate = new DateTime();
    DateTime secondDate = new DateTime();

    if (!int.TryParse(anualPercentageString, out anualPercentage) || !decimal.TryParse(principalForInterestString, out principalForInterest))
    {
        Console.WriteLine("Your input for Anual percentage or Principal for interest is not correct!");
        Console.WriteLine();
        continue;
    }

    if (!DateTime.TryParse(firstDateString, out firstDate) || !DateTime.TryParse(secondDateString, out secondDate))
    {
        Console.WriteLine("Please, enter correct Dates in following format: YYYY, MM, DD!");
        Console.WriteLine();
        continue;
    }

    if (baseForPeriod != "30E/360" && baseForPeriod != "30/360" && baseForPeriod != "Act/Act")
    {
        Console.WriteLine("Your input for Base for period is incorrect! Please enter the base in following format:" +
            "30/360, 30E360 or Act/Act.");
        Console.WriteLine();
        continue;
    }

    Console.WriteLine(RateOfInterestCalculator(firstDate, secondDate, anualPercentage, baseForPeriod, principalForInterest));

}
