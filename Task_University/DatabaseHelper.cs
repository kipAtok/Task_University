
using System.Data.SQLite;

class DatabaseHelper
{
    private SQLiteConnection _connection;
    private string _filePath;

    public DatabaseHelper(SQLiteConnection connection, string filePath)
    { 
        _connection = connection;
        _filePath = filePath;
        if (!File.Exists(_filePath))
        {
            _connection.Open();
            CreateDatabase();
            Console.WriteLine("База данных создана.");
        }
        else
        {
            _connection.Open();
            Console.WriteLine("Соединение с базой данных открыто.");
        }
    }

    public void Clear()
    {
        _connection.Close();
        File.Delete(_filePath);
        _connection.Open();
        CreateDatabase();
        Console.WriteLine("База данных очищена.");
    }

    public void Kill()
    {
        _connection.Close();
        Console.WriteLine("Соединение с базой данных закрыто.");
    }

    public void AddStudent(string name, string surname, string department, string dateOfBirth)
    {
        using (var command = new SQLiteCommand(_connection))
        {
            command.CommandText = "INSERT INTO Students (name, surname, department, date_of_birth) VALUES (@name, @surname, @department, @date_of_birth)";
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@surname", surname);
            command.Parameters.AddWithValue("@department", department);
            command.Parameters.AddWithValue("@date_of_birth", dateOfBirth);
            command.ExecuteNonQuery();
            Console.WriteLine($"Студент {name} {surname} добавлен.");
        }
    }

    public void AddTeacher(string name, string surname, string department)
    {
        using (var command = new SQLiteCommand(_connection))
        {
            command.CommandText = $"INSERT INTO Teachers (name, surname, department) VALUES (@name, @surname, @department)";
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@surname", surname);
            command.Parameters.AddWithValue("@department", department);
            command.ExecuteNonQuery();
            Console.WriteLine($"Преподаватель {name} {surname} добавлен.");
        }
    }

