namespace DatabaseLibrary.Models;

public interface ICreationTableCompatible
{
    public int ID { get; set; }
    public string Name { get; set; }
    public float Cost { get; set; }
    public ICreationTableCompatible Copy();
    public void Fill(ICreationTableCompatible source);
    public bool MatchesFilter(string filter);
}
