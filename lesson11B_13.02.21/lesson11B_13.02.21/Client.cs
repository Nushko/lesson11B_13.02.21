using System;
using System.Collections.Generic;
using System.Threading;
using System.IO.Pipes;
using System.Text;

namespace lesson11B_13._02._21
{
    public class Client
    {
        
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }

        public static List<Client> clientbase = new List<Client>();
        public static List<Client> checkbalancebase = new List<Client>();
    }
    public class Balance
    {
        public int Id { get; set; }
        public decimal OldBalance { get; set; }
        public decimal NewBalance { get; set; }
    }
}
