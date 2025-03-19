using System;
using System.Collections.Generic;

namespace Mondiguing_SolidPrinciples
{
    public class Order
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }

    public interface IInvoiceService
    {
        void GenerateInvoice(Order order);
    }

    public interface IPaymentService
    {
        void ProcessPayment(Order order);
    }

    public interface IShippingService
    {
        void ShipOrder(Order order);
    }

    public interface IOrderService
    {
        void PlaceOrder(Order order);
    }

    public class InvoiceService : IInvoiceService
    {
        public void GenerateInvoice(Order order)
        {
            Console.WriteLine($"Invoice generated for {order.ProductName}");
        }
    }

    public class CreditCardPaymentService : IPaymentService
    {
        public void ProcessPayment(Order order)
        {
            Console.WriteLine($"Payment of ${order.Price} processed for {order.ProductName}");
        }
    }

    public class ShippingService : IShippingService
    {
        public void ShipOrder(Order order)
        {
            Console.WriteLine($"{order.ProductName} has been shipped.");
        }
    }

    public class OrderService : IOrderService
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IPaymentService _paymentService;
        private readonly IShippingService _shippingService;

        public OrderService(IInvoiceService invoiceService, IPaymentService paymentService, IShippingService shippingService)
        {
            _invoiceService = invoiceService;
            _paymentService = paymentService;
            _shippingService = shippingService;
        }

        public void PlaceOrder(Order order)
        {
            _invoiceService.GenerateInvoice(order);
            _paymentService.ProcessPayment(order);
            _shippingService.ShipOrder(order);
            Console.WriteLine("Order placed successfully!");
        }
    }

    class Program
    {
        static void Main()
        {
            IInvoiceService invoiceService = new InvoiceService();
            IPaymentService paymentService = new CreditCardPaymentService();
            IShippingService shippingService = new ShippingService();
            IOrderService orderService = new OrderService(invoiceService, paymentService, shippingService);

            List<Order> orders = new List<Order>();
            int orderIdCounter = 1;
            bool exitProgram = false;

            while (!exitProgram)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Create Transaction");
                Console.WriteLine("2. Delete Transaction");
                Console.WriteLine("3. View History of Transactions");
                Console.WriteLine("4. Cancel (Exit Program)");
                Console.Write("Your choice: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter product name: ");
                        string productName = Console.ReadLine();

                        double price;
                        while (true)
                        {
                            Console.Write("Enter product price: ");
                            if (double.TryParse(Console.ReadLine(), out price) && price > 0)
                                break;
                            Console.WriteLine("Invalid price! Please enter a valid numeric value.");
                        }

                        Order newOrder = new Order { Id = orderIdCounter++, ProductName = productName, Price = price };
                        orders.Add(newOrder);

                        Console.WriteLine("\nProcessing order...\n");
                        orderService.PlaceOrder(newOrder);
                        break;

                    case "2":
                        Console.Write("Enter Order ID to delete: ");
                        int idToDelete;
                        if (int.TryParse(Console.ReadLine(), out idToDelete))
                        {
                            Order orderToRemove = orders.Find(o => o.Id == idToDelete);
                            if (orderToRemove != null)
                            {
                                orders.Remove(orderToRemove);
                                Console.WriteLine($"Order {idToDelete} deleted successfully.");
                            }
                            else
                            {
                                Console.WriteLine($"Order with ID {idToDelete} not found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Order ID!");
                        }
                        break;

                    case "3":
                        if (orders.Count > 0)
                        {
                            Console.WriteLine("History of Transactions:");
                            foreach (Order order in orders)
                            {
                                Console.WriteLine($"Order {order.Id}: {order.ProductName} - ${order.Price}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No transactions available.");
                        }
                        break;

                    case "4":
                        exitProgram = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
                Console.WriteLine(); 
            }

            Console.WriteLine("Exiting program. Press Enter to exit...");
            Console.ReadLine();
        }
    }
}
