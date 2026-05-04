using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppPraktychna1
{
    public enum StudentStatus { Active, AcademicLeave, Expelled, Graduated }
    public class Student : IComparable<Student>
    {
        private string fullName;
        private string recordBookNumber;
        private double averageGrade;

        public required DateTime DateOfBirth { get; init; }
        public required DateTime EnrollmentDate { get; init; }

        private string personalEmail;
        public required string PersonalEmail
        {
            get => personalEmail;
            init
            {
                if (!value.Contains("@")) throw new ArgumentException("Невірно написано Email");
                personalEmail = value;
            }
        }

        public string FullName
        {
            get => fullName;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 5)
                    throw new ArgumentException("ПІБ: не менше 5 символів.");
                fullName = value;
            }
        }

        public string RecordBookNumber
        {
            get => recordBookNumber;
            set
            {
                if (!Regex.IsMatch(value, @"^\d{8}$"))
                    throw new ArgumentException("Номер залікової: 8 цифри");
                recordBookNumber = value;
            }
        }

        public double AverageGrade
        {
            get => averageGrade;
            private set => averageGrade = Math.Round(value, 2);
        }

        public StudentStatus Status { get; set; }
        public string Notes { get; set; }
        public GradeJournal Journal { get; set; } = new GradeJournal();

        public int Age => CalculateAge();

        public int CalculateAge()
        {
            int age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }
        public void UpdateAverageGrade(double newGrade)
        {
            if (newGrade < 0 || newGrade > 100) throw new ArgumentException("Бал від 0 до 100.");
            this.AverageGrade = newGrade;
        }

        public bool IsExcellent() => AverageGrade >= 90;
        public bool IsFailing() => AverageGrade < 60;

        public int GetYearsToGraduation()
        {
            int totalYears = 4;
            int yearsPassed = DateTime.Now.Year - EnrollmentDate.Year;
            return Math.Max(0, totalYears - yearsPassed);
        }

        public void ShowDetailedInfo()
        {
            Console.WriteLine($"\n Інформація ");
            Console.WriteLine($"ПІБ: {FullName}\nID: {RecordBookNumber}\nВік: {Age}");
            Console.WriteLine($"Email: {PersonalEmail}\nСтатус: {Status}");
            Console.WriteLine($"Сер. бал: {AverageGrade}\nЗалишилось вчитися: {GetYearsToGraduation()} р.");
        }

        public void SyncWithJournal() => AverageGrade = Journal.CalculateAverage();

        public int CompareTo(Student other) => string.Compare(FullName, other?.FullName, StringComparison.OrdinalIgnoreCase);

        public override string ToString() => $"[{RecordBookNumber}] {FullName,-20} | Бал: {AverageGrade:F2}";

    }
}
