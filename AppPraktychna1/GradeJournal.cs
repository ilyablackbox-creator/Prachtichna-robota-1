using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppPraktychna1
{
    public class GradeJournal
    {
        public Dictionary<string, double> Grades { get; set; } = new Dictionary<string, double>();

        public void AddGrade(string subject, double grade) => Grades[subject] = grade;

        public double CalculateAverage() => Grades.Count > 0 ? Grades.Values.Average() : 0;

    }
}
