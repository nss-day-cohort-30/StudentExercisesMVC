using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;

namespace StudentExercisesMVC.Repositories
{
    public class ExerciseRepository
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

        public static List<Exercise> GetExercises ()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT e.Id, e.Name, e.Language
                            FROM Exercise e
                        ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exercises = new List<Exercise>();
                    while (reader.Read())
                    {
                        Exercise exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };

                        exercises.Add(exercise);
                    }

                    reader.Close();

                    return exercises;
                }
            }
        }

        public static Exercise CreateExercise(Exercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Exercise (Name, Language)         
                                         OUTPUT INSERTED.Id                                                       
                                         VALUES (@Name, @Language)";
                    cmd.Parameters.Add(new SqlParameter("@Name", exercise.Name));
                    cmd.Parameters.Add(new SqlParameter("@Language", exercise.Language));

                    int newId = (int)cmd.ExecuteScalar();
                    exercise.Id = newId;
                    return exercise;
                }
            }
        }

        public static bool DeleteExercise(int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Exercise WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0) return false;
                        return true;
                    }
                }
            } catch
            {
                return false;
            }
        }

        public static void AssignToStudent(int exerciseId, int studentId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO StudentExercise (InstructorId, StudentId, ExerciseId)         
                                         VALUES (1, @student, @exercise)";
                    cmd.Parameters.Add(new SqlParameter("@student", studentId));
                    cmd.Parameters.Add(new SqlParameter("@exercise", exerciseId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool ClearAssignedExercises(int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM StudentExercise WHERE StudentId = @id";
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

        public static void UpdateExercise (Exercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Exercise
                                            SET Name = @exerciseName,
                                                Language = @Language
                                            WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@exerciseName", exercise.Name));
                    cmd.Parameters.Add(new SqlParameter("@Language", exercise.Language));
                    cmd.Parameters.Add(new SqlParameter("@id", exercise.Id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static Exercise GetExercise(int id)
        {
            Exercise exercise = null;
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT e.Id, e.Name, e.Langauge
                            FROM Exercise e WHERE e.Id = @Id
                        ";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };
                    }

                    reader.Close();
                    return exercise;
                }
            }
        }
    }
}
