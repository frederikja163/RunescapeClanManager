using LiteDB;
using System;
using System.Linq;
using Discord;
using System.Collections.Generic;

namespace RunescapeClanManager.DatabaseItems
{
    public class RunescapeGuild
    {
        [BsonId]
        public Guid _id { get; set; }
        public ulong _token { get; set; }

        public List<Reward> rewards { get; set; } = new List<Reward>();

#region Database
        /// <summary>
        /// Find a runescape guild from database
        /// </summary>
        /// <param name="id">The id in database</param>
        /// <returns>Found runescape guild or null if none found</returns>
        public static RunescapeGuild Find(Guid id)
        {
            using (LiteDatabase db = new LiteDatabase("RunescapeClanManager.db"))
            {
                return db.GetCollection<RunescapeGuild>().FindById(id);
            }
        }
        /// <summary>
        /// Find a runescape guild from database
        /// </summary>
        /// <param name="token">The token used by discord, to search for in database</param>
        /// <returns>Found runescape guild or null if none found</returns>
        public static RunescapeGuild Find(ulong token)
        {
            using (LiteDatabase db = new LiteDatabase("RunescapeClanManager.db"))
            {
                return db.GetCollection<RunescapeGuild>().FindAll().ToList().Find(x => x._token == token);
            }
        }
        /// <summary>
        /// Finds a guild in database or creates a new one
        /// </summary>
        /// <param name="id">Id in database to look for a guild at</param>
        /// <returns>Found guild or the one created</returns>
        public static RunescapeGuild FindOrCreate(Guid id)
        {
            RunescapeGuild guild = Find(id);
            return (guild != null) ? guild : new RunescapeGuild() { _id = id };
        }
        /// <summary>
        /// Finds a guild in database or creates a new one
        /// </summary>
        /// <param name="token">The token used by discord, to search for in database</param>
        /// <returns>Found guild or the one created</returns>
        public static RunescapeGuild FindOrCreate(ulong token)
        {
            RunescapeGuild guild = Find(token);
            return (guild != null) ? guild : new RunescapeGuild() { _token = token };
        }

        public void Save()
        {
            using (LiteDatabase db = new LiteDatabase("RunescapeClanManager.db"))
            {
                db.GetCollection<RunescapeGuild>().Upsert(_id, this);
            }
        }
        #endregion Database


        #region Operators
#endregion Operators
        public void Change(string changes)
        {
            
        }
    }
}
