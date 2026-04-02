using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;


namespace Ephemera.WinConsole.Test
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the test application. TODO needs more tests.
        /// </summary>
        [STAThread]
        static void Main(string[] _)
        {

            Console.WriteLine("Hello");
            Debug.WriteLine($"{DateTime.Now} Call hide");
            Utils.Hide();
            //Console.WriteLine("Should be gone");  boom
            Thread.Sleep(2000);
            Debug.WriteLine($"{DateTime.Now} Exiting");

        }
    }
}
