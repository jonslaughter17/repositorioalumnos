using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace repositorios_alumnos
{

    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=DESKTOP-FFQD4PB\\MSSQLSERVER01;Initial Catalog=lista_alumnos;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validar que todos los campos estén llenos
            if (string.IsNullOrWhiteSpace(txtnombre.Text) ||
                string.IsNullOrWhiteSpace(txtapellido.Text) ||
                string.IsNullOrWhiteSpace(txtcorreo.Text) ||
                string.IsNullOrWhiteSpace(txtcontrol.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar formato de correo electrónico
            if (!EsCorreoValido(txtcorreo.Text))
            {
                MessageBox.Show("Por favor, ingrese un correo electrónico válido.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Intentar guardar en la base de datos
            try
            {
                GuardarAlumno(txtnombre.Text.Trim(), txtapellido.Text.Trim(),
                            txtcorreo.Text.Trim(), txtcontrol.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool EsCorreoValido(string correo)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(correo);
                return addr.Address == correo;
            }
            catch
            {
                return false;
            }
        }

        private void GuardarAlumno(string nombre, string apellido, string correo, string control)
        {
            // Consulta SQL para insertar
            string query = "INSERT INTO alumnos (nombre, apellido, correo, control) VALUES (@Nombre, @Apellido, @Correo, @Control)";

            // Usar using para asegurar que los recursos se liberen
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Agregar parámetros para evitar inyección SQL
                command.Parameters.AddWithValue("@Nombre", nombre);
                command.Parameters.AddWithValue("@Apellido", apellido);
                command.Parameters.AddWithValue("@Correo", correo);
                command.Parameters.AddWithValue("@Control", control);

                // Abrir conexión y ejecutar comando
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Alumno agregado correctamente!", "Éxito",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Limpiar campos después de guardar
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se pudo agregar el alumno.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimpiarCampos()
        {
            txtnombre.Text = "";
            txtapellido.Text = "";
            txtcorreo.Text = "";
            txtcontrol.Text = "";
            txtnombre.Focus(); // Poner el foco en el primer campo
        }

        // Método para probar la conexión (opcional)
        private void ProbarConexion()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Conexión exitosa a la base de datos!", "Éxito",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnProbarConexion_Click(object sender, EventArgs e)
        {
            ProbarConexion();
        }
    }
}
    

