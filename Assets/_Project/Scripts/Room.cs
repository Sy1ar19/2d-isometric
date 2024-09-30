public class Room
{
    public int X;      // Позиция по оси X
    public int Y;      // Позиция по оси Y
    public int Width;  // Ширина комнаты
    public int Height; // Высота комнаты

    public int CenterX => X + Width / 2;
    public int CenterY => Y + Height / 2;

    public Room(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
}