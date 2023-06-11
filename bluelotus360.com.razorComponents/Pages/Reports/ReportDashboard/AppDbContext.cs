using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.Com.MauiSupports;
using SQLite;

namespace bluelotus360.com.razorComponents.Pages.Reports.ReportDashboard
{
    public class AppDbContext 
    {
        

        public SQLiteAsyncConnection Database;

        public async Task Init()
        {
            if (Database is not null) return;
            Database = new SQLiteAsyncConnection(NativeConstants.DatabasePath, NativeConstants.flags);
            await Database.CreateTableAsync<RecentlyAccessedPage>();
        }

        public async Task<int> AddRecentPageAsync(string pageName, string pageUrl)
        {
            await Init();

            List<RecentlyAccessedPage> data = await this.GetContents();

            //var page = new RecentlyAccessedPage
            //{
            //    PageName = pageName,
            //    PageUrl = pageUrl,
            //    AccessTime = DateTime.UtcNow
            //};

            //if (data.Count >= 6)
            //{
            //    return await Database.UpdateAsync(page);
            //}
            //else if(data.Count<6)
            //{

            //    return await Database.InsertAsync(page);
            //}

            //return 0;

            if (data.Count >= 6)
            {
                RecentlyAccessedPage oldestPage = data.OrderBy(p => p.AccessTime).First();
                await Database.DeleteAsync(oldestPage);
            }

            // Insert the new recent page
            RecentlyAccessedPage page = new RecentlyAccessedPage
            {
                PageName = pageName,
                PageUrl = pageUrl,
                AccessTime = DateTime.UtcNow
            };
            return await Database.InsertAsync(page);

        }

        

        public async Task<List<RecentlyAccessedPage>> GetContents()
        {
            await Init();
            List<RecentlyAccessedPage> list = await Database.Table<RecentlyAccessedPage>()
         .OrderByDescending(p => p.AccessTime)
         .Take(6)
         .ToListAsync();


            return list;
        }
    }

    public class RecentlyAccessedPage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public DateTime AccessTime { get; set; }
    }
}
