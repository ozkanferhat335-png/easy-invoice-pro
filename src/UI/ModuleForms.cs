using System.Drawing;
using System.Windows.Forms;

namespace EasyInvoicePro.UI
{
    public class BankAccountsForm : BaseModuleForm
    {
        public BankAccountsForm() : base("Banka Hesapları") { }
    }

    public class BankTransactionsForm : BaseModuleForm
    {
        public BankTransactionsForm() : base("Hesap Hareketleri") { }
    }

    public class TransferOrdersForm : BaseModuleForm
    {
        public TransferOrdersForm() : base("EFT / Havale") { }
    }

    public class AccountingVouchersForm : BaseModuleForm
    {
        public AccountingVouchersForm() : base("Muhasebe Fişleri") { }
    }

    public class ReconciliationForm : BaseModuleForm
    {
        public ReconciliationForm() : base("Mutabakat") { }
    }

    public class CashFlowReportForm : BaseModuleForm
    {
        public CashFlowReportForm() : base("Nakit Akış Raporu") { }
    }

    public class FxDifferenceReportForm : BaseModuleForm
    {
        public FxDifferenceReportForm() : base("Kur Farkı Raporu") { }
    }

    public class UserManagementForm : BaseModuleForm
    {
        public UserManagementForm() : base("Kullanıcı ve Yetki Yönetimi") { }
    }

    public abstract class BaseModuleForm : Form
    {
        protected BaseModuleForm(string title)
        {
            Text = title;
            Width = 920;
            Height = 620;
            StartPosition = FormStartPosition.CenterParent;

            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var lbl = new Label
            {
                Text = $"{title} ekranı (MVP)\nBu ekran sonraki adımda servis/repository ile bağlanacaktır.",
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(25, 25)
            };

            var progress = new ProgressBar
            {
                Width = 350,
                Height = 24,
                Location = new Point(25, 100),
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30
            };

            var cancel = new Button
            {
                Text = "İptal",
                Width = 120,
                Height = 32,
                Location = new Point(390, 96)
            };
            cancel.Click += (_, __) => Close();

            panel.Controls.Add(lbl);
            panel.Controls.Add(progress);
            panel.Controls.Add(cancel);
            Controls.Add(panel);
        }
    }
}
