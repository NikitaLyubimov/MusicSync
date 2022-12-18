namespace IYandexMusicLogic.DTOs.Response
{
    public record UserAuthResponse(string Email, bool IsSuccess, string ExceptionString);
}
