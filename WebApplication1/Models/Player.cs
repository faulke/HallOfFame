using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;

namespace WebApplication1.Models
{
    public class SearchFilter 
    {
        public string name { get; set; }
        public string bats { get; set; }
    }

    public class Player
    {
        /****** BEGIN PROPERTIES *******/
        public string PlayerId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string BirthYear { get; }
        public string BirthMonth { get; }
        public string BirthDay { get; }
        public string BirthDate
        {
            get
            {
                if (String.IsNullOrEmpty(BirthYear))
                {
                    return "Unknown";
                }
                return BirthMonth + "/" + BirthDay + "/" + BirthYear;
            }
        }
        public string Weight { get; }
        public string Height { get; }
        public string Bats { get; }
        public string Throws { get; }
        public string Debut { get; }
        public string FinalGame { get; }
        /****** END PROPERTIES *******/


        /***** BEGIN METHODS **********/
        public static IEnumerable<Player> GetAll()
        {
            using (var conn = DataConnection.GetOpenConnection())
            {
                var sql = @"select top 50
                                m.*,
                                m.nameFirst as FirstName,
                                m.nameLast as LastName
                            from
                                Master as m";
                return conn.Query<Player>(sql).ToList();
            }
        }
        public static Player GetById(string id)
        {
            using (var conn = DataConnection.GetOpenConnection())
            {
                var sql = @"select
                                m.*,
                                m.nameFirst as FirstName,
                                m.nameLast as LastName
                            from
                                Master as m
                            where
                                playerID = @id";

                return conn.Query<Player>(sql, new { id = id }).FirstOrDefault();
            }
        }
        public static IEnumerable<Player> SearchPlayers(SearchFilter filter)
        {
            DynamicParameters dbArgs = new DynamicParameters();
            using (var conn = DataConnection.GetOpenConnection())
            {

                var sql = @"select 
                                m.*,
                                m.nameFirst as FirstName,
                                m.nameLast as LastName 
                            from 
                                Master as m
                            where
                                m.playerID is not null
                           ";

                // TO DO: Add TOP statement for calls with no query string (i.e., return top 50 instead of all)
                // TO DO: Add more query parameters (e.g., position, bats, throws, dates played, 

                if (!String.IsNullOrEmpty(filter.name))
                {
                    sql += "and concat(m.nameFirst, m.nameLast) like @name \n";
                    dbArgs.Add("name", "%" + filter.name + "%");
                }

                if (!String.IsNullOrEmpty(filter.bats))
                {
                    sql += "and m.bats = @bats \n";
                    dbArgs.Add("bats", filter.bats);
                }

                sql += "\norder by m.nameLast";

                return conn.Query<Player>(sql, dbArgs).ToList();
            }
        }
    }
}