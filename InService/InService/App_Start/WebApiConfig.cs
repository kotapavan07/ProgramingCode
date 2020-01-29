using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace InService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DepartmentsApiRoute",
                routeTemplate: "api/Locations/{locationId}/Departments/{departmentId}",
                defaults: new { Controller = "Departments", DepartmentId = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "CategoriesApiRoute",
                routeTemplate: "api/Locations/{locationId}/Departments/{departmentId}/Categories/{categoryId}",
                defaults: new { Controller = "Categories", categoryId = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "SubCategoriesApiRoute",
                routeTemplate: "api/Locations/{locationId}/Departments/{departmentId}/Categories/{categoryId}/SubCategories/{subCategoryId}",
                defaults: new { Controller = "SubCategories", subCategoryId = RouteParameter.Optional }
            );

            config.Filters.Add(new Util.InExceptionFilter());
            config.Services.Replace(typeof(IExceptionHandler), new Util.InExceptionHandler());
            config.Formatters.JsonFormatter.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
        }
    }
}
