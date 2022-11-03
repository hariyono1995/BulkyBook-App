using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalBulkyBook.Models.ViewModels
{
	public class ShoppingCartVM
	{
		public IEnumerable<ShoopingCart> ListCarts { get; set; }
		//public double CartTotal { get; set; }
		public OrderHeader OrderHeader { get; set; }
	}
}
