using linway_app.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linway_app.PresentationHelpers
{
    public class UIExecutor
    {
        public static async Task<T> ExecuteAsync<T>(IServiceScope scope, Func<IServiceProvider, Task<T>> action, string errorMessage, Form owner)
        {
            try
            {
                if (owner != null)
                {
                    LoadingOverlayHelper.ShowOverlay(owner);
                }
                return await action(scope.ServiceProvider);
            }
            catch (Exception ex)
            {
                scope.ServiceProvider.GetRequiredService<ISavingServices>().DiscardChanges();
                Logger.LogException(ex);
                string reason = Logger.GetReason(ex);
                MessageBox.Show($"{errorMessage}: {reason}");
                return default;
            }
            finally
            {
                if (owner != null)
                {
                    LoadingOverlayHelper.HideOverlay(owner);
                }
            }
        }
    }
}
