using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;

namespace StudentExercisesMVC.Repositories
{
    public class CohortRepository
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

        public static List<Cohort> GetCohorts ()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT c.Id,
                                c.Name
                            FROM Cohort c
                        ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();
                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        cohorts.Add(cohort);
                    }

                    reader.Close();

                    return cohorts;
                }
            }
        }

        public static Cohort CreateCohort(Cohort cohort)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Cohort (Name)         
                                         OUTPUT INSERTED.Id                                                       
                                         VALUES (@cohortName)";
                    cmd.Parameters.Add(new SqlParameter("@cohortName", cohort.Name));

                    int newId = (int)cmd.ExecuteScalar();
                    cohort.Id = newId;
                    return cohort;
                }
            }
        }

        public static bool DeleteCohort(int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Cohort WHERE Id = @id";
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

        public static void UpdateCohort (Cohort cohort)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Cohort
                                            SET Name = @cohortName
                                            WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@cohortName", cohort.Name));
                    cmd.Parameters.Add(new SqlParameter("@id", cohort.Id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static Cohort GetCohort(int id)
        {
            Cohort cohort = null;
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT c.Id,
                                c.Name,
                            FROM Cohort c WHERE c.Id = @Id
                        ";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }

                    reader.Close();
                    return cohort;
                }
            }
        }
    }
}
