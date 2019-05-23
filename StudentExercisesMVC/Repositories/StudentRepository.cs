using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;

namespace StudentExercisesMVC.Repositories
{
    public class StudentRepository
    {
        private static IConfiguration _config;

        public static void SetConfig(IConfiguration configuration)
        {
            _config = configuration;
        }

        public static SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public static List<Student> GetStudents()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT s.Id,
                                s.FirstName,
                                s.LastName,
                                s.SlackHandle,
                                s.CohortId
                            FROM Student s
                        ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> students = new List<Student>();
                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                        };

                        students.Add(student);
                    }

                    reader.Close();

                    return students;
                }
            }
        }

        public static Student CreateStudent(Student student)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId)         
                                         OUTPUT INSERTED.Id                                                       
                                         VALUES (@firstName, @lastName, @handle, @cId)";
                    cmd.Parameters.Add(new SqlParameter("@firstName", student.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", student.LastName));
                    cmd.Parameters.Add(new SqlParameter("@handle", student.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cId", student.CohortId));

                    int newId = (int)cmd.ExecuteScalar();
                    student.Id = newId;
                    return student;
                }
            }
        }

        public static bool DeleteStudent(int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Student WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0) return false;
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static void UpdateStudent(Student student)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Student
                                            SET FirstName = @firstName,
                                                LastName = @lastName,
                                                SlackHandle = @handle,
                                                CohortId = @cId
                                            WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@firstName", student.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", student.LastName));
                    cmd.Parameters.Add(new SqlParameter("@handle", student.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cId", student.CohortId));
                    cmd.Parameters.Add(new SqlParameter("@id", student.Id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static Student GetStudent(int id)
        {
            Student student = null;
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT s.Id,
                                s.FirstName,
                                s.LastName,
                                s.SlackHandle,
                                s.CohortId
                            FROM Student s WHERE s.Id = @StudentId
                        ";
                    cmd.Parameters.Add(new SqlParameter("@StudentId", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                        };
                    }

                    reader.Close();
                    return student;
                }
            }
        }
    }
}
