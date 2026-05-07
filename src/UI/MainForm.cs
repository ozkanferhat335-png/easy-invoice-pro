using System;
using System.Drawing;
using System.Windows.Forms;

namespace EasyInvoicePro.UI
{
    public class MainForm : Form
    {
        private readonly MenuStrip _menu;
        private readonly StatusStrip _status;
        private readonly ToolStripStatusLabel _statusLabel;

        public MainForm()
        {
            Text = "EasyInvoice Pro - Banka API Entegre Muhasebe";
            Width = 1280;
            Height = 800;
            StartPosition = FormStartPosition.CenterScreen;

            _menu = BuildMenu();
            _status = new StatusStrip();
            _statusLabel = new ToolStripStatusLabel("Hazır");
            _status.Items.Add(_statusLabel);

            var dashboard = new DashboardControl { Dock = DockStyle.Fill };

            Controls.Add(dashboard);
            Controls.Add(_status);
            Controls.Add(_menu);
            MainMenuStrip = _menu;
        }

        private MenuStrip BuildMenu()
        {
            var menu = new MenuStrip();

            var banka = new ToolStripMenuItem("Banka");
            banka.DropDownItems.Add(CreateOpenItem("Banka Hesapları", () => OpenChild(new BankAccountsForm())));
            banka.DropDownItems.Add(CreateOpenItem("Hesap Hareketleri", () => OpenChild(new BankTransactionsForm())));
            banka.DropDownItems.Add(CreateOpenItem("EFT / Havale", () => OpenChild(new TransferOrdersForm())));

            var muhasebe = new ToolStripMenuItem("Muhasebe");
            muhasebe.DropDownItems.Add(CreateOpenItem("Muhasebe Fişleri", () => OpenChild(new AccountingVouchersForm())));
            muhasebe.DropDownItems.Add(CreateOpenItem("Mutabakat", () => OpenChild(new ReconciliationForm())));

            var raporlar = new ToolStripMenuItem("Raporlar");
            raporlar.DropDownItems.Add(CreateOpenItem("Nakit Akış", () => OpenChild(new CashFlowReportForm())));
            raporlar.DropDownItems.Add(CreateOpenItem("Kur Farkı", () => OpenChild(new FxDifferenceReportForm())));

            var yonetim = new ToolStripMenuItem("Yönetim");
            yonetim.DropDownItems.Add(CreateOpenItem("Kullanıcı ve Yetkiler", () => OpenChild(new UserManagementForm())));

            menu.Items.AddRange(new ToolStripItem[] { banka, muhasebe, raporlar, yonetim });
            return menu;
        }

        private ToolStripMenuItem CreateOpenItem(string text, Action action)
        {
            var item = new ToolStripMenuItem(text);
            item.Click += (_, __) => action();
            return item;
        }

        private void OpenChild(Form form)
        {
            _statusLabel.Text = $"Açılıyor: {form.Text}";
            form.ShowDialog(this);
            _statusLabel.Text = "Hazır";
        }
    }

    public class DashboardControl : Panel
    {
        public DashboardControl()
        {
            BackColor = Color.WhiteSmoke;

            var title = new Label
            {
                Text = "Banka API Entegre Muhasebe Uygulaması",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(25, 30)
            };

            var summary = new Label
            {
                Text = "• Banka Hareketleri\n• EFT/Havale Yönetimi\n• Döviz Dönüşüm\n• Mutabakat ve Raporlama",
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(30, 90)
            };

            Controls.Add(title);
            Controls.Add(summary);
        }
    }
}
