using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.PresentationHelpers
{
    public static class LoadingOverlayHelper
    {
        public static void ShowOverlay(Form form)
        {
            var overlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(80, Color.Black), // semi-transparente
                Name = "GLOBAL_OVERLAY"
            };
            var progress = new ProgressBar
            {
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30,
                Size = new Size(220, 25),
                Anchor = AnchorStyles.None
            };
            var label = new Label
            {
                Text = "Cargando...",
                ForeColor = Color.White,
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Anchor = AnchorStyles.None
            };
            // Posicionamiento inicial
            progress.Location = new Point((overlay.Width - progress.Width) / 2, (overlay.Height - progress.Height) / 2);
            label.Location = new Point((overlay.Width - label.Width) / 2, progress.Bottom + 10);
            // Mantener centrado si cambia tamaño
            overlay.Resize += (s, e) =>
            {
                progress.Location = new Point(
                    (overlay.Width - progress.Width) / 2,
                    (overlay.Height - progress.Height) / 2);

                label.Location = new Point(
                    (overlay.Width - label.Width) / 2,
                    progress.Bottom + 10);
            };
            //
            overlay.Controls.Add(progress);
            overlay.Controls.Add(label);
            form.Controls.Add(overlay);
            overlay.BringToFront();
        }
        public static void HideOverlay(Form form)
        {
            var overlay = form.Controls.Find("GLOBAL_OVERLAY", false).FirstOrDefault();
            if (overlay != null)
            {
                form.Controls.Remove(overlay);
                overlay.Dispose();
            }
        }
    }
}
