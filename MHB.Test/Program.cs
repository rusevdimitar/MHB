using MHB.BL;

namespace MHB.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //MHB.UserManager.User usr = new MHB.UserManager.User(@"Data Source = localhost\SQLEXPRESS; Initial Catalog = Test01Db; User ID =sa; Password = mitko123");

            //usr.UserID = 1;
            //usr.Email = "rusev.dimitar@gmail.com";
            //usr.Password = "mitko123";

            //usr.UpdateUser();

            //User.GetUserID("rusev.dimitar@gmail.com", "mitko123", @"Data Source = localhost\SQLEXPRESS; Initial Catalog = Test01Db; User ID =sa; Password = mitko123");

            //MHB.BL.Category category = new BL.Category();

            //category.Name = "КАТЕГОРИЯ№1";
            //category.UserID = 1;
            //category.IsPayIconVisible = true;
            //category.IconPath = @"d:\papka\папка\папка\картинка.jpeg";
            //category.CategoryKeyWords = "НЕЩО1, нещо2, нещо3, нещо 4, нещо5";
            //category.Add(@"Data Source = localhost\SQLEXPRESS; Initial Catalog = Test01Db; User ID =sa; Password = mitko123", 0);

            //ExpenseManager manager = new ExpenseManager();
            //manager.UserID = 1;
            //List<tbMainTable01> expenses = manager.GetUserExpenses();

            string connectionString = @"Data Source = localhost\SQLEXPRESS; Initial Catalog = Test01Db; User ID =sa; Password = mitko123";

            Product p = new Product(1, 1, connectionString);

            Product.Delete(1, connectionString, 1);

            p.ConnectionString = connectionString;
            p.Name = "Луканка";
            p.Description = "Банско старец";
            p.KeyWords = "салам,шпек,луканка,бански старец,сушеница".Split(',');
            p.ListPrice = 13.77M;
            p.StandardCost = 54.998M;
            p.Color = "#444004";
            p.Picture = new byte[10];
            p.UserID = 1;
            p.Volume = 1.45M;
            p.Weight = 3.94M;

            //p.Update();
        }
    }
}