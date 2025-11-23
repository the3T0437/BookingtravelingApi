using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

public class FirebaseNotificationService
{
    public FirebaseNotificationService()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("Config/firebase-adminsdk.json")
            });
        }
    }

    public async Task<string> SendNotification(
        string token,
        string title,
        string body)
    {
        var message = new Message()
        {
            Token = token,
            Notification = new Notification()
            {
                Title = title,
                Body = body
            },



            Android = new AndroidConfig()
            {
                Priority = Priority.High,
                Notification = new AndroidNotification()
                {
                    ChannelId = "high_importance_channel",
                    Sound = "default"
                }
            },
        };

        string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        return response;
    }
    
    public async Task<BatchResponse> SendMulticastNotification(
       List<string> tokens,
       string title,
       string body)
    {
        var message = new MulticastMessage()
        {
            Tokens = tokens,
            Notification = new Notification()
            {
                Title = title,
                Body = body
            },
        };

        return await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
    }
}