namespace Lab02.Parsing
{
    public interface IParsingStrategy
    {
        List<Student> Parse(string filePath);
    }
}
