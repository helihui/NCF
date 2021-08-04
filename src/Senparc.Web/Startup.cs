using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.Ncf.Core.Models;
using Senparc.Ncf.Database;
//using Senparc.Ncf.Database.MySql;//������Ҫ��ӻ�ɾ����ʹ����Ҫ���� Senparc.Ncf.Database.MySql
//using Senparc.Ncf.Database.Sqlite;//������Ҫ��ӻ�ɾ����ʹ����Ҫ���� Senparc.Ncf.Database.Sqlite
using Senparc.Ncf.Database.SqlServer;//������Ҫ��ӻ�ɾ����ʹ����Ҫ���� Senparc.Ncf.Database.SqlServer
using Senparc.Ncf.Service.MultiTenant;
using Senparc.Web.Hubs;

namespace Senparc.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ָ�����ݿ�����
            /* AddDatabase<TDatabaseConfiguration>() ��������˵����
             *  SQLServerDatabaseConfiguration��     ʹ�� SQLServer���ݿ�
             *  SqliteMemoryDatabaseConfiguration��  ʹ�� SQLite ���ݿ�
             *  MySqlDatabaseConfiguration��         ʹ�� MySQL ���ݿ�
             */
            services.AddDatabase<SQLServerDatabaseConfiguration>();//Ĭ��ʹ�� SQLServer���ݿ⣬������Ҫ��д

            //��ӣ�ע�ᣩ Ncf ������Ҫ�����룡��
            services.AddNcfServices(Configuration, env, CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IOptions<SenparcCoreSetting> senparcCoreSetting,
            IOptions<SenparcSetting> senparcSetting,
            IHubContext<ReloadPageHub> hubContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            #region ���⻧

            app.UseMiddleware<TenantMiddleware>();//��������ö��⻧���ܣ�����ɾ��������

            #endregion


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            //Use NCF�����룩
            app.UseNcf(env, senparcCoreSetting, senparcSetting);
        }
    }
}