    public void AddCourse(string title, string description, int teacherId)
    {
        using (var command = new SQLiteCommand($"SELECT EXISTS(SELECT * from Teachers where Teachers.teacher_id = {teacherId}) as result", _connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if ((int)(long)reader["result"] == 1)
                    {
                        using (var command1 = new SQLiteCommand(_connection))
                        {
                            command1.CommandText = "INSERT INTO Courses (title, description, teacher_id) VALUES (@title, @description, @teacher_id)";
                            command1.Parameters.AddWithValue("@title", title);
                            command1.Parameters.AddWithValue("@description", description);
                            command1.Parameters.AddWithValue("@teacher_id", teacherId);
                            command1.ExecuteNonQuery();
                            Console.WriteLine($"Курс '{title}' добавлен.");
                        }
                    }
                }
            }
        } 
    }

    public void AddExam(string dateOfExam, int courseId, int maxScore)
    {
        using (var command = new SQLiteCommand($"SELECT EXISTS(SELECT * from Courses where Courses.course_id = {courseId}) as result", _connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if ((int)(long)reader["result"] == 1)
                    {
                        using (var command1 = new SQLiteCommand(_connection))
                        {
                            command1.CommandText = "INSERT INTO Exams (date_of_exam, course_id, max_score) VALUES (@date_of_exam, @course_id, @max_score)";
                            command1.Parameters.AddWithValue("@date_of_exam", dateOfExam);
                            command1.Parameters.AddWithValue("@course_id", courseId);
                            command1.Parameters.AddWithValue("@max_score", maxScore);
                            command1.ExecuteNonQuery();
                            Console.WriteLine($"Экзамен на {dateOfExam} добавлен.");
                        }
                    }
                }
            }
        }
    }

    public void AddGrade(int studentId, int examId, int score)
    {
        using (var command = new SQLiteCommand($"SELECT EXISTS(SELECT* from Students WHERE student_id = {studentId}) and EXISTS(SELECT* FROM Exams WHERE exam_id = {examId} and max_score >= {score}) as result", _connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if ((int)(long)reader["result"] == 1)
                    {
                        using (var command1 = new SQLiteCommand(_connection))
                        {
                            command1.CommandText = "INSERT INTo Grades (student_id, exam_id, score) VALUES (@student_id, @exam_id, @score)";
                            command1.Parameters.AddWithValue("@student_id", studentId);
                            command1.Parameters.AddWithValue("@exam_id", examId);
                            command1.Parameters.AddWithValue("@score", score);
                            command1.ExecuteNonQuery();
                            Console.WriteLine($"Оценка '{score}' добавлена.");
                        }
                    }
                }
            }
        }
    }

    public void ChangeStudent(int studentId, string name, string surname, string department, string dateOfBirt)
    {
        using (var command = new SQLiteCommand(_connection))
        {
            command.CommandText = $@"UPDATE Students SET name = {(!String.IsNullOrWhiteSpace(name) ? "@" : "")}name, 
                                    surname = {(!String.IsNullOrWhiteSpace(surname) ? "@" : "")}surname, 
                                    department = {(!String.IsNullOrWhiteSpace(department) ? "@" : "")}department, 
                                    date_of_birth = {(!String.IsNullOrWhiteSpace(dateOfBirt) ? "@" : "")}date_of_birth WHERE student_id = @student_id";
            if (!String.IsNullOrWhiteSpace(name))
            {
                command.Parameters.AddWithValue("@name", name);
            }
            if (!String.IsNullOrWhiteSpace(surname))
            {
                command.Parameters.AddWithValue("@surname", surname);
            }
            if (!String.IsNullOrWhiteSpace(department))
            {
                command.Parameters.AddWithValue("@department", department);
            }
            if (!String.IsNullOrWhiteSpace(dateOfBirt))
            {
                command.Parameters.AddWithValue("@date_of_birth", dateOfBirt);
            }
            using (var command1 = new SQLiteCommand($"SELECT EXISTS(SELECT * from Students where Students.student_id = {studentId}) as result", _connection))
            {
                using (var reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if ((int)(long)reader["result"] == 1 & studentId > 0)
                        {
                            command.Parameters.AddWithValue("@student_id", studentId);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            command.ExecuteNonQuery();
            Console.WriteLine($"Данные о студенте под номером '{studentId}' изменены.");
        }
    }

    public void ChangeTeacher(int teacherId, string name, string surname, string department)
    {
        using (var command = new SQLiteCommand(_connection))
        {
            command.CommandText = $@"UPDATE Teachers SET name = {(!String.IsNullOrWhiteSpace(name) ? "@" : "")}name, 
                                    surname = {(!String.IsNullOrWhiteSpace(surname) ? "@" : "")}surname, 
                                    department = {(!String.IsNullOrWhiteSpace(department) ? "@" : "")}department WHERE teacher_id = @teacher_id";
            if (!String.IsNullOrWhiteSpace(name))
            {
                command.Parameters.AddWithValue("@name", name);
            }
            if (!String.IsNullOrWhiteSpace(surname))
            {
                command.Parameters.AddWithValue("@surname", surname);
            }
            if (!String.IsNullOrWhiteSpace(department))
            {
                command.Parameters.AddWithValue("@department", department);
            }
            using (var command1 = new SQLiteCommand($"SELECT EXISTS(SELECT * from Teachers where Teachers.teacher_id = {teacherId}) as result", _connection))
            {
                using (var reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if ((int)(long)reader["result"] == 1 & teacherId > 0)
                        {
                            command.Parameters.AddWithValue("@teacher_id", teacherId);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            command.ExecuteNonQuery();
            Console.WriteLine($"Данные о преподователе под номером '{teacherId}' изменены.");
        }
    }

    public void ChangeCourse(int courseId, string title, string description, int teacherId=0)
    {
        using (var command = new SQLiteCommand(_connection))
        {
            command.CommandText = $@"UPDATE Courses SET title = {(!String.IsNullOrWhiteSpace(title) ? "@" : "")}title, 
                                    description = {(!String.IsNullOrWhiteSpace(description) ? "@" : "")}description, 
                                    teacher_id = {(teacherId > 0 ? "@" : "")}teacher_id WHERE course_id = @course_id";
            if (!String.IsNullOrWhiteSpace(title))
            {
                command.Parameters.AddWithValue("@title", title);
            }
            if (!String.IsNullOrWhiteSpace(description))
            {
                command.Parameters.AddWithValue("@description", description);
            }
            using (var command1 = new SQLiteCommand($"SELECT EXISTS(SELECT * from Teachers where Teachers.teacher_id = {teacherId}) as result", _connection))
            {
                using (var reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if ((int)(long)reader["result"] == 1 & teacherId > 0)
                        {
                            command.Parameters.AddWithValue("@teacher_id", teacherId);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            command.Parameters.AddWithValue("@course_id", courseId);
            command.ExecuteNonQuery();
            Console.WriteLine($"Данные о курсе под номером '{courseId}' изменены.");
        }
    }

    public void Remove(string table, string tableId, int id)
    {
        using (var command = new SQLiteCommand($"SELECT EXISTS(SELECT * FROM {table} WHERE {tableId} = {id}) as result", _connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if ((int)(long)reader["result"] == 1)
                    {
                        using (var command1 = new SQLiteCommand($"DELETE FROM {table} WHERE {tableId} = {id}", _connection))
                        {
                            command1.ExecuteNonQuery();
                            Console.WriteLine($"Элемент ряда '{table}' под номером '{id}'удален.");
                        }
                    }
                }
            }
        }
    }
     
    public void GetStudentsByDepartment(string department)
    {
        using (var command = new SQLiteCommand($"SELECT student_id, name, surname FROM Students WHERE department = '{department}'", _connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader[0]}, Имя: {reader[1]} {reader[2]}.");
                }
            }
        }
    }

    public void GetCoursesByTeacher(int teacherId)
    {
        using (var command = new SQLiteCommand($"SELECT course_id, title FROM Courses WHERE Courses.teacher_id = {teacherId}", _connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader[0]}, Название: {reader[1]}.");
                }
            }
        }
    }

    public void GetStudentsByCourse(int courseId)
    {
        string commandString = $@"SELECT Students.student_id, Students.name, Students.surname
                                from Grades
                                join Students on Students.student_id = Grades.student_id
                                join Exams on Exams.exam_id = Grades.exam_id
                                join Courses on Courses.course_id = Exams.course_id
                                where Courses.course_id = {courseId}";
        using (var command = new SQLiteCommand(commandString, _connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID студента: {reader[0]}, Имя: {reader[1]} {reader[2]}.");
                }
            }
        }
    }

    public void GetGradesByCourse(int courseId)
    {
        string commandString = $@"SELECT Students.student_id, Students.name, Students.surname, Grades.score
                                from Grades
                                join Students on Students.student_id = Grades.student_id
                                join Exams on Exams.exam_id = Grades.exam_id
                                join Courses on Courses.course_id = Exams.course_id
                                where Courses.course_id = {courseId}";
        using (var command = new SQLiteCommand(commandString, _connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID студента: {reader[0]}, Имя: {reader[1]} {reader[2]}, Оценка: {reader[3]}.");
                }
            }
        }
    }

    public void GetStudentCourseGPA(int studentId, int courseId)
    {
        string commandString = $@"SELECT avg(Grades.score)
                                from Grades 
                                join Students on Students.student_id = Grades.student_id and Students.student_id = {studentId}
                                join Exams on Exams.exam_id = Grades.exam_id
                                join Courses on Courses.course_id = Exams.course_id
                                where Courses.title = {courseId}";
        using (var command = new SQLiteCommand(commandString, _connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Средняя оценка студента за курс: {reader[0]}.");
                }
            }
        }
    }

    public void GetStudentOverallGPA(int studentId) 
    { 
        string commandString = $@"SELECT avg(Grades.score)
                                from Grades 
                                join Students on Students.student_id = Grades.student_id and Students.student_id = {studentId}";
        using (var command = new SQLiteCommand(commandString, _connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Средняя оценка студента в целом: {reader[0]}.");
                }
            }
        }
    }

    public void GetDepartmentOverallGPA(string department)
    {
        string commandString = $@"SELECT avg(Grades.score)
                                from Grades 
                                join Students on Students.student_id = Grades.student_id
                                WHERE Students.department = '{department}'";
        using (var command = new SQLiteCommand(commandString, _connection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Средняя оценка по факультету в целом: {reader[0]}.");
                }
            }
        }
    }

    private void CreateDatabase()
    {
        string createTables = @"
            CREATE TABLE Students (
                student_id INTEGER PRIMARY KEY AUTOINCREMENT, 
                name TEXT,
                surname TEXT,
                department TEXT,
                date_of_birth TEXT);
            CREATE TABLE Teachers (
                teacher_id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT,
                surname TEXT,
                department TEXT);
            CREATE TABLE Courses (
                course_id INTEGER PRIMARY KEY AUTOINCREMENT,
                title TEXT,
                description TEXT,
                teacher_id INTEGER,
                FOREIGN KEY (teacher_id) REFERENCES Teachers(teacher_id));
            CREATE TABLE Exams (
                exam_id INTEGER PRIMARY KEY AUTOINCREMENT,
                date_of_exam TEXT,
                course_id INTEGER,
                max_score INTEGER,
                FOREIGN KEY (course_id) REFERENCES Courses(course_id));
            CREATE TABLE Grades (
                grade_id INTEGER PRIMARY KEY AUTOINCREMENT,
                student_id INTEGER,
                exam_id INTEGER,
                score INTEGER,
                FOREIGN KEY (student_id) REFERENCES Students(student_id),
                FOREIGN KEY (exam_id) REFERENCES Exams(exam_id));";
        using (var command = new SQLiteCommand(_connection))
        {
            command.CommandText = createTables;
            command.ExecuteNonQuery();
        }
    }
}
