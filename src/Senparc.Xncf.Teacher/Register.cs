﻿using System;
using System.Collections.Generic;
using Senparc.Ncf.XncfBase;
using Senparc.Xncf.Teacher.Functions;
using Senparc.Xncf.Teacher.Models.DatabaseModel;
using Senparc.Ncf.Core.Enums;
using Senparc.Ncf.Core.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Ncf.Database;

namespace Senparc.Xncf.Teacher
{
    [XncfRegister]
    public partial class Register : XncfRegisterBase, IXncfRegister
    {
        #region IXncfRegister 接口

        public override string Name => "Senparc.Xncf.Teacher";

        public override string Uid => "2DA001CD-584A-4B3F-B8A1-E286BBEA4C63";//必须确保全局唯一，生成后必须固定，已自动生成，也可自行修改

        public override string Version => "1.0";//必须填写版本号

        public override string MenuName => "老师管理";

        public override string Icon => "fa fa-star";

        public override string Description => "老师";

        public override IList<Type> Functions => new Type[] { typeof(MyFunction) };


                public override async Task InstallOrUpdateAsync(IServiceProvider serviceProvider, InstallOrUpdate installOrUpdate)
        {
            //安装或升级版本时更新数据库
            await base.MigrateDatabaseAsync(serviceProvider);

            //根据安装或更新不同条件执行逻辑
            switch (installOrUpdate)
            {
                case InstallOrUpdate.Install:
                    //新安装
                                        break;
                case InstallOrUpdate.Update:
                    //更新
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override async Task UninstallAsync(IServiceProvider serviceProvider, Func<Task> unsinstallFunc)
        {
            #region 删除数据库（演示）

            var mySenparcEntitiesType = this.TryGetXncfDatabaseDbContextType;
            TeacherSenparcEntities mySenparcEntities = serviceProvider.GetService(mySenparcEntitiesType) as TeacherSenparcEntities;

            //指定需要删除的数据实体

            //注意：这里作为演示，在卸载模块的时候删除了所有本模块创建的表，实际操作过程中，请谨慎操作，并且按照删除顺序对实体进行排序！
            var dropTableKeys = EntitySetKeys.GetEntitySetInfo(this.TryGetXncfDatabaseDbContextType).Keys.ToArray();
            await base.DropTablesAsync(serviceProvider, mySenparcEntities, dropTableKeys);

            #endregion

            await unsinstallFunc().ConfigureAwait(false);
        }
        
        #endregion
    }
}