/// <summary>
/// Row and column entry of a 15*15 matrix
/// </summary>
public class Position
{
    private int row;
    private int col;

    public Position()
    {
        this.Row = 0;
        this.Col = 0;
    }

    public Position(int row, int col)
    {
        this.Row = row;
        this.Col = col;        
    }

    public int Row { get => this.row; set => this.row = value; }

    public int Col { get => this.col; set => this.col = value; }

    public bool IsEqual(Position obj)
    {
        return this.Row == obj.Row && this.Col == obj.Col;
    }
}
