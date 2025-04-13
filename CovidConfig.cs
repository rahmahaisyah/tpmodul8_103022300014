using System;
using System.IO;
using System.Text.Json;

namespace tpmodul8_103022300014
{
    // Kelas ini merepresentasikan konfigurasi COVID yang dapat dibaca dan disimpan ke file eksternal.
    // Menerapkan konsep Runtime Configuration (nilai bisa berubah tanpa compile ulang).
    class CovidConfig
    {
        // Properti konfigurasi yang akan disimpan/dibaca dari file JSON
        public string satuan_suhu { get; set; }
        public int batas_hari_deman { get; set; }
        public string pesan_ditolak { get; set; }
        public string pesan_diterima { get; set; }

        // Lokasi file konfigurasi
        private const string filePath = "covid_config.json";

        // Konstruktor membaca konfigurasi saat program dijalankan (Runtime Configuration)
        public CovidConfig()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    // Baca isi file JSON
                    string json = File.ReadAllText(filePath);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    // Deserialize ke class sementara untuk menghindari overwrite langsung
                    TempConfig? data = JsonSerializer.Deserialize<TempConfig>(json, options);

                    if (data == null)
                        throw new Exception("Data konfigurasi null");

                    // Set nilai properti dari hasil file konfigurasi
                    this.satuan_suhu = data.satuan_suhu;
                    this.batas_hari_deman = data.batas_hari_deman;
                    this.pesan_ditolak = data.pesan_ditolak;
                    this.pesan_diterima = data.pesan_diterima;
                }
                catch (Exception ex)
                {
                    // Jika gagal membaca file, gunakan nilai default dan buat file baru
                    Console.WriteLine($"Gagal membaca file konfigurasi. Menggunakan nilai default. Error: {ex.Message}");
                    SetDefaultValues();
                    SaveConfig();
                }
            }
            else
            {
                // Jika file tidak ada, gunakan default dan simpan ke file baru
                SetDefaultValues();
                SaveConfig();
            }
        }

        // Method untuk menetapkan nilai default konfigurasi
        private void SetDefaultValues()
        {
            this.satuan_suhu = "celcius";
            this.batas_hari_deman = 14;
            this.pesan_ditolak = "Anda tidak diperbolehkan masuk ke dalam gedung ini";
            this.pesan_diterima = "Anda dipersilahkan untuk masuk ke dalam gedung ini";
        }

        // Method untuk mengubah satuan suhu dari celcius ke fahrenheit, atau sebaliknya
        // Ini membantu menyesuaikan konfigurasi berdasarkan budaya pengguna (Internationalization step awal)
        public void UbahSatuan()
        {
            this.satuan_suhu = (this.satuan_suhu.ToLower() == "celcius") ? "fahrenheit" : "celcius";
            SaveConfig();
        }

        // Method untuk menyimpan konfigurasi ke file JSON
        public void SaveConfig()
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        // Class bantu untuk deserialize data JSON
        // Ini menjaga agar jika format JSON tidak cocok langsung ke CovidConfig, tidak merusak data utama
        private class TempConfig
        {
            public string satuan_suhu { get; set; } = "celcius";
            public int batas_hari_deman { get; set; } = 14;
            public string pesan_ditolak { get; set; } = "Anda tidak diperbolehkan masuk ke dalam gedung ini";
            public string pesan_diterima { get; set; } = "Anda dipersilahkan untuk masuk ke dalam gedung ini";
        }
    }
}
