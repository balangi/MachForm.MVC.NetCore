namespace MachForm.NetCore.Helpers;

public static class WebhookHelper
{
    public static async Task MfSendWebhookNotification(int formId, int entryId, int webhookRuleId)
    {
        // اینجا باید کد اتصال به دیتابیس و خواندن تنظیمات وب‌هوک را اضافه کنید
        // سپس از HttpClient برای ارسال درخواست استفاده کنید

        using (var client = new HttpClient())
        {
            var values = new Dictionary<string, string>
            {
                { "form_id", formId.ToString() },
                { "entry_id", entryId.ToString() }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://example.com/webhook", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error sending webhook: {response.StatusCode}");
            }
        }
    }
}
