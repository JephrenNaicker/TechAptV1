// Copyright © 2025 Always Active Technologies PTY Ltd

using Microsoft.JSInterop;

namespace TechAptV1.Client.Services
{
    public class NotificationService
    {
        private readonly IJSRuntime _jsRuntime;

        public NotificationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task ShowSuccessAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("showToast",
                new
                {
                    text = message,
                    duration = 3000,
                    close = true,
                    gravity = "top",
                    position = "right",
                    backgroundColor = "linear-gradient(to right, #00b09b, #96c93d)"
                });
        }

        public async Task ShowErrorAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("showToast",
                new
                {
                    text = message,
                    duration = 5000,
                    close = true,
                    gravity = "top",
                    position = "right",
                    backgroundColor = "#f03747"
                });
        }
    }
}
