
using System.Text;

while (true)
{
    Console.WriteLine("One-Time Pad Encryption");

    Console.Write("Enter the message 1: ");
    string message1 = Console.ReadLine();

    Console.Write("Enter the message 2: ");
    string message2 = ReadWithAsterisk();

    Console.WriteLine("------------");
    // Generate a one-time pad key
    byte[] oneTimePadBits = message1.Length > message2.Length ? GenerateOneTimePadBytes(message1.Length) : GenerateOneTimePadBytes(message2.Length);

    Console.WriteLine("ASCII message 1: " + message1);
    Console.WriteLine("ASCII Binary Representation: ");
    byte[] message1ToBit = ASCIIStringToBytes(message1, true);

    byte[] message2ToBit = ASCIIStringToBytes(message2, false);

    // Encrypt the message using the one-time pad
    Console.WriteLine("Binary Representation Cipher 1: ");
    byte[] c1 = Encrypt(message1, oneTimePadBits);

    Console.WriteLine("Binary Representation Cipher 2: ");
    byte[] c2 = Encrypt(message2, oneTimePadBits);

    Console.WriteLine("key: ");
    byte[] key = GetKey(message1, c1);


    string m2 = Decrypt(c2, key);
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


    byte[] key = new byte[message.Length];

    for (int i = 0; i < message.Length; i++)
    {
        // XOR the corresponding bytes of the message and one-time pad
        key[i] = (byte)(message[i] ^ subsetcipher[i]);
    }

    PrintBytes(key);

    return key;
}

static string Decrypt(byte[] cipher, byte[] key)
{
    var length = cipher.Length >= key.Length ? key.Length : cipher.Length;

    byte[] subsetcipher = new byte[length];
    Array.Copy(cipher, subsetcipher, length);

    byte[] dencryptedBytes = new byte[subsetcipher.Length];

    for (int i = 0; i < length; i++)
    {
        // XOR the corresponding bytes of the message and one-time pad
        dencryptedBytes[i] = (byte)(key[i] ^ subsetcipher[i]);
    }

    string asciiString = Encoding.ASCII.GetString(dencryptedBytes);

    var remainLength = cipher.Length - asciiString.Length;

    for (int i = 0; i < remainLength; i++)
    {
        asciiString = asciiString + "*";
    }

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