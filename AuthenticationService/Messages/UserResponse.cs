using Common.Messages.Response;

namespace AuthenticationService.Messages;

public class UserResponse : BaseResponse
{
    public UserResponse(string errorMessage, string token, bool success)
    {
        ErrorMessage = errorMessage;
        Token = token;
        Success = success;
    }

    public string Token { get; set; }
}