
class UserHelper
{
    private DatabaseHelper _dbHelper;

    public UserHelper(DatabaseHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    public void Start()
    {
        ShowOptions();
        string userOptionNumber = AskOptionNumber();
        while (userOptionNumber != "69")
        {
            if (userOptionNumber == "1")
            {
                Option1();
            }
            else if (userOptionNumber == "2")
            {
                Option2();
            }
            else if (userOptionNumber == "3")
            {
                Option3();
            }
            else if (userOptionNumber == "4")
            {
                Option4();
            }
            else if (userOptionNumber == "5")
            {
                Option5();
            }
            else if (userOptionNumber == "6")
            {
                Option6();
            }
            else if (userOptionNumber == "7")
            {   
                Option7();
            }
            else if (userOptionNumber == "8")
            {
                Option8();
            }
            else if (userOptionNumber == "9")
            {
                Option9();
            }
            else if (userOptionNumber == "10")
            {   
                Option10();
            }
            else if (userOptionNumber == "52")
            {
                Option52();
            }
            ShowOptions();
            userOptionNumber = AskOptionNumber();
        }
    }

    private void Option1()
    {
        Console.WriteLine("1. Добавление студента\n2. Добавление преподавателя\n3. Добавление курса\n4. Добавление экзамена\n5. Добавление оценки");
        string userOptionNumber = AskOptionNumber();
        if (userOptionNumber == "1")
        {
            Console.Write("Введите данные студента через пробел: ");
            string[] args = Console.ReadLine().Split();
            if (args.Length == 4)
            {
                _dbHelper.AddStudent(args[0], args[1], args[2], args[3]);
            }

        }
        else if (userOptionNumber == "2")
        {
            Console.Write("Введите данные преподавателя через пробел: ");
            string[] args = Console.ReadLine().Split();
            if (args.Length == 3)
            {
                _dbHelper.AddTeacher(args[0], args[1], args[2]);
            }
        }
        else if (userOptionNumber == "3")
        {
            Console.Write("Введите данные курса через пробел: ");
            string[] args = Console.ReadLine().Split();
            if (args.Length == 3) 
            {
                if (int.TryParse(args[2], out int intArg))
                {
                    _dbHelper.AddCourse(args[0], args[1], intArg);
                }
            }
        }
        else if (userOptionNumber == "4")
        {
            Console.Write("Введите данные экзамена через пробел: ");
            string[] args = Console.ReadLine().Split();
            if (args.Length == 3) 
            {
                if (int.TryParse(args[1], out int intArg1) & int.TryParse(args[2], out int intArg2))
                {
                    _dbHelper.AddExam(args[0], intArg1, intArg2);
                }
            }
        }
        else if (userOptionNumber == "5")
        {
            Console.Write("Введите данные оценки через пробел: ");
            string[] args = Console.ReadLine().Split();
            if (args.Length == 3) 
            {
                if (int.TryParse(args[0], out int intArg1) & int.TryParse(args[1], out int intArg2) & int.TryParse(args[2], out int intArg3))
                {
                    _dbHelper.AddGrade(intArg1, intArg2, intArg3);
                }
            }
        }
        Console.WriteLine("~~~");
    }

    private void Option2()
    {
        Console.WriteLine("1. Изменение информации о студенте\n2. Изменение информации о преподавателе\n3. Изменение информации о курсе");
        string userOptionNumber = AskOptionNumber();
        if (userOptionNumber == "1")
        {
            Console.Write("Введите ID студента: ");
            string id = Console.ReadLine();
            if (int.TryParse(id, out int intId))
            {
                Console.Write("Введите новое имя (пуст. поле => без изм.): "); 
                string newName = Console.ReadLine();
                Console.Write("Введите новую фамилию (пуст. поле => без изм.): ");
                string newSurname = Console.ReadLine();
                Console.Write("Введите новый факультет (пуст. поле => без изм.): ");
                string newDepartment = Console.ReadLine();
                Console.Write("Введите новую дату рождения (пуст. поле => без изм.): ");
                string newDateOfBirth = Console.ReadLine();
                _dbHelper.ChangeStudent(intId, newName, newSurname, newDepartment, newDateOfBirth);
            }
        }
        else if (userOptionNumber == "2")
        {
            Console.Write("Введите ID преподавателя: ");
            string id = Console.ReadLine();
            if (int.TryParse(id, out int intId))
            {
                Console.Write("Введите новое имя (пуст. поле => без изм.): ");
                string newName = Console.ReadLine();
                Console.Write("Введите новую фамилию (пуст. поле => без изм.): ");
                string newSurname = Console.ReadLine();
                Console.Write("Введите новый факультет (пуст. поле => без изм.): ");
                string newDepartment = Console.ReadLine();
                _dbHelper.ChangeTeacher(intId, newName, newSurname, newDepartment);
            }
        }
        else if (userOptionNumber == "3")
        {
            Console.Write("Введите ID курса: ");
            string courseId = Console.ReadLine();
            if (int.TryParse(courseId, out int intCourseId))
            {
                Console.Write("Введите новое название (пуст. поле => без изм.): ");
                string newTitle = Console.ReadLine();
                Console.Write("Введите новое описание (пуст. поле => без изм.): ");
                string newDescription = Console.ReadLine();
                Console.Write("Введите новое ID преподавателя: ");
                string teacherId = Console.ReadLine();
                if (int.TryParse(teacherId, out int intTeacherId))
                {
                    _dbHelper.ChangeCourse(intCourseId, newTitle, newDescription, intTeacherId);
                }
            }
        }
        Console.WriteLine("~~~");
    }

    private void Option3()
    {
        string[] tables = { "Students", "Teachers", "Courses", "Exams", "Grades" };
        string[] tableIds = { "student_id", "teacher_id", "course_id", "exam_id", "grade_id" };
        Console.WriteLine("1. Удаление студента\n2. Удаление преподавателя\n3. Удаление курсa\n4. Удаление экзамена");
        string userOptionNumber = AskOptionNumber();
        int.TryParse(userOptionNumber, out int intUserOptionNumber);
        if (1 <= intUserOptionNumber & intUserOptionNumber <= 4)
        {
            Console.Write("Введите соответствующее ID: ");
            string id = Console.ReadLine();
            if (int.TryParse(id, out int intId))
            {
                _dbHelper.Remove(tables[intUserOptionNumber - 1], tableIds[intUserOptionNumber - 1], intId);
            }
        }
        Console.WriteLine("~~~");
    }

    private void Option4()
    {
        Console.Write("Введите название факультета: ");
        string arg = Console.ReadLine();
        _dbHelper.GetStudentsByDepartment(arg);
        Console.WriteLine("~~~");
    }

    private void Option5()
    {
        Console.Write("Введите ID преподавателя: ");
        string arg = Console.ReadLine();
        if (int.TryParse(arg, out int intArg))
        {
            _dbHelper.GetCoursesByTeacher(intArg);
        }
        Console.WriteLine("~~~");
    }

    private void Option6()
    {
        Console.Write("Введите ID курса: ");
        string arg = Console.ReadLine();
        if (int.TryParse(arg, out int intArg))
        {
            _dbHelper.GetStudentsByCourse(intArg);
        }
        Console.WriteLine("~~~");
    }

    private void Option7()
    {
        Console.Write("Введите ID курса: ");
        string arg = Console.ReadLine();
        if (int.TryParse(arg, out int intArg))
        {
            _dbHelper.GetGradesByCourse(intArg);
        }
        Console.WriteLine("~~~");
    }

    private void Option8()
    {
        Console.Write("Введите ID студента и ID курса: ");
        string[] args = Console.ReadLine().Split();
        if (args.Length == 2)
        {
            if (int.TryParse(args[0], out int intArg1) & int.TryParse(args[1], out int intArg2))
            {
                _dbHelper.GetStudentCourseGPA(intArg1, intArg2);
            }
        }
        Console.WriteLine("~~~");
    }

    private void Option9()
    {
        Console.Write("Введите ID студента: ");
        string arg = Console.ReadLine();
        if (int.TryParse(arg, out int intArg))
        {
            _dbHelper.GetStudentOverallGPA(intArg);
        }
        Console.WriteLine("~~~");
    }

    private void Option10()
    {
        Console.Write("Введите название факультета: ");
        string arg = Console.ReadLine();
        _dbHelper.GetDepartmentOverallGPA(arg);
        Console.WriteLine("~~~");
    }

    private void Option52()
    {
        _dbHelper.Clear();
        Console.WriteLine("~~~");
    }

    private void ShowOptions()
    {
        Console.WriteLine(@"1. Добавление нового студента, преподавателя, курса, экзамена и оценки.
2. Изменение информации о студентах, преподавателях и курсах.
3. Удаление студентов, преподавателей, курсов и экзаменов.
4. Получение списка студентов по факультету.
5. Получение списка курсов, читаемых определенным преподавателем.
6. Получение списка студентов, зачисленных на конкретный курс.
7. Получение оценок студентов по определенному курсу.
8. Средний балл студента по определенному курсу.
9. Средний балл студента в целом.
10. Средний балл по факультету.
52. Очистить базу данных.
69. Завершение работу.");
    }

    private string AskOptionNumber()
    {
        Console.Write("Введите номер действия: ");
        string optionNumber = Console.ReadLine();
        Console.WriteLine("~~~");
        return optionNumber;
    }
}
