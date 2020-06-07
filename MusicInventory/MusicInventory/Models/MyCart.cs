using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicInventory.Models
{
    public class CartItem
    {
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int LineTotal
        {
            get
            {
                return Quantity * Price;
            }
            set
            {

            }
        }

    }
    public class MyCart
    {
        public List<CartItem> CartItems { get; set; }

        public MyCart()
        {
            CartItems = new List<CartItem>();
        }
        public double Total
        {
            get
            {
                double total = 0;
                foreach (var item in CartItems)
                {
                    total += item.LineTotal;
                }
                return total;
            }
            set
            {

            }
        }

    }
}