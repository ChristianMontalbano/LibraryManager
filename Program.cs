using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using LibraryManager;
using System.IO;
using System.Transactions;



class Program()
{
    static List<LibraryItem> list = new List<LibraryItem>();
    static List<CheckoutManager> checkoutList = new List<CheckoutManager>();


    static void Main(String[] args)
    {
        MainMenu();

    }


    public static void MainMenu()
    {
        while (true)
        { 
            Console.Clear();
            Console.WriteLine("\nPlease Select an Option");
            Console.WriteLine("1. Add a Library Item");
            Console.WriteLine("2. View Available Items");
            Console.WriteLine("3. Check Out an Item");
            Console.WriteLine("4. Return an Item");
            Console.WriteLine("5. View My Checkout Receipt");
            Console.WriteLine("6. Save My Checkout List To File");
            Console.WriteLine("7. Load My Previous Checkout List From File");
            Console.WriteLine("8. Exit Application");

            Console.WriteLine("\nPlease Select an Option (1-7): ");
            string choice = Console.ReadLine();



            switch (choice)
            {
                case "1":
                    AddLibraryItem();
                    break;

                case "2":
                    ViewAvailableItems();
                    break;

                case "3":
                    CheckoutItem();
                    break;

                case "4":
                    ReturnItem();
                    break;

                case "5":
                    PrintReceipt();
                    break;

                case "6":
                    SaveCheckoutListToFile();
                    break;

                case "7":
                    LoadCheckoutListFromFile();
                    break;

                case "8":
                    return;

                default:
                    Console.WriteLine("Invalid option. Please Press Enter and Try Again");
                    Console.ReadLine();
                    break;


















            }





        }



    }


    static void AddLibraryItem()
    {
        Console.Clear();
        Console.WriteLine("---Please add a Library Item---\n");


        Console.Write(" Please Enter Book ID: ");
        int id;
        while(!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Not A Valid ID. Please Enter A Positive Whole Number.");
            Console.Write("Please Enter Book ID: ");
        }



        Console.Write("Please Enter Title of Item: ");
        string title = Console.ReadLine();

        Console.Write("Please Enter Item Type (Book, Movie, Etc): ");
        string type = Console.ReadLine();

        Console.Write("Please Enter DailyLateFee: ");
        double dailyLateFee;
        while(!double.TryParse(Console.ReadLine(), out dailyLateFee))
        {
            Console.WriteLine("Not A Valid Fee, Please Enter Applicable USD Late Fee.");
            Console.Write("Please Enter Daily Late Fee");
        }


        LibraryItem item = new LibraryItem(id, title, type, dailyLateFee);

        list.Add(item);

        Console.WriteLine("\nItem(s) Added Successfully to your cart. Press Enter to Acknowledge.");
        Console.ReadLine();
    }


    static void ViewAvailableItems()
    {
        Console.Clear();
        Console.WriteLine("--- Availble Library Items ---\n");

        if (list.Count == 0)
        {
            Console.WriteLine("No Items Available Currently.");
        }
        else
        {
            foreach (LibraryItem item in list)
            {
                item.PrintLibraryItem();
                Console.WriteLine();
            }


        }

        Console.WriteLine("Please Press The Enter Key To Continue.");
        Console.ReadLine();
    }


    static void CheckoutItem()
    {
        Console.Clear();
        Console.WriteLine("---Check Out An Item---\n");

        if (list.Count == 0)
        {
            Console.WriteLine("There Are No Items In The Library At This Moment.");
            Console.WriteLine("Please Add An Item Before Checking Out.");
            Console.WriteLine("Please Press The Enter Key to Continue.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Available Items:\n ");
        foreach (LibraryItem item in list)
        {
            item.PrintLibraryItem();
            Console.WriteLine();
        }

        Console.Write("Enter ID of Item to Check Out: ");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Invalid Item ID. Try Again.");
            Console.Write("Enter Item ID: ");
          
        }

        LibraryItem Selecteditem = list.Find(b => b.id == id);

        if (Selecteditem == null)
        {
            Console.WriteLine("Item Not Found With Inputted ID.");
            Console.ReadLine();
            return;
        }


        bool alreadyCheckedOut = checkoutList.Any(cm => cm.Book.id == Selecteditem.id);

        if (alreadyCheckedOut)
        {
            Console.WriteLine("\nItem Is Already Checked Out. Please Select A Different Item.");
            Console.ReadLine();
            return;

        }

        Console.Write("Enter the Number Of Days The Item Is Late. Please Enter 0 If The Item Is Not Late: ");
        int daysLate;
        while (!int.TryParse(Console.ReadLine(), out daysLate) || daysLate < 0)
        {
            Console.WriteLine("Invalid Entry. Please Enter 0 Or A Positive Whole Number.");
            Console.Write("Enter the Number Of Days The Item Is Late. Please Enter 0 If The Item Is Not Late: ");

        }
        
           

        int dueDate = 0;

        CheckoutManager validCheckoutItem = new CheckoutManager(dueDate, daysLate, Selecteditem);
        checkoutList.Add(validCheckoutItem);
    }


