using System.ComponentModel.Design;
using System.IO;

#region Dizi Tanimlama
// Step 1 : Ilk olarak istenilen degerleri dizi icinde tanimladim

int[] studentIds = new int[5];
string[] firstNames = new string[5];
string[] lastNames = new string[5];
int[] ages = new int[5];
double[] averageGrades = new double[5];
Random random = new Random();
int studentCount = 0;
#endregion

#region Islem Sorgulama
// Step 2 : Program acildiginda yapmak istedigi islemi sorarim ve ona gore isleme devam ederim

while (true)
{
    Console.WriteLine("\nOGRENCİ YONETIM SISTEMİ");
    Console.WriteLine("***********************");
    Console.WriteLine("1 - Tum Ogrencileri Listele");
    Console.WriteLine("2 - Yeni Ogrenci Ekle");
    Console.WriteLine("3 - Ogrenci Sil");
    Console.WriteLine("4 - Soyadina Gore Ogrenci Ara");
    Console.WriteLine("5 - Ogrencileri Dosyaya kaydetme");
    Console.WriteLine("6 - Dosyadan Ogrenci Bilgilerini Yukleme");
    Console.WriteLine("7 - Not Ortalamasi 50'nin Altinda olanlarin Islemi");
    Console.WriteLine("8 - Cikis Yap");
    Console.Write("\nLutfen Yapmak Istediginiz Islemi Seciniz : ");

    // Secimi try-catch ile almamin sebebi; eger kullanici yanlis formatta girerse yani string bir ifade girerse hata versin ve tekrardan secim yaptirsin

    int choice = Convert.ToInt32(Console.ReadLine());

    try
    {
        if (choice == 1)
            ListAllStudents();

        else if (choice == 2)
            AddStudent();

        else if (choice == 3)
        {
            Console.Write("\nSilmek Istediginiz Ogrenci Numarasini Giriniz : ");
            int id = Convert.ToInt32(Console.ReadLine());
            DeleteStudent(id);
        }

        else if (choice == 4)
        {
            Console.Write("\nLutfen Aramak Istediginiz Soyisimi Yaziniz : ");
            string lastName = Console.ReadLine();
            SearchStudentByLastName(lastName);
        }

        else if (choice == 5)
        {
            SaveStudentsToFile("OgrenciListesi.txt");
        }

        else if (choice == 6)
            LoadStudentsFromFile("OgrenciListesi.txt");

        else if (choice == 7)
            ListLowAverageStudents();

        else if (choice == 8)
        {
            Console.WriteLine("\nProgramdan Cikis Yapilmistir...");
            Environment.Exit(0); // Programi tamamen kapat
        }
    }
    catch (FormatException) // Hatali format durumu icin
    {
        Console.WriteLine("\nHatali tuslama! Lutfen sadece sayi giriniz.");
    }
}
#endregion

#region Kullanilan Metotlar
// Step 3 : Metotlarimi tanimlarim ve iclerinde islem yaptiririm 

