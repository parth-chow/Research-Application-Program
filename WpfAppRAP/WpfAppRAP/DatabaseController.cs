using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace WpfAppRAP
{
    class DatabaseController
    {
        private const string db = "kit206";
        private const string user = "kit206";
        private const string pass = "kit206";
        private const string server = "alacritas.cis.utas.edu.au";

        private static MySqlConnection conn = null;

        private static bool reportingErrors = false;
        public static List<Publication> publications;
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }


        private static MySqlConnection GetConnection()
        {
            if (conn == null)
            {
                string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
                conn = new MySqlConnection(connectionString);
            }
            return conn;
        }

        public static List<Researcher> LoadAll()
        {
            List<Researcher> staff = new List<Researcher>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select id, given_name, family_name, title from researcher", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    staff.Add(new Researcher { ID = rdr.GetInt32(0), GivenName = rdr.GetString(1), FamilyName = rdr.GetString(2), Title = rdr.GetString(3) });
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading staff", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return staff;
        }
        public static void Loadresearcherdetails(List<Researcher> res)
        {
            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select given_name, family_name, unit, campus, email, photo, degree, supervisor_id, level, utas_start, current_start, type from researcher", conn);

                rdr = cmd.ExecuteReader();
                int i = 0;
                while (rdr.Read())
                {

                    if (i < res.Count)
                    {
                        res[i].GivenName = rdr[0].ToString();
                        res[i].FamilyName = rdr[1].ToString();
                        res[i].Unit = rdr[2].ToString();
                        res[i].Campus = rdr[3].ToString();
                        res[i].Email = rdr[4].ToString();
                        res[i].Photo = (rdr[5].ToString());
                        res[i].Degree = rdr[6].ToString();
                        res[i].SupervisorID = rdr[7].ToString();
                        Enum.TryParse(rdr[8].ToString(), out Level myStatus);
                        res[i].Level = myStatus;
                        res[i].CommencedWithInstitution = DateTime.Parse(rdr[9].ToString());
                        res[i].CommencedWithPosition = DateTime.Parse(rdr[10].ToString());
                        Enum.TryParse(rdr[11].ToString(), out typo ms);
                        res[i].Type = ms;
                    }
                    i++;
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public static void LoadPublicationdoiDetails(List<Publication> pw, int id)
        {
            publications = new List<Publication>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select researcher_id, doi from researcher_publication", conn);

                rdr = cmd.ExecuteReader();
                int i = 0;
                while (rdr.Read())
                    {
                    
                    while (i < pw.Count)
                    {
                        if (id == rdr.GetInt32(0))
                        {


                            pw[i].Doi = rdr.GetString(1);
                            //pw[i].Authors = rdr.GetString(2);
                            //pw[i].Citeas = rdr.GetString(3);
                            pw[i].Age = (DateTime.Now - pw[i].Certified).TotalDays;
                            i++;
                            


                        }
                        break;
                    }

                    }
                
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            //return publications;
        }

        public static List<Publication> LoadPublications(int id)
        {
            publications = new List<Publication>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select title, year, type, available, authors, cite_as " +
                                                    "from publication as pub, researcher_publication as respub " +
                                                    "where pub.doi=respub.doi and researcher_id=?id", conn);

                cmd.Parameters.AddWithValue("id", id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    publications.Add(new Publication
                    {
                        Title = rdr.GetString(0),
                        Year = rdr.GetInt32(1),
                        Mode = ParseEnum<Mode>(rdr.GetString(2)),
                        Certified = rdr.GetDateTime(3),
                        Authors=rdr.GetString(4),
                        Citeas= rdr.GetString(5)
                        
                });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return publications;
        }
        public static float CountrecentPublication(Researcher e, int startYear, int endYear)
        {
            MySqlConnection conn = GetConnection();
            float c = 0;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select count(*) from publication as pub, researcher_publication as respub " +
                "where pub.doi = respub.doi and researcher_id = ?id and year >= ?start and year <= ?end", conn);
                cmd.Parameters.AddWithValue("id", e.ID);
                cmd.Parameters.AddWithValue("start", startYear);
                cmd.Parameters.AddWithValue("end", endYear);
                c = Int32.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error connecting to database: " + ex);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return c;
        }
        private static void ReportError(string msg, Exception e)
        {
            if (reportingErrors)
            {
               // MessageBox.Show("An error occurred while " + msg + ". Try again later.\n\nError Details:\n" + e,
                //    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
