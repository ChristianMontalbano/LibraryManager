using System;

                             

namespace LibraryManager
{
    //Changed name from CheckoutItem to CheckoutManager for better accuracy as this class contains everything related for checking out a customer.

    public class CheckoutManager
    {
        public LibraryItem Book;
        public int DueDate {  get; set; }
        public int DaysLate {  get; set; }



        //constructor that sets the parameter of what a checkout manager needs to have.
        public CheckoutManager(int DueDate, int DaysLate, LibraryItem Book)
        {

            this.DueDate = DueDate;                                          
            this.DaysLate = DaysLate;
            this.Book = Book;

        }


        //Helper function used for determining late fees 
        public double lateFeeCalculator()
        {
            if (DaysLate <= 0)
                return 0;

            return Book.dailyLateFee * DaysLate;
        }



        //function that takes in the days late, due date and item.
        public string checkoutLineItem()
        {

            double finalLateFee = lateFeeCalculator();
             
            //string that contains all pertinent information for the reciept line item portion that is generated and returned after all calculations have been made.

            string recieptLineItem = $"{Book.id, -5}    {Book.title, -25}    {Book.type, -10}    {DaysLate, -5}    ${finalLateFee:F2} ";


            return recieptLineItem ;


        }
        
        

        

    }
}
