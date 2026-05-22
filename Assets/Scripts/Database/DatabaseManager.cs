using SQLite4Unity3d;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;

    SQLiteConnection db;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            string path = Application.persistentDataPath + "/game.db";
            Debug.Log("DB Path: " + path);

            db = new SQLiteConnection(path);

            // Create tables
            db.CreateTable<User>();

            // TEMP FIX (RUN ONLY ONCE, THEN REMOVE DropTable)
            
            db.CreateTable<PokemonData>();
            var columns = db.GetTableInfo("PokemonData");
            foreach (var col in columns)
            {
                Debug.Log("COLUMN: " + col.Name);
            }

            Debug.Log("Database Initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ================= USER =================
    public void RegisterUser(string username, string password)
    {
        User newUser = new User
        {
            Username = username,
            Password = password
        };

        db.Insert(newUser);
    }

    public User GetUser(string username, string password)
    {
        return db.Table<User>()
            .FirstOrDefault(x => x.Username == username && x.Password == password);
    }

    public bool UserExists(string username)
    {
        return db.Table<User>()
            .Any(x => x.Username == username);
    }

    // ================= POKEMON =================
    public void SavePokemon(string name, int level)
    {
        if (string.IsNullOrEmpty(name) || level <= 0)
        {
            Debug.Log("Skipping invalid Pokemon save");
            return;
        }

        PokemonData data = new PokemonData
        {
            PokemonName = name,
            Level = level
        };

        string currentUser = PlayerPrefs.GetString("CurrentUser");

        var user = db.Table<User>()
            .FirstOrDefault(x => x.Username == currentUser);

        if (user != null)
        {
            data.UserId = user.Id;
            db.Insert(data);

            Debug.Log("Saved Pokemon for user: " + user.Username);
        }
        else
        {
            Debug.LogError("User not found!");
        }
    }

    public List<PokemonData> GetAllPokemon()
    {
        return db.Table<PokemonData>().ToList();
    }

    public List<PokemonData> GetPokemonForCurrentUser()
    {
        string currentUser = PlayerPrefs.GetString("CurrentUser");

        var user = db.Table<User>()
            .FirstOrDefault(x => x.Username == currentUser);

        if (user == null)
        {
            Debug.LogError("User not found!");
            return new List<PokemonData>();
        }

        // Safe filtering (no .Where())
        var allPokemon = db.Table<PokemonData>().ToList();

        List<PokemonData> userPokemon = new List<PokemonData>();

        foreach (var p in allPokemon)
        {
            if (p.UserId == user.Id)
            {
                userPokemon.Add(p);
            }
        }

        return userPokemon;
    }

    // ================= MODEL =================
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}

// Separate class
public class PokemonData
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string PokemonName { get; set; }
    public int Level { get; set; }
    public int UserId { get; set; }
}