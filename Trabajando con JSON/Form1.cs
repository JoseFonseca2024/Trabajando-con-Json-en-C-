using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace Trabajando_con_JSON
{
    public partial class Form1 : Form
    {

        private List<Contacto> listacontactos = new List<Contacto>();

        public Form1()
        {
            InitializeComponent();
            cboTelefonia.DataSource = Enum.GetValues(typeof(Contacto.Telefonia));
            cboTelefonia.SelectedIndex = -1;
        }

        private void AgregarContactoaLista(object sender, EventArgs e)
        {
            //Validación de que no esten vacios los campos
            if (!string.IsNullOrWhiteSpace(txtNombre.Text) && !string.IsNullOrWhiteSpace(txtApellido.Text) && !string.IsNullOrWhiteSpace(mtbNumero.Text))
            {
                var nuevocontacto = new Contacto(txtNombre.Text, txtApellido.Text, mtbNumero.Text, (Contacto.Telefonia)cboTelefonia.SelectedItem);

                listacontactos.Add(nuevocontacto);
                MostrarLista();
                Limpiar();
                MessageBox.Show("Contacto registrado con exito", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else { MessageBox.Show("Complete todos los campos requeridos.", "Campos vacios o imcompletos", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }

        private void EliminarContactodeLista(object sender, EventArgs e)
        {
            if (dtgvLista.SelectedRows.Count > 0)
            {
                listacontactos.RemoveAt(dtgvLista.SelectedRows[0].Index);
                dtgvLista.CurrentCell = null;
                MostrarLista();
            }
            else
            {
                MessageBox.Show("Seleccione un contacto para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GuardarenJSON(object sender, EventArgs e)
        {
            string json =  JsonConvert.SerializeObject(listacontactos, Formatting.Indented);
            File.WriteAllText("Contactos.json", json);
            MessageBox.Show("Lista guardad en JSON con exito", "Guardado con exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LeerDocumentoenJson(object sender, EventArgs e)
        {
            if (File.Exists("Contactos.json"))
            {
                string json = File.ReadAllText("Contactos.json");
                listacontactos = JsonConvert.DeserializeObject<List<Contacto>>(json);
                MostrarLista();
            }
            else
            {
                MessageBox.Show("Archivo Json no encontrado", "Archivo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void LimpiarDataGriedView(object sender, EventArgs e)
        {
            dtgvLista.Rows.Clear();
        }

        private void MostrarListaenDataGriedView(object sender, EventArgs e)
        {
            MostrarLista();
        }

        private void SalirdeFormulario(object sender, EventArgs e)
        {
            DialogResult confirmación = MessageBox.Show("¿Esta seguro que quiere salir?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmación == DialogResult.Yes)
            {
                Application.Exit();
            }
        }


        private void Limpiar()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            mtbNumero.Clear();
            cboTelefonia.SelectedIndex = -1;
        }

        private void MostrarLista()
        {
            dtgvLista.Rows.Clear(); // Limpiar filas antes de registrar
            foreach (var contacto in listacontactos)
            {
                dtgvLista.Rows.Add(contacto.Nombre, contacto.Apellido, contacto.Numero, contacto.TipodeTelefonia.ToString());
            }
        }

        private void Limpiarhaciendoclick(object sender, EventArgs e)
        {
            Limpiar();
        }

    }
}
