﻿using MyWebSite.Areas.Configuration.ViewModels;
using MyWebSite.Datas;
using MyWebSite.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyWebSite.Areas.Configuration.Models;

namespace MyWebSite.Services
{
    public class NavMenuService : INavMenuService
    {
        private readonly ApplicationDbContext _context;
        public static IList<NavMenu> NavMenus { get; set; }

        public NavMenuService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 生成导航菜单
        /// </summary>
        /// <returns></returns>
        public IList<NavMenu> GenerateNavMenus()
        {
            var navMenus = new List<NavMenu>();

            var rootMenus = _context.Menus
                .Where(s => string.IsNullOrEmpty(s.ParentId)).AsNoTracking().ToList();

            foreach (var rootMenu in rootMenus)
            {
                navMenus.Add(GetOneNavMenu(rootMenu));
            }

            return navMenus;
        }

        public NavMenu GetOneNavMenu(Menu menu)
        {
            var navMenu = new NavMenu
            {
                Id = menu.Id,
                Name = menu.Name,
                MenuType = menu.MenuType.Value,
                Url = menu.Url,
                Icon = menu.Icon
            };

            //查找子菜单
            var subMenus = _context.Menus.Where(s => s.ParentId == menu.Id).AsNoTracking().ToList();

            foreach (var subMenu in subMenus)
            {
                navMenu.SubNavMenus.Add(GetOneNavMenu(subMenu));
            }

            return navMenu;
        }
    }
}