using SQLite4Unity3d;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DatabaseManager;
using System.Collections;
public class LoginManager : MonoBehaviour 
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text message;
    public TMP_Text registerButtonText;
    public GameObject loginButton;
    public GameObject returnButton;
    bool isRegisterMode = false;
    SQLiteConnection db;
    void Start()
    {
        string path = Application.persistentDataPath + "/game.db";
        db = new SQLiteConnection(path);
        returnButton.SetActive(false);

        if (!string.IsNullOrEmpty(SceneMessage.message))
        {
            message.text = SceneMessage.message;
            SceneMessage.message = ""; // clear after showing
        }
        else
        {
            message.text = "";
        }
    }
    public void Login()
    {
        var user = db.Table<User>().FirstOrDefault(x => x.Username == usernameInput.text && x.Password == passwordInput.text);
        if (user != null) 
        {
            message.text = "Login Success!";
            PlayerPrefs.SetString("CurrentUser", usernameInput.text);
            SceneManager.LoadScene("SampleScene");
        }
        else 
        {
            message.text = "Invalid Login";
        }

        IEnumerator LoadGame()
        {
            yield return new WaitForSeconds(1f);
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
    }
    public void Register()
    {
        // First click → enter register mode
        if (!isRegisterMode) 
        {
            isRegisterMode = true; 
            message.text = "Enter new username & password.";
            loginButton.SetActive(false);
            // hide login button
            registerButtonText.text = "Confirm";
            return; 
        }
        // Second click → create account
         string username = usernameInput.text;
        string password = passwordInput.text;
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) 
        {
            message.text = "Enter username and password"; 
            return;
        }
        var existingUser = db.Table<User>() .FirstOrDefault(x => x.Username == username); if (existingUser != null) 
        {
            message.text = "User already exists"; returnButton.SetActive(true);
            // show return button
            return;
        }
        User newUser = new User();
        newUser.Username = username;
        newUser.Password = password; 
        db.Insert(newUser);
        message.text = "Account created! Now login.";
        // Reset mode
        isRegisterMode = false;
        loginButton.SetActive(true);
        // show login again
        registerButtonText.text = "Register"; 
        usernameInput.text = "";
        passwordInput.text = "";
    }
    public void ReturnToLogin() 
    {
        isRegisterMode = false; 
        message.text = "Please login";
        loginButton.SetActive(true); 
        returnButton.SetActive(false); 
        registerButtonText.text = "Register"; 
        usernameInput.text = ""; 
        passwordInput.text = "";
    }
}