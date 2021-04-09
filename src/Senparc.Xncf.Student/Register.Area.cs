﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET.Trace;
using Senparc.Ncf.Core.Areas;
using Senparc.Ncf.Core.Config;
using System;
using Senparc.Ncf.XncfBase;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace Senparc.Xncf.Student
{
	public partial class Register : IAreaRegister, //注册 XNCF 页面接口（按需选用）
									IXncfRazorRuntimeCompilation  //赋能 RazorPage 运行时编译
	{
		#region IAreaRegister 接口

		public string HomeUrl => "/Admin/Student/Index";

		public List<AreaPageMenuItem> AareaPageMenuItems => new List<AreaPageMenuItem>() {
			 new AreaPageMenuItem(GetAreaHomeUrl(),"首页","fa fa-laptop"),
			 		};

		public IMvcBuilder AuthorizeConfig(IMvcBuilder builder, IHostEnvironment env)
		{
			builder.AddRazorPagesOptions(options =>
			{
				//此处可配置页面权限
			});

			SenparcTrace.SendCustomLog("Student 启动", "完成 Area:Senparc.Xncf.Student 注册");

			return builder;
		}

		#endregion

		#region IXncfRazorRuntimeCompilation 接口
		public string LibraryPath => Path.GetFullPath(Path.Combine(SiteConfig.WebRootPath, "..", "..", "Senparc.Xncf.Student"));
		#endregion
	}
}