void ListAllStudents() // Tum ogrencileri listeleyen metot
{
    Console.WriteLine("\nTum Ogrencilerin Bilgileri Asagidaki Gibidir...\n");
    for (int i = 0; i < studentCount; i++)
    {
        Console.WriteLine($"{studentIds[i]} - {firstNames[i]} {lastNames[i]} - {ages[i]} yasinda - Not Ortalamasi: {averageGrades[i]}");
    }
}
void AddStudent() // Ogrenci ekleme islemi yapar
{
    if (studentCount >= 5) // Eğer ogrenci sayısı 5'ten buyukse
    {
        Console.WriteLine("En Fazla 5 Ogrenci Grisi Yapilir..!");
        return;
    }

    // id numarasinin aynı gelmemesi icin random olarak id aldim
    int id;

    do
    {
        id = random.Next(1000, 10000);
    } while (studentIds.Contains(id));
    studentIds[studentCount] = id; // Urettigim id'yi ogrenci sayisi dizisine ekle

    Console.Write("\nOgrenci Adini Giriniz : ");
    firstNames[studentCount] = Console.ReadLine();

    Console.Write("Ogrenci Soyadini Giriniz : ");
    lastNames[studentCount] = Console.ReadLine();

    // Yasi alırken hata firlatmam icin try-catch metodunu kullandim

    bool validAge = false; // Gecerli yas mi?

    while (!validAge)
    {
        try
        {
            Console.Write("Ogrenci Yasi Giriniz : ");
            ages[studentCount] = Convert.ToInt32(Console.ReadLine());
            if (ages[studentCount] <= 0)
            {
                throw new ArgumentOutOfRangeException(); // index hatasi verir
            }
            validAge = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hata Mesajiniz : " + ex.Message); //  Hata mesaji verir
        }
    }
    // Not ortalamasını alırken hata firlatmam icin try-catch metodunu kullandim

    bool validAverage = false; //Gecerli ortalama mi?

    while (!validAverage)
    {
        try
        {
            Console.Write("Ogrenci Not Ortalamasini Giriniz : ");
            averageGrades[studentCount] = double.Parse(Console.ReadLine()); // Kullanıcıdan not ortalamasini al

            if (averageGrades[studentCount] < 0 || averageGrades[studentCount] > 100)
            {
                throw new ArgumentOutOfRangeException(); // Index hatasi verir
            }
            validAverage = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hata Mesajiniz : " + ex.Message); //  Hata mesaji verir
        }
    }
    studentCount++;
    Console.WriteLine("\nOgrenci Kaydi Basariyla Tamamlanmistir.\n");
}
void DeleteStudent(int id) // Ogrenci silme islemi yapar
{
    // array.findindex = Bir dizide belirli kosulu saglayan elemanın dizideki indexini bulur

    int index = Array.FindIndex(studentIds, 0, studentCount, s => s == id);
    if (index == -1)
    {
        Console.WriteLine("\nBu numaraya Ait Ogrenci Mevcut Degildir..!");
        return;
    }

    // Ogrenciyi diziden silmek için elemanlari kaydiriyoruz
    // +1 olmasinin sebebi diziyi sola kaydirmak

    for (int i = index; i < studentCount - 1; i++)
    {
        studentIds[i] = studentIds[i + 1];
        firstNames[i] = firstNames[i + 1];
        lastNames[i] = lastNames[i + 1];
        ages[i] = ages[i + 1];
        averageGrades[i] = averageGrades[i + 1];
    }
    studentCount--;
    Console.WriteLine("\nOgrenci Silme Islemi Tamamlanmistir...");
}
void SearchStudentByLastName(string lastName) // Soyisime gore ogrenci aramasi yapan metot
{
    // equals metodu, ogrenci soyadi ile karsilastirma yapar
    // StringComparison.OrdinalIgnoreCase hazir metodu, büyük-küçük harf duyarsız bir karşılaştırma yapılmasını sağlar

    bool found = false;
    for (int i = 0; i < studentCount; i++)
    {
        if (lastNames[i].Equals(lastName))
        {
            Console.WriteLine($"\n{studentIds[i]} - {firstNames[i]} {lastNames[i]} - {ages[i]} yasinda - Not Ortalamasi: {averageGrades[i]}");
            found = true;
        }
    }

    if (!found)
    {
        Console.WriteLine("\nBoyle Bir Soyisimde Ogrenci Bulunamadi..!");
    }
}
void SaveStudentsToFile(string filePath) // Ogrencileri dosyaya kaydetme metodu
{
    //StreamWriter ve StreamWriter hazir metotlari dosya yazma ve okuma islemini gerceklestirir

    using (StreamWriter writer = new StreamWriter(filePath))
    {
        for (int i = 0; i < studentCount; i++)
        {
            writer.WriteLine($"\n{studentIds[i]}-{firstNames[i]}-{lastNames[i]}-{ages[i]}-{averageGrades[i]}");
        }
    }
    Console.WriteLine("\nDosyaya Kayit Islemi Tamamlanmistir.");
}
void LoadStudentsFromFile(string filePath) // Ogrencileri dosyadan yukleme metodu
{
    if (!File.Exists(filePath)) // Dosya yoksa hata mesajı ver
    {
        Console.WriteLine("\nDosya bulunamadi!");
        return;
    }

    using (StreamReader reader = new StreamReader(filePath))
    {
        studentCount = 0; // Dosyadan yüklerken öğrenci sayısını sıfırla

        while (!reader.EndOfStream && studentCount < 5) // Her bir satırda bir öğrenci bilgisi olduğu varsayılıyor
        {
            string line = reader.ReadLine();
            if (!string.IsNullOrWhiteSpace(line))
            {
                string[] parts = line.Split('-');

                // Dosyadaki veriler 5 parça (id, isim, soyisim, yaş, ortalama) olmalı
                if (parts.Length == 5)
                {
                    studentIds[studentCount] = int.Parse(parts[0]);
                    firstNames[studentCount] = parts[1];
                    lastNames[studentCount] = parts[2];
                    ages[studentCount] = int.Parse(parts[3]);
                    averageGrades[studentCount] = double.Parse(parts[4]);

                    studentCount++;
                }
            }
        }
    }

    Console.WriteLine("\nOgrenciler Dosyadan Yuklenmistir");
}

void ListLowAverageStudents() // Ortalamasi dusuk olan ogrenciler metodu
{
    Console.WriteLine("\nNot Ortalamasi 50'nin Altinda Olan Ogrenciler :");
    bool found = false;

    for (int i = 0; i < studentCount; i++)
    {
        // Ortalamasi 50 altinda olan varsa bu dongu calisir
        if (averageGrades[i] < 50)
        {
            Console.WriteLine($"\n{studentIds[i]} - {firstNames[i]} {lastNames[i]} - Not Ortalamasi: {averageGrades[i]}");
            found = true;
        }
    }

    if (!found)
    {
        Console.WriteLine("\nNot Ortalamasi 50'nin Altinda Ogrenci Bulunmamaktadir..!");
    }
}
#endregion
