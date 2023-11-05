using ILoveKFC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ILoveKFC.Controllers
{
    public class ProductController : Controller
    {
        QL_KFCEntities kfc=new QL_KFCEntities();    
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ShowAllProduct()
        {
            //single lay ra 1 doi tuong
            // con where lay ra 1 tap hop doi tuong thoa dk
            var list = kfc.PRODUCTs.Where(t => t.ID_PRODUCT != null).ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult ShowAllProduct(FormCollection f)
        {
            var s = f["txtTim"];

            List<PRODUCT> list_new = kfc.PRODUCTs.Where(t => t.NAME_PRODUCT.Contains(s)).ToList();
            if (list_new.Count > 0)
            {

                ViewBag.tenDM = "Sản Phẩm Bạn Tìm ' " + s + " ' gồm";
            }
            else
            {
                ViewBag.tenDM = "Không tìm thấy sản phẩm này !";
            }

            return View(list_new);

        }
        public ActionResult ShowProductByCategory(string ID_CATEGORY)
        {
            if (ID_CATEGORY == null)
            {
                string tenDM = kfc.CATEGORies.SingleOrDefault(t => t.ID_CATEGORY == "DM001").NAME_CATEGORY.ToString();
                var list = kfc.PRODUCTs.Where(t => t.ID_CATEGORY == "DM001").ToList();
                ViewBag.tenDM = tenDM;
                //ViewData["ResponseLogin"] = TempData["ResponseLogin"].ToString();
                return View(list);
            }
            else
            {

                string tenDM = kfc.CATEGORies.SingleOrDefault(t => t.ID_CATEGORY == ID_CATEGORY).NAME_CATEGORY.ToString();
                var list = kfc.PRODUCTs.Where(t => t.ID_CATEGORY == ID_CATEGORY).ToList();

                if (list.Count <= 0)
                {
                    ViewBag.tenDM = "Không món ở danh mục này " + tenDM;
                }
                else
                    ViewBag.tenDM = tenDM;
                return View(list);
            }
        }
        public ActionResult ShowDetailProduct(string masp)
        {
            //single lay ra 1 doi tuong
            // con where lay ra 1 tap hop doi tuong thoa dk
            PRODUCT k = kfc.PRODUCTs.Single(t => t.ID_PRODUCT == masp);
            if (k == null)
            {
                return HttpNotFound();
            }
            ViewBag.tensp = k.NAME_PRODUCT;
            return View(k);
        }
    }
}