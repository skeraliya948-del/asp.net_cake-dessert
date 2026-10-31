using System;
using System.Data.SqlClient;
using System.Configuration;

namespace SweetDelights
{
    public static class DatabaseHelper
    {
        public const string AdminEmail = "admin@gmail.com";

        public static bool IsAdminEmail(string email)
        {
            return string.Equals(email, AdminEmail, StringComparison.OrdinalIgnoreCase);
        }

        public static SqlConnection GetOpenConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            try
            {
                con.Open();
                EnsureTablesExist(con);
                return con;
            }
            catch (SqlException ex)
            {
                // Error 4060: Cannot open database requested by login
                if (ex.Number == 4060 || ex.Message.Contains("Cannot open database") || ex.Message.Contains("login failed"))
                {
                    // 1. Connect to master database and create SweetDelightsDB automatically
                    string masterConnectionString = connectionString
                        .Replace("Initial Catalog=SweetDelightsDB", "Initial Catalog=master")
                        .Replace("Database=SweetDelightsDB", "Database=master");

                    using (SqlConnection masterCon = new SqlConnection(masterConnectionString))
                    {
                        masterCon.Open();
                        using (SqlCommand cmd = new SqlCommand("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SweetDelightsDB') CREATE DATABASE SweetDelightsDB;", masterCon))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // 2. Open connection to newly created SweetDelightsDB
                    con = new SqlConnection(connectionString);
                    con.Open();
                    EnsureTablesExist(con);
                    return con;
                }
                else
                {
                    throw;
                }
            }
        }

        public static void EnsureTablesExist(SqlConnection con)
        {
            string createTablesSql = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'emp_tbl')
                BEGIN
                    CREATE TABLE emp_tbl (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name VARCHAR(100) NOT NULL,
                        Gender VARCHAR(20) NOT NULL,
                        Email VARCHAR(100) NOT NULL UNIQUE,
                        Password VARCHAR(100) NOT NULL,
                        City VARCHAR(50) NOT NULL,
                        Address VARCHAR(255) NULL,
                        Mobile VARCHAR(20) NULL,
                        Image VARCHAR(255) NULL
                    );
                    INSERT INTO emp_tbl (Name, Gender, Email, Password, City, Address, Mobile, Image)
                    VALUES ('Admin User', 'Male', 'admin@gmail.com', '123456', 'Ahmedabad', 'Sweet Plaza, CG Road', '9876543210', 'images/default-user.jpg');
                END

                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Add_Company_Tbl')
                BEGIN
                    CREATE TABLE Add_Company_Tbl (
                        Comp_Id INT IDENTITY(1,1) PRIMARY KEY,
                        Comp_Name VARCHAR(150) NOT NULL,
                        Comp_Owner_Name VARCHAR(150) NULL,
                        Comp_Email VARCHAR(100) NULL,
                        Comp_Address VARCHAR(255) NULL,
                        Comp_Cover_Img VARCHAR(255) NULL,
                        Comp_Image VARCHAR(255) NULL,
                        Comp_Desc VARCHAR(MAX) NULL
                    );
                    INSERT INTO Add_Company_Tbl (Comp_Name, Comp_Owner_Name, Comp_Email, Comp_Address, Comp_Cover_Img, Comp_Image, Comp_Desc)
                    VALUES 
                    ('Celebration Cakes', 'Chef Pierre', 'pierre@bakery.com', 'CG Road, Ahmedabad', 'https://images.unsplash.com/photo-1578985545062-69928b1d9587?auto=format&fit=crop&w=600&q=80', 'https://images.unsplash.com/photo-1578985545062-69928b1d9587?auto=format&fit=crop&w=600&q=80', 'Custom birthday, anniversary and designer multi-tiered cakes.'),
                    ('French Macarons & Desserts', 'Meera Desai', 'meera@bakery.com', 'SG Highway, Ahmedabad', 'https://images.unsplash.com/photo-1569864358642-9d1684040f43?auto=format&fit=crop&w=600&q=80', 'https://images.unsplash.com/photo-1569864358642-9d1684040f43?auto=format&fit=crop&w=600&q=80', 'Authentic French macarons, dessert cups and mousse jars.'),
                    ('Gourmet Pastries & Cheesecakes', 'Ramesh Patel', 'ramesh@bakery.com', 'Navrangpura, Ahmedabad', 'https://images.unsplash.com/photo-1533134242443-d4fd215305ad?auto=format&fit=crop&w=600&q=80', 'https://images.unsplash.com/photo-1533134242443-d4fd215305ad?auto=format&fit=crop&w=600&q=80', 'Fresh single slice pastries, New York cheesecakes and tarts.');
                END

                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Add_Products_Tbl')
                BEGIN
                    CREATE TABLE Add_Products_Tbl (
                        Prod_Id INT IDENTITY(1,1) PRIMARY KEY,
                        Prod_Comp_Id INT NOT NULL DEFAULT 1,
                        Prod_Name VARCHAR(150) NOT NULL,
                        Prod_Model VARCHAR(150) NULL,
                        Prod_Desc VARCHAR(MAX) NULL,
                        Prod_Weight VARCHAR(50) NULL,
                        Prod_Config VARCHAR(255) NULL,
                        Prod_Img VARCHAR(255) NULL,
                        Prod_Image VARCHAR(255) NULL,
                        Prod_Price DECIMAL(18,2) NOT NULL DEFAULT 0.00
                    );
                END

