using ILoveKFC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ILoveKFC.Controllers
{
    public class MainController : Controller
    {
        QL_KFCEntities db = new QL_KFCEntities();
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DMPartial_TOP()
        {
            List<CATEGORY> list;
            list = db.CATEGORies.Where(t => t.POSITION == "TOP").ToList();
            return PartialView(list);
        }

        public ActionResult DMPartial_BOT()
        {
            List<CATEGORY> list;
            list = db.CATEGORies.Where(t => t.POSITION == "BOT").ToList();
            return PartialView(list);
        }


        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            var user_name = f["txtUsername"];
            var password = f["txtPassword"];
            //if (string.IsNullOrEmpty(user_name))
            //    ViewData["errUsername"] = " Username is required !";
            //if (string.IsNullOrEmpty(password))
            //    ViewData["errPass"] = " Password is required !";
            if (!string.IsNullOrEmpty(user_name) && !string.IsNullOrEmpty(password))
            {
                ACCOUNT_CUSTOMER user = db.ACCOUNT_CUSTOMER.SingleOrDefault(t => t.USERNAME_CUSTOMER == user_name && t.PASSWORD_CUSTOMER == password);
                if (user != null)
                {
                    CUSTOMER kh = db.CUSTOMERs.SingleOrDefault(t => t.ID_CUSTOMER == user.ID_CUSTOMER);
                    Session["User"] = kh;
                    //kt gio hang
                    List<CART> list_giohang = db.CARTs.Where(t => t.ID_CUSTOMER == kh.ID_CUSTOMER).ToList();
                    if (list_giohang != null)
                    {
                        ////khoi tao gio hang tren session
                        //List<Cart> list = new List<Cart>();
                        ////do gio hang db -> giohang session
                        //foreach (CART item in list_giohang)
                        //{
                        //    Cart hang = new Cart(item);
                        //    list.Add(hang);
                        //}
                        ////
                        //Session["cart"] = list;
                    }
                    TempData["stateLogin"] = 1;
                    return RedirectToAction("ShowProductByCategory", "Product");
                }
                else
                {
                    TempData["stateLogin"] = -1;
                    TempData["messageError"] = "";
                    return RedirectToAction("ShowProductByCategory", "Product");
                }
            }
            else
            {
                TempData["stateLogin"] = 0;
                TempData["messageError"] = "Please enter your infomation !";
                return RedirectToAction("ShowProductByCategory", "Product");
            }
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        public string taoID()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString() + "";
        }
        [HttpPost]
        public ActionResult DangKy(CUSTOMER new_kh, FormCollection f)
        {
            var username = f["txtusername"];
            var password = f["txtpassword"];
            var repass = f["txtrepassword"];
            var name = f["txtname"];
            var phone = f["txtphone"];
            var email = f["txtEmail"];
            var city = f["city"];
            var district = f["district"];
            var ward = f["ward"];
            var street = f["txtStreet"];
            var sex = f["sex"];
            if (string.IsNullOrEmpty(name))
                ViewData["errName"] = " Name is required !";
            if (string.IsNullOrEmpty(username))
                ViewData["errUsername"] = " Username is required !";
            if (string.IsNullOrEmpty(password))
                ViewData["errPass"] = " Password is required !";
            if (string.IsNullOrEmpty(repass))
                ViewData["errRepass"] = " is required !";
            if (string.IsNullOrEmpty(phone))
                ViewData["errPhone"] = " Phone is required !";
            if (string.IsNullOrEmpty(email))
                ViewData["errEmail"] = " Email is required!";
            if (string.IsNullOrEmpty(sex))
                ViewData["errSex"] = " Giới Tính is required!";
            if (string.IsNullOrEmpty(city))
                ViewData["errCity"] = " Thành Phố is required!";
            if (string.IsNullOrEmpty(district))
                ViewData["errDistrict"] = " Quận is required!";
            if (string.IsNullOrEmpty(ward))
                ViewData["errward"] = " Phường is required!";
            if (string.IsNullOrEmpty(street))
                ViewData["errStreet"] = " Tên Đường is required!";

            if (password != repass)
            {
                ViewData["errRepass"] = " Retype Password is incorrect !";
            }

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(repass) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(sex) && !string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(district) && !string.IsNullOrEmpty(ward) && !string.IsNullOrEmpty(street))
            {
                int tk = db.ACCOUNT_CUSTOMER.Where(t => t.USERNAME_CUSTOMER == username).ToList().Count;
                if (tk == 0)
                {
                    //add table Account
                    string ID_Customer = taoID();
                    ACCOUNT_CUSTOMER new_tk = new ACCOUNT_CUSTOMER();
                    new_tk.USERNAME_CUSTOMER = username;
                    new_tk.PASSWORD_CUSTOMER = password;
                    new_tk.ID_CUSTOMER = ID_Customer;
                    //add table customer
                    new_kh.ID_CUSTOMER = ID_Customer;
                    new_kh.NAME_CUSTOMER = name;
                    new_kh.DATEOFBIRTH_CUSTOMER = null;
                    new_kh.ADDRESS_CITY = city;
                    new_kh.ADDRESS_DISTRICT = district;
                    new_kh.ADDRESS_WARD = ward;
                    new_kh.ADDRESS_CUSTOMER = street;
                    new_kh.PHONE_CUSTOMER = phone;
                    new_kh.GMAIL = email;
                    new_kh.SEX_CUSTOMER = null;
                    //db.ACCOUNT_CUSTOMER.InsertOnSubmit(new_tk); hỏi cô
                    //db.CUSTOMERs.InsertOnSubmit(new_kh);
                    //db.SubmitChanges();
                    return RedirectToAction("DangNhap", "Main");

                }
                else
                {
                    Response.Write("<script>alert('Tên Đăng Nhập Đã tồn tại!!!')</script>");

                }
            }
            return View();
        }


        public ActionResult DangXuat()
        {
            Session["User"] = null;
            return RedirectToAction("ShowProductByCategory", "Product");
        }
        [HttpGet]
        public ActionResult DoiMatKhau(string makh)
        {
            ACCOUNT_CUSTOMER kh = db.ACCOUNT_CUSTOMER.SingleOrDefault(t => t.ID_CUSTOMER == makh);

            return View(kh);
        }
        [HttpPost]
        public ActionResult DoiMatKhau(FormCollection f, string makh)
        {
            //ACCOUNT_CUSTOMER kh = db.ACCOUNT_CUSTOMERs.SingleOrDefault(t => t.ID_CUSTOMER == makh);
            var old_pass = f["txtpass"];
            var new_pass = f["txtRepass"];
            ACCOUNT_CUSTOMER tk_kh = db.ACCOUNT_CUSTOMER.SingleOrDefault(t => t.ID_CUSTOMER == makh);
            if (old_pass != null && new_pass != null)
            {
                if (old_pass != new_pass)
                {
                    tk_kh.PASSWORD_CUSTOMER = new_pass;
                    //db.;
                    ViewBag.thongbao = "Lưu Thành Công !";
                }
                else
                {
                    ViewBag.thongbao = "Mật khẩu mới không được trùng lại !";

                }
            }
            return View(tk_kh);
        }
    }
}