    static void ReturnItem()
    {
        Console.Clear();
        Console.WriteLine("--- Return an Item---");

        if (checkoutList.Count == 0)
        {
            Console.WriteLine("You Have no Library Items Checked Out");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Items Currently Checked Out: \n");

        foreach(CheckoutManager cm in checkoutList)
        {
            Console.WriteLine($"ID: {cm.Book.id} Title: {cm.Book.title} Type: {cm.Book.type}");
        }

        Console.Write("\nEnter ID Of Item To Return: ");
        int id;
        while (!int.TryParse(Console.ReadLine(),out id))
        {
            Console.WriteLine("Not A Valid ID. Please Try Again.");
            Console.Write("Enter ID of Item to Return: ");       
            
        }

        CheckoutManager removecheckoutItem = checkoutList.Find(cm=> cm.Book.id == id);

        if (removecheckoutItem == null)
        {
            Console.WriteLine("No Items Checked Out With That ID");
        }

        else
        {
            checkoutList.Remove(removecheckoutItem);
            Console.WriteLine("Item Successfully Returned. Thank You.");
            
        }

        Console.WriteLine("Press Enter To Continue.");
        Console.ReadLine();

    }


    static void PrintReceipt()
    {
        Console.Clear();
        Console.WriteLine("---Checkout Receipt---\n");

        if(checkoutList.Count == 0)
        {
            Console.WriteLine("No Items In Checkout List.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("ID   Title                     Type       Days Late   Late Fee");
        Console.WriteLine("---------------------------------------------------------------");

        double totalLateFees = 0;

        foreach(CheckoutManager cm in checkoutList)
        {
            string line = cm.checkoutLineItem();
            Console.WriteLine(line);

            totalLateFees += cm.lateFeeCalculator();
        }


        Console.WriteLine("---------------------------------------------------------------");
        Console.WriteLine($"Total Late Fees: ${totalLateFees:F2}");

        Console.WriteLine("\nPress Enter to continue.");
        Console.ReadLine();

    }


    static void SaveCheckoutListToFile()
    {
        Console.Clear();
        Console.WriteLine("--- Save Checkout List To A File ---\n");

        if (checkoutList.Count == 0)
        {
            Console.WriteLine("No Items In List To Checkout. Please Add At Least One Item And Try Again.");
            Console.ReadLine();
            return; 
        }

        string saveFileName = "Library_Checkout.txt";

        List<string> lines = new List<string>();

        foreach(CheckoutManager cm in checkoutList)
        {
            int dueDate = cm.DueDate;
            int daysLate = cm.DaysLate;

            double lateFee = cm.lateFeeCalculator();
            string lateFeeAddToReceipt = lateFee.ToString("F2");

            string line = string.Join("|",
                cm.Book.id,
                cm.Book.title,
                cm.Book.type,
                cm.Book.dailyLateFee,
                dueDate,
                daysLate,
                lateFeeAddToReceipt
               );

            lines.Add(line);

        }

        File.WriteAllLines(saveFileName, lines);

        Console.WriteLine($"Checkout List Successfully Saved: {saveFileName}");
        Console.ReadLine();
    }


    static void LoadCheckoutListFromFile()
    {
        Console.Clear();
        Console.WriteLine("--- Load Checkout List From File---\n");

        string filename = "Library_Checkout.txt";

        if (!File.Exists(filename))
        {
            Console.WriteLine($"File '{filename}' Not Found. Save A List First And Try Again");
            Console.ReadLine();
            return;
        }

        string[] lines = File.ReadAllLines(filename);

        if( lines.Length == 0)
        {
            Console.WriteLine("Checkout File Empty");
            Console.ReadLine();
            return;
        }

        checkoutList.Clear();

        foreach(string line in lines)
        {
            if(string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split('|');

            if( parts.Length < 6 )
                continue;

            try
            {
                int id = int.Parse(parts[0]);
                string title = parts[1];
                string type = parts[2];
                double dailyLateFee = double.Parse(parts[3]);
                int dueDate = int.Parse(parts[4]);
                int daysLate = int.Parse(parts[5]);

                LibraryItem item = new LibraryItem(id, title, type, dailyLateFee);

                CheckoutManager cm = new CheckoutManager(dueDate, daysLate, item);

                checkoutList.Add(cm);
            }
            catch
            {

                continue;
            }

           
           

        }

        Console.WriteLine("List Sucessfully Loaded From .txt File.");
        Console.ReadLine();
    }

}
    
