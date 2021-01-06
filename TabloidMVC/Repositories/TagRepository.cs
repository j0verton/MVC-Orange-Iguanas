using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IConfiguration config) : base(config) { }

        public List<Tag> GetAllTags()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name
                                        FROM Tag
                                        WHERE Active=1";
                    var reader = cmd.ExecuteReader();

                    var tags = new List<Tag>();

                    while (reader.Read())
                    {
                        tags.Add(NewTagFromReader(reader));
                    }

                    reader.Close();

                    return tags;
                }
            }
        }

        //need to double check this sql query 
        public List<Tag> GetTagsByPost(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT t.Id, t.Name
                                        FROM Tag t
                                        LEFT JOIN PostTag pt ON pt.TagId = t.Id
                                        WHERE pt.PostId = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    var tags = new List<Tag>();

                    while (reader.Read())
                    {
                        tags.Add(NewTagFromReader(reader));
                    }

                    reader.Close();

                    return tags;
                }
            }
        }
        
        public List<PostTag> GetPostTagsByPost(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT t.Id, t.Name, pt.Id AS PostTagId, pt.PostId
                                        FROM Tag t
                                        JOIN PostTag as pt ON pt.TagId = t.Id
                                        WHERE pt.PostId = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    var tags = new List<PostTag>();

                    while (reader.Read())
                    {
                        PostTag posttag = new PostTag()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("PostTagId")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            Tag = new Tag()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            },
                            TagId = reader.GetInt32(reader.GetOrdinal("Id")),
                        };
                        tags.Add(posttag);
                    }
                    reader.Close();
                    return tags;
                }
            }
        }

        public Tag GetTagById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name]
                        FROM Tag
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    var tag = new Tag();
                    if (reader.Read())
                    {
                        tag = NewTagFromReader(reader);
                    };
                    reader.Close();
                    return tag;
                }


            
            }
        }

        public void Add(Tag tag)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Tag (Name, Active)
                                        OUTPUT INSERTED.ID
                                        VALUES(@Name, 1)";
                    cmd.Parameters.AddWithValue("@Name", tag.Name);


                    tag.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void AddTagToPost(int tagId, int postId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO PostTag (
                        PostId, TagId )
                        OUTPUT INSERTED.ID
                        VALUES (
                        @postid, @tagid )";
                    cmd.Parameters.AddWithValue("@postid", postId);
                    cmd.Parameters.AddWithValue("@tagid", tagId);
                    cmd.ExecuteScalar();
                    //post.Id = (int)cmd.ExecuteScalar();


                }
            }
        }

        public void EditTag(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Tag
                            SET [Name] = @name
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    cmd.Parameters.AddWithValue("@id", tag.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTag(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Tag SET [Active]=0
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        public void RemoveTagFromPost(int postTagId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM PostTag 
                        WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", postTagId);
                    cmd.ExecuteScalar();

                }
            }
        }



        private Tag NewTagFromReader(SqlDataReader reader)
        {
            return new Tag()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
            };
        }
    }
}