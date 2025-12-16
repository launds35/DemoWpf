using DemoWpf.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
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
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при работе с БД: {ex.Message}");
            }

            return null;
        }

        public static List<ComboBoxItems> GetCategories()
        {
            var list = new List<ComboBoxItems>();
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    conn.Open();

                    string sql = @"SELECT id_category, category_name FROM categories";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ComboBoxItems
                            {
                                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? null : reader.GetString(1)
                            });
                        }
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}");
            }
            

            throw new Exception("Не удалось получить ID");
        }

        public static List<ComboBoxItems> GetFabrics()
        {
            var list = new List<ComboBoxItems>();
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    conn.Open();

                    string sql = @"SELECT id_fabric, fabric FROM fabrics";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ComboBoxItems
                            {
                                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? null : reader.GetString(1)
                            });
                        }
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}");
            }


            throw new Exception("Не удалось получить ID");
        }


        public static List<ComboBoxItems> GetLabels()
        {
            var list = new List<ComboBoxItems>();
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    conn.Open();

                    string sql = @"SELECT id_label, label FROM labels";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ComboBoxItems
                            {
                                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? null : reader.GetString(1)
                            });
                        }
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}");
            }


            throw new Exception("Не удалось получить ID");
        }

        public static List<ComboBoxItems> GetProviders()
        {
            var list = new List<ComboBoxItems>();
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    conn.Open();

                    string sql = @"SELECT id_provider, provider_name FROM providers";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ComboBoxItems
                            {
                                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? null : reader.GetString(1)
                            });
                        }
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}");
            }


            throw new Exception("Не удалось получить ID");
        }

        public static bool AddGood(string article, int idCategory, int idLabel,
                                      string description, int idFabric, int idProvider,
                                      double price, string unitOfMeasure, int count, double discount)
        {
            try
            {
                using (SqlConnection connection = Db.GetConnection())
                {
                    connection.Open();

                    string sql = @"INSERT INTO goods (
                                   article, id_category, id_label, 
                                   description, id_fabric, id_provider, price,
                                   unit_of_measure, count, sale)
                                   VALUES (@article, @id_category, @id_label, 
                                   @description, @id_fabric, @id_provider, @price,
                                   @unit_of_measure, @count, @sale)";


                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@article", article);
                    cmd.Parameters.AddWithValue("@id_category", idCategory);
                    cmd.Parameters.AddWithValue("@id_label", idLabel);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@id_fabric", idFabric);
                    cmd.Parameters.AddWithValue("@id_provider", idProvider);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@unit_of_measure", unitOfMeasure);
                    cmd.Parameters.AddWithValue("@count", count);
                    cmd.Parameters.AddWithValue("@sale", discount);
                    int row_affected = cmd.ExecuteNonQuery();

                    return row_affected > 0;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex}");
                return false;
            }
        }

        public static void DeleteGood(string article)
        {
            try
            {
                using (SqlConnection connection = Db.GetConnection())
                {
                    connection.Open();

                    string sql = @"DELETE FROM TABLE goods WHERE goods.article = @article";

                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@article", article);
                    int row_affected = cmd.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex}");
            }
        }

        public static bool UpdateGood(string article, string newArticle, int idCategory, int idLabel, 
                                      string description, int idFabric, int idProvider, 
                                      double price, string unitOfMeasure, int count, double discount)
        {
            try
            {
                using (SqlConnection connection = Db.GetConnection())
                {
                    connection.Open();

                    string sql = @"UPDATE goods 
                                   SET article = @newArticle, id_category = @id_category, id_label = @id_label, 
                                   description = @description, id_fabric = @id_fabric, id_provider = @id_provider, price = @price,
                                   unit_of_measure = @unit_of_measure, count = @count, sale = @sale
                                   WHERE article = @article";

                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@article", article);
                    cmd.Parameters.AddWithValue("@newArticle", newArticle);
                    cmd.Parameters.AddWithValue("@id_category", idCategory);
                    cmd.Parameters.AddWithValue("@id_label", idLabel);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@id_fabric", idFabric);
                    cmd.Parameters.AddWithValue("@id_provider", idProvider);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@unit_of_measure", unitOfMeasure);
                    cmd.Parameters.AddWithValue("@count", count);
                    cmd.Parameters.AddWithValue("@sale", discount);

                    int row_affected = cmd.ExecuteNonQuery();

                    return row_affected > 0;
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex}");
                return false;
            }
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
                                Article = reader.GetString(0),
                                Category = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Label = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Desctiption = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Fabric = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Provider = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Price = reader.IsDBNull(6) ? 0f : (float)reader.GetDouble(6),
                                Unit_of_measure = reader.IsDBNull(7) ? null : reader.GetString(7),
                                Count = reader.IsDBNull(8) ? 0 : Convert.ToInt32(reader[8]),
                                Discount = reader.IsDBNull(9) ? 0f : (float)reader.GetDouble(9),
                                Photo = reader.IsDBNull(10) ? null : reader.GetString(10)
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