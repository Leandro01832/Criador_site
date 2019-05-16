using DataContextCriacaoSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Classes
{
    public class ComboHelpers : IDisposable
    {
        private static BD db = new BD();


        //public static List<Departaments> getDepartaments()
        //{
        //    var dep = db.Departaments.ToList();
        //    dep.Add(new Departaments
        //    {
        //        Departamentsid = 0,
        //        Name = "[ Selecione um departamento ]"
        //    });
        //    dep = dep.OrderBy(d => d.Name).ToList();           

        //    return dep;
        //}

        //public static List<City> getCities()
        //{
        //    var cit = db.Cities.ToList();
        //    cit.Add(new City
        //    {
        //        Cityid = 0,
        //        Name = "[Selecione  uma Cidade..]"
        //    });
        //    cit = cit.OrderBy(d => d.Name).ToList();

        //    return cit;
        //}

        //public static List<Company> getCompanies()
        //{
        //    var com = db.Companies.ToList();
        //    com.Add(new Company
        //    {
        //        Companyid = 0,
        //        Name = "[Selecione  uma Cidade..]"
        //    });
        //    com = com.OrderBy(d => d.Name).ToList();

        //    return com;
        //}

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}