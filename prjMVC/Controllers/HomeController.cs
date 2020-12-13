using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using prjMVC.Models;

namespace prjMVC.Controllers
{
    public class HomeController : Controller
    {
        dbProductEntities db = new dbProductEntities();
        // GET: Home
        public ActionResult Index()
        {
            var products = db.tProduct.ToList();
            return View(products);
        }

        public ActionResult Delete(string fId)
        {
            //依網址傳來的fId編號取得要刪除的產品記錄
            var product = db.tProduct.Where(m => m.fId == fId).FirstOrDefault();
            string fileName = product.fImg;  //取得要刪除產品的圖檔
            if (fileName != "")
            {
                //刪除指定圖檔
                System.IO.File.Delete(Server.MapPath("~/Images") + "/" + fileName);
            }
            db.tProduct.Remove(product);
            db.SaveChanges();  //依編號刪除產品記錄
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string fId, string fName, decimal fPrice, HttpPostedFileBase fImg)
        {
            try
            {
                //上傳圖檔
                string fileName = "";
                //檔案上傳
                if (fImg != null)
                {
                    if (fImg.ContentLength > 0)
                    {
                        //取得圖檔名稱
                        fileName =
                           System.IO.Path.GetFileName(fImg.FileName);
                        var path = System.IO.Path.Combine(Server.MapPath("~/Images"), fileName);
                        fImg.SaveAs(path); //檔案儲存到Images資料夾下
                    }
                }
                //新增記錄
                tProduct product = new tProduct();
                product.fId = fId;
                product.fName = fName;
                product.fPrice = fPrice;
                product.fImg = fileName;
                db.tProduct.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index"); //導向Index的Action方法
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }


        public ActionResult Edit(string fId)
        {
            var product = db.tProduct.Where(m => m.fId == fId).FirstOrDefault();
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(string fId, string fName, decimal fPrice, HttpPostedFileBase fImg, string oldImg)
        {
            string fileName = "";
            //檔案上傳
            if (fImg != null)
            {
                if (fImg.ContentLength > 0)
                {
                    //取得圖檔名稱
                    fileName = System.IO.Path.GetFileName(fImg.FileName);
                    var path = System.IO.Path.Combine(Server.MapPath("~/Images"), fileName);
                    fImg.SaveAs(path);
                }
            }
            else
            {
                fileName = oldImg; //若無上傳圖檔，則指定hidden隱藏欄位的資料
            }
            // 修改資料
            var product = db.tProduct.Where(m => m.fId == fId).FirstOrDefault();
            product.fName = fName;
            product.fPrice = fPrice;
            product.fImg = fileName;
            db.SaveChanges();
            return RedirectToAction("Index"); //導向Index的Action方法
        }
    }
}