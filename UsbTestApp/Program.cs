using System;
using System.Management;

namespace SmartCardReaderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // WMI'ye erişmek için bir ManagementScope nesnesi oluşturun
            ManagementScope scope = new ManagementScope();

            // Tüm akıllı kart okuyucularını çeken genel bir sorgu
            string queryString = "SELECT * FROM Win32_PnPEntity WHERE PNPClass='SmartCardReader'";
            var query = new WqlObjectQuery(queryString);

            using (var searcher = new ManagementObjectSearcher(scope, query))
            {
                // Akıllı kart okuyucularının koleksiyonunu alın
                var smartCardReaders = searcher.Get();

                // Cihaz bulundu mu kontrol edin
                if (smartCardReaders.Count > 0)
                {
                    // Koleksiyon üzerinde döngü yapın ve elemanlara erişin
                    foreach (var reader in smartCardReaders)
                    {
                        // Hata ayıklama ekleyin
                        try
                        {
                            // DeviceID değerini kontrol et
                            string deviceId = reader["DeviceID"] as string;
                            Console.WriteLine("Smart Card Reader DeviceID: " + deviceId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No Smart Card Readers found.");
                }
            }

            // Uygulamanın kapanmasını engellemek için bekleyin
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}
