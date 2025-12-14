using DemoWpf.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Documents;

namespace DemoWpf.Data
{
    internal static class DbHelpers
    {

        public static User Authorize(string login, string password)
        {
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    conn.Open();

                    string sql = @"SELECT u.id_user, u.surname, u.name, u.last_name, r.role_name
                               FROM users u JOIN roles r ON r.id_role = u.id_role
                               WHERE u.login = @login AND u.password = @password";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(0),
                                Surname = reader.GetString(1),
                                Name = reader.GetString(2),
                                LastName = reader.GetString(3),
                                Role = reader.GetString(4)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при работе с БД: {ex.Message}");
            }

            return null;
        }

        public static List<Good> GetGoodsList()
        {
            var list = new List<Good>();
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    conn.Open();

                    string sql = @"SELECT g.article, c.category_name, l.label, g.description,
                               f.fabric, p.provider_name, g.price, g.unit_of_measure, g.count, g.sale, g.photo
                               FROM goods g 
                               JOIN categories c ON c.id_category = g.id_category
                               JOIN labels l ON g.id_label = l.id_label
                               JOIN providers p ON g.id_provider = p.id_provider
                               JOIN fabrics f ON g.id_fabric = f.id_fabric";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Good
                            {
                                article = reader.GetString(0),
                                category = reader.IsDBNull(1) ? null : reader.GetString(1),
                                label = reader.IsDBNull(2) ? null : reader.GetString(2),
                                desctiption = reader.IsDBNull(3) ? null : reader.GetString(3),
                                fabric = reader.IsDBNull(4) ? null : reader.GetString(4),
                                provider = reader.IsDBNull(5) ? null : reader.GetString(5),
                                price = reader.IsDBNull(6) ? 0f : (float)reader.GetDouble(6),
                                unit_of_measure = reader.IsDBNull(7) ? null : reader.GetString(7),
                                count = reader.IsDBNull(8) ? 0 : Convert.ToInt32(reader[8]),
                                discount = reader.IsDBNull(9) ? 0f : (float)reader.GetDouble(9),
                                photo = reader.IsDBNull(10) ? null : reader.GetString(10)
                            }
                            );
                        }

                    }
                    return list;
                }
            
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вытяжке: {ex.Message}");
                return null;
            }
        }
    } 
}