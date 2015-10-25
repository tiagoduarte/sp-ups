using System;
using System.Linq;

namespace SPUPS
{
    class Program
    {
        static void Main(string[] args)
        {
            UserProfileServiceHelper ups = new UserProfileServiceHelper();
            if(args.Count() != 3)
            {
                Console.WriteLine("Usage: SPUPS.exe <website-url> <domain> <username>");
            }
            else
            {
                string[] properties = { "AccountName", "Department", "Section", "Group" };
                ups.DisplayProperties(args[0], args[1], args[2], properties);
            }

        }
    }
}
