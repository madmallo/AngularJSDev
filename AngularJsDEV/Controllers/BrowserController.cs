using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data.Linq;
using System.Reflection;

namespace AngularJsDEV.Controllers
{
   public class BrowserController : Controller
   {
      // GET: Browser
      public ActionResult Index()
      {
         return View();
      }

      public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
      {
         // ExpandoObject supports IDictionary so we can extend it like this
         var expandoDict = expando as IDictionary<string, object>;
         if (expandoDict.ContainsKey(propertyName))
            expandoDict[propertyName] = propertyValue;
         else
            expandoDict.Add(propertyName, propertyValue);
      }

      public class Th
      {
         public string nome { get; set; }
         public string caption { get; set; }
         public string tipo { get; set; }
         public string width { get; set; }
         public string align { get; set; }
         public string format { get; set; }
      }

      public class Tr
      {
         public /*List<object>*/ object Td { get; set; }
         /*public Tr()
         {
            Td = new List<object>();
         }*/
      }

      public class JsonRS
      {
         public List<Th> head { get; set; }
         public List</*Tr*/object> body { get; set; }
         public JsonRS()
         {
            head = new List<Th>();
            /*body = new List<Tr>();*/
            body = new List<object>();
         }
      }

      public JsonResult GetData()
      {
         return GetDbData();

         /* creo l'oggetto radice */
         JsonRS json_ret = new JsonRS();

         /* aggiungo i vari headers */
         Th Header = new Th();
         Header.nome = "Colonna1";
         Header.caption = "Col 1";
         Header.tipo = "string";
         Header.width = "250px";
         Header.align = "left";
         Header.format = "";
         json_ret.head.Add(Header);
         Header = new Th();
         Header.nome = "Colonna2";
         Header.caption = "Col 2";
         Header.tipo = "string";
         Header.width = "";
         Header.align = "left";
         Header.format = "";
         json_ret.head.Add(Header);
         Header = new Th();
         Header.nome = "Colonna3";
         Header.caption = "Col 3";
         Header.tipo = "string";
         Header.width = "";
         Header.align = "left";
         Header.format = "";
         json_ret.head.Add(Header);
         Header = new Th();
         Header.nome = "Colonna4";
         Header.caption = "Col 4";
         Header.tipo = "number";
         Header.width = "100px";
         Header.align = "right";
         Header.format = "";
         json_ret.head.Add(Header);
         Header = new Th();
         Header.nome = "Colonna5";
         Header.caption = "Col 5";
         Header.tipo = "datetime";
         Header.width = "100px";
         Header.align = "center";
         Header.format = "";
         json_ret.head.Add(Header);

         /* aggiungo le varie righe */
         Tr Riga = new Tr();
         dynamic expObj = new ExpandoObject();
         AddProperty(expObj, "Colonna1", "uallabalooza");
         AddProperty(expObj, "Colonna2", "in the");
         AddProperty(expObj, "Colonna3", "sky");
         AddProperty(expObj, "Colonna4", 245.1);
         AddProperty(expObj, "Colonna5", "01/01/9999");
         //json_ret.body.Add(new { Colonna1 = "uallabalooza", Colonna2 = "in the", Colonna3 = "sky", Colonna4 = 245.1, Colonna5 = "01/01/9999" });
         json_ret.body.Add(expObj);
         expObj = new ExpandoObject();
         AddProperty(expObj, "Colonna1", "rasti");
         AddProperty(expObj, "Colonna2", "blasti");
         AddProperty(expObj, "Colonna3", "lasagne");
         AddProperty(expObj, "Colonna4", -14000);
         AddProperty(expObj, "Colonna5", "01/01/9999");
         //json_ret.body.Add(new { Colonna1 = "rasti", Colonna2 = "blasti", Colonna3 = "lasagne", Colonna4 = -14000, Colonna5 = "01/01/9999" });
         json_ret.body.Add(expObj);
         expObj = new ExpandoObject();
         AddProperty(expObj, "Colonna1", "similili");
         AddProperty(expObj, "Colonna2", "la formica");
         AddProperty(expObj, "Colonna3", "brombagliumbal");
         AddProperty(expObj, "Colonna4", 15);
         AddProperty(expObj, "Colonna5", "01/01/9999");
         //json_ret.body.Add(new { Colonna1 = "similili", Colonna2 = "la formica", Colonna3 = "brombagliumbal", Colonna4 = 245.1, Colonna5 = "01/01/9999" });
         json_ret.body.Add(expObj);

         /* restituisco l'output come JSON serializzato */
         JsonConvert.SerializeObject(json_ret);
         return Json(json_ret);
      }

