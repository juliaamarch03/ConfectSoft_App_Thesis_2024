using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

public class UserProfile
{
    public Dictionary<string, string> UserPhotos { get; set; }

    public UserProfile()
    {
        LoadUserPhotos();
    }

    private void LoadUserPhotos()
    {
        string jsonFilePath = "D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Model/userPhotos.json"; // Вкажіть шлях до JSON файлу
        if (File.Exists(jsonFilePath))
        {
            string jsonContent = File.ReadAllText(jsonFilePath);
            UserPhotos = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);
        }
        else
        {
            UserPhotos = new Dictionary<string, string>();
        }
    }

    public void BindUserProfilePic(Ellipse userEllipse, string userName)
    {
        if (UserPhotos.ContainsKey(userName))
        {
            string photoPath = UserPhotos[userName];
            userEllipse.Fill = new ImageBrush(new BitmapImage(new Uri(photoPath)));
        }
        else if (UserPhotos.ContainsKey("default"))
        {
            string photoPath = UserPhotos["default"];
            userEllipse.Fill = new ImageBrush(new BitmapImage(new Uri(photoPath)));
        }
        else
        {
            // Встановити заповнювач, якщо нічого не знайдено
            userEllipse.Fill = Brushes.Gray;
        }
    }
}
