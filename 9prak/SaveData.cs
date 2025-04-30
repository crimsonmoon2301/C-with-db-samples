using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace _9prak
{
   public class SaveData
    {
        private NpgsqlDataAdapter adapterMaja;
        private NpgsqlDataAdapter adapterIpasnieks;
        private NpgsqlDataAdapter adapterProjekts;

        public SaveData(NpgsqlDataAdapter adapterMaja, NpgsqlDataAdapter adapterIpasnieks, NpgsqlDataAdapter adapterProjekts)
        {
            this.adapterMaja = adapterMaja;
            this.adapterIpasnieks = adapterIpasnieks;
            this.adapterProjekts = adapterProjekts;
        }

        public void Save(DataTable dtmaja, DataTable dtipasnieks, DataTable dtprojekts)
        {
            try
            {
                adapterMaja.Update(dtmaja);
                adapterIpasnieks.Update(dtipasnieks);
                adapterProjekts.Update(dtprojekts);

                MessageBox.Show("Dati veiksmīgi saglabāti!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kļūda saglabājot datus:\n" + ex.Message);
            }
        }
    }
}
