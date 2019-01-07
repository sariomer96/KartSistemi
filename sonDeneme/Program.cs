using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace sonDeneme
{
    class Program
    {
        
        static SerialPort port;
        static System.Threading.Timer TTimer;

        static void Main(string[] args)
        {

            //port = new SerialPort("COM4", 9600);


            port = new SerialPort("COM3");
            port.BaudRate = 9600;
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
            port.DataBits = 8;
            port.Handshake = Handshake.None;
            port.RtsEnable = true;

            port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            TTimer = new System.Threading.Timer(new TimerCallback(TickTimer), null, 1000, 1500);
            try
            {
                port.Open();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
            }


            Console.ReadLine();
        }

        static void TickTimer(object state)
        {

            port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            Thread.Sleep(1500);
        }

        public static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            
            try
            {
                string konua = port.ReadLine();
                if (string.IsNullOrEmpty(konua) == false)
                {
                    YurtOtomasyonuEntities1 db = new YurtOtomasyonuEntities1();
                    GirisCikis grs = new GirisCikis();
                    Ogrenciler ogr = new Ogrenciler();
                    int id = 1;
                    foreach (var i in db.Ogrenciler)
                    {
                        if (konua == i.KartID)
                        {
                            id = i.ID;
                            
                            //  grs.Ogrenciler_ID 
                           

                        }
                    }

                    grs.YurtGiris = DateTime.Now;
                    grs.Ogrenciler_ID = id;
                    db.GirisCikis.Add(grs);
                    Console.WriteLine(grs.YurtGiris);

                    db.SaveChanges();


                }
                else
                {
                    //Console.Write(port.ReadExisting());
                }

            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
            }
        }
       
    }
}