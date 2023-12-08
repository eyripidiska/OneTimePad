﻿
using System.Text;

while (true)
{
    Console.WriteLine("One-Time Pad Encryption");

    Console.Write("Enter the message 1: ");
    string message1 = Console.ReadLine();

    Console.Write("Enter the message 2: ");
    string message2 = ReadWithAsterisk();
    //string message2 = Console.ReadLine();

    Console.WriteLine("------------");
    // Generate a one-time pad key
    //Console.WriteLine("Binary Representation Key: ");
    byte[] oneTimePadBits = message1.Length > message2.Length ? GenerateOneTimePadBytes(message1.Length) : GenerateOneTimePadBytes(message2.Length);

    Console.WriteLine("ASCII message 1: " + message1);
    Console.WriteLine("ASCII Binary Representation: ");
    byte[] message1ToBit = ASCIIStringToBytes(message1, true);

    //Console.WriteLine("ASCII message 2: " + message2);
    //Console.WriteLine("ASCII Binary Representation: ");
    byte[] message2ToBit = ASCIIStringToBytes(message2, false);

    // Encrypt the message using the one-time pad
    Console.WriteLine("Binary Representation Cipher 1: ");
    byte[] c1 = Encrypt(message1, oneTimePadBits);

    Console.WriteLine("Binary Representation Cipher 2: ");
    byte[] c2 = Encrypt(message2, oneTimePadBits);

    Console.WriteLine("key: ");
    byte[] key = GetKey(message1, c1);


    string m2 = DeCrypt(c2, key);
    Console.WriteLine("message 2: " + m2);

    Console.WriteLine("\n");
}



static byte[] ASCIIStringToBytes(string asciiString, bool isPrinted)
{
    // Convert ASCII string to bytes
    byte[] asciiBytes = Encoding.ASCII.GetBytes(asciiString);

    // Display the result

    if (isPrinted)
    {
        PrintBytes(asciiBytes);
    }

    return asciiBytes;
}

static byte[] GenerateOneTimePadBytes(int length)
{
    Random random = new Random();
    byte[] oneTimePad = new byte[length];

    random.NextBytes(oneTimePad);

    //PrintBytes(oneTimePad);

    return oneTimePad;
}


static byte[] Encrypt(string message, byte[] oneTimePad)
{

    byte[] subsetOneTimePad = new byte[message.Length];
    Array.Copy(oneTimePad, subsetOneTimePad, message.Length);
    

    byte[] encryptedBytes = new byte[message.Length];

    for (int i = 0; i < message.Length; i++)
    {
        // XOR the corresponding bytes of the message and one-time pad
        encryptedBytes[i] = (byte)(message[i] ^ subsetOneTimePad[i]);
    }

    PrintBytes(encryptedBytes);

    return encryptedBytes;
}

static byte[] GetKey(string message, byte[] cipher)
{

    byte[] subsetcipher = new byte[message.Length];
    Array.Copy(cipher, subsetcipher, message.Length);


    byte[] encryptedBytes = new byte[message.Length];

    for (int i = 0; i < message.Length; i++)
    {
        // XOR the corresponding bytes of the message and one-time pad
        encryptedBytes[i] = (byte)(message[i] ^ subsetcipher[i]);
    }

    PrintBytes(encryptedBytes);

    return encryptedBytes;
}

static string DeCrypt(byte[] cipher, byte[] key)
{

    byte[] subsetOneTimePad = new byte[cipher.Length];
    Array.Copy(key, subsetOneTimePad, cipher.Length);


    byte[] dencryptedBytes = new byte[cipher.Length];

    for (int i = 0; i < cipher.Length; i++)
    {
        // XOR the corresponding bytes of the message and one-time pad
        dencryptedBytes[i] = (byte)(cipher[i] ^ subsetOneTimePad[i]);
    }

    string asciiString = Encoding.ASCII.GetString(dencryptedBytes);

    return asciiString;
}

static string ReadWithAsterisk()
{
    string password = "";
    ConsoleKeyInfo key;

    do
    {
        key = Console.ReadKey(true); // The true parameter hides the pressed key
                                     // Ignore non-character keys (e.g., Enter)
        if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
        {
            password += key.KeyChar;
            Console.Write("*"); // Display an asterisk for each character
        }
        else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
        {
            password = password.Substring(0, password.Length - 1);
            Console.Write("\b \b"); // Move the cursor back, erase the last character, and move the cursor back again
        }
    } while (key.Key != ConsoleKey.Enter);

    Console.WriteLine(); // Move to the next line after the user presses Enter
    return password;
}


static void PrintBytes(byte[] bytes)
{
    foreach (byte b in bytes)
    {
        // Convert each byte to binary representation
        string binaryRepresentation = Convert.ToString(b, 2).PadLeft(8, '0');

        // Display the binary representation
        Console.Write(binaryRepresentation + " ");
    }
    Console.WriteLine("\n------------");
}