namespace y1000.Source.Animation;

public interface IAtdRepository
{
    AtdStructure LoadByName(string fileName);

    bool HasFile(string fileName);

}