      public JsonResult GetDbData()
      {
         /* Linq to SQL: effettuo la query */
         /*DataContext Db = new DataContext("Data Source=idea-dev\\IDEA;Initial Catalog=ZZZDEV001;Integrated Security=False;User Id=sa;Password=master2k5*;");
        Table<WWWStatistiche> stat = Db.GetTable<WWWStatistiche>();
        IQueryable<object> query = from c in stat
                                   where 1 == 1
                                   select c;
       foreach(object record in query)
        {
           record.
        }
        Db.Dispose();*/
         AngularDEVContainer context = new AngularDEVContainer();
         List<WWWStatistiche> stat = (from c in context.WWWStatistiche
                                            where 1 == 1
                                            select c).ToList<WWWStatistiche>();


         /* creo gli oggetti JSON */
         JsonRS json_ret = new JsonRS();
         Th Header = new Th();
         Tr Riga = new Tr();
         PropertyInfo[] fields;
         dynamic expObj = new ExpandoObject();

         for (var i=0; i < stat.Count; i++)
         {
            fields = stat[i].GetType().GetProperties();
            if (i == 0) /* definizione intestazioni */
            {
               for (var j = 0; j < fields.Length; j++)
               {
                  Header = new Th();
                  Header.nome = fields[j].Name;
                  Header.caption = fields[j].Name;
                  Header.tipo = fields[j].GetType().ToString();
                  Header.width = "";
                  switch(fields[j].GetType().ToString().ToLower())
                  {
                     case "string": Header.align = "left"; break;
                     default: Header.align = "left"; break;
                  }
                  Header.format = "";
                  json_ret.head.Add(Header);
               }
            }

            /* aggiungo le varie righe */
            Riga = new Tr();
            expObj = new ExpandoObject();
            for (var j = 0; j < fields.Length; j++)
            {
               AddProperty(expObj, fields[j].Name, stat[i].GetType().GetProperty(fields[j].Name).GetValue(stat[i]).ToString());
            }
            json_ret.body.Add(expObj);
         }

         /* restituisco l'output come JSON serializzato */
         JsonConvert.SerializeObject(json_ret);
         return Json(json_ret);

         /* creo l'oggetto radice */
         JsonRS json_retz = new JsonRS();

         /* aggiungo i vari headers */
         Th Headerz = new Th();
         Header.nome = "Colonna1";
         Header.caption = "Col 1";
         Header.tipo = "string";
         Header.width = "250px";
         Header.align = "left";
         Header.format = "";
         json_ret.head.Add(Header);
         Header = new Th();
         Header.nome = "Colonna2";
         Header.caption = "Col 2";
         Header.tipo = "string";
         Header.width = "";
         Header.align = "left";
         Header.format = "";
         json_ret.head.Add(Header);
         Header = new Th();
         Header.nome = "Colonna3";
         Header.caption = "Col 3";
         Header.tipo = "string";
         Header.width = "";
         Header.align = "left";
         Header.format = "";
         json_ret.head.Add(Header);
         Header = new Th();
         Header.nome = "Colonna4";
         Header.caption = "Col 4";
         Header.tipo = "number";
         Header.width = "100px";
         Header.align = "right";
         Header.format = "";
         json_ret.head.Add(Header);
         Header = new Th();
         Header.nome = "Colonna5";
         Header.caption = "Col 5";
         Header.tipo = "datetime";
         Header.width = "100px";
         Header.align = "center";
         Header.format = "";
         json_ret.head.Add(Header);

         /* aggiungo le varie righe */
         Tr Rigaz = new Tr();
         dynamic expObjz = new ExpandoObject();
         AddProperty(expObj, "Colonna1", "uallabalooza");
         AddProperty(expObj, "Colonna2", "in the");
         AddProperty(expObj, "Colonna3", "sky");
         AddProperty(expObj, "Colonna4", 245.1);
         AddProperty(expObj, "Colonna5", "01/01/9999");
         //json_ret.body.Add(new { Colonna1 = "uallabalooza", Colonna2 = "in the", Colonna3 = "sky", Colonna4 = 245.1, Colonna5 = "01/01/9999" });
         json_ret.body.Add(expObj);
         expObj = new ExpandoObject();
         AddProperty(expObj, "Colonna1", "rasti");
         AddProperty(expObj, "Colonna2", "blasti");
         AddProperty(expObj, "Colonna3", "lasagne");
         AddProperty(expObj, "Colonna4", -14000);
         AddProperty(expObj, "Colonna5", "01/01/9999");
         //json_ret.body.Add(new { Colonna1 = "rasti", Colonna2 = "blasti", Colonna3 = "lasagne", Colonna4 = -14000, Colonna5 = "01/01/9999" });
         json_ret.body.Add(expObj);
         expObj = new ExpandoObject();
         AddProperty(expObj, "Colonna1", "similili");
         AddProperty(expObj, "Colonna2", "la formica");
         AddProperty(expObj, "Colonna3", "brombagliumbal");
         AddProperty(expObj, "Colonna4", 15);
         AddProperty(expObj, "Colonna5", "01/01/9999");
         //json_ret.body.Add(new { Colonna1 = "similili", Colonna2 = "la formica", Colonna3 = "brombagliumbal", Colonna4 = 245.1, Colonna5 = "01/01/9999" });
         json_ret.body.Add(expObj);

         /* restituisco l'output come JSON serializzato */
         JsonConvert.SerializeObject(json_ret);
         return Json(json_ret);
      }
   }
}