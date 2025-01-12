using System;
using System.Data.SQLite;
using System.Security.Cryptography.X509Certificates;

class BankDemo
{
    private static string name, email, phone, address, dateOfBirth, accountNumber, currentTime, id;
    public static void Main()
    {
        GetUserInformation();
        CreateConnectionSQLite();
    }
    public static void GetUserInformation()
    {
        Console.WriteLine("To create an account insert your name");

        name = Console.ReadLine();

        Console.WriteLine("To create an account insert your email");

        email = Console.ReadLine();

        Console.WriteLine("To create an account insert your phone");

        phone = Console.ReadLine();

        Console.WriteLine("To create an account insert your address");

        address = Console.ReadLine();

        Console.WriteLine("To create an account insert your date of birth");

        dateOfBirth = Console.ReadLine();

        GenerateRandomNumber();

        GetCurrentTime();
    }
    public static void CreateConnectionSQLite()
    {
        string connectionString = "Data Source=users.db;Version=3;";
        using (SQLiteConnection connection = new SQLiteConnection(connectionString)) 
        {
            connection.Open();
            CreateTables(connection);
            InsertData(connection);
            
        }
    }
    public static void CreateTables(SQLiteConnection connection)
    {
        string createCustomersTable =
        @" CREATE TABLE IF NOT EXISTS customers 
            (
                id INTEGER PRIMARY KEY,
                name TEXT,
                email TEXT UNIQUE,
                phone TEXT,
                address TEXT,
                date_of_birth DATE,
                account_number TEXT UNIQUE
            );
        ";

        string createAccountsTable = 
        @" CREATE TABLE IF NOT EXISTS accounts 
            ( 
                account_id INTEGER PRIMARY KEY,
                customer_id INTEGER,
                account_type TEXT,
                balance REAL,
                created_date DATE,
                FOREIGN KEY (customer_id) REFERENCES customers(id) 
            );
        ";

        string createTransactionsTable = 
        @" CREATE TABLE IF NOT EXISTS transactions 
            ( 
                transaction_id INTEGER PRIMARY KEY,
                account_id INTEGER,
                transaction_type TEXT,
                amount REAL,
                transaction_date DATE,
                description TEXT,
                FOREIGN KEY (account_id) REFERENCES accounts(account_id) 
            );
        ";

        using (SQLiteCommand command = new SQLiteCommand(createCustomersTable, connection))
        {
            command.ExecuteNonQuery();
        }

        using (SQLiteCommand command =  new SQLiteCommand(createAccountsTable, connection))
        {
            command.ExecuteNonQuery();
        }

        using (SQLiteCommand command = new SQLiteCommand(createTransactionsTable, connection))
        {
            command.ExecuteNonQuery();
        }



    }
    public static void InsertData(SQLiteConnection connection)
    {
        string insertDataQuery = $"INSERT INTO customers (name, email, phone, address, date_of_birth, account_number) VALUES ('{name}', '{email}', '{phone}', '{address}', '{dateOfBirth}', '{accountNumber}')";
        using (SQLiteCommand command = new SQLiteCommand(insertDataQuery, connection))
        {
            command.ExecuteNonQuery();
        }

        string query = "SELECT * FROM customers";
        using (SQLiteCommand command = new SQLiteCommand(query, connection))
        {
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = reader["id"].ToString();
                }
            }
        }

        string insertDataQuery1 = $"INSERT INTO accounts (customer_id, account_type, balance, created_date) VALUES ('{id}', 'normal', '1000.00', '{currentTime}')";
        using (SQLiteCommand command = new SQLiteCommand(insertDataQuery1, connection))
        {
            command.ExecuteNonQuery();
        }
    }
    public static void GenerateRandomNumber()
    {
        Random random = new Random();

        int accountNumberP = random.Next(1, 999);

        accountNumber = accountNumberP.ToString();
    }
    public static void GetCurrentTime()
    {
        DateTime currentTimeInt = DateTime.Now;
        currentTime = currentTimeInt.ToString("G");
    }
}