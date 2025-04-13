using tpmodul8_103022300014;

class Program
{
    static void Main(string[] args)
    {
        // Membuat objek konfigurasi, membaca dari file JSON saat runtime (Runtime Configuration)
        CovidConfig config = new CovidConfig();

        // ubah satuan suhu (internationalization awal)
        config.UbahSatuan();

        // Menampilkan nilai konfigurasi setelah perubahan satuan
        Console.WriteLine($"[DEBUG] Satuan suhu yang digunakan sekarang: {config.satuan_suhu}");
        Console.WriteLine($"batas hari demam: {config.batas_hari_deman}");

        // Menampilkan range suhu normal berdasarkan satuan
        if (config.satuan_suhu.ToLower() == "celcius")
            Console.WriteLine("Range suhu normal: 36.5°C – 37.5°C");
        else
            Console.WriteLine("Range suhu normal: 97.7°F – 99.5°F");

        // Input suhu tubuh dari user
        Console.Write($"\nBerapa suhu badan anda saat ini? Dalam nilai {config.satuan_suhu}: ");
        if (!double.TryParse(Console.ReadLine(), out double suhu))
        {
            Console.WriteLine("Input suhu tidak valid.");
            return;
        }

        // Input hari terakhir mengalami gejala demam
        Console.Write("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam? ");
        if (!int.TryParse(Console.ReadLine(), out int hari))
        {
            Console.WriteLine("Input hari tidak valid.");
            return;
        }

        // Cek apakah suhu tubuh dalam rentang normal berdasarkan satuan
        bool suhuNormal = false;
        if (config.satuan_suhu.ToLower() == "celcius")
            suhuNormal = suhu >= 36.5 && suhu <= 37.5;
        else if (config.satuan_suhu.ToLower() == "fahrenheit")
            suhuNormal = suhu >= 97.7 && suhu <= 99.5;

        // Cek apakah gejala demam terakhir sudah cukup lama
        bool gejalaRingan = hari < config.batas_hari_deman;

        // Cek apakah gejala demam terakhir sudah cukup lama
        if (suhuNormal && gejalaRingan)
            Console.WriteLine(config.pesan_diterima);
        else
            Console.WriteLine(config.pesan_ditolak);
    }
}
