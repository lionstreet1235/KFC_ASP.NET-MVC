using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILoveKFC.Models
{
    public class CartItem
    {
        public PRODUCT _product {  get; set; }
        public int _quantity { get; set; }
    }
    public class Cart1
    {
        List<CartItem> _items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return _items; }
        }
        public void Add_Product_Cart(PRODUCT _pro, int _quan=1)
        {
            var item= Items.FirstOrDefault(s=>s._product.ID_PRODUCT==_pro.ID_PRODUCT);
            if(item==null)
            {
                _items.Add(new CartItem
                {
                    _product = _pro,
                    _quantity = _quan,
                });
            }
            else
            {
                item._quantity += _quan;
            }
        }
        public int Total_quantity()
        {
            return _items.Sum(s=>s._quantity );
        }
        public decimal Total_money()
        {
            var total = Items.Sum(s=>s._quantity+s._product.COST);
            return (decimal)total;
        }
        public void Update_quantity(string id, int _new_quan)
        {
             var item = _items.Find(s => s._product.ID_PRODUCT == id);
            if(item!=null)
            {
                item._quantity = _new_quan;
            }
        }
        public void Remove_CartItem(string id)
        {
            _items.RemoveAll(s => s._product.ID_PRODUCT == id);
        }
        public void ClearCart()
        {
            _items.Clear();
        }
    }
}