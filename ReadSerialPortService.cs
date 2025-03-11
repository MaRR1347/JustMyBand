using System;
using System.IO.Ports;


namespace BlazorApp1
{
    public class SerialPortService
    {
        public string SerialPortValue { get; set; }
        
        static SerialPort mySerialPort;

        public SerialPortService(string COMPort)
        {
            mySerialPort = new(COMPort, 921600);

            try
            {
                if (!mySerialPort.IsOpen)
                {
                    mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    try { mySerialPort.Open(); }
                    catch { Console.WriteLine("Nie znaleziono portu"); }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Błąd: Brak dostępu do portu. Szczegóły: " + ex.Message);
            }

        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            try
            {
                string indata = sp.ReadLine();
                SerialPortValue = indata.ToString();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }

        }

        public Task<string> GetSerialValue()
        {
            return Task.FromResult(SerialPortValue);
        }

        public void SendDataToArduino(string data)
        {
            try
            {
                if (mySerialPort != null && mySerialPort.IsOpen)
                {
                    mySerialPort.WriteLine(data);
                    Console.WriteLine($"Wysłano dane: {data}");
                }
                else if (mySerialPort == null)
                {
                    Console.WriteLine("Błąd: Port szeregowy niezainicjalizowany.");
                }
                else if (mySerialPort.IsOpen == false)
                {
                    Console.WriteLine("Błąd: Port szeregowy jest zamknięty.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas wysyłania danych: {ex.Message}");
            }
        }
    }
}