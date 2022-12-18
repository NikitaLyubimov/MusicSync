namespace IYandexMusicLogic.DTOs.Response
{
    public class UserAuthResponse
    {
        public string Email { get; set; }
        public bool IsSuccess { get; set; }
        public string ExceptionString { get; set; }
    }
}
