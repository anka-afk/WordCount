using System.Collections;

public class WordCount
{
    //定义运行参数，初始化为0
    public static bool ist = false;

    public static bool iss = false;
    public static bool ish = false;

    //定义帮助函数
    public static void display_usage()
    {
        string usage = @"usage: WordCount[-s][-t][-h] < ""textfile.txt"" >
                         where[] indicates an optional argument
                         -s prints a series of performance measurements
                         -t prints a trace of the program
                         -h prints this message";
        Console.WriteLine(usage);
    }

    public static void Main(string[] args)
    {
        args = new string[] { @"C:\Users\anka\Desktop\111.txt" };
        //定义要用的局部变量
        string in_flie = null;
        string out_file = @"C:\output.txt";
        //如果参数个数为0，输出错误信息
        if (args.Length == 0)
        {
            Console.WriteLine("Error: No input file specified.");
            display_usage();
            return;
        }
        foreach (string word in args)
        {
            //判断参数是否为运行参数
            switch (word)
            {
                case "-t":
                    ist = true;
                    break;

                case "-s":
                    iss = true;
                    break;

                case "-h":
                    ish = true;
                    display_usage();
                    return;
                    break;

                default:
                    in_flie = word;
                    break;
            }
        }
        if (in_flie == null)
        {
            Console.WriteLine("Error: No input file specified.");
            display_usage();
            return;
        }
        //创建实例正式开始工作
        WordCounter wordcounter = new WordCounter(in_flie, out_file, iss, ist);
        wordcounter.process();
    }
}

public class WordCounter
{
    //定义WordCounter的字段
    private string fileName;

    private string fileOutput;
    private bool spy;
    private bool trace;
    private StreamReader reader = null;
    private StreamWriter writer = null;
    private ArrayList words = new ArrayList();
    private Hashtable Hashtable = new Hashtable();
    private string[][] sentences;

    private static char[] separators = new char[]
{ ' ', '\n', '\t', '.', '\"', ';', ',', '?', '!',
')', '(', '<', '>', '[', ']','’','"' };

    //定义构造函数
    public WordCounter(string fileName, string fileOutput, bool spy, bool trace)
    {
        this.fileName = fileName;
        this.fileOutput = fileOutput;
        this.spy = spy;
        this.trace = trace;
    }

    //定义充当外部接口的函数
    public void process()
    {
        try
        {
            openFile();
            readFile();
            countWords();
            writeWords();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception:\n " + e.Message);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
            if (writer != null)
            {
                writer.Close();
            }
        }
    }

    //定义打开文件的函数
    protected StreamReader openFile()
    {
        if (fileName == null)
            throw new ArgumentNullException();

        if (!File.Exists(fileName))
        {
            string msg = "Invalid File name: " + fileName;
            throw new ArgumentException(msg);
        }

        if (!fileName.EndsWith(".txt"))
        {
            string msg = "Sorry. ";
            string ext = Path.GetExtension(fileName);
            if (ext != string.Empty)
                msg += "Do not support " + ext + " files.";
            throw new Exception(msg);
        }

        reader = File.OpenText(fileName);
        writer = new StreamWriter(fileOutput);
        return reader;
    }

    //定义读取文件的函数
    protected void readFile()
    {
        string text_line = null;
        while ((text_line = reader.ReadLine()) != null)
        {
            if (text_line.Length == 0)
                continue;
            //跳过空行
            words.Add(text_line);
            //将读取的每一行加入到words中
        }
    }

    //定义统计单词的函数
    protected void countWords()
    {
        sentences = new string[words.Count][];
        Hashtable = new Hashtable();
        string str;
        for (int ix = 0; ix < words.Count; ix++)
        {
            str = (string)words[ix];
            sentences[ix] = str.Split(separators);
            //将每一行分割成单词
            for (int jx = 0; jx < sentences[ix].Length; jx++)
            {
                if (sentences[ix][jx].Length == 0)
                    continue;
                if (Hashtable.ContainsKey(sentences[ix][jx]))
                {
                    Hashtable[sentences[ix][jx]] = (int)Hashtable[sentences[ix][jx]] + 1;
                }
                else
                {
                    Hashtable.Add(sentences[ix][jx], 1);
                }
            }
            //进行计数
        }
    }

    //定义输出单词的函数
    protected void writeWords()
    {
        //从哈希表中取出所有的键值对排序
        ArrayList akeys = new ArrayList(Hashtable.Keys);
        akeys.Sort();
        //将排序后的键值对写入文件
        foreach (string key in akeys)
        {
            writer.WriteLine("{0}:{1}", key, Hashtable[key]);
        }
    }
}