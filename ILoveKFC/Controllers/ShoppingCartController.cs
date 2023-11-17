using ILoveKFC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ILoveKFC.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        QL_KFCEntities database = new QL_KFCEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("EmptyCart");
            Cart1 _cart = Session["Cart"] as Cart1;
            return View(_cart);
        }
        public Cart1 GetCart()
        {
            Cart1 cart = Session["Cart"] as Cart1;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart1();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ActionResult AddToCart(string id)
        {
            var _pro = database.PRODUCTs.SingleOrDefault(s => s.ID_PRODUCT == id);    //lấy Pro theo ID
            if (_pro != null)
            {
                GetCart().Add_Product_Cart(_pro);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart1 cart = Session["Cart"] as Cart1;
            string id_pro = "default";
            int _quantity = int.Parse(form["cartQuantity"]);
            cart.Update_quantity(id_pro, _quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult RemoveCart(string id)
        {
            Cart1 cart = Session["Cart"] as Cart1;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart1 cart = Session["Cart"] as Cart1;
            if (cart != null)
            {
                total_quantity_item = cart.Total_quantity();
            }
            ViewBag.QuantityCart = total_quantity_item;
            return PartialView("BagCart");
        }
        public ActionResult Checkout()
        {
            // Tạo một hóa đơn mới
            RECEIPT newReceipt = new RECEIPT
            {
                ID_RECEIPT = Guid.NewGuid().ToString(), // Tạo một ID mới cho hóa đơn
                ORDER_DATE = DateTime.Now, // Ngày đặt hàng là ngày hiện tại
                
                // Các thông tin khác của hóa đơn có thể được cập nhật từ form hoặc thông tin khách hàng
                // Ví dụ: ID_CUSTOMER, DELIVERY_DATE, STATUS_RECEIPT, CODE_DISCOUNT, TOTAL, etc.
            };


            // Lấy giỏ hàng từ session hoặc từ cơ sở dữ liệu (tuỳ thuộc vào cách bạn lưu trữ giỏ hàng)
            Cart1 cart = Session["Cart"] as Cart1; // Giả sử bạn lưu giỏ hàng trong Session

            // Kiểm tra nếu giỏ hàng không rỗng
            if (cart != null && cart.Items.Any())
            {
                // Lặp qua từng sản phẩm trong giỏ hàng để thêm chi tiết hóa đơn
                foreach (var cartItem in cart.Items)
                {
                    // Tạo một chi tiết hóa đơn mới
                    DETAIL_RECEIPT detail = new DETAIL_RECEIPT
                    {
                        ID = Guid.NewGuid().ToString(), // Tạo một ID mới cho chi tiết hóa đơn
                        ID_RECEIPT = newReceipt.ID_RECEIPT, // Gán ID hóa đơn mới tạo
                        ID_PRODUCT = cartItem._product.ID_PRODUCT, // Lấy ID sản phẩm từ giỏ hàng
                        QUANTITY = cartItem._quantity, // Lấy số lượng từ giỏ hàng
                        COST = cartItem._product.COST // Lấy giá sản phẩm từ giỏ hàng
                        // Các thông tin khác của chi tiết hóa đơn có thể được cập nhật tùy thuộc vào nhu cầu
                    };

                    // Thêm chi tiết hóa đơn vào hóa đơn mới tạo
                    newReceipt.DETAIL_RECEIPT.Add(detail);
                }

                // Tính tổng tiền của hóa đơn từ giỏ hàng
                newReceipt.TOTAL = cart.Total_money();

                // Thêm hóa đơn mới vào cơ sở dữ liệu
                database.RECEIPTs.Add(newReceipt);
                database.SaveChanges();

                // Xóa giỏ hàng sau khi thanh toán
                cart.ClearCart();
                Session["Cart"] = cart; // Cập nhật giỏ hàng trong Session

                // Chuyển hướng đến trang cảm ơn hoặc trang thông báo thanh toán thành công
                return RedirectToAction("CheckOut_Success", "ShoppingCart");
            }
            else
            {
                // Giỏ hàng rỗng, có thể hiển thị thông báo lỗi
                return RedirectToAction("EmptyCart", "Cart");
            }
        }
    

        public ActionResult CheckOut_Success()
        {
            return View();
        }
       

    }
}