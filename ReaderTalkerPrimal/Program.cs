using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("winscard.dll", SetLastError = true)]
    static extern int SCardEstablishContext(uint dwScope, IntPtr pvReserved1, IntPtr pvReserved2, out IntPtr phContext);

    [DllImport("winscard.dll", SetLastError = true)]
    static extern int SCardConnect(IntPtr hContext, string szReader, uint dwShareMode, uint dwPreferredProtocols, out IntPtr phCard, out uint pdwActiveProtocol);

    [DllImport("winscard.dll", SetLastError = true)]
    static extern int SCardDisconnect(IntPtr hCard, uint dwDisposition);

    [DllImport("winscard.dll", SetLastError = true)]
    static extern int SCardFreeMemory(IntPtr hContext, IntPtr pvMem);

    [DllImport("winscard.dll", SetLastError = true)]
    static extern int SCardListReaders(IntPtr hContext, byte[] mszGroups, byte[] mszReaders, ref uint pcchReaders);

    [DllImport("winscard.dll", SetLastError = true)]
    static extern int SCardTransmit(IntPtr hCard, IntPtr pioSendPci, byte[] pbSendBuffer, uint cbSendLength, IntPtr pioRecvPci, byte[] pbRecvBuffer, ref uint pcbRecvLength);

    [StructLayout(LayoutKind.Sequential)]
    struct SCARD_IO_REQUEST
    {
        public uint dwProtocol;
        public uint cbPciLength;
    }

    const uint SCARD_SCOPE_SYSTEM = 2;
    const uint SCARD_SHARE_SHARED = 2;
    const uint SCARD_PROTOCOL_T0 = 0x0001;
    const uint SCARD_PROTOCOL_T1 = 0x0002;
    const uint SCARD_LEAVE_CARD = 0;
    const uint SCARD_READER_NOT_PRESENT = 0x80100011;

    static void Main()
    {
        IntPtr hContext = IntPtr.Zero;
        IntPtr hCard = IntPtr.Zero;
        uint activeProtocol;

        // Bağlamayı oluştur
        int result = SCardEstablishContext(SCARD_SCOPE_SYSTEM, IntPtr.Zero, IntPtr.Zero, out hContext);

        if (result == 0)
        {
            Console.WriteLine("Bağlantı başarılı. Bağlanmış okuyucuları listeliyor...");

            // Bağlı okuyucuları al
            byte[] readers = new byte[256];
            uint readerSize = (uint)readers.Length;

            result = SCardListReaders(hContext, null, readers, ref readerSize);

            if (result == 0)
            {
                string readerName = System.Text.Encoding.UTF8.GetString(readers, 0, (int)readerSize).TrimEnd('\0');
                Console.WriteLine($"Okuyucu: {readerName}");

                // Okuyucuya bağlan
                result = SCardConnect(hContext, readerName, SCARD_SHARE_SHARED, SCARD_PROTOCOL_T0 | SCARD_PROTOCOL_T1, out hCard, out activeProtocol);

                if (result == 0)
                {
                    Console.WriteLine("Okuyucuya başarıyla bağlandı.");

                    // Yeni APDU oluştur ve kartla iletişim kur
                    byte[] apduCommand = { 0xA0, 0xDA };
                    byte[] responseBuffer = new byte[256];
                    uint responseSize = (uint)responseBuffer.Length;

                    SCARD_IO_REQUEST ioRequest = new SCARD_IO_REQUEST { dwProtocol = activeProtocol, cbPciLength = (uint)Marshal.SizeOf(typeof(SCARD_IO_REQUEST)) };
                    result = SCardTransmit(hCard, IntPtr.Zero, apduCommand, (uint)apduCommand.Length, IntPtr.Zero, responseBuffer, ref responseSize);

                    if (result == 0)
                    {
                        Console.WriteLine($"Karttan gelen yanıt: {BitConverter.ToString(responseBuffer, 0, (int)responseSize)}");
                    }
                    else
                    {
                        Console.WriteLine($"Kartla iletişim kurulamadı. Hata Kodu: {result}");
                    }

                    // Karttan ayrıl
                    SCardDisconnect(hCard, SCARD_LEAVE_CARD);
                }
                else
                {
                    Console.WriteLine($"Okuyucuya bağlanırken bir hata oluştu. Hata Kodu: {result}");
                }

            }
            else
            {
                Console.WriteLine($"Okuyucu listesi alınırken bir hata oluştu. Hata Kodu: {result}");
            }

            // Bağlamayı kapat
            SCardEstablishContext(SCARD_SCOPE_SYSTEM, IntPtr.Zero, IntPtr.Zero, out hContext);
        }
        else
        {
            Console.WriteLine($"Bağlanırken bir hata oluştu. Hata Kodu: {result}");
        }

        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}