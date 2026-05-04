using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace AppPraktychna1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            StudentGroup group = new StudentGroup { };
            string jsonPath = "group_data.json";

            while (true)
            {
                Console.WriteLine($"\n МЕНЮ УПРАВЛІННЯ ГРУПОЮ П-21 ");
                Console.WriteLine("1. Додати студента");
                Console.WriteLine("2. Видалити студента (за номером залікової)");
                Console.WriteLine("3. Вивести всіх студентів (пагінація по 10)");
                Console.WriteLine("4. Пошук студента (ПІБ або номер залікової)");
                Console.WriteLine("5. Редагування даних студента");
                Console.WriteLine("6. Вивести відмінників та боржників (< 60)");
                Console.WriteLine("7. Вивести cтатистику групи");
                Console.WriteLine("8. Зберегти / Завантажити дані (JSON)");
                Console.WriteLine("0. Вийти");
                Console.WriteLine("////////////////////////////////////////");
                Console.Write("Вибір: ");

                string choice = Console.ReadLine();
                if (choice == "0") break;

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Введіть ПІБ: "); string name = Console.ReadLine();
                            Console.Write("Введіть номер залікової (8 цифр): "); string id = Console.ReadLine();
                            Console.Write("Введіть Email: "); string email = Console.ReadLine();
                            Console.Write("Введіть дату народження (рррр-мм-дд): ");
                            DateTime dob = DateTime.Parse(Console.ReadLine());

                            Student newStudent = new Student
                            {
                                FullName = name,
                                RecordBookNumber = id,
                                PersonalEmail = email,
                                DateOfBirth = dob,
                                EnrollmentDate = DateTime.Now,
                                Status = StudentStatus.Active
                            };

                            Console.Write("Введіть середній бал: ");
                            if (double.TryParse(Console.ReadLine(), out double grade))
                                newStudent.UpdateAverageGrade(grade);

                            group.AddStudent(newStudent);
                            Console.WriteLine("Студента успішно додано.");
                            break;

                        case "2":
                            Console.Write("Введіть номер залікової для видалення: ");
                            group.RemoveStudent(Console.ReadLine());
                            break;

                        case "3":
                            var all = group.GetAllStudents();
                            Console.WriteLine($"\n Список студентів (Кількість: {group.GroupSize})");
                            for (int i = 0; i < all.Count; i++)
                            {
                                Console.WriteLine(all[i]);
                                if ((i + 1) % 10 == 0 && i != all.Count - 1) Console.ReadKey();
                            }
                            break;

                        case "4":
                            Console.Write("Введіть ПІБ або номер залікової: ");
                            string query = Console.ReadLine();
                            var searchRes = group.FindStudent(query, true);
                            if (searchRes.Any()) searchRes.ForEach(s => s.ShowDetailedInfo());
                            else
                            {
                                var sById = group.FindStudent(query);
                                if (sById != null) sById.ShowDetailedInfo();
                                else Console.WriteLine("Не знайдено.");
                            }
                            break;

                        case "5":
                            Console.Write("Введіть номер залікової студента: ");
                            var stToEdit = group.FindStudent(Console.ReadLine());
                            if (stToEdit != null)
                            {
                                Console.Write($"Новий ПІБ (теперішній: {stToEdit.FullName}): ");
                                string newName = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newName)) stToEdit.FullName = newName;

                                Console.WriteLine("1. Активний | 2. Академічна відпусткка | 3. Відрахований | 4. Випускник");
                                Console.Write("Оберіть новий статус студента: ");
                                if (int.TryParse(Console.ReadLine(), out int sIdx))
                                    stToEdit.Status = (StudentStatus)(sIdx - 1);

                                Console.WriteLine("Дані оновлено.");
                            }
                            else Console.WriteLine("Студента не знайдено.");
                            break;

                        case "6":
                            Console.WriteLine("\nВІДМІННИКИ (>= 90) ");
                            group.GetExcellentStudents().ForEach(s => Console.WriteLine(s));

                            Console.WriteLine("\nБОРЖНИКИ (< 60) ");
                            group.GetAllStudents().Where(s => s.IsFailing()).ToList().ForEach(s => Console.WriteLine(s));
                            break;

                        case "7":
                            int total = group.GroupSize;
                            double perc = total > 0 ? (double)group.GetExcellentStudents().Count / total * 100 : 0;
                            Console.WriteLine($"Група: {total} чол. | Сер. бал: {group.AverageGroupGrade:F2} | Відмінники: {perc:F1}%");
                            break;

                        case "8":
                            Console.WriteLine("1. Зберегти у JSON | 2. Завантажити з JSON");
                            string fChoice = Console.ReadLine();
                            if (fChoice == "1") group.SaveToFile(jsonPath);
                            else group.LoadFromFile(jsonPath);
                            break;

                        default:
                            Console.WriteLine("Невірний вибір!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nПОМИЛКА: {ex.Message}");
                }
            }
        }
    }
}
