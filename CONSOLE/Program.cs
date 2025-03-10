namespace CONSOLE
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                We can work with files using static classes.
                1. File
                2. Path
                3. Directory
                    ---learned so far
            */
            string path = @"D:\FileSystem";
            string myPath = @"D:\FileSystem\HelloDirectory";
            bool isExists = Directory.Exists(myPath);
            if(!isExists)
            {
                Console.WriteLine($"Directory Do not Exists!");
                Directory.CreateDirectory(myPath);
                Console.WriteLine($"Success Messege: Created 'HelloDirectory' Successfully!");
            }
            else
            {
                Console.WriteLine($"Directory Exists.");
            }

            string[] getAllSubDirectories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);


            Console.WriteLine($"----------Directories----------");
            for(int i = 0; i < getAllSubDirectories.Length; i++)
            {
                Console.WriteLine($"{i+1}. {getAllSubDirectories[i]}");
            }
            Console.WriteLine($"--------Files Inside Directories-------");
            foreach(var dir in getAllSubDirectories)
            {
                string[] files = Directory.GetFiles(dir, "*.*",SearchOption.AllDirectories);
                for(int i = 0; i < files.Length; i++)
                {
                    var info = new FileInfo(files[i]);
                    Console.WriteLine($"{i+1}. {Path.GetFileName(files[i])}. Size: {(info.Length / 1024.0):F2} KB - Created At: {info.CreationTimeUtc}");
                }
            }
        }
    }
}