                IF (SELECT COUNT(*) FROM Add_Products_Tbl) < 5
                BEGIN
                    DELETE FROM Add_Products_Tbl;
                    INSERT INTO Add_Products_Tbl (Prod_Comp_Id, Prod_Name, Prod_Model, Prod_Desc, Prod_Weight, Prod_Config, Prod_Img, Prod_Image, Prod_Price)
                    VALUES 
                    (1, 'Belgian Chocolate Truffle Cake', 'Belgian Chocolate Truffle', 'Rich Belgian dark chocolate sponge layered with smooth ganache, cocoa dust & gold leaf flakes.', '0.5 kg', '100% Eggless • 24k Gold Flakes', 'https://images.unsplash.com/photo-1578985545062-69928b1d9587?auto=format&fit=crop&w=800&q=80', 'https://images.unsplash.com/photo-1578985545062-69928b1d9587?auto=format&fit=crop&w=800&q=80', 650.00),
                    (1, 'Red Velvet Cream Cheese Cake', 'Red Velvet Cream Cheese', 'Crimson cocoa sponge infused with real vanilla bean and iced with silky cream cheese frosting.', '0.5 kg', '100% Eggless • Cream Cheese', 'https://images.unsplash.com/photo-1586985289688-ca3cf47d3e6e?auto=format&fit=crop&w=800&q=80', 'https://images.unsplash.com/photo-1586985289688-ca3cf47d3e6e?auto=format&fit=crop&w=800&q=80', 700.00),
                    (3, 'Fresh Strawberry NY Cheesecake', 'Strawberry Cheesecake', 'Classic baked New York cheesecake topped with homemade fresh strawberry compote.', '1 Slice', '100% Eggless • Real Strawberries', 'https://images.unsplash.com/photo-1533134242443-d4fd215305ad?auto=format&fit=crop&w=800&q=80', 'https://images.unsplash.com/photo-1533134242443-d4fd215305ad?auto=format&fit=crop&w=800&q=80', 750.00),
                    (2, 'Parisian Macaron Assortment Box', 'Parisian Macaron Box', 'Assorted delicate french macarons: Pistachio blush, Strawberry cream, Lemon curd, and Dark Chocolate.', 'Box of 12', 'Artisanal • 12 Pieces', 'https://images.unsplash.com/photo-1569864358642-9d1684040f43?auto=format&fit=crop&w=800&q=80', 'https://images.unsplash.com/photo-1569864358642-9d1684040f43?auto=format&fit=crop&w=800&q=80', 480.00),
                    (1, 'Classic Black Forest Cherry Cake', 'Black Forest Special', 'Moist chocolate layers packed with sour cherries, fresh whipped cream and dark chocolate curls.', '0.5 kg', '100% Eggless • Fresh Cherries', 'https://images.unsplash.com/photo-1606890737304-57a1ca8a5b62?auto=format&fit=crop&w=800&q=80', 'https://images.unsplash.com/photo-1606890737304-57a1ca8a5b62?auto=format&fit=crop&w=800&q=80', 600.00),
                    (2, 'Blueberry Mousse Dessert Jar', 'Blueberry Mousse Jar', 'Creamy blueberry mousse layered with crushed biscuit crust and fresh blueberry glaze.', '1 Jar', '100% Eggless • Glass Jar', 'https://images.unsplash.com/photo-1488477181946-6428a0291777?auto=format&fit=crop&w=800&q=80', 'https://images.unsplash.com/photo-1488477181946-6428a0291777?auto=format&fit=crop&w=800&q=80', 280.00),
                    (3, 'Italian Coffee Tiramisu Slice', 'Tiramisu Pastry', 'Authentic Italian ladyfinger sponge soaked in espresso coffee and layered with mascarpone cream.', '1 Slice', '100% Eggless • Real Espresso', 'https://images.unsplash.com/photo-1571115177098-24ec42ed204d?auto=format&fit=crop&w=800&q=80', 'https://images.unsplash.com/photo-1571115177098-24ec42ed204d?auto=format&fit=crop&w=800&q=80', 220.00),
                    (3, 'Molten Choco Lava Delight', 'Choco Lava Cake', 'Warm chocolate cake with a rich molten liquid chocolate center that oozes out on first bite.', 'Set of 2', '100% Eggless • Serve Warm', 'https://images.unsplash.com/photo-1624353365286-3f8d62daad51?auto=format&fit=crop&w=800&q=80', 'https://images.unsplash.com/photo-1624353365286-3f8d62daad51?auto=format&fit=crop&w=800&q=80', 190.00),
                    (1, 'Alphonso Mango Cream Cake', 'Mango Alphonso Special', 'Golden vanilla sponge infused with natural Alphonso mango pulp and topped with fresh mango slices.', '0.5 kg', '100% Eggless • Fresh Alphonso', 'https://images.unsplash.com/photo-1535141192574-5d4897c13136?auto=format&fit=crop&w=800&q=80', 'https://images.unsplash.com/photo-1535141192574-5d4897c13136?auto=format&fit=crop&w=800&q=80', 680.00);
                END

                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Cart_Tbl')
                BEGIN
                    CREATE TABLE Cart_Tbl (
                        Cart_Id INT IDENTITY(1,1) PRIMARY KEY,
                        Cart_User_Id INT NOT NULL,
                        Cart_Prod_Id INT NOT NULL,
                        Quentity INT NOT NULL DEFAULT 1,
                        AddedDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END

                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Order_Tbl')
                BEGIN
                    CREATE TABLE Order_Tbl (
                        Order_No INT IDENTITY(1,1) PRIMARY KEY,
                        Order_User_Id INT NOT NULL,
                        Order_Date DATETIME NOT NULL DEFAULT GETDATE(),
                        Ship_Address VARCHAR(500) NOT NULL,
                        Pay_Method VARCHAR(50) NOT NULL,
                        Pay_Status VARCHAR(30) NOT NULL DEFAULT 'Pending',
                        Order_Status VARCHAR(30) NOT NULL DEFAULT 'Pending',
                        Subtotal DECIMAL(18,2) NOT NULL,
                        Delivery_Charge DECIMAL(18,2) NOT NULL,
                        Grand_Total DECIMAL(18,2) NOT NULL
                    );
                END

                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Order_Details_Tbl')
                BEGIN
                    CREATE TABLE Order_Details_Tbl (
                        Order_Detail_Id INT IDENTITY(1,1) PRIMARY KEY,
                        Order_No INT NOT NULL,
                        Order_Prod_Id INT NOT NULL,
                        Product_Name VARCHAR(150) NOT NULL,
                        Quentity INT NOT NULL,
                        Unit_Price DECIMAL(18,2) NOT NULL,
                        CONSTRAINT FK_Order_Details_Tbl_Order_Tbl
                            FOREIGN KEY (Order_No) REFERENCES Order_Tbl(Order_No)
                    );
                END
            ";

            using (SqlCommand cmd = new SqlCommand(createTablesSql, con))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
