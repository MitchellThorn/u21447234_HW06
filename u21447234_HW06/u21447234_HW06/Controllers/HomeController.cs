using u21447234_HW06.ViewModels;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;

namespace u21447234_HW06.Controllers
{
    public class HomeController : Controller
    {
        private SqlConnection connection = new SqlConnection(@"Data Source = BICENTENNIAL-MA\SQLEXPRESS; Initial Catalog = BikeStores; Integrated Security = True");

        public ActionResult Products()
        {
            List<ProductsVM> productsList = new List<ProductsVM>();

            try
            {
                connection.Open();
                SqlCommand myCommand = new SqlCommand("Select product_name, model_year, list_price, category_name, brand_name from production.products as p inner join production.categories as c on p.category_id = c.category_id inner join production.brands as b on p.brand_id = b.brand_id", connection);
                SqlDataReader reader = myCommand.ExecuteReader();

                while (reader.Read())
                {
                    ProductsVM product = new ProductsVM();

                    product.Name = reader.GetString(0);
                    product.Year = reader.GetInt32(1);
                    product.Price = reader.GetDecimal(2);
                    product.Brand = reader.GetString(3);
                    product.Category = reader.GetString(4);

                    productsList.Add(product);
                }
            }
            catch (Exception err)
            {
                ViewBag.Status = 0;
            }
            finally
            {
                connection.Close();
            }

            return View(productsList);
        }

        public ActionResult Orders()
        {
            dynamic myModel = new ExpandoObject();
            myModel.info = getOrderInfo();
            myModel.order = getOrders();

            return View(myModel);
        }

        [HttpPost]
        public ActionResult Orders(DateTime searchString)
        {
            dynamic myModel = new ExpandoObject();

            if (searchString != null)
            {
                List<OrderVM> Order = getOrders().Where(x => x.OrderDate.Equals(searchString)).ToList();
                List<OrderInfoVM> OrderInfo = getOrderInfo().Where(x => x.OrderDate.Equals(searchString)).ToList();
                myModel.info = OrderInfo;
                myModel.order = Order;
            }
            else
            {
                myModel.info = getOrderInfo();
                myModel.order = getOrders();
            }

            return View(myModel);
        }

        public List<OrderInfoVM> getOrderInfo()
        {
            List<OrderInfoVM> OrderInfoList = new List<OrderInfoVM>();

            try
            {
                connection.Open();
                SqlCommand myCommand = new SqlCommand("select order_id, order_date from sales.orders", connection);
                SqlDataReader reader = myCommand.ExecuteReader();

                while (reader.Read())
                {
                    OrderInfoVM OrderInfo = new OrderInfoVM();

                    OrderInfo.OrderID = reader.GetInt32(0);
                    OrderInfo.OrderDate = reader.GetDateTime(1);

                    OrderInfoList.Add(OrderInfo);
                }
            }
            catch (Exception err)
            {
                ViewBag.Status = 0;
            }
            finally
            {
                connection.Close();
            }

            return OrderInfoList;
        }

        public List<OrderVM> getOrders()
        {
            List<OrderVM> orderList = new List<OrderVM>();

            try
            {
                connection.Open();
                SqlCommand myCommand = new SqlCommand("Select product_name, i.list_price, quantity, i.order_id, order_date from sales.order_items as i inner join production.products as p on i.product_id = p.product_id inner join sales.orders as o on i.order_id = o.order_id ", connection);
                SqlDataReader reader = myCommand.ExecuteReader();

                while (reader.Read())
                {
                    OrderVM order = new OrderVM();

                    order.ProductName = reader.GetString(0);
                    order.ListPrice = reader.GetDecimal(1);
                    order.Quantity = reader.GetInt32(2);
                    order.OrderID = reader.GetInt32(3);
                    order.OrderDate = reader.GetDateTime(4);

                    orderList.Add(order);
                }
            }
            catch (Exception err)
            {
                ViewBag.Status = 0;
            }
            finally
            {
                connection.Close();
            }

            return orderList;
        }

        public ActionResult Report()
        {
            return View();
        }
    }
}