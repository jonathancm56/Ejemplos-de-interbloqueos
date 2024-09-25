using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EjemploInterbloqueo
{
    public class Program : Form
    {
        private Label cuenta1Label;
        private Label cuenta2Label;
        private Label estadoLabel;
        private Button iniciarTransferenciaBtn;

        private object lockCuenta1 = new object();
        private object lockCuenta2 = new object();
        private int saldoCuenta1 = 1000;
        private int saldoCuenta2 = 1000;

        public Program()
        {
            this.Text = "Simulación de Interbloqueo";
            this.Size = new Size(400, 300);

            cuenta1Label = new Label
            {
                Text = $"Cuenta 1: ${saldoCuenta1}",
                Location = new Point(50, 50),
                AutoSize = true
            };
            cuenta2Label = new Label
            {
                Text = $"Cuenta 2: ${saldoCuenta2}",
                Location = new Point(50, 100),
                AutoSize = true
            };
            estadoLabel = new Label
            {
                Text = "Estado: Listo",
                Location = new Point(50, 150),
                AutoSize = true
            };

            iniciarTransferenciaBtn = new Button
            {
                Text = "Iniciar Transferencia",
                Location = new Point(50, 200)
            };
            iniciarTransferenciaBtn.Click += new EventHandler(IniciarTransferencia_Click);

            this.Controls.Add(cuenta1Label);
            this.Controls.Add(cuenta2Label);
            this.Controls.Add(estadoLabel);
            this.Controls.Add(iniciarTransferenciaBtn);
        }

        private async void IniciarTransferencia_Click(object sender, EventArgs e)
        {
            estadoLabel.Text = "Estado: Iniciando transferencias...";
            iniciarTransferenciaBtn.Enabled = false;

            Task tarea1 = Task.Run(() => TransferirDeCuenta1ACuenta2());
            Task tarea2 = Task.Run(() => TransferirDeCuenta2ACuenta1());

            await Task.WhenAll(tarea1, tarea2);

            estadoLabel.Text = "Estado: Transferencias completadas o interbloqueo detectado.";
            iniciarTransferenciaBtn.Enabled = true;
        }

        private void TransferirDeCuenta1ACuenta2()
        {
            try
            {
                lock (lockCuenta1)
                {
                    Invoke(new Action(() => estadoLabel.Text = "Transferencia de Cuenta 1 a Cuenta 2 en progreso..."));
                    Thread.Sleep(1000);  // Simular tiempo de operación

                    lock (lockCuenta2)
                    {
                        saldoCuenta1 -= 100;
                        saldoCuenta2 += 100;
                        ActualizarSaldos();
                    }
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => estadoLabel.Text = $"Error: {ex.Message}"));
            }
        }

        private void TransferirDeCuenta2ACuenta1()
        {
            try
            {
                lock (lockCuenta2)
                {
                    Invoke(new Action(() => estadoLabel.Text = "Transferencia de Cuenta 2 a Cuenta 1 en progreso..."));
                    Thread.Sleep(1000);  // Simular tiempo de operación

                    lock (lockCuenta1)
                    {
                        saldoCuenta2 -= 100;
                        saldoCuenta1 += 100;
                        ActualizarSaldos();
                    }
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => estadoLabel.Text = $"Error: {ex.Message}"));
            }
        }

        private void ActualizarSaldos()
        {
            Invoke(new Action(() =>
            {
                cuenta1Label.Text = $"Cuenta 1: ${saldoCuenta1}";
                cuenta2Label.Text = $"Cuenta 2: ${saldoCuenta2}";
            }));
        }

        [STAThread]
        public static void Main()
        {
            Application.Run(new Program());
        }
    }
}
