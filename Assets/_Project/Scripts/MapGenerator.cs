using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _floorTile;  // Спрайт пола
    [SerializeField] private TileBase _wallTile;   // Спрайт стены

    [SerializeField] private int _dungeonWidth = 50;
    [SerializeField] private int _dungeonHeight = 50;

    // Параметры комнат
    [SerializeField] private int _minRoomWidth = 5;
    [SerializeField] private int _maxRoomWidth = 15;
    [SerializeField] private int _minRoomHeight = 5;
    [SerializeField] private int _maxRoomHeight = 15;
    [SerializeField] private int _roomCount = 10;

    private List<Room> _rooms = new List<Room>(); // Список для хранения комнат

    private void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Room previousRoom = null; // Для хранения предыдущей комнаты

        for (int i = 0; i < _roomCount; i++)
        {
            // Случайные размеры комнаты
            int roomWidth = Random.Range(_minRoomWidth, _maxRoomWidth);
            int roomHeight = Random.Range(_minRoomHeight, _maxRoomHeight);

            // Случайная позиция для комнаты
            int roomX = Random.Range(0, _dungeonWidth - roomWidth);
            int roomY = Random.Range(0, _dungeonHeight - roomHeight);

            // Создаём и добавляем новую комнату
            Room newRoom = new Room(roomX, roomY, roomWidth, roomHeight);
            _rooms.Add(newRoom);

            // Генерация пола комнаты
            CreateRoom(roomX, roomY, roomWidth, roomHeight);

            // Соединяем новую комнату с предыдущей
            if (previousRoom != null)
            {
                CreateCorridor(previousRoom, newRoom);
            }

            // Обновляем предыдущую комнату
            previousRoom = newRoom;
        }
    }

    void CreateRoom(int startX, int startY, int width, int height)
    {
        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y < startY + height; y++)
            {
                // Устанавливаем тайлы пола
                _tilemap.SetTile(new Vector3Int(x, y, 0), _floorTile);
            }
        }

        // Обводим комнату стенами
        for (int x = startX - 1; x <= startX + width; x++)
        {
            _tilemap.SetTile(new Vector3Int(x, startY - 1, 0), _wallTile); // Нижняя стена
            _tilemap.SetTile(new Vector3Int(x, startY + height, 0), _wallTile); // Верхняя стена
        }

        for (int y = startY - 1; y <= startY + height; y++)
        {
            _tilemap.SetTile(new Vector3Int(startX - 1, y, 0), _wallTile); // Левая стена
            _tilemap.SetTile(new Vector3Int(startX + width, y, 0), _wallTile); // Правая стена
        }
    }

    void CreateCorridor(Room room1, Room room2)
    {
        Vector2Int start = new Vector2Int(room1.CenterX, room1.CenterY);
        Vector2Int end = new Vector2Int(room2.CenterX, room2.CenterY);

        // Сначала прокладываем горизонтальный коридор
        for (int x = Mathf.Min(start.x, end.x); x <= Mathf.Max(start.x, end.x); x++)
        {
            _tilemap.SetTile(new Vector3Int(x, start.y, 0), _floorTile);
        }

        // Затем вертикальный коридор
        for (int y = Mathf.Min(start.y, end.y); y <= Mathf.Max(start.y, end.y); y++)
        {
            _tilemap.SetTile(new Vector3Int(end.x, y, 0), _floorTile);
        }
    }
}
