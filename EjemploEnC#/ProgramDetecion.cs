using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

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

        private bool proceso1Esperando = false;
        private bool proceso2Esperando = false;
        private bool interbloqueoDetectado = false;

        public Program()
        {
            this.Text = "Simulación de Interbloqueo con Detección";
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

            // Espera que las tareas terminen o se detecte un interbloqueo
            await Task.WhenAny(tarea1, tarea2);

            if (interbloqueoDetectado)
            {
                estadoLabel.Text = "Estado: Interbloqueo detectado, liberando recursos...";
                // Resolver interbloqueo (en este caso, cancelar una de las transferencias)
                if (proceso1Esperando)
                {
                    proceso1Esperando = false; // Simulamos la liberación de recursos
                }
                else if (proceso2Esperando)
                {
                    proceso2Esperando = false;
                }
            }

            await Task.WhenAll(tarea1, tarea2);

            estadoLabel.Text = "Estado: Transferencias completadas o interbloqueo resuelto.";
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

                    if (!Monitor.TryEnter(lockCuenta2))
                    {
                        proceso1Esperando = true;
                        DetectarInterbloqueo();
                        return;
                    }

                    try
                    {
                        saldoCuenta1 -= 100;
                        saldoCuenta2 += 100;
                        ActualizarSaldos();
                    }
                    finally
                    {
                        Monitor.Exit(lockCuenta2);
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

                    if (!Monitor.TryEnter(lockCuenta1))
                    {
                        proceso2Esperando = true;
                        DetectarInterbloqueo();
                        return;
                    }

                    try
                    {
                        saldoCuenta2 -= 100;
                        saldoCuenta1 += 100;
                        ActualizarSaldos();
                    }
                    finally
                    {
                        Monitor.Exit(lockCuenta1);
                    }
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => estadoLabel.Text = $"Error: {ex.Message}"));
            }
        }

        private void DetectarInterbloqueo()
        {
            if (proceso1Esperando && proceso2Esperando)
            {
                Invoke(new Action(() => estadoLabel.Text = "Interbloqueo detectado."));
                interbloqueoDetectado = true;
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
