using Domain;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESHtUNI.Data
{
    public class Database
    {
        SQLiteAsyncConnection DB;
        public Database()
        {
            
        }
        async Task Init()
        {
            if (DB is not null)
            {
                return;
            }
            DB = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await DB.CreateTableAsync<Lesson>();
            await DB.CreateTableAsync<Day>();
            await DB.CreateTableAsync<Week>();
            await DB.CreateTableAsync<GroupTimeTable>();
        }

        public async Task<Week> GetWeekAsync(int weekNumber)
        {
            await Init();
            return await DB.Table<Week>().FirstOrDefaultAsync(x => x.Number == weekNumber);
        }

        public async Task<GroupTimeTable> GetGroupTimetableAsync()
        {
            await Init();
            return await DB.Table<GroupTimeTable>().FirstOrDefaultAsync();

            // SQL queries are also possible
            //return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        //public async task<todoitem> getitemasync(int id)
        //{
        //    await init();
        //    return await database.table<todoitem>().where(i => i.id == id).firstordefaultasync();
        //}

        public async Task<int> SaveTimeTableAsync(GroupTimeTable timetable)
        {
            await Init();
            await DeleteTimeTableAsync();
            return await DB.InsertAsync(timetable);
        }

        public async Task<int> DeleteTimeTableAsync()
        {
            await Init();
            return await DB.DeleteAllAsync<GroupTimeTable>();
        }
    }
}
