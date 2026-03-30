using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace linway_app.PresentationHelpers
{
    public static class LoadingOverlayHelper
    {
        private const string OverlayName = "GLOBAL_OVERLAY";

        public static void ShowOverlay(Form form)
        {
            if (form == null || form.IsDisposed)
            {
                return;
            }
            if (form.InvokeRequired)
            {
                form.Invoke((MethodInvoker)(() => ShowOverlay(form)));
                return;
            }
            var existingOverlay = form.Controls.Find(OverlayName, false).FirstOrDefault();
            if (existingOverlay != null)
            {
                existingOverlay.BringToFront();
                return;
            }
            var overlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(170, Color.Black),
                Name = OverlayName
            };
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            var content = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Anchor = AnchorStyles.None,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            var progress = new ProgressBar
            {
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30,
                Size = new Size(220, 25),
                Margin = new Padding(0)
            };
            var label = new Label
            {
                Text = "Cargando...",
                ForeColor = Color.White,
                AutoSize = false,
                Width = progress.Width,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 10, 0, 0)
            };
            content.Controls.Add(progress);
            content.Controls.Add(label);
            layout.Controls.Add(content, 0, 1);
            overlay.Controls.Add(layout);
            form.Controls.Add(overlay);
            overlay.BringToFront();
            overlay.Update();
            form.Update();
            Application.DoEvents();
        }
        public static void HideOverlay(Form form)
        {
            if (form == null || form.IsDisposed)
            {
                return;
            }
            if (form.InvokeRequired)
            {
                form.Invoke((MethodInvoker)(() => HideOverlay(form)));
                return;
            }
            var overlay = form.Controls.Find(OverlayName, false).FirstOrDefault();
            if (overlay != null)
            {
                form.Controls.Remove(overlay);
                overlay.Dispose();
            }
        }
    }
}
