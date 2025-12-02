using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManager
{
    public class LibraryItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public double dailyLateFee { get; set; }



        public LibraryItem(int id, string title, string type, double dailyLateFee)
        {
            this.id = id;
            this.title = title;
            this.type = type;
            this.dailyLateFee = dailyLateFee;

        }

        
 

        public void PrintLibraryItem()
        {
            Console.WriteLine($"ID: {id} ");
            Console.WriteLine($"Title: {title} ");
            Console.WriteLine($"Type: {type} ");
            Console.WriteLine($"Daily Late Fee: {dailyLateFee:F2}\n ");
        }
        
       
    }   




}
