using ILoveKFC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace ILoveKFC.Controllers
{
    public class AdminController : Controller
    {
        QL_KFCEntities db = new QL_KFCEntities();
        // GET: Admin
        [HttpGet]
        public ActionResult DangNhap_Admin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap_Admin(FormCollection f)
        {
            var user_name = f["txtUsername"];
            var password = f["txtPassword"];
            if (!string.IsNullOrEmpty(user_name) && !string.IsNullOrEmpty(password))
            {
                ACCOUNT_EMPLOYEE user = db.ACCOUNT_EMPLOYEE.SingleOrDefault(t => t.USERNAME_EMPLOYEE == user_name && t.PASSWORD_EMPLOYEE == password);
                if (user != null)
                {
                    EMPLOYEE nv = db.EMPLOYEEs.SingleOrDefault(t => t.ID_EMPLOYEE == user.ID_EMPLOYEE);
                    Session["Admin"] = nv;
                    return RedirectToAction("ShowCategory", "Admin");
                }
                else
                {
                    Response.Write("<script>alert('Tài Khoản hoặc mật khẩu không đúng !!!')</script>");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult ShowCategory()
        {
            var list = db.CATEGORies.OrderBy(t => t.ID_CATEGORY).ToList();
            return View(list);
        }

        [HttpPost]
        public ActionResult ShowCategory(FormCollection f)
        {
            var id = f["txtID"];
            var name = f["txtName"];
            var pos = f["txtPosition"];
            var list = db.CATEGORies.OrderBy(t => t.ID_CATEGORY).ToList();
            if (id.Length > 0 && name.Length > 0 && pos.Length > 0)
            {
                int count = db.CATEGORies.Where(t => t.ID_CATEGORY == id).Count();
                if (count > 0)
                {
                    ViewBag.Report = "Danh Sách Tất Cả sản phẩm";
                    return View(list);
                }
                else
                {

                    CATEGORY danhmuc = new CATEGORY();
                    danhmuc.ID_CATEGORY = id;
                    danhmuc.NAME_CATEGORY = name;
                    danhmuc.POSITION = pos;
                    db.CATEGORies.Add(danhmuc);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ShowCategory", "Admin");
        }

        public ActionResult DeleteCate(string madm)
        {
            CATEGORY del = db.CATEGORies.SingleOrDefault(t => t.ID_CATEGORY == madm);
            if (del != null)
            {
                db.CATEGORies.Remove(del);
                db.SaveChanges();

            }
            return RedirectToAction("ShowCategory", "Admin");

        }

        //san phẩm
        [HttpGet]
        public ActionResult ShowProduct(string madm)
        {
            List<PRODUCT> list_pro = db.PRODUCTs.Where(t => t.ID_CATEGORY == madm).ToList();
            if (list_pro.Count > 0)
            {
                CATEGORY dm = db.CATEGORies.SingleOrDefault(t => t.ID_CATEGORY == madm);
                ViewBag.title = dm.NAME_CATEGORY;
                ViewBag.madm = madm;
                return View(list_pro);
            }
            List<PRODUCT> list = db.PRODUCTs.Where(t => t.ID_PRODUCT != null).ToList();
            return View(list);
        }

        [HttpPost]
        public ActionResult ShowProduct(FormCollection f, string madm)
        {
            var masp = f["txtID"];
            var tensp = f["txtName"];
            var gia = f["txtCost"];
            var mota = f["txtDescribe"];
            var anh = f["txtImage"];
            if (masp != null && tensp != null && gia != null && mota != null && anh != null)
            {
                PRODUCT sp = new PRODUCT();
                sp.ID_CATEGORY = madm;
                sp.ID_PRODUCT = masp;
                sp.NAME_PRODUCT = tensp;
                sp.COST = Convert.ToInt32(gia);
                sp.DESCRIBE = mota;
                sp.IMAGE_PRODUCT = anh;
                db.PRODUCTs.Add(sp);
                db.SaveChanges();
                Response.Write("<script>alert('Thêm Sản phẩm thành công !!!')</script>");
            }
            List<PRODUCT> list_pro = db.PRODUCTs.Where(t => t.ID_CATEGORY == madm).ToList();
            CATEGORY dm = db.CATEGORies.SingleOrDefault(t => t.ID_CATEGORY == madm);
            ViewBag.title = dm.NAME_CATEGORY;
            ViewBag.madm = madm;
            return View(list_pro);
        }

        public ActionResult DeTailProduct(string masp)
        {
            //PRODUCT sp = db.PRODUCTs.SingleOrDefault(t => t.ID_PRODUCT == masp);
            //if (sp!=null)
            //{
            //    return View(sp);

            //}
            return View();
        }

        public ActionResult DeleteProduct(string masp)
        {
            PRODUCT del = db.PRODUCTs.SingleOrDefault(t => t.ID_PRODUCT == masp);
            var madm = del.ID_CATEGORY;
            if (del != null)
            {
                db.PRODUCTs.Remove(del);
                db.SaveChanges();
            }
            return RedirectToAction("ShowProduct", "Admin", new { @madm = madm });
        }
        //hoa don
       
        [HttpGet]
        public ActionResult ShowReceipt()
        {
            List<RECEIPT> list = db.RECEIPTs.Where(t => t.ID_RECEIPT != null).ToList();
            return View(list);
        }

        [HttpPost]//overload nó bởi phương thức post
        public ActionResult ShowReceipt(FormCollection f, string mahd)
        {
            var date = f["txtdate"];
            var state = f["txtState"];

            if (date != null && state != null)
            {
                RECEIPT hoadon = db.RECEIPTs.SingleOrDefault(t => t.ID_RECEIPT == mahd);//lấy ra đối tượng cần sửa
                hoadon.DELIVERY_DATE = Convert.ToDateTime(date);//gán lại cho những thuộc tính cần sửa
                hoadon.STATUS_RECEIPT = state;
                db.SaveChanges(); ;//lưu

            }
            List<RECEIPT> list = db.RECEIPTs.Where(t => t.ID_RECEIPT != null).ToList();

            return View(list);
        }

        public ActionResult DetailReceipt(string mahd, string makh)
        {
            //lay thong tin kh
            CUSTOMER kh = db.CUSTOMERs.SingleOrDefault(t => t.ID_CUSTOMER == makh);
            //lay chi tiet hoa don
            List<DETAIL_RECEIPT> list = db.DETAIL_RECEIPT.Where(t => t.ID_RECEIPT == mahd).ToList();
            ViewBag.Tenkh = kh.NAME_CUSTOMER;
            ViewBag.Mahd = mahd;
            return View(list);
        }

        [HttpGet]
        public ActionResult ShowEmployee()
        {
            List<EMPLOYEE> list = db.EMPLOYEEs.Where(t => t.ID_EMPLOYEE != null).ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult ShowEmployee(FormCollection f)
        {
            var manv = f["txtID"];
            var tennv = f["txtName"];
            var ngaysinh = f["txtDate"];
            var diachi = f["txtAddress"];
            var sdt = f["txtPhone"];
            var mail = f["txtGmail"];
            var gioitinh = f["txtSex"];

            if (manv != null && tennv != null && ngaysinh != null && diachi != null && sdt != null && mail != null && gioitinh != null)
            {

                if (db.EMPLOYEEs.Where(t => t.ID_EMPLOYEE == manv).Count() > 0)
                {
                    ViewBag.thongbao = manv + "Đã Tồn Tại!!";
                }
                else
                {
                    EMPLOYEE nv = new EMPLOYEE();
                    nv.ID_EMPLOYEE = manv;
                    nv.NAME_EMPLOYEE = tennv;
                    nv.DATEOFBIRTH_EMPLOYEE = Convert.ToDateTime(ngaysinh);
                    nv.ADDRESS_EMPLOYEE = diachi;
                    nv.PHONE_EMPLOYEE = sdt;
                    nv.GMAIL = mail;
                    nv.SEX_EMPLOYEE = gioitinh;
                    db.EMPLOYEEs.Add(nv);
                    db.SaveChanges();

                }
            }
            List<EMPLOYEE> list = db.EMPLOYEEs.Where(t => t.ID_EMPLOYEE != null).ToList();
            return View(list);
        }

        public ActionResult DeleteEmployee(string manv)
        {
            EMPLOYEE del = db.EMPLOYEEs.SingleOrDefault(t => t.ID_EMPLOYEE == manv);//lấy ra đối tượng cần xóa
            if (del != null)
            {
                db.EMPLOYEEs.Remove(del);//xóa
                db.SaveChanges();//lưu thay đổi
            }
            return RedirectToAction("ShowEmployee", "Admin");
        }



        //qua li khach hang
        public ActionResult ShowCustomer()
        {
            List<CUSTOMER> list = db.CUSTOMERs.Where(t => t.ID_CUSTOMER != null).ToList();

            return View(list);
        }

        public ActionResult ShowAccountCustomer(string makh)
        {
            CUSTOMER kh = db.CUSTOMERs.SingleOrDefault(t => t.ID_CUSTOMER == makh);
            ViewBag.Tenkh = kh.NAME_CUSTOMER;

            ACCOUNT_CUSTOMER list = db.ACCOUNT_CUSTOMER.SingleOrDefault(t => t.ID_CUSTOMER == makh);
            return View(list);
        }


        //
        public string taoID()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString() + "";
        }
        [HttpGet]
        public ActionResult ShowCodeDiscount()
        {
            List<DISCOUNT> list = db.DISCOUNTs.OrderBy(t => t.END_DAY).ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult ShowCodeDiscount(FormCollection f)
        {
            var percent = f["txtPercent"];
            var money = f["txtDiscount"];
            var start = f["txtstart"];
            var end = f["txtend"];
            var quantity = f["txtQuantity"];
            if (percent != null && money != null && start != null && end != null && quantity != null)
            {
                DISCOUNT giamgia = new DISCOUNT();
                giamgia.CODE_DISCOUNT = taoID();
                giamgia.PERCENT_DISCOUNT = Convert.ToInt32(percent);
                giamgia.MONEY_DISCOUNT = Convert.ToInt32(money);
                giamgia.START_DAY = Convert.ToDateTime(start);
                giamgia.END_DAY = Convert.ToDateTime(end);
                giamgia.QUANTITY = Convert.ToInt32(quantity);
                db.DISCOUNTs.Add(giamgia);
                db.SaveChanges();
            }


            List<DISCOUNT> list = db.DISCOUNTs.OrderBy(t => t.END_DAY).ToList();
            return View(list);
        }
        public ActionResult OrderStatistics()
        {
            // Lấy số lượng đơn hàng từ cơ sở dữ liệu
            var totalOrders = db.RECEIPTs.Count();

            // Truyền số lượng đơn hàng sang view để hiển thị
            ViewBag.TotalOrders = totalOrders;

            // Trả về view để hiển thị thông tin thống kê
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["Admin"] = null;
            return RedirectToAction("DangNhap_Admin", "Admin");
        }







    }
}