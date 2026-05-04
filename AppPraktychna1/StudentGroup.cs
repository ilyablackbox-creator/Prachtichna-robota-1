using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AppPraktychna1
{
    internal class StudentGroup
    {
        public string GroupName { get; set; }
        public string Specialty { get; set; }
        public int Course { get; set; }

        private List<Student> students = new List<Student>();

        public int GroupSize => students.Count;

        public double AverageGroupGrade => students.Any()
            ? Math.Round(students.Average(s => s.AverageGrade), 2)
            : 0;

        public void AddStudent(Student s)
        {
            if (s == null) return;
            students.Add(s);
        }

        public void RemoveStudent(string recordBookNumber)
        {
            int removedCount = students.RemoveAll(s => s.RecordBookNumber == recordBookNumber);
            if (removedCount > 0)
                Console.WriteLine($"Студента з ID {recordBookNumber} видалено.");
            else
                Console.WriteLine("Студента з таким ID не знайдено.");
        }

        public Student FindStudent(string recordBookNumber)
        {
            return students.FirstOrDefault(s => s.RecordBookNumber == recordBookNumber);
        }

        public List<Student> FindStudent(string namePart, bool isNameSearch)
        {
            return students.Where(s => s.FullName.Contains(namePart, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Student> GetExcellentStudents()
        {
            return students.Where(s => s.IsExcellent()).ToList();
        }

        public List<Student> GetStudentsByStatus(StudentStatus status)
        {
            return students.Where(s => s.Status == status).ToList();
        }

        public List<Student> GetAllStudents() => students;

        public void SaveToFile(string fileName)
        {



            try
            {
                string json = JsonConvert.SerializeObject(students, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(fileName, json);
                Console.WriteLine("Дані успішно збережено у файл.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні: {ex.Message}");
            }
        }

        public void LoadFromFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    string json = File.ReadAllText(fileName);
                    students = JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
                    Console.WriteLine("Дані успішно завантажено з файлу.");
                }
                else
                {
                    Console.WriteLine("Файл не знайдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні: {ex.Message}");
            }
        }

    }
}
