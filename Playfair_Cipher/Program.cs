using Playfair_Cipher;

Console.WriteLine("Palyfair Cipher : ");

Console.Write("Introduce text : ");
string text = Console.ReadLine();

Console.Write("\nIntroduce key :");
string key = Console.ReadLine();

Playfair playfair = new Playfair();
string encrypted = playfair.Encrypt(text, key);

Console.WriteLine("Encrypted text : " + encrypted);
Console.WriteLine("Decrypted text : " + playfair.Decrypt(encrypted, key));
