namespace MachForm.NetCore.Helpers;

public static class NotificationHelper
{
    public static void MfShowMessage()
    {
        var session = HttpContext.Current.Session;
        var response = HttpContext.Current.Response;

        if (session["MF_SUCCESS"] != null)
        {
            string successMessage = session["MF_SUCCESS"].ToString();
            response.Write($@"
                <div class='box_blue content_notification'>
                    <div class='cn_icon'>
                        <span class='icon-checkmark-circle'></span>
                    </div>
                    <div class='cn_message'>
                        <h6 style='font-size: 16px'>Success!</h6>
                        <h6>{successMessage}</h6>
                    </div>
                    <a id='close_notification' href='#' onclick=""$('.content_notification').fadeOut();return false;"" title='Close Notification'>
                        <span class='icon-cross2'></span>
                    </a>
                </div>");
            session.Remove("MF_SUCCESS");
        }
        else if (session["MF_ERROR"] != null)
        {
            string errorMessage = session["MF_ERROR"].ToString();
            response.Write($@"
                <div class='box_red content_notification'>
                    <div class='cn_icon'>
                        <span class='icon-notification'></span>
                    </div>
                    <div class='cn_message'>
                        <h6 style='font-size: 16px'>Error!</h6>
                        <h6>{errorMessage}</h6>
                    </div>
                    <a id='close_notification' href='#' onclick=""$('.content_notification').fadeOut();return false;"" title='Close Notification'>
                        <img src='images/icons/52_red_16.png' />
                    </a>
                </div>");
            session.Remove("MF_ERROR");
        }
    }
}
