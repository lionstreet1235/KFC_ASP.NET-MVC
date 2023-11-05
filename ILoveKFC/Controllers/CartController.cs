using ILoveKFC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ILoveKFC.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        QL_KFCEntities db = new QL_KFCEntities();
        public string taoID()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString() + "";
        }
        public ActionResult Cart()
        {

            if (Session["cart"] == null)
            {
                Response.Write("<script>alert('Giỏ Hàng Của bạn hiện tại chưa có sản phẩm nào !!!')</script>");
                return RedirectToAction("ShowProductByCategory", "Product");
            }
            else
            {
                return View();
            }
            //else
            //{
            //    List<CART> list = GetCart();
            //    ViewBag.TongSL = TongSoLuong();
            //    ViewBag.TongThanhTien = TongThanhTien();
            //    CUSTOMER kh = Session["User"] as CUSTOMER;
            //    ViewBag.makh = kh.ID_CUSTOMER;
            //    return View(list);
            //}


        }
        //public ActionResult CartPartial()
        //{
        //    ViewBag.TongSL = TongSoLuong();
        //    ViewBag.tongThanhTien = (int)TongThanhTien();
        //    return View();
        //}
        //public List<Cart> GetCart()
        //{
        //    List<Cart> list = Session["cart"] as List<Cart>;
        //    if (list == null)
        //    {
        //        list = new List<Cart>();
        //        Session["cart"] = list;
        //    }
        //    return list;
        //}

        //public ActionResult AddCart(string masp, int sl, string url)
        //{
        //    List<Cart> list = GetCart();
        //    CUSTOMER kh = Session["User"] as CUSTOMER;
        //    if (kh == null)
        //    {
        //        Response.Write("<script>alert('Vui lòng đăng nhập để mua sản phẩm !!!')</script>");
        //        Session["vertifyID"] = null;
        //        return Redirect(url);

        //    }
        //    Cart hang = list.Find(t => t.sMasp == masp);
        //    if (hang == null)
        //    {
        //        hang = new Cart(masp, sl);
        //        list.Add(hang);
        //        CART giohang = new CART();
        //        giohang.ID_PRODUCT = hang.sMasp;
        //        giohang.ID_CUSTOMER = kh.ID_CUSTOMER;
        //        giohang.NAME_PRODUCT = hang.sTensp;
        //        giohang.IMAGE_PRODUCT = hang.sAnh;
        //        giohang.COST = hang.dGia;
        //        giohang.DESCRIBE = hang.sDescribe;
        //        giohang.QUANTITY_PRODUCT = hang.iSoLuong;
        //        giohang.TOTAL = hang.dThanhTien;
        //        db.CARTs.InsertOnSubmit(giohang);
        //        db.SubmitChanges();
        //        return Redirect(url);
        //    }
        //    else
        //    {

        //        hang.iSoLuong++;
        //        CART giohang = db.CARTs.SingleOrDefault(t => t.ID_PRODUCT == masp && t.ID_CUSTOMER == kh.ID_CUSTOMER);
        //        giohang.QUANTITY_PRODUCT = hang.iSoLuong;
        //        giohang.TOTAL = hang.dThanhTien;
        //        db.SubmitChanges();
        //        return Redirect(url);
        //    }
        //}

        //private int TongSoLuong()
        //{
        //    int tong = 0;
        //    List<Cart> list = Session["cart"] as List<Cart>;
        //    if (list != null)
        //    {
        //        tong += list.Sum(t => t.iSoLuong);
        //    }
        //    return tong;
        //}

        //private double TongThanhTien()
        //{
        //    double tong = 0;
        //    List<Cart> list = Session["cart"] as List<Cart>;
        //    if (list != null)
        //    {
        //        tong += list.Sum(t => t.dThanhTien);
        //    }
        //    return tong;
        //}

        //public ActionResult UpdateCart(string masp, string type)
        //{
        //    CUSTOMER kh = Session["User"] as CUSTOMER;

        //    List<Cart> list = GetCart();
        //    Cart hang = list.Single(t => t.sMasp == masp);

        //    if (hang != null)
        //    {
        //        if (type == "plus")
        //        {
        //            hang.iSoLuong += 1;
        //        }
        //        else
        //        {
        //            if (hang.iSoLuong <= 1)
        //            {
        //                return RedirectToAction("Cart", "Cart");
        //            }
        //            else
        //            {
        //                hang.iSoLuong -= 1;
        //            }
        //        }
        //        CART giohang = db.CARTs.SingleOrDefault(t => t.ID_PRODUCT == masp && t.ID_CUSTOMER == kh.ID_CUSTOMER);
        //        giohang.QUANTITY_PRODUCT = hang.iSoLuong;
        //        giohang.TOTAL = hang.dThanhTien;
        //        db.SubmitChanges();
        //    }
        //    return RedirectToAction("Cart", "Cart");
        //}

        //public ActionResult DeleteCartByID(string masp)
        //{
        //    List<Cart> list = GetCart();
        //    CUSTOMER kh = Session["User"] as CUSTOMER;

        //    Cart hang = list.Single(t => t.sMasp == masp);
        //    if (hang != null)
        //    {
        //        list.RemoveAll(t => t.sMasp == masp);
        //        CART giohang = db.CARTs.SingleOrDefault(t => t.ID_PRODUCT == masp && t.ID_CUSTOMER == kh.ID_CUSTOMER);
        //        db.CARTs.DeleteOnSubmit(giohang);
        //        db.SubmitChanges();
        //        return RedirectToAction("Cart", "Cart");
        //    }
        //    return RedirectToAction("Cart", "Cart");
        //}

        //public ActionResult XoaHetGioHang()
        //{
        //    List<Cart> list = GetCart();
        //    list.Clear();
        //    return RedirectToAction("Cart", "Cart");
        //}
        //public ActionResult Pay(string makh)
        //{
        //    List<CART> listCart = db.CARTs.Where(t => t.ID_CUSTOMER == makh).ToList();
        //    if (listCart == null)
        //    {
        //        return RedirectToAction("Cart", "Cart");

        //    }
        //    else
        //    {
        //        //tao hoa don
        //        string mahd = taoID();
        //        RECEIPT hoadon = new RECEIPT();
        //        hoadon.ID_RECEIPT = mahd;
        //        hoadon.ID_CUSTOMER = makh;
        //        hoadon.ORDER_DATE = DateTime.Now;
        //        hoadon.DELIVERY_DATE = null;
        //        hoadon.STATUS_RECEIPT = null;
        //        hoadon.CODE_DISCOUNT = null;
        //        hoadon.TOTAL = listCart.Sum(t => t.TOTAL);
        //        db.RECEIPTs.InsertOnSubmit(hoadon);
        //        db.SubmitChanges();

        //        //them chi tiet
        //        //copy gio hang vào ct hoa don
        //        foreach (CART item in listCart)
        //        {
        //            DETAIL_RECEIPT ct = new DETAIL_RECEIPT();
        //            ct.ID = taoID();
        //            ct.ID_RECEIPT = mahd;
        //            ct.ID_PRODUCT = item.ID_PRODUCT;
        //            ct.QUANTITY = item.QUANTITY_PRODUCT;
        //            ct.COST = item.COST;
        //            // insert vao database

        //            db.DETAIL_RECEIPTs.InsertOnSubmit(ct);
        //            db.SubmitChanges();

        //        }


        //        // thanh toan thanh cong
        //        List<Cart> list = GetCart();
        //        CUSTOMER kh = Session["User"] as CUSTOMER;
        //        List<CART> dsgiohang = db.CARTs.Where(t => t.ID_CUSTOMER == kh.ID_CUSTOMER).ToList();
        //        db.CARTs.DeleteAllOnSubmit(dsgiohang);
        //        db.SubmitChanges();
        //        list.Clear();

        //    }
        //    Response.Write("<script>alert('Tài Khoản hoặc mật khẩu không đúng !!!')</script>");
        //    return RedirectToAction("Cart", "Cart");
        //}
    }
}